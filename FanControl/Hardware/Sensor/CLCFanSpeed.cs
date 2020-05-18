using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class CLCFanSpeed : BaseSensor
    {
        private CLC mCLC = null;

        public CLCFanSpeed(CLC clc) : base(SENSOR_TYPE.FAN)
        {
            mCLC = clc;
            Name = "EVGA CLC Fan";
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }
        public override void update()
        {
            Value = mCLC.getFanSpeed();
        }
    }
}
