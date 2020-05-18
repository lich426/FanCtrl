using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;
using OpenHardwareMonitor.Hardware;

namespace FanControl
{
    public class HardwareFanSpeed : BaseSensor
    {
        // ISensor
        private LibreHardwareMonitor.Hardware.ISensor mLHMSensor = null;
        private OpenHardwareMonitor.Hardware.ISensor mOHMSensor = null;

        public HardwareFanSpeed(LibreHardwareMonitor.Hardware.ISensor sensor, string name) : base(SENSOR_TYPE.FAN)
        {
            mLHMSensor = sensor;
            Name = name;
        }

        public HardwareFanSpeed(OpenHardwareMonitor.Hardware.ISensor sensor, string name) : base(SENSOR_TYPE.FAN)
        {
            mOHMSensor = sensor;
            Name = name;
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }

        public override void update()
        {
            if (mLHMSensor != null)
            {
                Value = (mLHMSensor.Value.HasValue == true) ? (int)mLHMSensor.Value : Value;
            }
            else if (mOHMSensor != null)
            {
                Value = (mOHMSensor.Value.HasValue == true) ? (int)mOHMSensor.Value : Value;
            }
        }

    }
}
