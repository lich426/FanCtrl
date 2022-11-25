using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class LHMFanSpeed : BaseSensor
    {
        // ISensor
        private ISensor mSensor = null;

        public LHMFanSpeed(string id, ISensor sensor, string name) : base(LIBRARY_TYPE.LHM)
        {
            ID = id;
            mSensor = sensor;
            Name = name;
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }

        public override void update()
        {
            if (mSensor != null)
            {
                Value = (mSensor.Value.HasValue == true) ? (int)mSensor.Value : Value;
            }
        }

    }
}
