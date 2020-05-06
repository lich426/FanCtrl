using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class GigabyteAmdGpuFanSpeed : BaseSensor
    {
        public delegate float OnGetGigabyteAmdFanSpeedHandler(int index);

        public event OnGetGigabyteAmdFanSpeedHandler onGetGigabyteAmdFanSpeedHandler;

        private int mIndex = -1;

        public GigabyteAmdGpuFanSpeed(string name, int index) : base(SENSOR_TYPE.FAN)
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
            float speed = onGetGigabyteAmdFanSpeedHandler(mIndex);
            Value = (int)Math.Round(speed);
        }
        
    }
}
