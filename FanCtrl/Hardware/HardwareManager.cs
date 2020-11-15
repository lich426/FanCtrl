//#define MY_DEBUG

using System;
using System.Collections.Generic;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;
using NvAPIWrapper;
using NvAPIWrapper.GPU;
using System.Text;

namespace FanCtrl
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
        private List<Kraken> mKrakenList = new List<Kraken>();
        public List<Kraken> getKrakenList() { return mKrakenList; }

        // EVGA CLC
        private List<CLC> mCLCList = new List<CLC>();
        public List<CLC> getCLCList() { return mCLCList; }

        // NZXT RGB & Fan Controller
        private List<RGBnFC> mRGBnFCList = new List<RGBnFC>();
        public List<RGBnFC> getRGBnFCList() { return mRGBnFCList; }

        // Temperature sensor List
        private List<BaseSensor> mSensorList = new List<BaseSensor>();        

        // Fan List
        private List<BaseSensor> mFanList = new List<BaseSensor>();       

        // Control List
        private List<BaseControl> mControlList = new List<BaseControl>();

        // OSD sensor List
        private List<OSDSensor> mOSDSensorList = new List<OSDSensor>();

        // next tick change value
        private List<int> mChangeValueList = new List<int>();
        private List<BaseControl> mChangeControlList = new List<BaseControl>();

        // update timer
        private System.Timers.Timer mUpdateTimer = new System.Timers.Timer();

        public event UpdateTimerEventHandler onUpdateCallback;
        public delegate void UpdateTimerEventHandler();

#if MY_DEBUG
        private int mDebugUpdateCount = 10;
