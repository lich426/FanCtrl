using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class OHMTemp : BaseSensor
    {
        // ISensor
        private ISensor mSensor = null;

        public OHMTemp(string id, ISensor sensor, string name) : base(LIBRARY_TYPE.OHM)
        {
            ID = id;
            mSensor = sensor;
            Name = name;
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
            double temp = ((mSensor.Value.HasValue == true) ? Math.Round((double)mSensor.Value) : 0);
            if (temp > 0.0f)
            {
                Value = (int)temp;
            }
        }
    }
}
