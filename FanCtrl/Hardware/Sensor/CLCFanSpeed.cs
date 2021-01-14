using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class CLCFanSpeed : BaseSensor
    {
        private CLC mCLC = null;

        public CLCFanSpeed(string id, CLC clc, uint num) : base(LIBRARY_TYPE.EVGA_CLC)
        {
            ID = id;
            mCLC = clc;
            Name = "EVGA CLC Fan";
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
            Value = mCLC.getFanSpeed();
        }
    }
}