#endif

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
                try
                {
                    NVIDIA.Initialize();
                }
                catch { }                
            }

            this.createTemp();
            this.createFan();
            this.createControl();

            // NZXT Kraken
            if (OptionManager.getInstance().IsKraken == true)
            {                
                try
                {
                    uint num = 1;

                    // X2
                    uint devCount = HidUSBController.getDeviceCount(USBVendorID.NZXT, USBProductID.KrakenX2);
                    for (uint i = 0; i < devCount; i++)
                    {
                        var kraken = new Kraken();
                        if (kraken.start(i, USBProductID.KrakenX2) == true)
                        {
                            mKrakenList.Add(kraken);

                            var sensor = new KrakenLiquidTemp(kraken, num);
                            mSensorList.Add(sensor);

                            var fan = new KrakenFanSpeed(kraken, num);
                            mFanList.Add(fan);

                            var pump = new KrakenPumpSpeed(kraken, num);
                            mFanList.Add(pump);

                            var fanControl = new KrakenFanControl(kraken, num);
                            mControlList.Add(fanControl);
                            this.addChangeValue(30, fanControl);

                            var pumpControl = new KrakenPumpControl(kraken, num);
                            mControlList.Add(pumpControl);
                            this.addChangeValue(50, pumpControl);

                            num++;
                        }
                    }

                    // X3
                    devCount = HidUSBController.getDeviceCount(USBVendorID.NZXT, USBProductID.KrakenX3);
                    for (uint i = 0; i < devCount; i++)
                    {
                        var kraken = new Kraken();
                        if (kraken.start(i, USBProductID.KrakenX3) == true)
                        {
                            mKrakenList.Add(kraken);

                            var sensor = new KrakenLiquidTemp(kraken, num);
                            mSensorList.Add(sensor);

                            var pump = new KrakenPumpSpeed(kraken, num);
                            mFanList.Add(pump);

                            var pumpControl = new KrakenPumpControl(kraken, num);
                            mControlList.Add(pumpControl);
                            this.addChangeValue(50, pumpControl);

                            num++;
                        }
                    }
                }
                catch { }
            }

            // EVGA CLC
            if (OptionManager.getInstance().IsCLC == true)
            {
                try
                {
                    uint num = 1;
                    uint clcIndex = 0;

                    // SiUSBController
                    uint devCount = SiUSBController.getDeviceCount(USBVendorID.ASETEK, USBProductID.CLC);
                    for (uint i = 0; i < devCount; i++)
                    {
                        var clc = new CLC();
                        if (clc.start(true, clcIndex, i) == true)
                        {
                            mCLCList.Add(clc);

                            var sensor = new CLCLiquidTemp(clc, num);
                            mSensorList.Add(sensor);

                            var fan = new CLCFanSpeed(clc, num);
                            mFanList.Add(fan);

                            var pump = new CLCPumpSpeed(clc, num);
                            mFanList.Add(pump);

                            var fanControl = new CLCFanControl(clc, num);
                            mControlList.Add(fanControl);
                            this.addChangeValue(25, fanControl);

                            var pumpControl = new CLCPumpControl(clc, num);
                            mControlList.Add(pumpControl);
                            this.addChangeValue(50, pumpControl);

                            clcIndex++;
                            num++;
                        }
                    }

                    if (WinUSBController.init() == true)
                    {
                        // WinUSBController
                        devCount = WinUSBController.getDeviceCount(USBVendorID.ASETEK, USBProductID.CLC);
                        for (uint i = 0; i < devCount; i++)
                        {
                            var clc = new CLC();
                            if (clc.start(false, clcIndex, i) == true)
                            {
                                mCLCList.Add(clc);

                                var sensor = new CLCLiquidTemp(clc, num);
                                mSensorList.Add(sensor);

                                var fan = new CLCFanSpeed(clc, num);
                                mFanList.Add(fan);

                                var pump = new CLCPumpSpeed(clc, num);
                                mFanList.Add(pump);

                                var fanControl = new CLCFanControl(clc, num);
                                mControlList.Add(fanControl);
                                this.addChangeValue(25, fanControl);

                                var pumpControl = new CLCPumpControl(clc, num);
                                mControlList.Add(pumpControl);
                                this.addChangeValue(50, pumpControl);

                                clcIndex++;
                                num++;
                            }
                        }
                    }
                }
                catch { }
            }

            if (OptionManager.getInstance().IsRGBnFC == true)
            {
                try
                {
                    uint num = 1;
                    uint devCount = HidUSBController.getDeviceCount(USBVendorID.NZXT, USBProductID.RGBAndFanController);
                    for (uint i = 0; i < devCount; i++)
                    {
                        var rgb = new RGBnFC();
                        if (rgb.start(i) == true)
                        {
                            mRGBnFCList.Add(rgb);

                            for (int j = 0; j < RGBnFC.MAX_FAN_COUNT; j++)
                            {
                                var fan = new RGBnFCFanSpeed(rgb, j, num);
                                mFanList.Add(fan);

                                var control = new RGBnFCControl(rgb, j, num);
                                mControlList.Add(control);
                                this.addChangeValue(control.getMinSpeed(), control);

                                num++;
                            }
                        }
                    }
                }
                catch { }
            }

            // DIMM thermal sensor
            if (OptionManager.getInstance().IsDimm == true)
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

            // osd sensor
            this.createOSDSensor();

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

            mUpdateTimer.Stop();

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

            for (int i = 0; i < mKrakenList.Count; i++)
            {
                try
                {
                    mKrakenList[i].stop();
                }
                catch { }
            }
            mKrakenList.Clear();

            for (int i = 0; i < mCLCList.Count; i++)
            {
                try
                {
                    mCLCList[i].stop();
                }
                catch { }
            }
            mCLCList.Clear();

            for (int i = 0; i < mRGBnFCList.Count; i++)
            {
                try
                {
                    mRGBnFCList[i].stop();
                }
                catch { }
            }
            mRGBnFCList.Clear();

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

            OSDController.release();
            WinUSBController.exit();         

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

            mUpdateTimer.Interval = OptionManager.getInstance().Interval;
            mUpdateTimer.Elapsed += onUpdateTimer;
            mUpdateTimer.Start();

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
            mUpdateTimer.Interval = interval;
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
            try
            {
                mISABusMutex.WaitOne();
            }
            catch { }

            try
            {
                mPCIMutex.WaitOne();
            }
            catch { }            
        }

        private void unlockBus()
        {
            try
            {
                mISABusMutex.ReleaseMutex();
            }
            catch { }
            try
            {
                mPCIMutex.ReleaseMutex();
            }
            catch { }
        }

        private bool lockSMBus(int ms)
        {
            if (ms > 0)
            {
                try
                {
                    return mSMBusMutex.WaitOne(ms);
                }
                catch { }
                return false;
            }
            try
            {
                mSMBusMutex.WaitOne();
                return true;
            }
            catch { }            
            return false;
        }

        private void unlockSMBus()
        {
            try
            {
                mSMBusMutex.ReleaseMutex();
            }
            catch { }
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

        private void createOSDSensor()
        {
            if (mIsGigabyte == true) {}

            // LibreHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
            {
                mLHM.createOSDSensor(ref mOSDSensorList);
            }

            // OpenHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.OpenHardwareMonitor)
            {
                mOHM.createOSDSensor(ref mOSDSensorList);
            }

            if (OptionManager.getInstance().IsNvAPIWrapper == true)
            {
                this.lockBus();
                try
                {
                    var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                    for (int i = 0; i < gpuArray.Length; i++)
                    {
                        int subIndex = 0;

                        var osdSensor = new OSDSensor(OSDUnitType.kHz, "[Clock] GPU Graphics", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);

                        osdSensor = new OSDSensor(OSDUnitType.kHz, "[Clock] GPU Memory", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);

                        osdSensor = new OSDSensor(OSDUnitType.kHz, "[Clock] GPU Processor", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);

                        osdSensor = new OSDSensor(OSDUnitType.kHz, "[Clock] GPU Video Decoding", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);
                        
                        osdSensor = new OSDSensor(OSDUnitType.Percent, "[Load] GPU Core", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);

                        osdSensor = new OSDSensor(OSDUnitType.Percent, "[Load] GPU Frame Buffer", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);

                        osdSensor = new OSDSensor(OSDUnitType.Percent, "[Load] GPU Video Engine", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);

                        osdSensor = new OSDSensor(OSDUnitType.Percent, "[Load] GPU Bus Interface", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);

                        osdSensor = new OSDSensor(OSDUnitType.Percent, "[Load] GPU Memory", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);

                        osdSensor = new OSDSensor(OSDUnitType.KB, "[Data] GPU Memory Free", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);

                        osdSensor = new OSDSensor(OSDUnitType.KB, "[Data] GPU Memory Used", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);

                        osdSensor = new OSDSensor(OSDUnitType.KB, "[Data] GPU Memory Total", i, subIndex++);
                        osdSensor.onOSDSensorUpdate += onOSDSensorUpdate;
                        mOSDSensorList.Add(osdSensor);
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
            try
            {                
                var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                if (index >= gpuArray.Length)
                {
                    this.unlockBus();
                    return temp;
                }

                var e = gpuArray[index].ThermalInformation.ThermalSensors.GetEnumerator();
                while (e.MoveNext())
                {
                    var value = e.Current;
                    temp = value.CurrentTemperature;
                    break;
                }
            }
            catch { }            
            this.unlockBus();
            return temp;
        }

        private int onGetNvAPIFanSpeed(int index, int coolerID)
        {
            this.lockBus();
            int speed = 0;
            try
            {
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
                    if (value.CoolerId == coolerID)
                    {
                        speed = value.CurrentFanSpeedInRPM;
                        break;
                    }
                }
            }
            catch { }
            this.unlockBus();
            return speed;
        }

        private void onSetNvApiControl(int index, int coolerID, int value)
        {
            this.lockBus();
            try
            {
                var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                if (index >= gpuArray.Length)
                {
                    this.unlockBus();
                    return;
                }
                var info = gpuArray[index].CoolerInformation;
                info.SetCoolerSettings(coolerID, value);
            }
            catch { }            
            this.unlockBus();
        }

        private double onOSDSensorUpdate(OSDLibraryType libraryType, int index, int subIndex)
        {
            double value = 0;
            if (libraryType == OSDLibraryType.NvApiWrapper)
            {                
                this.lockBus();
                try
                {
                    var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                    var gpu = gpuArray[index];
                    
                    switch (subIndex)
                    {
                        case 0:
                            value = (double)gpu.CurrentClockFrequencies.GraphicsClock.Frequency;
                            break;
                        case 1:
                            value = (double)gpu.CurrentClockFrequencies.MemoryClock.Frequency;
                            break;
                        case 2:
                            value = (double)gpu.CurrentClockFrequencies.ProcessorClock.Frequency;
                            break;
                        case 3:
                            value = (double)gpu.CurrentClockFrequencies.VideoDecodingClock.Frequency;
                            break;
                        case 4:
                            value = (double)gpu.UsageInformation.GPU.Percentage;
                            break;
                        case 5:
                            value = (double)gpu.UsageInformation.FrameBuffer.Percentage;
                            break;
                        case 6:
                            value = (double)gpu.UsageInformation.VideoEngine.Percentage;
                            break;
                        case 7:
                            value = (double)gpu.UsageInformation.BusInterface.Percentage;
                            break;
                        case 8:
                            value = (double)(((double)gpu.MemoryInformation.PhysicalFrameBufferSizeInkB - (double)gpu.MemoryInformation.CurrentAvailableDedicatedVideoMemoryInkB) / (double)gpu.MemoryInformation.PhysicalFrameBufferSizeInkB * 100.0);
                            break;
                        case 9:
                            value = (double)gpu.MemoryInformation.CurrentAvailableDedicatedVideoMemoryInkB;
                            break;
                        case 10:
                            value = (double)(gpu.MemoryInformation.PhysicalFrameBufferSizeInkB - gpu.MemoryInformation.CurrentAvailableDedicatedVideoMemoryInkB);
                            break;
                        case 11:
                            value = (double)gpu.MemoryInformation.PhysicalFrameBufferSizeInkB;
                            break;
                        default:
                            value = 0;
                            break;
                    }
                }
                catch { }
                this.unlockBus();
            }
            return value;
        }

        private void onUpdateTimer(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(mLock) == false)
                return;

#if MY_DEBUG
            if (ControlManager.getInstance().ModeIndex == 2)
            {
                if (mDebugUpdateCount <= 0)
                {
                    Monitor.Exit(mLock);
                    return;
                }
                mDebugUpdateCount--;
            }
            else
            {
                mDebugUpdateCount = 10;
            }
#endif

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

            var osdManager = OSDManager.getInstance();
            if (osdManager.IsEnable == true)
            {
                var osdHeaderString = "<A0=-5><A1=5><S0=50>\r";

                var osdString = new StringBuilder();
                if (osdManager.IsTime == true)
                {
                    osdString.Append(DateTime.Now.ToString("HH:mm:ss") + "\n");
                }

                int maxNameLength = 0;
                for (int i = 0; i < osdManager.getGroupCount(); i++)
                {
                    var group = osdManager.getGroup(i);
                    if (group == null)
                        break;
                    if (group.Name.Length > maxNameLength)
                        maxNameLength = group.Name.Length;
                }

                for (int i = 0; i < osdManager.getGroupCount(); i++)
                {
                    var group = osdManager.getGroup(i);
                    if (group == null)
                        break;
                    osdString.Append(group.getOSDString(maxNameLength));
                }

                if (osdString.ToString().Length > 0)
                {
                    var sendString = osdHeaderString + osdString.ToString();
                    OSDController.update(sendString);
                    osdManager.IsUpdate = true;
                }
            }
            else
            {
                if (osdManager.IsUpdate == true)
                {
                    OSDController.release();
                    osdManager.IsUpdate = false;
                }
            }

            Monitor.Exit(mLock);
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

        public int getOSDSensorCount()
        {
            Monitor.Enter(mLock);
            int count = mOSDSensorList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public OSDSensor getOSDSensor(int index)
        {
            Monitor.Enter(mLock);
            if (index >= mOSDSensorList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }
            var other = mOSDSensorList[index];
            Monitor.Exit(mLock);
            return other;
        }
    }
}
