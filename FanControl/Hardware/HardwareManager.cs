using System;
using System.Collections.Generic;
using System.Timers;
using System.Threading;
using LibreHardwareMonitor.Hardware;
using NZXTSharp.KrakenX;
using Gigabyte.Engine.EnvironmentControl.CoolingDevice.Fan;
using Gigabyte.Engine.ServiceProcess;
using Gigabyte.EnvironmentControl.Common.HardwareMonitor;
using Gigabyte.Engine.EnvironmentControl.HardwareMonitor;
using Gigabyte.Engine.GraphicsCard.ServiceProcess;
using Gigabyte.Engine.GraphicsCard;
using Gigabyte.Engine.GraphicsCard.Nvidia;
using Gigabyte.Engine.GraphicsCard.Amd;
using System.Security.AccessControl;
using System.Security.Principal;
using Gigabyte.EnvironmentControl.Common.CoolingDevice.Fan;
using System.Collections;

namespace FanControl
{
    public class HardwareManager : IVisitor
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
        private Mutex mECMutex = null;
        private Mutex mAPICMutex = null;

        // LibreHardwareMonitorLib
        private Computer mComputer = null;
        private List<IHardware> mHardwareList = new List<IHardware>();

        // NZXT Kraken
        private KrakenX mKrakenX = null;
        public KrakenX getKrakenX() { return mKrakenX; }

        // Gigabyte
        private bool mIsGigabyte = false;
        private SmartGuardianFanControlModule mGigabyteSmartGuardianFanControlModule = null;
        private HardwareMonitorControlModule mGigabyteHardwareMonitorControlModule = null;
        private GraphicsCardControlModule mGigabyteGraphicsCardControlModule = null;

        private List<NvidiaGeforceGraphicsModule> mGigabyteNvidiaGeforceGraphicsModuleList = new List<NvidiaGeforceGraphicsModule>();
        private List<AmdRadeonGraphicsModule> mGigabyteAmdRadeonGraphicsModuleList = new List<AmdRadeonGraphicsModule>();

        private List<float> mGigabyteTemperatureList = new List<float>();
        private List<float> mGigabyteFanSpeedList = new List<float>();

        public List<float> GigabyteTemperatureList { get { return mGigabyteTemperatureList; } }
        public List<float> GigabyteFanSpeedList { get { return mGigabyteFanSpeedList; }}

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
        public int UpdateInterval { get; set; }
        private bool mUpdateThreadState = false;
        private Thread mUpdateThread = null;
        private long mUpdateTime = 0;

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

            // createGigabyte
            this.createGigabyte();

            ////////////////////////// LibreHardwareMonitor //////////////////////////
            if(mIsGigabyte == false)
            {
                string mutexName = "Global\\Access_SMBUS.HTP.Method";
                this.createMutex(mutexName, ref mSMBusMutex);

                mutexName = "Global\\Access_PCI";
                this.createMutex(mutexName, ref mPCIMutex);

                mutexName = "Global\\Access_EC";
                this.createMutex(mutexName, ref mECMutex);

                mutexName = "Global\\Access_APIC_Clk_Measure";
                this.createMutex(mutexName, ref mAPICMutex);

                mComputer = new Computer();
                mComputer.IsCpuEnabled = true;
                mComputer.IsGpuEnabled = true;
                mComputer.IsControllerEnabled = true;
                mComputer.IsMotherboardEnabled = true;

                mComputer.Open();
                mComputer.Accept(this);
            }

            this.createTemp();
            this.createFan();
            this.createControl();

            ////////////////////////// NZXT Kraken //////////////////////////
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

            UpdateInterval = OptionManager.getInstance().Interval;
            mUpdateThreadState = true;
            mUpdateThread = new Thread(new ThreadStart(onUpdateThread));
            mUpdateThread.Start();

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

