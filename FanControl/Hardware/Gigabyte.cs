using System;
using System.Collections.Generic;
using System.Threading;
using Gigabyte.Engine.EnvironmentControl.CoolingDevice.Fan;
using Gigabyte.Engine.ServiceProcess;
using Gigabyte.EnvironmentControl.Common.HardwareMonitor;
using Gigabyte.Engine.EnvironmentControl.HardwareMonitor;
using Gigabyte.Engine.GraphicsCard.ServiceProcess;
using Gigabyte.Engine.GraphicsCard;
using Gigabyte.Engine.GraphicsCard.Nvidia;
using Gigabyte.Engine.GraphicsCard.Amd;
using Gigabyte.EnvironmentControl.Common.CoolingDevice.Fan;
using Gigabyte.GraphicsCard.Common;

namespace FanControl
{
    public class Gigabyte
    {
        public delegate void LockBusHandler();
        public delegate int AddChangeValueHandler(int value, BaseControl control);

        private SmartGuardianFanControlModule mGigabyteSmartGuardianFanControlModule = null;
        private HardwareMonitorControlModule mGigabyteHardwareMonitorControlModule = null;
        private GraphicsCardControlModule mGigabyteGraphicsCardControlModule = null;

        private List<NvidiaGeforceGraphicsModule> mGigabyteNvidiaGeforceGraphicsModuleList = new List<NvidiaGeforceGraphicsModule>();
        private List<AmdRadeonGraphicsModule> mGigabyteAmdRadeonGraphicsModuleList = new List<AmdRadeonGraphicsModule>();

        private List<float> mGigabyteTemperatureList = new List<float>();
        private List<float> mGigabyteFanSpeedList = new List<float>();

        public event LockBusHandler LockBus;
        public event LockBusHandler UnlockBus;
        public event AddChangeValueHandler AddChangeValue;

        public Gigabyte() { }
        
        public bool start()
        {
            bool isNvAPIWrapper = OptionManager.getInstance().IsNvAPIWrapper;            
            try
            {
                var controller = new EngineServiceController("EasyTuneEngineService");
                if(controller.IsInstall() == false)
                {
                    controller.Dispose();
                    this.stop();
                    return false;
                }

                if (controller.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    controller.Start();
                }
                controller.Dispose();

                this.lockBus();

                mGigabyteHardwareMonitorControlModule = new HardwareMonitorControlModule();
                mGigabyteHardwareMonitorControlModule.Initialize(HardwareMonitorSourceTypes.HwRegister);

                mGigabyteSmartGuardianFanControlModule = new SmartGuardianFanControlModule();
                var temperatureList = new List<float>();
                mGigabyteSmartGuardianFanControlModule.GetHardwareMonitorDatas(ref temperatureList, ref mGigabyteFanSpeedList);

                var management = new GraphicsCardServiceManagement();
                if (management.IsProcessExist() == true)
                {
                    mGigabyteGraphicsCardControlModule = new GraphicsCardControlModule();
                    if (mGigabyteGraphicsCardControlModule.AmdGpuCount > 0)
                    {
                        mGigabyteGraphicsCardControlModule.GetObjects(ref mGigabyteAmdRadeonGraphicsModuleList);
                    }

                    if (isNvAPIWrapper == false)
                    {
                        if (mGigabyteGraphicsCardControlModule.NvidiaGpuCount > 0)
                        {
                            mGigabyteGraphicsCardControlModule.GetObjects(ref mGigabyteNvidiaGeforceGraphicsModuleList);
                        }
                    }
                }

                this.unlockBus();
                return true;
            }
            catch
            {
                this.stop();
            }
            return false;
        }

        public static void stopService()
        {
            try
            {
                var controller = new EngineServiceController("EasyTuneEngineService");
                if (controller.IsInstall() == false)
                    return;

                if (controller.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    controller.Stop();
                }
                controller.Dispose();
            }
            catch { }            
        }

        public void stop()
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

            mGigabyteTemperatureList.Clear();
            mGigabyteFanSpeedList.Clear();
        }

        private void lockBus()
        {
            LockBus();
        }

        private void unlockBus()
        {
            UnlockBus();
        }

