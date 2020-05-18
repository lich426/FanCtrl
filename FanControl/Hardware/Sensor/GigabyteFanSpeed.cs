using Gigabyte.Engine.EnvironmentControl.CoolingDevice.Fan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class GigabyteFanSpeed : BaseSensor
    {
        public delegate float OnGetGigabyteFanSpeedHandler(int index);

        public event OnGetGigabyteFanSpeedHandler onGetGigabyteFanSpeedHandler;

        private int mIndex = -1;
        
        public GigabyteFanSpeed(string name, int index) : base(SENSOR_TYPE.FAN)
        {
            Name = name;
            mIndex = index;
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }

        public override void update()
        {
            float speed = onGetGigabyteFanSpeedHandler(mIndex);
            Value = (int)Math.Round(speed);
        }
        
    }
}
