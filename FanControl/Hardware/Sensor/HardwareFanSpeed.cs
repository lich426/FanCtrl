using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanControl
{
    public class HardwareFanSpeed : BaseSensor
    {
        // ISensor
        private ISensor mSensor = null;

        public HardwareFanSpeed(ISensor sensor) : base(SENSOR_TYPE.FAN)
        {
            mSensor = sensor;
        }
        
        public override string getName()
        {
            return mSensor.Name;
        }

        public override string getString()
        {
            string valueString = string.Format("{0:D4}", Value);
            return valueString + " RPM (" + this.getName() +")";
        }

        public override void update()
        {
            Value = (mSensor.Value.HasValue == true) ? (int)mSensor.Value : Value;
        }

    }
}
