using System;
using System.Collections.Generic;
using System.Threading;
using NZXTSharp.KrakenX;
using System.Security.AccessControl;
using System.Security.Principal;

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
        private bool mIsBusLock = false;
        private Mutex mISABusMutex = null;
        private Mutex mSMBusMutex = null;
        private Mutex mPCIMutex = null;

        // Gigabyte
        private bool mIsGigabyte = false;
        private GigabyteManager mGigabyteManager = null;

        // LibreHardwareMonitor
        private LHMManager mLHMManager = null;

        // OpenHardwareMonitor
        private OHMManager mOHMManager = null;

        // NZXT Kraken
        private KrakenX mKrakenX = null;
        public KrakenX getKrakenX() { return mKrakenX; }        

        // Temperature sensor List
        private List<BaseSensor> mSensorList = new List<BaseSensor>();        

        // Fan List
        private List<BaseSensor> mFanList = new List<BaseSensor>();       

        // Control List
        private List<BaseControl> mControlList = new List<BaseControl>();

        // next tick change value
        private List<int> mChangeValueList = new List<int>();
        private List<BaseControl> mChangeControlList = new List<BaseControl>();

        // update timer
        private System.Timers.Timer mUpdateTimer = null;

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
            mGigabyteManager = new GigabyteManager();
            mGigabyteManager.AddChangeValue += addChangeValue;
            mGigabyteManager.LockBus += lockBus;
            mGigabyteManager.UnlockBus += unlockBus;

            mIsGigabyte = mGigabyteManager.createGigabyte();
            if (mIsGigabyte == false)
            {
                mGigabyteManager = null;

                // LibreHardwareMonitor
                if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
                {
                    mLHMManager = new LHMManager();
                    mLHMManager.start();
                }

                // OpenHardwareMonitor
                else
                {
                    mOHMManager = new OHMManager();
                    mOHMManager.start();
                }
            }

            this.createTemp();
            this.createFan();
            this.createControl();

            // NZXT Kraken
            try
            {
                try
                {
                    mKrakenX = new KrakenX(NZXTSharp.NZXTDeviceType.KrakenX);
                }
                catch
                {
                    try
                    {
                        mKrakenX = new KrakenX(NZXTSharp.NZXTDeviceType.KrakenX3);
                    }
                    catch
                    {
                        mKrakenX = null;
                    }
                }

                if(mKrakenX != null)
                {
                    var sensor = new NZXTKrakenLiquidTemp(mKrakenX);
                    mSensorList.Add(sensor);

                    if(mKrakenX.Type == NZXTSharp.NZXTDeviceType.KrakenX)
                    {
                        var fan = new NZXTKrakenFanSpeed(mKrakenX);
                        mFanList.Add(fan);
                    }                    

                    var pump = new NZXTKrakenPumpSpeed(mKrakenX);
                    mFanList.Add(pump);

                    if (mKrakenX.Type == NZXTSharp.NZXTDeviceType.KrakenX)
                    {
                        var fanControl = new NZXTKrakenFanControl(mKrakenX);
                        mControlList.Add(fanControl);
                        this.addChangeValue(fanControl.getMinSpeed(), fanControl);
                    }

                    var pumpControl = new NZXTKrakenPumpControl(mKrakenX);
                    mControlList.Add(pumpControl);
                    this.addChangeValue(pumpControl.getMinSpeed(), pumpControl);                    
                }                
            }
            catch
            {
                mKrakenX = null;
            }

            // DIMM thermal sensor
            if (SMBus.open() == true)
            {
                int num = 1;
                var detectBytes = SMBus.i2cDetect();
                if(detectBytes != null)
                {
                    // 0x18 ~ 0x20
                    for (int i = 0; i < detectBytes.Length; i++)
                    {
                        if (i < 24)
                            continue;
                        else if (i > 32)
                            break;

                        if (detectBytes[i] == (byte)i)
                        {
                            var sensor = new DimmTemp("DIMM #" + num++, detectBytes[i]);
                            sensor.onSetDimmTemperature += onSetDimmTemperature;
                            mSensorList.Add(sensor);
                        }
                    }
                }
            }

            // Motherboard temperature
            this.createMotherBoardTemp();

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

            if (mUpdateTimer != null)
            {
                mUpdateTimer.Enabled = false;
                mUpdateTimer.Stop();
                mUpdateTimer = null;
            }

            if (mLHMManager != null)
            {
                mLHMManager.stop();
                mLHMManager = null;
            }

            if (mOHMManager != null)
            {
                mOHMManager.stop();
                mOHMManager = null;
            }

            try
            {
                if (mKrakenX != null)
                {
                    mKrakenX.Dispose();
                    mKrakenX = null;
                }
            }
            catch { }            

            if(mIsGigabyte == true && mGigabyteManager != null)
            {
                mIsGigabyte = false;
                mGigabyteManager.destroyGigabyte();
                mGigabyteManager = null;
            }

            mSensorList.Clear();
            mFanList.Clear();
            mControlList.Clear();

            SMBus.close();

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

            mUpdateTimer = new System.Timers.Timer();
            mUpdateTimer.Interval = OptionManager.getInstance().Interval;
            mUpdateTimer.Elapsed += onUpdate;
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
            mUpdateTimer.Interval = OptionManager.getInstance().Interval;
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
            if (mIsBusLock == true)
                return;
            mISABusMutex.WaitOne();
            mSMBusMutex.WaitOne();
            mPCIMutex.WaitOne();
            mIsBusLock = true;
        }

        private void unlockBus()
        {
            if (mIsBusLock == false)
                return;
            mISABusMutex.ReleaseMutex();
            mSMBusMutex.ReleaseMutex();
            mPCIMutex.ReleaseMutex();
            mIsBusLock = false;
        }

        private void createTemp()
        {
            // Gigabyte
            if (mIsGigabyte == true)
            {
                mGigabyteManager.createTemp(ref mSensorList);
            }

            // LibreHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
            {
                mLHMManager.createTemp(ref mSensorList);
            }

            // OpenHardwareMonitor
            else
            {
                mOHMManager.createTemp(ref mSensorList);
            }
        }

        private void createMotherBoardTemp()
        {
            if (mIsGigabyte == true)
                return;

            // LibreHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
            {
                mLHMManager.createMotherBoardTemp(ref mSensorList);
            }

            // OpenHardwareMonitor
            else
            {
                mOHMManager.createMotherBoardTemp(ref mSensorList);
            }
        }

        private void createFan()
        {
            // Gigabyte
            if (mIsGigabyte == true)
            {
                mGigabyteManager.createFan(ref mFanList);
            }

            // LibreHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
            {
                mLHMManager.createFan(ref mFanList);
            }

            // OpenHardwareMonitor
            else
            {
                mOHMManager.createFan(ref mFanList);
            }
        }        

        private void createControl()
        {
            // Gigabyte
            if (mIsGigabyte == true)
            {
                mGigabyteManager.createControl(ref mControlList);
            }

            // LibreHardwareMonitor
            else if (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor)
            {
                mLHMManager.createControl(ref mControlList);
            }

            // OpenHardwareMonitor
            else
            {
                mOHMManager.createControl(ref mControlList);
            }
        }

        private void onSetDimmTemperature(object sender, byte address)
        {
            var sensor = (DimmTemp)sender;

            this.lockBus();
            var wordArray = SMBus.i2cWordData(address, 10);
            this.unlockBus();

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
        }

        private void onUpdate(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(mLock) == false)
                return;

            if (mIsGigabyte == true && mGigabyteManager != null)
            {
                mGigabyteManager.update();
            }

            if (mLHMManager != null)
            {
                mLHMManager.update();
            }

            if (mOHMManager != null)
            {
                mOHMManager.update();
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
