using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;
using OpenHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class HardwareTemp : BaseSensor
    {
        // IHardware
        private LibreHardwareMonitor.Hardware.IHardware mLHMHardware = null;
        private OpenHardwareMonitor.Hardware.IHardware mOHMHardware = null;

        // SensorList
        private List<LibreHardwareMonitor.Hardware.ISensor> mLHMList = new List<LibreHardwareMonitor.Hardware.ISensor>();
        private List<OpenHardwareMonitor.Hardware.ISensor> mOHMList = new List<OpenHardwareMonitor.Hardware.ISensor>();

        public HardwareTemp(LibreHardwareMonitor.Hardware.IHardware hardware, string name) : base(SENSOR_TYPE.TEMPERATURE)
        {
            mLHMHardware = hardware;
            Name = name;
        }

        public HardwareTemp(OpenHardwareMonitor.Hardware.IHardware hardware, string name) : base(SENSOR_TYPE.TEMPERATURE)
        {
            mOHMHardware = hardware;
            Name = name;
        }

        public void setTemperatureSensor()
        {
            if (mLHMList.Count > 0 || mOHMList.Count > 0)
                return;

            if(mLHMHardware != null)
            {
                LibreHardwareMonitor.Hardware.ISensor packageSensor = null;
                var sensor = mLHMHardware.Sensors;
                for (int i = 0; i < sensor.Length; i++)
                {
                    if (sensor[i].SensorType == LibreHardwareMonitor.Hardware.SensorType.Temperature)
                    {
                        if (mLHMHardware.HardwareType == LibreHardwareMonitor.Hardware.HardwareType.Cpu ||
                            mLHMHardware.HardwareType == LibreHardwareMonitor.Hardware.HardwareType.GpuAmd ||
                            mLHMHardware.HardwareType == LibreHardwareMonitor.Hardware.HardwareType.GpuNvidia)
                        {
                            if (sensor[i].Name.IndexOf("Core") >= 0)
                            {
                                mLHMList.Add(sensor[i]);
                            }

                            if (sensor[i].Name.IndexOf("Package") >= 0)
                            {
                                packageSensor = sensor[i];
                                break;
                            }
                        }
                    }
                }

                if (packageSensor != null)
                {
                    mLHMList.Clear();
                    mLHMList.Add(packageSensor);
                }
            }

            else if (mOHMHardware != null)
            {
                OpenHardwareMonitor.Hardware.ISensor packageSensor = null;
                var sensor = mOHMHardware.Sensors;
                for (int i = 0; i < sensor.Length; i++)
                {
                    if (sensor[i].SensorType == OpenHardwareMonitor.Hardware.SensorType.Temperature)
                    {
                        if (mOHMHardware.HardwareType == OpenHardwareMonitor.Hardware.HardwareType.CPU ||
                            mOHMHardware.HardwareType == OpenHardwareMonitor.Hardware.HardwareType.GpuAti ||
                            mOHMHardware.HardwareType == OpenHardwareMonitor.Hardware.HardwareType.GpuNvidia)
                        {
                            if (sensor[i].Name.IndexOf("Core") >= 0)
                            {
                                mOHMList.Add(sensor[i]);
                            }

                            if (sensor[i].Name.IndexOf("Package") >= 0)
                            {
                                packageSensor = sensor[i];
                                break;
                            }
                        }
                    }
                }

                if (packageSensor != null)
                {
                    mOHMList.Clear();
                    mOHMList.Add(packageSensor);
                }
            }
        }

        public override string getString()
        {
            if (OptionManager.getInstance().IsFahrenheit == true)
                return Util.getFahrenheit(Value) + " °F";
            else
                return Value + " °C";
        }
        public override void update()
        {
            // setTemperatureSensor
            this.setTemperatureSensor();

            double temp = 0.0f;

            if (mLHMHardware != null)
            {
                for (int i = 0; i < mLHMList.Count; i++)
                {
                    temp = temp + ((mLHMList[i].Value.HasValue == true) ? Math.Round((double)mLHMList[i].Value) : 0);
                }

                if (temp > 0.0f)
                {
                    temp = temp / mLHMList.Count;
                    Value = (int)temp;
                }
            }

            else if (mOHMHardware != null)
            {
                for (int i = 0; i < mOHMList.Count; i++)
                {
                    temp = temp + ((mOHMList[i].Value.HasValue == true) ? Math.Round((double)mOHMList[i].Value) : 0);
                }

                if (temp > 0.0f)
                {
                    temp = temp / mOHMList.Count;
                    Value = (int)temp;
                }
            }

        }
    }
}
