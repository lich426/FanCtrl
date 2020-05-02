using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;
using OpenHardwareMonitor.Hardware;

namespace FanControl
{
    public class HardwareControl : BaseControl
    {
        // ISensor
        private LibreHardwareMonitor.Hardware.ISensor mLHMSensor = null;
        private OpenHardwareMonitor.Hardware.ISensor mOHMSensor = null;

        public HardwareControl(LibreHardwareMonitor.Hardware.ISensor sensor, string name) : base()
        {
            mLHMSensor = sensor;
            Name = name;
            Value = 0;
            LastValue = 0;
        }

        public HardwareControl(OpenHardwareMonitor.Hardware.ISensor sensor, string name) : base()
        {
            mOHMSensor = sensor;
            Name = name;
            Value = 0;
            LastValue = 0;
        }

        public override void update()
        {
            double temp = 0.0f;            
            if(mLHMSensor != null)
            {
                temp = (mLHMSensor.Value.HasValue == true) ? (double)mLHMSensor.Value : 0.0f;
            }
            else if (mOHMSensor != null)
            {
                temp = (mOHMSensor.Value.HasValue == true) ? (double)mOHMSensor.Value : 0.0f;
            }
            temp = Math.Round(temp);
            Value = (int)temp;
            LastValue = (int)temp;
        }

        public override int getMinSpeed()
        {
            if (mLHMSensor != null)
            {
                return (int)mLHMSensor.Control.MinSoftwareValue;
            }
            else if (mOHMSensor != null)
            {
                return (int)mOHMSensor.Control.MinSoftwareValue;
            }
            return 0;
        }

        public override int getMaxSpeed()
        {
            if (mLHMSensor != null)
            {
                return (int)mLHMSensor.Control.MaxSoftwareValue;
            }
            else if (mOHMSensor != null)
            {
                return (int)mOHMSensor.Control.MaxSoftwareValue;
            }
            return 100;
        }

        public override int setSpeed(int value)
        {
            if (mLHMSensor != null)
            {
                mLHMSensor.Control.SetSoftware((float)value);
            }
            else if (mOHMSensor != null)
            {
                mOHMSensor.Control.SetSoftware((float)value);
            }
            Value = value;
            LastValue = value;
            return value;
        }
    }
}
