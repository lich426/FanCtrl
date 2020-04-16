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

        public HardwareControl(ISensor sensor) : base()
        {
            mSensor = sensor;
            Value = 30;
            LastValue = 30;
        }
        
        public override string getName()
        {
            return mSensor.Name;
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
            LastValue = (int)value;
            return value;
        }
    }
}
