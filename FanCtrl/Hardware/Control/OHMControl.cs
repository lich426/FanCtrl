using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class OHMControl : BaseControl
    {
        // ISensor
        private ISensor mSensor = null;

        public OHMControl(string id, ISensor sensor, string name) : base(LIBRARY_TYPE.OHM)
        {
            ID = id;
            mSensor = sensor;
            Name = name;
            Value = 0;
            LastValue = 0;
        }

        public override void update()
        {
            double temp = 0.0f;            
            if (mSensor != null && mSensor.Value.HasValue == true)
            {
                temp = (double)mSensor.Value;
            }
            temp = Math.Round(temp);
            Value = (int)temp;
            LastValue = (int)temp;
        }

        public override int getMinSpeed()
        {
            if (mSensor != null && mSensor.Control != null)
            {
                return (int)mSensor.Control.MinSoftwareValue;
            }
            return 0;
        }

        public override int getMaxSpeed()
        {
            if (mSensor != null && mSensor.Control != null)
            {
                return (int)mSensor.Control.MaxSoftwareValue;
            }
            return 100;
        }

        public override int setSpeed(int value)
        {
            if (mSensor != null && mSensor.Control != null)
            {
                mSensor.Control.SetSoftware(value);
                IsSetSpeed = true;
            }
            Value = value;
            LastValue = value;
            return value;
        }

        public override void setAuto()
        {
            if (IsSetSpeed == false)
                return;

            if (mSensor != null && mSensor.Control != null)
            {
                mSensor.Control.SetDefault();
                IsSetSpeed = false;
            }
        }
    }
}
