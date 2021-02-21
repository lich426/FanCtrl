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

namespace FanCtrl
{
    public class Gigabyte
    {
        private const string mIDPrefixTemperature = "Gigabyte/Temp";
        private const string mIDPrefixFan = "Gigabyte/Fan";
        private const string mIDPrefixControl = "Gigabyte/Control";

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

        public Gigabyte() { }
        
        public bool start()
        {
            try
            {
                var controller = new EngineServiceController("EasyTuneEngineService");
                if (controller.IsInstall() == false)
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
            }
            catch { }

            this.lockBus();
            try
            {
                mGigabyteHardwareMonitorControlModule = new HardwareMonitorControlModule();
                mGigabyteHardwareMonitorControlModule.Initialize(HardwareMonitorSourceTypes.HwRegister);

                mGigabyteSmartGuardianFanControlModule = new SmartGuardianFanControlModule();
                var temperatureList = new List<float>();
                mGigabyteSmartGuardianFanControlModule.GetHardwareMonitorDatas(ref temperatureList, ref mGigabyteFanSpeedList);

                if (OptionManager.getInstance().IsGigabyteGpu == true)
                {
                    var management = new GraphicsCardServiceManagement();
                    if (management.IsProcessExist() == true)
                    {
                        mGigabyteGraphicsCardControlModule = new GraphicsCardControlModule();
                        if (mGigabyteGraphicsCardControlModule.AmdGpuCount > 0)
                        {
                            mGigabyteGraphicsCardControlModule.GetObjects(ref mGigabyteAmdRadeonGraphicsModuleList);
                        }

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
                this.unlockBus();
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
            if (OptionManager.getInstance().IsGigabyteGpu == true)
            {
                for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
                {
                    try
                    {
                        mGigabyteAmdRadeonGraphicsModuleList[i].Dispose();
                    }
                    catch { }
                }
                mGigabyteAmdRadeonGraphicsModuleList.Clear();

                for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
                {
                    try
                    {
                        mGigabyteNvidiaGeforceGraphicsModuleList[i].Dispose();
                    }
                    catch { }
                }
                mGigabyteNvidiaGeforceGraphicsModuleList.Clear();
            }

            try
            {
                if (mGigabyteSmartGuardianFanControlModule != null)
                {
                    mGigabyteSmartGuardianFanControlModule.Dispose();
                    mGigabyteSmartGuardianFanControlModule = null;
                }
            }
            catch { }

            try
            {
                if (mGigabyteHardwareMonitorControlModule != null)
                {
                    mGigabyteHardwareMonitorControlModule.Dispose();
                    mGigabyteHardwareMonitorControlModule = null;
                }
            }
            catch { }

            try
            {
                if (mGigabyteGraphicsCardControlModule != null)
                {
                    mGigabyteGraphicsCardControlModule.Dispose();
                    mGigabyteGraphicsCardControlModule = null;
                }
            }
            catch { }

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

        public void createTemp(ref List<HardwareDevice> deviceList)
        {
            var device = new HardwareDevice("Gigabyte");

            if (OptionManager.getInstance().IsGigabyteMotherboard == true)
            {
                this.lockBus();
                try
                {
                    var pHwMonitoredDataList = new HardwareMonitoredDataCollection();
                    mGigabyteHardwareMonitorControlModule.GetCurrentMonitoredData(SensorTypes.Temperature, ref pHwMonitoredDataList);
                    for (int i = 0; i < pHwMonitoredDataList.Count; i++)
                    {
                        string name = pHwMonitoredDataList[i].Title;
                        string id = string.Format("{0}/{1}/{2}", mIDPrefixTemperature, name, pHwMonitoredDataList[i].DeviceUUID);

                        mGigabyteTemperatureList.Add(pHwMonitoredDataList[i].Value);

                        var sensor = new GigabyteTemp(id, name, i);
                        sensor.onGetGigabyteTemperatureHandler += onGetGigabyteTemperature;
                        device.addDevice(sensor);
                    }
                }
                catch { }
                this.unlockBus();
            }

            if (OptionManager.getInstance().IsGigabyteGpu == true)
            {
                for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
                {
                    string name = mGigabyteAmdRadeonGraphicsModuleList[i].ProductName;
                    string id = string.Format("{0}/{1}/{2}", mIDPrefixTemperature, name, i);
                    var sensor = new GigabyteAmdGpuTemp(id, name, i);
                    sensor.onGetGigabyteAmdTemperatureHandler += onGetGigabyteAmdTemperature;
                    device.addDevice(sensor);
                }

                for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
                {
                    string name = mGigabyteNvidiaGeforceGraphicsModuleList[i].ProductName;
                    string id = string.Format("{0}/{1}/{2}", mIDPrefixTemperature, name, i);
                    var sensor = new GigabyteNvidiaGpuTemp(id, name, i);
                    sensor.onGetGigabyteNvidiaTemperatureHandler += onGetGigabyteNvidiaTemperature;
                    device.addDevice(sensor);
                }
            }

            if (device.DeviceList.Count > 0)
            {
                deviceList.Add(device);
            }
        }

        public void createFan(ref List<HardwareDevice> deviceList)
        {
            var device = new HardwareDevice("Gigabyte");

            if (OptionManager.getInstance().IsGigabyteMotherboard == true)
            {
                int num = 1;
                this.lockBus();
                try
                {
                    for (int i = 0; i < mGigabyteSmartGuardianFanControlModule.FanControlCount; i++)
                    {
                        string name;
                        mGigabyteSmartGuardianFanControlModule.GetFanControlTitle(i, out name);
                        if (name.Length == 0)
                        {
                            name = "Fan #" + num++;
                        }

                        string id = string.Format("{0}/{1}/{2}", mIDPrefixFan, name, i);
                        var fan = new GigabyteFanSpeed(id, name, i);
                        fan.onGetGigabyteFanSpeedHandler += onGetGigabyteFanSpeed;
                        device.addDevice(fan);
                    }
                }
                catch { }
                this.unlockBus();
            }

            if (OptionManager.getInstance().IsGigabyteGpu == true)
            {
                int num = 1;
                for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
                {
                    string name = mGigabyteAmdRadeonGraphicsModuleList[i].DisplayName;
                    if (name.Length == 0)
                    {
                        name = "GPU Fan #" + num++;
                    }

                    string id = string.Format("{0}/{1}/{2}", mIDPrefixFan, name, i);
                    var fan = new GigabyteAmdGpuFanSpeed(name, i);
                    fan.onGetGigabyteAmdFanSpeedHandler += onGetGigabyteAmdFanSpeed;
                    device.addDevice(fan);
                }

                for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
                {
                    string name = mGigabyteNvidiaGeforceGraphicsModuleList[i].DisplayName;
                    if (name.Length == 0)
                    {
                        name = "GPU Fan #" + num++;
                    }

                    string id = string.Format("{0}/{1}/{2}", mIDPrefixFan, name, i);
                    var fan = new GigabyteNvidiaFanSpeed(id, name, i);
                    fan.onGetGigabyteNvidiaFanSpeedHandler += onGetGigabyteNvidiaFanSpeed;
                    device.addDevice(fan);
                }
            }                

            if (device.DeviceList.Count > 0)
            {
                deviceList.Add(device);
            }
        }        

        public void createControl(ref List<HardwareDevice> deviceList)
        {
            var device = new HardwareDevice("Gigabyte");

            if (OptionManager.getInstance().IsGigabyteMotherboard == true)
            {
                int num = 1;
                this.lockBus();
                try
                {
                    for (int i = 0; i < mGigabyteSmartGuardianFanControlModule.FanControlCount; i++)
                    {
                        string name;
                        mGigabyteSmartGuardianFanControlModule.GetFanControlTitle(i, out name);
                        if (name.Length == 0)
                        {
                            name = "Fan Control #" + num++;
                        }

                        var config = new SmartFanControlConfig();
                        mGigabyteSmartGuardianFanControlModule.Get(i, ref config);

                        double pwm = (double)config.FanConfig.StartPWM;
                        int value = (int)Math.Round(pwm / 255.0f * 100.0f);

                        string id = string.Format("{0}/{1}/{2}", mIDPrefixControl, name, i);
                        var control = new GigabyteFanControl(id, name, i, value);
                        control.onSetGigabyteControlHandler += onSetGigabyteControl;
                        control.onSetGigabyteControlAutoHandler += onSetGigabyteControlAuto;
                        device.addDevice(control);
                    }
                }
                catch { }
                this.unlockBus();
            }

            if (OptionManager.getInstance().IsGigabyteGpu == true)
            {
                int num = 1;
                for (int i = 0; i < mGigabyteAmdRadeonGraphicsModuleList.Count; i++)
                {
                    var info = new GraphicsFanSpeedInfo();
                    mGigabyteAmdRadeonGraphicsModuleList[i].GetFanSpeedInfo(ref info);

                    string name = mGigabyteAmdRadeonGraphicsModuleList[i].DisplayName;
                    if (name.Length == 0)
                    {
                        name = "GPU Fan Control #" + num++;
                    }

                    string id = string.Format("{0}/{1}/{2}", mIDPrefixControl, name, i);
                    var control = new GigabyteAmdGpuFanControl(id, name, i, info.MinPercent, info.MaxPercent);
                    control.onSetGigabyteAmdControlHandler += onSetGigabyteAmdControl;
                    control.onSetGigabyteAmdControlAutoHandler += onSetGigabyteAmdControlAuto;
                    device.addDevice(control);
                }

                for (int i = 0; i < mGigabyteNvidiaGeforceGraphicsModuleList.Count; i++)
                {
                    var info = new GraphicsCoolerSetting();
                    mGigabyteNvidiaGeforceGraphicsModuleList[i].GetFanSpeedInfo(ref info);
                    info.Support = true;
                    info.Manual = true;

                    int minPercent = (int)Math.Ceiling(info.Config.Minimum);
                    int maxPercent = (int)Math.Ceiling(info.Config.Maximum);

                    string name = mGigabyteNvidiaGeforceGraphicsModuleList[i].DisplayName;
                    if (name.Length == 0)
                    {
                        name = "GPU Fan Control #" + num++;
                    }

                    string id = string.Format("{0}/{1}/{2}", mIDPrefixControl, name, i);
                    var control = new GigabyteNvidiaGpuFanControl(id, name, i, minPercent, maxPercent);
                    control.onSetGigabyteNvidiaControlHandler += onSetGigabyteNvidiaControl;
                    control.onSetGigabyteNvidiaControlAutoHandler += onSetGigabyteNvidiaControlAuto;
                    device.addDevice(control);
                }
            }                

            if (device.DeviceList.Count > 0)
            {
                deviceList.Add(device);
            }
        }

        private float onGetGigabyteFanSpeed(int index)
        {
            try
            {
                return mGigabyteFanSpeedList[index];
            }
            catch { }
            return 0.0f;
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
            try
            {
                return mGigabyteTemperatureList[index];
            }
            catch { }
            return 0.0f;
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

        private void onSetGigabyteControlAuto(int index)
        {
            this.lockBus();
            try
            {
                mGigabyteSmartGuardianFanControlModule.SetFanSpeedControlMode(index, FanSpeedControlModes.Auto);
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

        private void onSetGigabyteAmdControlAuto(int index)
        {
            this.lockBus();
            try
            {
                mGigabyteAmdRadeonGraphicsModuleList[index].SetFanSpeedToDefault();
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

        private void onSetGigabyteNvidiaControlAuto(int index)
        {
            this.lockBus();
            try
            {
                mGigabyteNvidiaGeforceGraphicsModuleList[index].SetFanSpeedToDefault();
            }
            catch { }
            this.unlockBus();
        }

        public void update()
        {
            if (OptionManager.getInstance().IsGigabyteMotherboard == true)
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
        }
    }
}
