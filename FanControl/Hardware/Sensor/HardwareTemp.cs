using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanControl
{
    public class HardwareTemp : BaseSensor
    {
        // IHardware
        private IHardware mHardware = null;

        // SensorList
        private List<ISensor> mList = new List<ISensor>();

        public HardwareTemp(IHardware hardware, string name) : base(SENSOR_TYPE.TEMPERATURE)
        {
            mHardware = hardware;
            Name = name;
        }

        public void setTemperatureSensor()
        {
            if (mList.Count > 0)
                return;

            ISensor packageSensor = null;
            var sensor = mHardware.Sensors;
            for (int i = 0; i < sensor.Length; i++)
            {
                if (sensor[i].SensorType == LibreHardwareMonitor.Hardware.SensorType.Temperature)
                {
                    if (mHardware.HardwareType == HardwareType.Cpu ||
                        mHardware.HardwareType == HardwareType.GpuAmd ||
                        mHardware.HardwareType == HardwareType.GpuNvidia)
                    {
                        if (sensor[i].Name.IndexOf("Core") >= 0)
                        {
                            mList.Add(sensor[i]);
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
                mList.Clear();
                mList.Add(packageSensor);
            }
        }

        public override string getString()
        {
            return Value + " ℃";
        }
        public override void update()
        {
            // setTemperatureSensor
            this.setTemperatureSensor();

            double temp = 0.0f;
            for (int i = 0; i < mList.Count; i++)
            {
                temp = temp + ((mList[i].Value.HasValue == true) ? Math.Round((double)mList[i].Value) : 0);
            }

            if (temp > 0.0f)
            {
                temp = temp / mList.Count;
                Value = (int)temp;
            }
        }

    }
}