        public void createTemp(ref List<BaseSensor> sensorList)
        {
            bool isNvAPIWrapper = OptionManager.getInstance().IsNvAPIWrapper;

            this.lockBus();
            var pHwMonitoredDataList = new HardwareMonitoredDataCollection();
            mGigabyteHardwareMonitorControlModule.GetCurrentMonitoredData(SensorTypes.Temperature, ref pHwMonitoredDataList);
            this.unlockBus();

            int num = 2;
            for (int i = 0; i < pHwMonitoredDataList.Count; i++)
            {
                string name = pHwMonitoredDataList[i].Title;
                mGigabyteTemperatureList.Add(pHwMonitoredDataList[i].Value);

                while (this.isExistTemp(ref sensorList, name) == true)
                {
                    name = pHwMonitoredDataList[i].Title + " #" + num++;
                }

                var sensor = new GigabyteTemp(name, i);
                sensor.onGetGigabyteTemperatureHandler += onGetGigabyteTemperature;
                sensorList.Add(sensor);                
            }

            num = 2;
            for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
            {
                string name = mGigabyteAmdRadeonGraphicsModuleList[i].ProductName;
                while (this.isExistTemp(ref sensorList, name) == true)
                {
                    name = mGigabyteAmdRadeonGraphicsModuleList[i].ProductName + " #" + num++;
                }

                var sensor = new GigabyteAmdGpuTemp(name, i);
                sensor.onGetGigabyteAmdTemperatureHandler += onGetGigabyteAmdTemperature;
                sensorList.Add(sensor);
            }

            if (isNvAPIWrapper == false)
            {
                num = 2;
                for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
                {
                    string name = mGigabyteNvidiaGeforceGraphicsModuleList[i].ProductName;
                    while (this.isExistTemp(ref sensorList, name) == true)
                    {
                        name = mGigabyteNvidiaGeforceGraphicsModuleList[i].ProductName + " #" + num++;
                    }

                    var sensor = new GigabyteNvidiaGpuTemp(name, i);
                    sensor.onGetGigabyteNvidiaTemperatureHandler += onGetGigabyteNvidiaTemperature;
                    sensorList.Add(sensor);
                }
            }
        }

        public void createFan(ref List<BaseSensor> fanList)
        {
            bool isNvAPIWrapper = OptionManager.getInstance().IsNvAPIWrapper;

            int num = 2;
            for (int i = 0; i < mGigabyteSmartGuardianFanControlModule.FanControlCount; i++)
            {
                string originName;
                mGigabyteSmartGuardianFanControlModule.GetFanControlTitle(i, out originName);
                if (originName.Equals("PCH") == true)
                    continue;

                var name = originName;
                while (this.isExistFan(ref fanList, name) == true)
                {
                    name = originName + " #" + num++;
                }

                var fan = new GigabyteFanSpeed(name, i);
                fan.onGetGigabyteFanSpeedHandler += onGetGigabyteFanSpeed;
                fanList.Add(fan);
            }

            int gpuNum = 1;
            for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
            {
                var name = "GPU Fan #" + gpuNum++;
                while (this.isExistFan(ref fanList, name) == true)
                {
                    name = "GPU Fan #" + gpuNum++;
                }

                var fan = new GigabyteAmdGpuFanSpeed(name, i);
                fan.onGetGigabyteAmdFanSpeedHandler += onGetGigabyteAmdFanSpeed;
                fanList.Add(fan);
            }

            if (isNvAPIWrapper == false)
            {
                for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
                {
                    var name = "GPU Fan #" + gpuNum++;
                    while (this.isExistFan(ref fanList, name) == true)
                    {
                        name = "GPU Fan #" + gpuNum++;
                    }

                    var fan = new GigabyteNvidiaFanSpeed(name, i);
                    fan.onGetGigabyteNvidiaFanSpeedHandler += onGetGigabyteNvidiaFanSpeed;
                    fanList.Add(fan);
                }
            }
        }        

        public void createControl(ref List<BaseControl> controlList)
        {
            bool isNvAPIWrapper = OptionManager.getInstance().IsNvAPIWrapper;

            int num = 2;
            for (int i = 0; i < mGigabyteSmartGuardianFanControlModule.FanControlCount; i++)
            {
                string originName;
                mGigabyteSmartGuardianFanControlModule.GetFanControlTitle(i, out originName);
                if (originName.Equals("PCH") == true)
                    continue;

                var name = originName;
                while (this.isExistControl(ref controlList, name) == true)
                {
                    name = originName + " #" + num++;
                }

                var config = new SmartFanControlConfig();
                mGigabyteSmartGuardianFanControlModule.Get(i, ref config);

                double pwm = (double)config.FanConfig.StartPWM;
                int value = (int)Math.Round(pwm / 255.0f * 100.0f);
                
                var control = new GigabyteFanControl(name, i, value);
                control.onSetGigabyteControlHandler += onSetGigabyteControl;
                controlList.Add(control);
            }

            int gpuNum = 1;
            for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
            {
                this.lockBus();
                var info = new GraphicsFanSpeedInfo();
                mGigabyteAmdRadeonGraphicsModuleList[i].GetFanSpeedInfo(ref info);
                this.unlockBus();

                var name = "GPU Fan #" + gpuNum++;
                while (this.isExistControl(ref controlList, name) == true)
                {
                    name = "GPU Fan #" + gpuNum++;
                }

                var control = new GigabyteAmdGpuFanControl(name, i, info.MinPercent, info.MaxPercent);
                control.onSetGigabyteAmdControlHandler += onSetGigabyteAmdControl;
                controlList.Add(control);

                this.addChangeValue(control.getMinSpeed(), control);
            }

            if (isNvAPIWrapper == false)
            {
                for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
                {
                    this.lockBus();
                    var info = new GraphicsCoolerSetting();
                    mGigabyteNvidiaGeforceGraphicsModuleList[i].GetFanSpeedInfo(ref info);
                    info.Support = true;
                    info.Manual = true;
                    this.unlockBus();

                    int minPercent = (int)Math.Ceiling(info.Config.Minimum);
                    int maxPercent = (int)Math.Ceiling(info.Config.Maximum);

                    var name = "GPU Fan #" + gpuNum++;
                    while (this.isExistControl(ref controlList, name) == true)
                    {
                        name = "GPU Fan #" + gpuNum++;
                    }

                    var control = new GigabyteNvidiaGpuFanControl(name, i, minPercent, maxPercent);
                    control.onSetGigabyteNvidiaControlHandler += onSetGigabyteNvidiaControl;
                    controlList.Add(control);

                    this.addChangeValue(control.getMinSpeed(), control);
                }
            }
        }