            if (mComputer != null)
            {
                mComputer.Close();
                mComputer = null;
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

            this.destroyGigabyte();

            mGigabyteTemperatureList.Clear();
            mGigabyteFanSpeedList.Clear();
            mSensorList.Clear();
            mFanList.Clear();
            mControlList.Clear();

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

            if (mECMutex != null)
            {
                mECMutex.Close();
                mECMutex = null;
            }

            if (mAPICMutex != null)
            {
                mAPICMutex.Close();
                mAPICMutex = null;
            }

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
            UpdateInterval = interval;
            Monitor.Exit(mLock);
        }
        
        private void createGigabyte()
        {
            try
            {
                string mutexName = "Global\\Access_ISABUS.HTP.Method";
                this.createMutex(mutexName, ref mISABusMutex);

                mutexName = "Global\\Access_SMBUS.HTP.Method";
                this.createMutex(mutexName, ref mSMBusMutex);

                mutexName = "Global\\Access_PCI";
                this.createMutex(mutexName, ref mPCIMutex);

                mutexName = "Global\\Access_EC";
                this.createMutex(mutexName, ref mECMutex);

                mutexName = "Global\\Access_APIC_Clk_Measure";
                this.createMutex(mutexName, ref mAPICMutex);

                var controller = new EngineServiceController("EasyTuneEngineService");
                if(controller.IsInstall() == false)
                {
                    controller.Dispose();
                    this.destroyGigabyte();
                    return;
                }

                if (controller.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    controller.Start();
                }
                controller.Dispose();

                var management = new GraphicsCardServiceManagement();
                if(management.IsProcessExist() == false)
                {
                    this.destroyGigabyte();
                    return;
                }

                mGigabyteGraphicsCardControlModule = new GraphicsCardControlModule();
                if(mGigabyteGraphicsCardControlModule.AmdGpuCount > 0)
                {
                    mGigabyteGraphicsCardControlModule.GetObjects(ref mGigabyteAmdRadeonGraphicsModuleList);                    
                }
                if (mGigabyteGraphicsCardControlModule.NvidiaGpuCount > 0)
                {
                    mGigabyteGraphicsCardControlModule.GetObjects(ref mGigabyteNvidiaGeforceGraphicsModuleList);
                }

                mGigabyteHardwareMonitorControlModule = new HardwareMonitorControlModule();
                mGigabyteHardwareMonitorControlModule.Initialize(HardwareMonitorSourceTypes.HwRegister);
                
                mGigabyteSmartGuardianFanControlModule = new SmartGuardianFanControlModule();
                var temperatureList = new List<float>();
                mGigabyteSmartGuardianFanControlModule.GetHardwareMonitorDatas(ref temperatureList, ref mGigabyteFanSpeedList);

                mIsGigabyte = true;
            }
            catch
            {
                this.destroyGigabyte();
            }
        }

        private void destroyGigabyte()
        {
            for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
            {
                mGigabyteAmdRadeonGraphicsModuleList[i].Dispose();
            }
            mGigabyteAmdRadeonGraphicsModuleList.Clear();

            for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
            {
                mGigabyteNvidiaGeforceGraphicsModuleList[i].Dispose();
            }
            mGigabyteNvidiaGeforceGraphicsModuleList.Clear();

            if (mGigabyteSmartGuardianFanControlModule != null)
            {
                mGigabyteSmartGuardianFanControlModule.Dispose();
                mGigabyteSmartGuardianFanControlModule = null;
            }
            if (mGigabyteHardwareMonitorControlModule != null)
            {
                mGigabyteHardwareMonitorControlModule.Dispose();
                mGigabyteHardwareMonitorControlModule = null;
            }
            if(mGigabyteGraphicsCardControlModule != null)
            {
                mGigabyteGraphicsCardControlModule.Dispose();
                mGigabyteGraphicsCardControlModule = null;
            }

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

            if (mECMutex != null)
            {
                mECMutex.Close();
                mECMutex = null;
            }

            if (mAPICMutex != null)
            {
                mAPICMutex.Close();
                mAPICMutex = null;
            }

            mIsGigabyte = false;
        }

        private void createMutex(string mutexName, ref Mutex mutex)
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

