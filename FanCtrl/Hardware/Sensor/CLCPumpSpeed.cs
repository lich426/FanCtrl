using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class CLCPumpSpeed : BaseSensor
    {
        private CLC mCLC = null;

        public CLCPumpSpeed(CLC clc, uint num) : base(SENSOR_TYPE.FAN)
        {
            mCLC = clc;
            Name = "EVGA CLC Pump";
            if (num > 1)
            {
                Name = Name + " #" + num;
            }
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
