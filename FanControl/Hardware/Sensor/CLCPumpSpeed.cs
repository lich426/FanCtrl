using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class CLCPumpSpeed : BaseSensor
    {
        private CLC mCLC = null;

        public CLCPumpSpeed(CLC clc) : base(SENSOR_TYPE.FAN)
        {
            mCLC = clc;
            Name = "EVGA CLC Pump";
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }

        public override void update()
        {
            Value = mCLC.getPumpSpeed();
        }
    }
}