        private void createTemp()
        {
            if(mIsGigabyte == true)
            {
                var pHwMonitoredDataList = new HardwareMonitoredDataCollection();
                mGigabyteHardwareMonitorControlModule.GetCurrentMonitoredData(SensorTypes.Temperature, ref pHwMonitoredDataList);

                int num = 2;
                for(int i = 0; i < pHwMonitoredDataList.Count; i++)
                {
                    string name = pHwMonitoredDataList[i].Title;
                    if (this.isExistTemp(name) == true)
                    {
                        name = name + " #" + num++;
                    }
                    var sensor = new GigabyteTemp(name, i);
                    mSensorList.Add(sensor);

                    mGigabyteTemperatureList.Add(pHwMonitoredDataList[i].Value);
                }

                num = 2;
                for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
                {
                    string name = mGigabyteAmdRadeonGraphicsModuleList[i].ProductName;
                    if (this.isExistTemp(name) == true)
                    {
                        name = name + " #" + num++;
                    }
                    var sensor = new GigabyteAmdGpuTemp(name, mGigabyteAmdRadeonGraphicsModuleList[i]);
                    mSensorList.Add(sensor);
                }

                num = 2;
                for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
                {
                    string name = mGigabyteNvidiaGeforceGraphicsModuleList[i].ProductName;
                    if (this.isExistTemp(name) == true)
                    {
                        name = name + " #" + num++;
                    }
                    var sensor = new GigabyteNvidiaGpuTemp(name, mGigabyteNvidiaGeforceGraphicsModuleList[i]);
                    mSensorList.Add(sensor);
                }
                return;
            }

            int cpuNum = 2;
            int gpuAmdNum = 2;
            int gpuNvidiaNum = 2;
            var hardwareList = mComputer.Hardware;
            for (int i = 0; i < hardwareList.Length; i++)
            {
                if (hardwareList[i].HardwareType == HardwareType.Cpu)
                {
                    mHardwareList.Add(hardwareList[i]);

                    string name = hardwareList[i].Name;
                    if (this.isExistTemp(name) == true)
                    {
                        name = name + " #" + cpuNum++;
                    }
                    var sensor = new HardwareTemp(hardwareList[i], name);
                    mSensorList.Add(sensor);                    
                }

                else if (hardwareList[i].HardwareType == HardwareType.GpuAmd)
                {
                    mHardwareList.Add(hardwareList[i]);

                    string name = hardwareList[i].Name;
                    if (this.isExistTemp(name) == true)
                    {
                        name = name + " #" + gpuAmdNum++;
                    }
                    var sensor = new HardwareTemp(hardwareList[i], name);
                    mSensorList.Add(sensor);
                }

                else if (hardwareList[i].HardwareType == HardwareType.GpuNvidia)
                {
                    mHardwareList.Add(hardwareList[i]);

                    string name = hardwareList[i].Name;
                    if (this.isExistTemp(name) == true)
                    {
                        name = name + " #" + gpuNvidiaNum++;
                    }
                    var sensor = new HardwareTemp(hardwareList[i], name);
                    mSensorList.Add(sensor);
                }
            }
        }

