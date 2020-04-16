using System;
using System.Collections.Generic;
using System.Timers;
using System.Threading;
using LibreHardwareMonitor.Hardware;
using NZXTSharp.KrakenX;
using Gigabyte.Engine.EnvironmentControl.CoolingDevice.Fan;
using Gigabyte.Engine.ServiceProcess;
using Gigabyte.Engine;

namespace FanControl
{
    public class HardwareManager : IVisitor
    {
        // Singletone
        private HardwareManager() { }
        private static HardwareManager sManager = new HardwareManager();
        public static HardwareManager getInstance() { return sManager; }
        
        // Start state
        private bool mIsStart = false;

        // LibreHardwareMonitorLib
        private Computer mComputer = null;
        private List<IHardware> mHardwareList = new List<IHardware>();

        // NZXT Kraken
        private KrakenX mKrakenX = null;

        // Gigabyte
        private bool mIsGigabyte = false;
        private EngineServiceController mGigabyteEngineServiceController = null;
        private SmartGuardianFanControlModule mGigabyteSmartGuardianFanControlModule = null;

        // Temperature sensor List
        private List<BaseSensor> mSensorList = new List<BaseSensor>();
        public List<BaseSensor> SensorList { get { return mSensorList; }}

        // Fan List
        private List<BaseSensor> mFanList = new List<BaseSensor>();
        public List<BaseSensor> FanList { get { return mFanList; }}

        // Control List
        private List<BaseControl> mControlList = new List<BaseControl>();
        public List<BaseControl> ControlList { get { return mControlList; } }

        // update timer
        private object mUpdateTimerLock = new object();
        private System.Timers.Timer mUpdateTimer = new System.Timers.Timer();
        public event UpdateTimerEventHandler UpdateCallback;

        public delegate void UpdateTimerEventHandler(bool isOK);

        // restart timer
        private System.Timers.Timer mRestartTimer = new System.Timers.Timer();

        public void start()
        {
            if (mIsStart == true)
                return;
            mIsStart = true;

            ////////////////////////// LibreHardwareMonitor //////////////////////////
            mComputer = new Computer();
            mComputer.IsCpuEnabled = true;
            mComputer.IsGpuEnabled = true;
            mComputer.IsControllerEnabled = true;

            // createGigabyte
            this.createGigabyte();

            mComputer.IsMotherboardEnabled = !mIsGigabyte;

            mComputer.Open();
            mComputer.Accept(this);

            this.createTemp();
            this.createFan();
            this.createControl();

            ////////////////////////// NZXT Kraken //////////////////////////
            try
            {
                mKrakenX = new KrakenX();

                NZXTKrakenLiquidTemp sensor = new NZXTKrakenLiquidTemp(mKrakenX);
                mSensorList.Add(sensor);

                NZXTKrakenFanSpeed fan = new NZXTKrakenFanSpeed(mKrakenX);
                mFanList.Add(fan);
                NZXTKrakenPumpSpeed pump = new NZXTKrakenPumpSpeed(mKrakenX);
                mFanList.Add(pump);

                NZXTKrakenFanControl fanControl = new NZXTKrakenFanControl(mKrakenX);
                mControlList.Add(fanControl);
                NZXTKrakenPumpControl pumpControl = new NZXTKrakenPumpControl(mKrakenX);
                mControlList.Add(pumpControl);

                fanControl.setSpeed(25);
                pumpControl.setSpeed(50);
            }
            catch(Exception e){}

            mUpdateTimer.Interval = OptionManager.getInstance().Interval;
            mUpdateTimer.Elapsed += onUpdateTimer;
            mUpdateTimer.Start();
        }

        public void stop()
        {
            if (mIsStart == false)
                return;
            mIsStart = false;           

            Monitor.Enter(mUpdateTimerLock);
            mUpdateTimer.Enabled = false;
            mUpdateTimer.Stop();
            Monitor.Exit(mUpdateTimerLock);

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
            catch (Exception e) { }

            if(mGigabyteEngineServiceController != null)
            {
                mGigabyteEngineServiceController.Dispose();
                mGigabyteEngineServiceController = null;
            }
            if (mGigabyteSmartGuardianFanControlModule != null)
            {
                mGigabyteSmartGuardianFanControlModule.Dispose();
                mGigabyteSmartGuardianFanControlModule = null;
            }
        }

        public void restartTimer(int interval)
        {
            if (mIsStart == false)
                return;

            Monitor.Enter(mUpdateTimerLock);
            mUpdateTimer.Interval = interval;
            Monitor.Exit(mUpdateTimerLock);
        }
        
        private void createGigabyte()
        {
            try
            {
                mGigabyteEngineServiceController = new EngineServiceController("EasyTuneEngineService");
                if(mGigabyteEngineServiceController.IsInstall() == false)
                {
                    mGigabyteEngineServiceController.Dispose();
                    mGigabyteEngineServiceController = null;
                    return;
                }

                if(mGigabyteEngineServiceController.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    mGigabyteEngineServiceController.Start();
                }

                mGigabyteSmartGuardianFanControlModule = new SmartGuardianFanControlModule();
                if (mGigabyteSmartGuardianFanControlModule.IsSupported == false)
                {
                    mGigabyteSmartGuardianFanControlModule.Dispose();
                    mGigabyteSmartGuardianFanControlModule = null;
                    return;
                }

                mIsGigabyte = true;
                return;
            }
            catch(Exception e)
            {
                if (mGigabyteEngineServiceController != null)
                {
                    mGigabyteEngineServiceController.Dispose();
                    mGigabyteEngineServiceController = null;
                }
                if (mGigabyteSmartGuardianFanControlModule != null)
                {
                    mGigabyteSmartGuardianFanControlModule.Dispose();
                    mGigabyteSmartGuardianFanControlModule = null;
                }
            }
        }

