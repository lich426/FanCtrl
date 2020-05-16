using System;
using System.Collections.Generic;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;
using NvAPIWrapper;
using NvAPIWrapper.GPU;

namespace FanControl
{
    public class HardwareManager
    {
        // Singletone
        private HardwareManager(){}
        private static HardwareManager sManager = new HardwareManager();
        public static HardwareManager getInstance() { return sManager; }

        // Start state
        private bool mIsStart = false;

        // lock
        private object mLock = new object();

        // Mutex
        private Mutex mISABusMutex = null;
        private Mutex mSMBusMutex = null;
        private Mutex mPCIMutex = null;

        // Gigabyte
        private bool mIsGigabyte = false;
        private Gigabyte mGigabyte = null;

        // LibreHardwareMonitor
        private LHM mLHM = null;

        // OpenHardwareMonitor
        private OHM mOHM = null;

        // NZXT Kraken
        private Kraken mKraken = null;
        public Kraken getKraken() { return mKraken; }

        // EVGA CLC
        private CLC mCLC = null;
        public CLC getCLC() { return mCLC; }

        // Temperature sensor List
        private List<BaseSensor> mSensorList = new List<BaseSensor>();        

        // Fan List
        private List<BaseSensor> mFanList = new List<BaseSensor>();       

        // Control List
        private List<BaseControl> mControlList = new List<BaseControl>();

        // next tick change value
        private List<int> mChangeValueList = new List<int>();
        private List<BaseControl> mChangeControlList = new List<BaseControl>();

        // update thread
        private long mUpdateInterval = 1000;
        private bool mUpdateThreadState = false;
        private Thread mUpdateThread = null;

        public event UpdateTimerEventHandler onUpdateCallback;
        public delegate void UpdateTimerEventHandler();

        public void start()
        {
            Monitor.Enter(mLock);
            if (mIsStart == true)
            {
                Monitor.Exit(mLock);
                return;
            }
            mIsStart = true;

            string mutexName = "Global\\Access_ISABUS.HTP.Method";
            this.createBusMutex(mutexName, ref mISABusMutex);

            mutexName = "Global\\Access_SMBUS.HTP.Method";
            this.createBusMutex(mutexName, ref mSMBusMutex);

            mutexName = "Global\\Access_PCI";
            this.createBusMutex(mutexName, ref mPCIMutex);

            // Gigabyte
            if(OptionManager.getInstance().IsGigabyte == true)
            {
                mGigabyte = new Gigabyte();
                mGigabyte.AddChangeValue += addChangeValue;
                mGigabyte.LockBus += lockBus;
                mGigabyte.UnlockBus += unlockBus;

                mIsGigabyte = mGigabyte.start();
            }
            else
            {
                mIsGigabyte = false;
                Gigabyte.stopService();
            }
            
            if (mIsGigabyte == false)
            {
                mGigabyte = null;

                // LibreHardwareMonitor
                if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
                {
                    mLHM = new LHM();
                    mLHM.start();
                }

                // OpenHardwareMonitor
                else
                {
                    mOHM = new OHM();
                    mOHM.start();
                }
            }

            // NvAPIWrapper
            if(OptionManager.getInstance().IsNvAPIWrapper == true)
            {
                NVIDIA.Initialize();
            }

            this.createTemp();
            this.createFan();
            this.createControl();

            if (OptionManager.getInstance().IsKraken == true)
            {
                // NZXT Kraken
                try
                {
                    mKraken = new Kraken();

                    // X2
                    if (mKraken.start(USBProductID.KrakenX2) == false)
                    {
                        // X3
                        if (mKraken.start(USBProductID.KrakenX3) == false)
                        {
                            mKraken = null;
                        }
                    }

                    if (mKraken != null)
                    {
                        var sensor = new KrakenLiquidTemp(mKraken);
                        mSensorList.Add(sensor);

                        if (mKraken.ProductID == USBProductID.KrakenX2)
                        {
                            var fan = new KrakenFanSpeed(mKraken);
                            mFanList.Add(fan);
                        }

                        var pump = new KrakenPumpSpeed(mKraken);
                        mFanList.Add(pump);

                        if (mKraken.ProductID == USBProductID.KrakenX2)
                        {
                            var fanControl = new KrakenFanControl(mKraken);
                            mControlList.Add(fanControl);
                            this.addChangeValue(30, fanControl);
                        }

                        var pumpControl = new KrakenPumpControl(mKraken);
                        mControlList.Add(pumpControl);
                        this.addChangeValue(50, pumpControl);
                    }
                }
                catch
                {
                    mKraken = null;
                }
            }

            if (OptionManager.getInstance().IsCLC == true)
            {
                try
                {
                    mCLC = new CLC();
                    if(mCLC.start(USBProductID.CLC) == false)
                    {
                        mCLC = null;
                    }

                    if (mCLC != null)
                    {
                        var sensor = new CLCLiquidTemp(mCLC);
                        mSensorList.Add(sensor);

                        var fan = new CLCFanSpeed(mCLC);
                        mFanList.Add(fan);

                        var pump = new CLCPumpSpeed(mCLC);
                        mFanList.Add(pump);

                        var fanControl = new CLCFanControl(mCLC);
                        mControlList.Add(fanControl);
                        this.addChangeValue(25, fanControl);

                        var pumpControl = new CLCPumpControl(mCLC);
                        mControlList.Add(pumpControl);
                        this.addChangeValue(50, pumpControl);
                    }
                }
                catch
                {
                    mCLC = null;
                }
            }

            // DIMM thermal sensor
            if(OptionManager.getInstance().IsDimm == true)
            {
                this.lockSMBus(0);
                if (SMBusController.open(false) == true)
                {
                    int num = 1;
                    int busCount = SMBusController.getCount();

                    for (int i = 0; i < busCount; i++)
                    {
                        var detectBytes = SMBusController.i2cDetect(i);
                        if (detectBytes != null)
                        {
                            // 0x18 ~ 0x20
                            for (int j = 0; j < detectBytes.Length; j++)
                            {
                                if (j < 24)
                                    continue;
                                else if (j > 32)
                                    break;

                                if (detectBytes[j] == (byte)j)
                                {
                                    var sensor = new DimmTemp("DIMM #" + num++, i, detectBytes[j]);
                                    sensor.onSetDimmTemperature += onSetDimmTemperature;
                                    mSensorList.Add(sensor);
                                }
                            }
                        }
                    }
                }
                this.unlockSMBus();
            }            

            // Motherboard temperature
            this.createMotherBoardTemp();

            // GPU
            this.createGPUTemp();
            this.createGPUFan();
            this.createGPUControl();

            Monitor.Exit(mLock);
        }        

