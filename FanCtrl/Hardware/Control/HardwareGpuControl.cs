using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;
using OpenHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class HardwareGpuControl : BaseControl
    {
        // ISensor
        private LibreHardwareMonitor.Hardware.ISensor mLHMSensor = null;
        private OpenHardwareMonitor.Hardware.ISensor mOHMSensor = null;

        public HardwareGpuControl(LibreHardwareMonitor.Hardware.ISensor sensor, string name) : base()
        {
            mLHMSensor = sensor;
            Name = name;
            Value = 0;
            LastValue = 0;
        }

        public HardwareGpuControl(OpenHardwareMonitor.Hardware.ISensor sensor, string name) : base()
        {
            mOHMSensor = sensor;
            Name = name;
            Value = 0;
            LastValue = 0;
        }

        public override void update()
        {
            double temp = 0.0f;
            if(mLHMSensor != null && mLHMSensor.Value.HasValue == true)
            {
                temp = (double)mLHMSensor.Value;
            }
            else if (mOHMSensor != null && mOHMSensor.Value.HasValue == true)
            {
                temp = (double)mOHMSensor.Value;
            }
            temp = Math.Round(temp);
            Value = (int)temp;
            LastValue = (int)temp;
        }

        public override int getMinSpeed()
        {
            if (mLHMSensor != null && mLHMSensor.Control != null)
            {
                return (int)mLHMSensor.Control.MinSoftwareValue;
            }
            else if (mOHMSensor != null && mOHMSensor.Control != null)
            {
                return (int)mOHMSensor.Control.MinSoftwareValue;
            }
            return 0;
        }

        public override int getMaxSpeed()
        {
            if (mLHMSensor != null && mLHMSensor.Control != null)
            {
                return (int)mLHMSensor.Control.MaxSoftwareValue;
            }
            else if (mOHMSensor != null && mOHMSensor.Control != null)
            {
                return (int)mOHMSensor.Control.MaxSoftwareValue;
            }
            return 100;
        }

        public override int setSpeed(int value)
        {
            if (mLHMSensor != null && mLHMSensor.Control != null)
            {
                mLHMSensor.Control.SetSoftware(value);
            }
            else if (mOHMSensor != null && mOHMSensor.Control != null)
            {
                mOHMSensor.Control.SetSoftware(value);
            }
            Value = value;
            LastValue = value;
            return value;
        }
    }
}
