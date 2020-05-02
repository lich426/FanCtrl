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

namespace FanControl
{
    public class GigabyteManager
    {
        public delegate void LockBusHandler();
        public delegate int AddChangeValueHandler(int value, BaseControl control);

        // Gigabyte
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

        public GigabyteManager() { }

        public bool createGigabyte()
        {
            try
            {
                var controller = new EngineServiceController("EasyTuneEngineService");
                if(controller.IsInstall() == false)
                {
                    controller.Dispose();
                    this.destroyGigabyte();
                    return false;
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
                    return false;
                }

                this.lockBus();

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

                this.unlockBus();
                return true;
            }
            catch
            {
                this.destroyGigabyte();
            }
            return false;
        }

        public void destroyGigabyte()
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
            this.lockBus();
            var pHwMonitoredDataList = new HardwareMonitoredDataCollection();
            mGigabyteHardwareMonitorControlModule.GetCurrentMonitoredData(SensorTypes.Temperature, ref pHwMonitoredDataList);
            this.unlockBus();

            int num = 2;
            for (int i = 0; i < pHwMonitoredDataList.Count; i++)
            {
                string name = pHwMonitoredDataList[i].Title;
                if (this.isExistTemp(ref sensorList, name) == true)
                {
                    name = name + " #" + num++;
                }
                var sensor = new GigabyteTemp(name, i);
                sensor.onGetTemperatureHandler += onGetGigabyteTemperature;
                sensorList.Add(sensor);

                mGigabyteTemperatureList.Add(pHwMonitoredDataList[i].Value);
            }

            num = 2;
            for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
            {
                string name = mGigabyteAmdRadeonGraphicsModuleList[i].ProductName;
                if (this.isExistTemp(ref sensorList, name) == true)
                {
                    name = name + " #" + num++;
                }
                var sensor = new GigabyteAmdGpuTemp(name, mGigabyteAmdRadeonGraphicsModuleList[i]);
                sensorList.Add(sensor);
            }

            num = 2;
            for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
            {
                string name = mGigabyteNvidiaGeforceGraphicsModuleList[i].ProductName;
                if (this.isExistTemp(ref sensorList, name) == true)
                {
                    name = name + " #" + num++;
                }
                var sensor = new GigabyteNvidiaGpuTemp(name, mGigabyteNvidiaGeforceGraphicsModuleList[i]);
                sensorList.Add(sensor);
            }
        }

        public void createFan(ref List<BaseSensor> fanList)
        {
            int num = 2;
            for (int i = 0; i < mGigabyteSmartGuardianFanControlModule.FanControlCount; i++)
            {
                string name;
                mGigabyteSmartGuardianFanControlModule.GetFanControlTitle(i, out name);
                if (name.Equals("PCH") == true)
                    continue;

                if (this.isExistFan(ref fanList, name) == true)
                {
                    name = name + " #" + num++;
                }
                var fan = new GigabyteFanSpeed(name, i);
                fan.onGetFanSpeed += onGetGigabyteFanSpeed;
                fanList.Add(fan);
            }

            int gpuNum = 1;
            for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
            {
                var fan = new GigabyteAmdGpuFanSpeed(mGigabyteAmdRadeonGraphicsModuleList[i], gpuNum++);
                fanList.Add(fan);
            }
            for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
            {
                var fan = new GigabyteNvidiaFanSpeed(mGigabyteNvidiaGeforceGraphicsModuleList[i], gpuNum++);
                fanList.Add(fan);
            }
        }        

        public void createControl(ref List<BaseControl> controlList)
        {
            int num = 2;
            for (int i = 0; i < mGigabyteSmartGuardianFanControlModule.FanControlCount; i++)
            {
                string name;
                mGigabyteSmartGuardianFanControlModule.GetFanControlTitle(i, out name);
                if (name.Equals("PCH") == true)
                    continue;

                if (this.isExistControl(ref controlList, name) == true)
                {
                    name = name + " #" + num++;
                }

                var config = new SmartFanControlConfig();
                mGigabyteSmartGuardianFanControlModule.Get(i, ref config);

                double pwm = (double)config.FanConfig.StartPWM;
                int value = (int)Math.Round(pwm / 255.0f * 100.0f);

                var control = new GigabyteFanControl(name, i, value);
                control.onSetSpeedCallback += onSetGigabyteFanSpeed;
                controlList.Add(control);
            }

            int gpuNum = 1;
            for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
            {
                var control = new GigabyteAmdGpuFanControl(mGigabyteAmdRadeonGraphicsModuleList[i], gpuNum++);
                controlList.Add(control);

                this.addChangeValue(control.getMinSpeed(), control);
            }
            for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
            {
                var control = new GigabyteNvidiaGpuFanControl(mGigabyteNvidiaGeforceGraphicsModuleList[i], gpuNum++);
                controlList.Add(control);

                this.addChangeValue(control.getMinSpeed(), control);
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

        private void onSetGigabyteFanSpeed(int index, int value)
        {
            this.lockBus();
            mGigabyteSmartGuardianFanControlModule.SetCalibrationPwm(index, value);
            this.unlockBus();
        }

        private float onGetGigabyteFanSpeed(int index)
        {
            return mGigabyteFanSpeedList[index];
        }

        private float onGetGigabyteTemperature(int index)
        {
            return mGigabyteTemperatureList[index];
        }

        public void update()
        {
            this.lockBus();
            var tempDataList = new HardwareMonitoredDataCollection();
            mGigabyteHardwareMonitorControlModule.GetCurrentMonitoredData(SensorTypes.Temperature, ref tempDataList);

            var fanDataList = new HardwareMonitoredDataCollection();
            mGigabyteHardwareMonitorControlModule.GetCurrentMonitoredData(SensorTypes.Fan, ref fanDataList);
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