        private void createFan()
        {
            if (mIsGigabyte == true)
            {
                int num = 2;
                for (int i = 0; i < mGigabyteSmartGuardianFanControlModule.FanControlCount; i++)
                {
                    string name;
                    mGigabyteSmartGuardianFanControlModule.GetFanControlTitle(i, out name);
                    if (name.Equals("PCH") == true)
                        continue;

                    if (this.isExistFan(name) == true)
                    {
                        name = name + " #" + num++;
                    }
                    var fan = new GigabyteFanSpeed(name, i);
                    mFanList.Add(fan);
                }

                int gpuNum = 1;
                for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
                {
                    var fan = new GigabyteAmdGpuFanSpeed(mGigabyteAmdRadeonGraphicsModuleList[i], gpuNum++);
                    mFanList.Add(fan);
                }
                for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
                {
                    var fan = new GigabyteNvidiaFanSpeed(mGigabyteNvidiaGeforceGraphicsModuleList[i], gpuNum++);
                    mFanList.Add(fan);
                }
                return;
            }

            int fanNum = 1;
            int otherFanNum = 2;
            var hardwareList = mComputer.Hardware;
            for (int i = 0; i < hardwareList.Length; i++)
            {
                bool isExist = false;
                var sensorList = hardwareList[i].Sensors;
                for (int j = 0; j < sensorList.Length; j++)
                {
                    if (sensorList[j].SensorType != LibreHardwareMonitor.Hardware.SensorType.Fan)
                        continue;

                    isExist = true;

                    if (hardwareList[i].HardwareType == HardwareType.SuperIO)
                    {
                        var fan = new HardwareFanSpeed(sensorList[j], "Fan #" + fanNum++);
                        mFanList.Add(fan);
                    }
                    else
                    {
                        string name = sensorList[j].Name;
                        if (this.isExistFan(name) == true)
                        {
                            name = name + " #" + otherFanNum++;
                        }
                        var fan = new HardwareFanSpeed(sensorList[j], name);
                        mFanList.Add(fan);
                    }
                }
                if(isExist == true)
                {
                    mHardwareList.Add(hardwareList[i]);
                }

                var subHardwareList = hardwareList[i].SubHardware;
                for (int j = 0; j < subHardwareList.Length; j++)
                {
                    bool isExist2 = false;
                    var subSensorList = subHardwareList[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType != LibreHardwareMonitor.Hardware.SensorType.Fan)
                            continue;

                        isExist2 = true;

                        if (subHardwareList[j].HardwareType == HardwareType.SuperIO)
                        {
                            var fan = new HardwareFanSpeed(subSensorList[k], "Fan #" + fanNum++);
                            mFanList.Add(fan);
                        }
                        else
                        {
                            string name = subSensorList[k].Name;
                            if (this.isExistFan(name) == true)
                            {
                                name = name + " #" + otherFanNum++;
                            }
                            var fan = new HardwareFanSpeed(subSensorList[k], name);
                            mFanList.Add(fan);
                        }
                    }
                    if(isExist2 == true)
                    {
                        mHardwareList.Add(subHardwareList[j]);
                    }
                }
            }
        }        

        private void createControl()
        {
            if (mIsGigabyte == true)
            {
                int num = 2;
                for (int i = 0; i < mGigabyteSmartGuardianFanControlModule.FanControlCount; i++)
                {
                    string name;
                    mGigabyteSmartGuardianFanControlModule.GetFanControlTitle(i, out name);
                    if (name.Equals("PCH") == true)
                        continue;

                    if (this.isExistControl(name) == true)
                    {
                        name = name + " #" + num++;
                    }

                    var config = new SmartFanControlConfig();
                    mGigabyteSmartGuardianFanControlModule.Get(i, ref config);

                    double pwm = (double)config.FanConfig.StartPWM;
                    int value = (int)Math.Round(pwm / 255.0f * 100.0f);                    

                    var control = new GigabyteFanControl(name, i, value);
                    control.onSetSpeedCallback += onSetGigabyteFanSpeed;
                    mControlList.Add(control);
                }

                int gpuNum = 1;
                for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
                {
                    var control = new GigabyteAmdGpuFanControl(mGigabyteAmdRadeonGraphicsModuleList[i], gpuNum++);
                    mControlList.Add(control);

                    this.addChangeValue(control.getMinSpeed(), control);
                }
                for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
                {
                    var control = new GigabyteNvidiaGpuFanControl(mGigabyteNvidiaGeforceGraphicsModuleList[i], gpuNum++);
                    mControlList.Add(control);

                    this.addChangeValue(control.getMinSpeed(), control);
                }
                return;
            }

            int fanNum = 1;
            int otherFanNum = 2;
            var hardwareList = mComputer.Hardware;
            for (int i = 0; i < hardwareList.Length; i++)
            {
                bool isExist = false;
                var sensorList = hardwareList[i].Sensors;
                for (int j = 0; j < sensorList.Length; j++)
                {
                    if (sensorList[j].SensorType != LibreHardwareMonitor.Hardware.SensorType.Control || sensorList[j].Control == null)
                        continue;

                    isExist = true;

                    if (hardwareList[i].HardwareType == HardwareType.SuperIO)
                    {
                        var control = new HardwareControl(sensorList[j], "Fan Control #" + fanNum++);
                        mControlList.Add(control);
                    }
                    else
                    {
                        string name = sensorList[j].Name;
                        if (this.isExistControl(name) == true)
                        {
                            name = name + " #" + otherFanNum++;
                        }
                        var control = new HardwareControl(sensorList[j], name);
                        mControlList.Add(control);
                    }
                }
                if(isExist == true)
                {
                    mHardwareList.Add(hardwareList[i]);
                }

                var subHardwareList = hardwareList[i].SubHardware;
                for (int j = 0; j < subHardwareList.Length; j++)
                {
                    bool isExist2 = false;
                    var subSensorList = subHardwareList[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType != LibreHardwareMonitor.Hardware.SensorType.Control || subSensorList[k].Control == null)
                            continue;

                        isExist2 = true;

                        if (subHardwareList[j].HardwareType == HardwareType.SuperIO)
                        {
                            var control = new HardwareControl(subSensorList[k], "Fan Control #" + fanNum++);
                            mControlList.Add(control);
                        }
                        else
                        {
                            string name = subSensorList[k].Name;
                            if (this.isExistControl(name) == true)
                            {
                                name = name + " #" + otherFanNum++;
                            }
                            var control = new HardwareControl(subSensorList[k], name);
                            mControlList.Add(control);
                        }
                    }
                    if(isExist2 == true)
                    {
                        mHardwareList.Add(subHardwareList[j]);
                    }
                }
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

        private void onSetGigabyteFanSpeed(int index, int value)
        {
            mGigabyteSmartGuardianFanControlModule.SetCalibrationPwm(index, value);
        }

        private void onUpdateThread()
        {
            while(true)
            {
                if(Monitor.TryEnter(mLock) == false)
                {
                    if (mUpdateThreadState == false)
                        break;

                    Thread.Sleep(100);
                    continue;
                }

                long nowTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                if (mUpdateTime == 0 || nowTime - mUpdateTime >= UpdateInterval)
                {
                    mUpdateTime = nowTime;
                }
                else
                {
                    Monitor.Exit(mLock);
                    Thread.Sleep(100);
                    continue;
                }

                if (mIsGigabyte == true)
                {
                    var tempDataList = new HardwareMonitoredDataCollection();
                    mGigabyteHardwareMonitorControlModule.GetCurrentMonitoredData(SensorTypes.Temperature, ref tempDataList);
                    for (int i = 0; i < tempDataList.Count; i++)
                    {
                        mGigabyteTemperatureList[i] = tempDataList[i].Value;
                    }

                    var fanDataList = new HardwareMonitoredDataCollection();
                    mGigabyteHardwareMonitorControlModule.GetCurrentMonitoredData(SensorTypes.Fan, ref fanDataList);

                    for(int i = 0; i < fanDataList.Count; i++)
                    {
                        mGigabyteFanSpeedList[i] = fanDataList[i].Value;
                    }
                }

                for (int i = 0; i < mHardwareList.Count; i++)
                {
                    mHardwareList[i].Update();
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
                        Thread.Sleep(100);
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

                    foreach(var keyPair in controlDictionary)
                    {
                        var control = keyPair.Value;
                        if (control.Value == control.NextValue)
                            continue;
                        control.setSpeed(control.NextValue);
                        Thread.Sleep(10);
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
        
        /////////////////////////// Visitor ///////////////////////////
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }

        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware)
                subHardware.Accept(this);
        }

        public void VisitSensor(ISensor sensor) { }

        public void VisitParameter(IParameter parameter){}
    }
}
