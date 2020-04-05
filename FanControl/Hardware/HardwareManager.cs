using System;
using System.Collections.Generic;
using System.Timers;
using System.Threading;
using LibreHardwareMonitor.Hardware;
using NZXTSharp.KrakenX;

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

        // NZXT Kraken
        private KrakenX mKrakenX = null;

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
        private System.Timers.Timer mUpdateTimer = new System.Timers.Timer();
        private readonly object mUpdateTimerLock = new object();

        public void start(int interval)
        {
            if (mIsStart == true)
                return;
            mIsStart = true;

            ////////////////////////// LibreHardwareMonitor //////////////////////////
            mComputer = new Computer();
            mComputer.IsCpuEnabled = true;
            mComputer.IsMotherboardEnabled = true;
            mComputer.IsMemoryEnabled = true;
            mComputer.IsGpuEnabled = true;
            mComputer.IsControllerEnabled = true;
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

            // start update timer
            mUpdateTimer.Interval = interval;
            mUpdateTimer.Elapsed += onUpdateTimer;
            mUpdateTimer.Start();
        }

        public void stop()
        {
            if (mIsStart == false)
                return;
            mIsStart = false;

            Monitor.Enter(mUpdateTimerLock);
            mUpdateTimer.Stop();
            mUpdateTimer.Dispose();
            Monitor.Exit(mUpdateTimerLock);

            if(mComputer != null)
            {
                mComputer.Close();
                mComputer = null;
            }

            mSensorList.Clear();
            mFanList.Clear();
            mControlList.Clear();            

            try
            {
                if (mKrakenX != null)
                {
                    mKrakenX.Dispose();
                    mKrakenX = null;
                }
            }
            catch (Exception e){}
        }

        public void restartTimer(int interval)
        {
            if (mIsStart == false)
                return;
            mUpdateTimer.Interval = interval;
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
                }
            }
        }

        private void createFan()
        {
            IHardware[] hardwareList = mComputer.Hardware;
            for (int i = 0; i < hardwareList.Length; i++)
            {
                ISensor[] sensorList = hardwareList[i].Sensors;
                for (int j = 0; j < sensorList.Length; j++)
                {
                    if (sensorList[j].SensorType == LibreHardwareMonitor.Hardware.SensorType.Fan)
                    {
                        HardwareFanSpeed fan = new HardwareFanSpeed(sensorList[j]);
                        mFanList.Add(fan);
                    }
                }
                IHardware[] subHardwareList = hardwareList[i].SubHardware;
                for (int j = 0; j < subHardwareList.Length; j++)
                {
                    ISensor[] subSensorList = subHardwareList[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType == LibreHardwareMonitor.Hardware.SensorType.Fan)
                        {
                            HardwareFanSpeed fan = new HardwareFanSpeed(subSensorList[k]);
                            mFanList.Add(fan);
                        }
                    }
                }
            }
        }

        private void createControl()
        {
            IHardware[] hardwareList = mComputer.Hardware;
            for (int i = 0; i < hardwareList.Length; i++)
            {
                ISensor[] sensorList = hardwareList[i].Sensors;
                for (int j = 0; j < sensorList.Length; j++)
                {
                    if (sensorList[j].SensorType == LibreHardwareMonitor.Hardware.SensorType.Control)
                    {
                        if (sensorList[j].Control != null)
                        {
                            HardwareControl control = new HardwareControl(sensorList[j]);
                            mControlList.Add(control);
                        }
                    }
                }
                IHardware[] subHardwareList = hardwareList[i].SubHardware;
                for (int j = 0; j < subHardwareList.Length; j++)
                {
                    ISensor[] subSensorList = subHardwareList[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType == LibreHardwareMonitor.Hardware.SensorType.Control)
                        {
                            if(subSensorList[k].Control != null)
                            {
                                HardwareControl control = new HardwareControl(subSensorList[k]);
                                mControlList.Add(control);
                            }
                        }
                    }
                }
            }
        }

        private void onUpdateTimer(object sender, ElapsedEventArgs e)
        {
            if (mIsStart == false)
                return;

            if (Monitor.TryEnter(mUpdateTimerLock))
            {
                mComputer.Accept(this);
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

                // Control
                var controlManager = ControlManager.getInstance();
                if(controlManager.IsEnable == true)
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
                            if(control.Value != percent)
                            {
                                control.setSpeed(percent);
                            }
                        }
                    }
                }
                Monitor.Exit(mUpdateTimerLock);
            }
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