        private void createTemp()
        {
            IHardware[] hardwareList = mComputer.Hardware;
            for (int i = 0; i < hardwareList.Length; i++)
            {
                if (hardwareList[i].HardwareType == HardwareType.Cpu ||
                    hardwareList[i].HardwareType == HardwareType.GpuAmd ||
                    hardwareList[i].HardwareType == HardwareType.GpuNvidia)
                {
                    HardwareTemp sensor = new HardwareTemp(hardwareList[i]);
                    mSensorList.Add(sensor);
                    mHardwareList.Add(hardwareList[i]);
                }
            }
        }

        private void createFan()
        {
            if (mIsGigabyte == true)
            {
                if (mGigabyteSmartGuardianFanControlModule != null)
                {
                    for (int i = 0; i < mGigabyteSmartGuardianFanControlModule.FanControlCount; i++)
                    {
                        string name;
                        mGigabyteSmartGuardianFanControlModule.GetFanControlTitle(i, out name);
                        if (name.Equals("PCH") == true)
                            continue;

                        GigabyteFanSpeed fan = new GigabyteFanSpeed(name, i, mGigabyteSmartGuardianFanControlModule);
                        mFanList.Add(fan);
                    }
                }
            }

            IHardware[] hardwareList = mComputer.Hardware;
            for (int i = 0; i < hardwareList.Length; i++)
            {
                bool isExist = false;
                ISensor[] sensorList = hardwareList[i].Sensors;
                for (int j = 0; j < sensorList.Length; j++)
                {
                    if (sensorList[j].SensorType == LibreHardwareMonitor.Hardware.SensorType.Fan)
                    {
                        HardwareFanSpeed fan = new HardwareFanSpeed(sensorList[j]);
                        mFanList.Add(fan);
                        isExist = true;
                    }
                }
                if(isExist == true)
                {
                    mHardwareList.Add(hardwareList[i]);
                }

                IHardware[] subHardwareList = hardwareList[i].SubHardware;
                for (int j = 0; j < subHardwareList.Length; j++)
                {
                    bool isExist2 = false;
                    ISensor[] subSensorList = subHardwareList[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType == LibreHardwareMonitor.Hardware.SensorType.Fan)
                        {
                            HardwareFanSpeed fan = new HardwareFanSpeed(subSensorList[k]);
                            mFanList.Add(fan);
                            isExist2 = true;
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
                if (mGigabyteSmartGuardianFanControlModule != null)
                {
                    for (int i = 0; i < mGigabyteSmartGuardianFanControlModule.FanControlCount; i++)
                    {
                        string name;
                        mGigabyteSmartGuardianFanControlModule.GetFanControlTitle(i, out name);
                        if (name.Equals("PCH") == true)
                            continue;

                        GigabyteFanControl control = new GigabyteFanControl(name, i, mGigabyteSmartGuardianFanControlModule);
                        mControlList.Add(control);
                        control.setSpeed(control.getMinSpeed());
                    }
                }
            }

            IHardware[] hardwareList = mComputer.Hardware;
            for (int i = 0; i < hardwareList.Length; i++)
            {
                bool isExist = false;
                ISensor[] sensorList = hardwareList[i].Sensors;
                for (int j = 0; j < sensorList.Length; j++)
                {
                    if (sensorList[j].SensorType == LibreHardwareMonitor.Hardware.SensorType.Control)
                    {
                        if (sensorList[j].Control != null)
                        {
                            HardwareControl control = new HardwareControl(sensorList[j]);
                            mControlList.Add(control);
                            isExist = true;
                        }
                    }
                }
                if(isExist == true)
                {
                    mHardwareList.Add(hardwareList[i]);
                }

                IHardware[] subHardwareList = hardwareList[i].SubHardware;
                for (int j = 0; j < subHardwareList.Length; j++)
                {
                    bool isExist2 = false;
                    ISensor[] subSensorList = subHardwareList[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType == LibreHardwareMonitor.Hardware.SensorType.Control)
                        {
                            if(subSensorList[k].Control != null)
                            {
                                HardwareControl control = new HardwareControl(subSensorList[k]);
                                mControlList.Add(control);
                                isExist2 = true;
                            }
                        }
                    }
                    if(isExist2 == true)
                    {
                        mHardwareList.Add(subHardwareList[j]);
                    }
                }
            }
        }

        private void onUpdateTimer(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(mUpdateTimerLock) == false)
                return;

            bool isOK = true;
            for (int i = 0; i < mHardwareList.Count; i++)
            {
                mHardwareList[i].Update();
                if (i != 0 && i % 5 == 0)
                    Thread.Sleep(10);
            }

            for (int i = 0; i < mSensorList.Count; i++)
            {
                mSensorList[i].update();
                if(mSensorList[i].IsIncorrectValue == true)
                {
                    isOK = false;
                }
            }

            for (int i = 0; i < mFanList.Count; i++)
            {
                mFanList[i].update();
            }

            for (int i = 0; i < mControlList.Count; i++)
            {
                mControlList[i].update();
            }

            // Control
            var controlManager = ControlManager.getInstance();
            if (controlManager.IsEnable == true)
            {
                for (int i = 0; i < controlManager.Count(); i++)
                {
                    var controlData = controlManager.getControlData(i);
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
                        if (control.Value != percent)
                        {
                            control.setSpeed(percent);
                        }
                    }
                }
            }

            if (UpdateCallback != null)
                UpdateCallback(isOK);

            Monitor.Exit(mUpdateTimerLock);
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
