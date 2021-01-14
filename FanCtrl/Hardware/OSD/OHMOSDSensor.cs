using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class OHMOSDSensor : OSDSensor
    {
        private ISensor mSensor = null;

        public OHMOSDSensor(string id, string prefix, string name, OSDUnitType unitType, ISensor sensor) : base(id, prefix, name, unitType)
        {
            mSensor = sensor;
        }  

        public override void update()
        {
            if (mSensor != null)
            {
                DoubleValue = (mSensor.Value.HasValue == true) ? (double)mSensor.Value : (double)Value;
            }
        }
    }
}