        public void stop()
        {
            Monitor.Enter(mLock);
            if (mIsStart == false)
            {
                Monitor.Exit(mLock);
                return;
            }
            mIsStart = false;

            mChangeControlList.Clear();
            mChangeValueList.Clear();

            mUpdateThreadState = false;
            if (mUpdateThread != null)
            {
                mUpdateThread.Join();
                mUpdateThread = null;
            }

            if (mLHM != null)
            {
                mLHM.stop();
                mLHM = null;
            }

            if (mOHM != null)
            {
                mOHM.stop();
                mOHM = null;
            }

            try
            {
                if (mKraken != null)
                {
                    mKraken.stop();
                    mKraken = null;
                }
            }
            catch { }

            try
            {
                if (mCLC != null)
                {
                    mCLC.stop();
                    mCLC = null;
                }
            }
            catch { }

            if (mIsGigabyte == true && mGigabyte != null)
            {
                mIsGigabyte = false;
                mGigabyte.stop();
                mGigabyte = null;
            }

            mSensorList.Clear();
            mFanList.Clear();
            mControlList.Clear();

            SMBusController.close();

            if (mISABusMutex != null)
            {
                mISABusMutex.Close();
                mISABusMutex = null;
            }

            if (mSMBusMutex != null)
            {
                mSMBusMutex.Close();
                mSMBusMutex = null;
            }

            if (mPCIMutex != null)
            {
                mPCIMutex.Close();
                mPCIMutex = null;
            }
            Monitor.Exit(mLock);
        }

        public void startUpdate()
        {
            Monitor.Enter(mLock);
            if (mIsStart == false)
            {
                Monitor.Exit(mLock);
                return;
            }

            mUpdateInterval = OptionManager.getInstance().Interval;
            mUpdateThreadState = true;
            mUpdateThread = new Thread(onUpdateThread);
            mUpdateThread.Start();

            Monitor.Exit(mLock);
        }

