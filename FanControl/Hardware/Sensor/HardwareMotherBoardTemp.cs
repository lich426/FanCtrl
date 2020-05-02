using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;
using OpenHardwareMonitor.Hardware;

namespace FanControl
{
    public class HardwareMotherBoardTemp : BaseSensor
    {
        // ISensor
        private LibreHardwareMonitor.Hardware.ISensor mLHMSensor = null;
        private OpenHardwareMonitor.Hardware.ISensor mOHMSensor = null;

        public HardwareMotherBoardTemp(LibreHardwareMonitor.Hardware.ISensor sensor, string name) : base(SENSOR_TYPE.TEMPERATURE)
        {
            mLHMSensor = sensor;
            Name = name;
        }

        public HardwareMotherBoardTemp(OpenHardwareMonitor.Hardware.ISensor sensor, string name) : base(SENSOR_TYPE.TEMPERATURE)
        {
            mOHMSensor = sensor;
            Name = name;
        }

        public override string getString()
        {
            return Value + " ℃";
        }
        public override void update()
        {
            if (mLHMSensor != null)
            {
                double temp = ((mLHMSensor.Value.HasValue == true) ? Math.Round((double)mLHMSensor.Value) : 0);
                if (temp > 0.0f)
                {
                    Value = (int)temp;
                }
            }
            else if (mOHMSensor != null)
            {
                double temp = ((mOHMSensor.Value.HasValue == true) ? Math.Round((double)mOHMSensor.Value) : 0);
                if (temp > 0.0f)
                {
                    Value = (int)temp;
                }
            }
        }

    }
}
