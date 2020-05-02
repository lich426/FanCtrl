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
        public delegate float OnGetFanSpeedHandler(int index);

        public event OnGetFanSpeedHandler onGetFanSpeed;

        private int mIndex = -1;
        
        public GigabyteFanSpeed(string name, int index) : base(SENSOR_TYPE.TEMPERATURE)
        {
            Name = name;
            mIndex = index;
        }

        public override string getString()
        {
            var valueString = string.Format("{0:D4}", Value);
            return valueString + " RPM";
        }

        public override void update()
        {
            float speed = onGetFanSpeed(mIndex);
            Value = (int)Math.Round(speed);
        }
        
    }
}