        public void restartTimer(int interval)
        {
            Monitor.Enter(mLock);
            if (mIsStart == false)
            {
                Monitor.Exit(mLock);
                return;
            }
            mUpdateInterval = interval;
            Monitor.Exit(mLock);
        }

        private void createBusMutex(string mutexName, ref Mutex mutex)
        {
            try
            {
                //mutex permissions set to everyone to allow other software to access the hardware
                //otherwise other monitoring software cant access
                var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
                var securitySettings = new MutexSecurity();
                securitySettings.AddAccessRule(allowEveryoneRule);
                mutex = new Mutex(false, mutexName, out _, securitySettings);
            }
            catch (UnauthorizedAccessException)
            {
                try
                {
                    mutex = Mutex.OpenExisting(mutexName, MutexRights.Synchronize);
                }
                catch { }
            }
        }

        private void lockBus()
        {
            mISABusMutex.WaitOne();
            mPCIMutex.WaitOne();
        }

        private void unlockBus()
        {
            mISABusMutex.ReleaseMutex();
            mPCIMutex.ReleaseMutex();
        }

        private bool lockSMBus(int ms)
        {
            if (ms > 0)
            {
                return mSMBusMutex.WaitOne(ms);
            }
            mSMBusMutex.WaitOne();
            return true;
        }

        private void unlockSMBus()
        {
            mSMBusMutex.ReleaseMutex();
        }        

        private void createTemp()
        {
            // Gigabyte
            if (mIsGigabyte == true)
            {
                mGigabyte.createTemp(ref mSensorList);
            }

            // LibreHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
            {
                mLHM.createTemp(ref mSensorList);
            }

            // OpenHardwareMonitor
            else
            {
                mOHM.createTemp(ref mSensorList);
            }            
        }

        private void createMotherBoardTemp()
        {
            if (mIsGigabyte == true)
                return;

            // LibreHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
            {
                mLHM.createMotherBoardTemp(ref mSensorList);
            }

            // OpenHardwareMonitor
            else
            {
                mOHM.createMotherBoardTemp(ref mSensorList);
            }
        }

        private void createFan()
        {
            // Gigabyte
            if (mIsGigabyte == true)
            {
                mGigabyte.createFan(ref mFanList);
            }

            // LibreHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
            {
                mLHM.createFan(ref mFanList);
            }

            // OpenHardwareMonitor
            else
            {
                mOHM.createFan(ref mFanList);
            }
        }

        private void createControl()
        {
            // Gigabyte
            if (mIsGigabyte == true)
            {
                mGigabyte.createControl(ref mControlList);
            }

            // LibreHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
            {
                mLHM.createControl(ref mControlList);
            }

            // OpenHardwareMonitor
            else
            {
                mOHM.createControl(ref mControlList);
            }
        }

        private void createGPUTemp()
        {
            if (OptionManager.getInstance().IsNvAPIWrapper == true)
            {
                this.lockBus();
                try
                {
                    int gpuNum = 2;
                    var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                    for (int i = 0; i < gpuArray.Length; i++)
                    {
                        var gpu = gpuArray[i];
                        var name = gpu.FullName;
                        while (this.isExistTemp(name) == true)
                        {
                            name = gpu.FullName + " #" + gpuNum++;
                        }

                        var temp = new NvAPITemp(name, i);
                        temp.onGetNvAPITemperatureHandler += onGetNvAPITemperature;
                        mSensorList.Add(temp);
                    }
                }
                catch { }                
                this.unlockBus();
            }
        }

        private void createGPUFan()
        {
            // Gigabyte
            if (mIsGigabyte == true) { }

            // LibreHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
            {
                mLHM.createGPUFan(ref mFanList);
            }

            // OpenHardwareMonitor
            else
            {
                mOHM.createGPUFan(ref mFanList);
            }

            if (OptionManager.getInstance().IsNvAPIWrapper == true)
            {
                this.lockBus();
                try
                {
                    int gpuFanNum = 1;
                    var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                    for (int i = 0; i < gpuArray.Length; i++)
                    {
                        var e = gpuArray[i].CoolerInformation.Coolers.GetEnumerator();
                        while (e.MoveNext())
                        {
                            var value = e.Current;
                            var name = "GPU Fan #" + gpuFanNum++;
                            while (this.isExistFan(name) == true)
                            {
                                name = "GPU Fan #" + gpuFanNum++;
                            }

                            var fan = new NvAPIFanSpeed(name, i, value.CoolerId);
                            fan.onGetNvAPIFanSpeedHandler += onGetNvAPIFanSpeed;
                            mFanList.Add(fan);
                        }
                    }
                }
                catch { }                
                this.unlockBus();
            }
        }

