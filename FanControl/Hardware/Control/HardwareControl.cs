using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanControl
{
    public class HardwareControl : BaseControl
    {
        // ISensor
        private ISensor mSensor = null;

        public HardwareControl(ISensor sensor, string name) : base()
        {
            mSensor = sensor;
            Name = name;
            Value = 0;
            LastValue = 0;
        }

        public override void update()
        {
            double temp = (mSensor.Value.HasValue == true) ? (double)mSensor.Value : 0.0f;
            temp = Math.Round(temp);
            Value = (int)temp;
            LastValue = (int)temp;
        }

        public override int getMinSpeed()
        {
            return (int)mSensor.Control.MinSoftwareValue;
        }

        public override int getMaxSpeed()
        {
            return (int)mSensor.Control.MaxSoftwareValue;
        }

        public override int setSpeed(int value)
        {
            mSensor.Control.SetSoftware((float)value);
            Value = value;
            LastValue = value;
            return value;
        }
    }
}
