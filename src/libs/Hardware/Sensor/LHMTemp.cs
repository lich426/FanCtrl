using System;
using LibreHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class LHMTemp : BaseSensor
    {
        // ISensor
        private ISensor mSensor = null;

        public LHMTemp(string id, ISensor sensor, string name) : base(LIBRARY_TYPE.LHM)
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