        private void createGPUControl()
        {
            // Gigabyte
            if (mIsGigabyte == true) { }

            // LibreHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
            {
                mLHM.createGPUFanControl(ref mControlList);
            }

            // OpenHardwareMonitor
            else
            {
                mOHM.createGPUFanControl(ref mControlList);
            }

            if (OptionManager.getInstance().IsNvAPIWrapper == true)
            {
                this.lockBus();
                try
                {
                    int gpuFanNum = 1;
                    var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                    for (int i = 0; i < gpuArray.Length; i++)
                    {
                        var e = gpuArray[i].CoolerInformation.Coolers.GetEnumerator();
                        while (e.MoveNext())
                        {
                            var value = e.Current;
                            int coolerID = value.CoolerId;
                            int speed = value.CurrentLevel;
                            int minSpeed = value.DefaultMinimumLevel;
                            int maxSpeed = value.DefaultMaximumLevel;

                            var name = "GPU Fan Control #" + gpuFanNum++;
                            while (this.isExistControl(name) == true)
                            {
                                name = "GPU Fan Control #" + gpuFanNum++;
                            }

                            var control = new NvAPIFanControl(name, i, coolerID, speed, minSpeed, maxSpeed);
                            control.onSetNvAPIControlHandler += onSetNvApiControl;
                            mControlList.Add(control);
                        }
                    }
                }
                catch { }                
                this.unlockBus();
            }
        }

