using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class BaseControl : BaseDevice
    {
        // Value
        public int Value { get; set; }

        public int NextValue { get; set; }

        public int LastValue { get; set; }

        public bool IsSetSpeed { get; set; }

        public BaseControl(LIBRARY_TYPE type)
        {
            Type = type;
            Value = 0;
            NextValue = 0;
            LastValue = 0;
            IsSetSpeed = false;
        }

        public virtual int getMinSpeed()
        {
            return 0;
        }

        public virtual int getMaxSpeed()
        {
            return 100;
        }

        public virtual int setSpeed(int value)
        {
            return 0;
        }

        public virtual void setAuto()
        {
            
        }
    }
}