        private bool isExistTemp(ref List<BaseSensor> sensorList, string name)
        {
            for (int i = 0; i < sensorList.Count; i++)
            {
                if (sensorList[i].Name.Equals(name) == true)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isExistFan(ref List<BaseSensor> fanList, string name)
        {
            for (int i = 0; i < fanList.Count; i++)
            {
                if (fanList[i].Name.Equals(name) == true)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isExistControl(ref List<BaseControl> controlList, string name)
        {
            for (int i = 0; i < controlList.Count; i++)
            {
                if (controlList[i].Name.Equals(name) == true)
                {
                    return true;
                }
            }
            return false;
        }

        private float onGetGigabyteFanSpeed(int index)
        {
            return mGigabyteFanSpeedList[index];
        }

        private float onGetGigabyteAmdFanSpeed(int index)
        {
            float speed = 0.0f;
            FanSpeedType type = FanSpeedType.RPM;
            this.lockBus();
            try
            {
                mGigabyteAmdRadeonGraphicsModuleList[index].GetFanSpeed(ref speed, ref type);
            }
            catch { }            
            this.unlockBus();
            return speed;
        }

        private float onGetGigabyteNvidiaFanSpeed(int index)
        {
            float speed = 0.0f;
            this.lockBus();
            try
            {
                mGigabyteNvidiaGeforceGraphicsModuleList[index].GetFanSpeed(ref speed);
            }
            catch { }
            this.unlockBus();
            return speed;
        }

        private float onGetGigabyteTemperature(int index)
        {
            return mGigabyteTemperatureList[index];
        }

        private float onGetGigabyteAmdTemperature(int index)
        {
            float temp = 0.0f;
            this.lockBus();
            try
            {
                mGigabyteAmdRadeonGraphicsModuleList[index].GetTemperature(ref temp);
            }
            catch { }
            this.unlockBus();
            return temp;
        }

        private float onGetGigabyteNvidiaTemperature(int index)
        {
            float temp = 0.0f;
            this.lockBus();
            try
            {
                mGigabyteNvidiaGeforceGraphicsModuleList[index].GetTemperature(ref temp);
            }
            catch { }
            this.unlockBus();
            return temp;
        }

        private void onSetGigabyteControl(int index, int value)
        {
            this.lockBus();
            try
            {
                mGigabyteSmartGuardianFanControlModule.SetCalibrationPwm(index, value);
            }
            catch { }
            this.unlockBus();
        }

        private void onSetGigabyteAmdControl(int index, int value)
        {
            this.lockBus();
            try
            {
                mGigabyteAmdRadeonGraphicsModuleList[index].SetFanSpeed(value);
            }
            catch { }
            this.unlockBus();
        }

        private void onSetGigabyteNvidiaControl(int index, int value)
        {
            this.lockBus();
            try
            {
                mGigabyteNvidiaGeforceGraphicsModuleList[index].SetFanSpeed(value);
            }
            catch { }
            this.unlockBus();
        }

        public void update()
        {
            var tempDataList = new HardwareMonitoredDataCollection();
            var fanDataList = new HardwareMonitoredDataCollection();

            this.lockBus();
            try
            {
                mGigabyteHardwareMonitorControlModule.GetCurrentMonitoredData(SensorTypes.Temperature, ref tempDataList);
                mGigabyteHardwareMonitorControlModule.GetCurrentMonitoredData(SensorTypes.Fan, ref fanDataList);
            }
            catch { }
            this.unlockBus();

            for (int i = 0; i < tempDataList.Count; i++)
            {
                mGigabyteTemperatureList[i] = tempDataList[i].Value;
            }

            for (int i = 0; i < fanDataList.Count; i++)
            {
                mGigabyteFanSpeedList[i] = fanDataList[i].Value;
            }
        }

        private int addChangeValue(int value, BaseControl control)
        {
            return AddChangeValue(value, control);
        }
    }
}