        private bool isExistTemp(string name)
        {
            for (int i = 0; i < mSensorList.Count; i++)
            {
                if (mSensorList[i].Name.Equals(name) == true)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isExistFan(string name)
        {
            for (int i = 0; i < mFanList.Count; i++)
            {
                if (mFanList[i].Name.Equals(name) == true)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isExistControl(string name)
        {
            for (int i = 0; i < mControlList.Count; i++)
            {
                if (mControlList[i].Name.Equals(name) == true)
                {
                    return true;
                }
            }
            return false;
        }

        private void onSetDimmTemperature(object sender, int busIndex, byte address)
        {
            var sensor = (DimmTemp)sender;

            if (this.lockSMBus(10) == false)
                return;

            var wordArray = SMBusController.i2cWordData(busIndex, address, 10);
            if(wordArray == null)
            {
                this.unlockSMBus();
                return;
            }
            this.unlockSMBus();

            if (wordArray != null && wordArray.Length == 10)
            {
                var temp = BitConverter.GetBytes(wordArray[5]);
                temp[1] = (byte)(temp[1] & 0x0F);

                ushort count = BitConverter.ToUInt16(temp, 0);
                double value = Math.Round(count * 0.0625f);
                if(value > 0)
                {
                    sensor.Value = (int)value;
                }
            }
            return;
        }

        private int onGetNvAPITemperature(int index)
        {
            this.lockBus();
            int temp = 0;
            var gpuArray = PhysicalGPU.GetPhysicalGPUs();
            if (index >= gpuArray.Length)
            {
                this.unlockBus();
                return temp;
            }
            
            var e = gpuArray[index].ThermalInformation.ThermalSensors.GetEnumerator();
            while(e.MoveNext())
            {
                var value = e.Current;
                temp = value.CurrentTemperature;
                break;
            }
            this.unlockBus();
            return temp;
        }

        private int onGetNvAPIFanSpeed(int index, int coolerID)
        {
            this.lockBus();
            int speed = 0;
            var gpuArray = PhysicalGPU.GetPhysicalGPUs();
            if (index >= gpuArray.Length)
            {
                this.unlockBus();
                return speed;
            }

            var e = gpuArray[index].CoolerInformation.Coolers.GetEnumerator();
            while (e.MoveNext())
            {
                var value = e.Current;
                if(value.CoolerId == coolerID)
                {
                    speed = value.CurrentFanSpeedInRPM;
                    break;
                }
            }
            this.unlockBus();
            return speed;
        }

        private void onSetNvApiControl(int index, int coolerID, int value)
        {
            this.lockBus();
            var gpuArray = PhysicalGPU.GetPhysicalGPUs();
            if (index >= gpuArray.Length)
            {
                this.unlockBus();
                return;
            }
            var info = gpuArray[index].CoolerInformation;
            info.SetCoolerSettings(coolerID, value);
            this.unlockBus();
        }

        private void onUpdateThread()
        {
            long startTime = Util.getNowMS();
            while (mUpdateThreadState == true)
            {
                if (Monitor.TryEnter(mLock) == false)
                {
                    Thread.Sleep(10);
                    continue;
                }

                long nowTime = Util.getNowMS();
                if(nowTime - startTime < mUpdateInterval)
                {
                    Monitor.Exit(mLock);
                    Thread.Sleep(10);
                    continue;
                }
                startTime = nowTime;

                if (mIsGigabyte == true && mGigabyte != null)
                {
                    mGigabyte.update();
                }

                if (mLHM != null)
                {
                    mLHM.update();
                }

                if (mOHM != null)
                {
                    mOHM.update();
                }

                for (int i = 0; i < mSensorList.Count; i++)
                {
                    mSensorList[i].update();
                }

                for (int i = 0; i < mFanList.Count; i++)
                {
                    mFanList[i].update();
                }

                for (int i = 0; i < mControlList.Count; i++)
                {
                    mControlList[i].update();
                }

                // change value
                bool isExistChange = false;
                if (mChangeValueList.Count > 0)
                {
                    for (int i = 0; i < mChangeControlList.Count; i++)
                    {
                        isExistChange = true;
                        mChangeControlList[i].setSpeed(mChangeValueList[i]);
                    }
                    mChangeControlList.Clear();
                    mChangeValueList.Clear();
                }

                // Control
                var controlManager = ControlManager.getInstance();
                if (controlManager.IsEnable == true && isExistChange == false)
                {
                    var controlDictionary = new Dictionary<int, BaseControl>();
                    int modeIndex = controlManager.ModeIndex;

                    for (int i = 0; i < controlManager.getControlDataCount(modeIndex); i++)
                    {
                        var controlData = controlManager.getControlData(modeIndex, i);
                        if (controlData == null)
                            break;

                        int sensorIndex = controlData.Index;
                        int temperature = mSensorList[sensorIndex].Value;

                        for (int j = 0; j < controlData.FanDataList.Count; j++)
                        {
                            var fanData = controlData.FanDataList[j];
                            int controlIndex = fanData.Index;
                            int percent = fanData.getValue(temperature);

                            var control = mControlList[controlIndex];

                            if (controlDictionary.ContainsKey(controlIndex) == false)
                            {
                                controlDictionary[controlIndex] = control;
                                control.NextValue = percent;
                            }
                            else
                            {
                                control.NextValue = (control.NextValue >= percent) ? control.NextValue : percent;
                            }
                        }
                    }

                    foreach (var keyPair in controlDictionary)
                    {
                        var control = keyPair.Value;
                        if (control.Value == control.NextValue)
                            continue;
                        control.setSpeed(control.NextValue);
                    }
                }

                // onUpdateCallback
                onUpdateCallback();

                Monitor.Exit(mLock);
            }
        }

        public int addChangeValue(int value, BaseControl control)
        {
            Monitor.Enter(mLock);
            if (value < control.getMinSpeed())
            {
                value = control.getMinSpeed();
            }
            else if(value > control.getMaxSpeed())
            {
                value = control.getMaxSpeed();
            }
            mChangeValueList.Add(value);
            mChangeControlList.Add(control);
            Monitor.Exit(mLock);
            return value;
        }        

        public int getSensorCount()
        {
            Monitor.Enter(mLock);
            int count = mSensorList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public BaseSensor getSensor(int index)
        {
            Monitor.Enter(mLock);
            if (index >= mSensorList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }
            var sensor = mSensorList[index];
            Monitor.Exit(mLock);
            return sensor;
        }

        public int getFanCount()
        {
            Monitor.Enter(mLock);
            int count = mFanList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public BaseSensor getFan(int index)
        {
            Monitor.Enter(mLock);
            if (index >= mFanList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }
            var fan = mFanList[index];
            Monitor.Exit(mLock);
            return fan;
        }

        public int getControlCount()
        {
            Monitor.Enter(mLock);
            int count = mControlList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public BaseControl getControl(int index)
        {
            Monitor.Enter(mLock);
            if (index >= mControlList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }
            var control = mControlList[index];
            Monitor.Exit(mLock);
            return control;
        }
    }
}
