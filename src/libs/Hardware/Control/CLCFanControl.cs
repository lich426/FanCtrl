using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class CLCFanControl : BaseControl
    {
        private CLC mCLC = null;

        public CLCFanControl(string id, CLC clc, uint num) : base(LIBRARY_TYPE.EVGA_CLC)
        {
            ID = id;
            mCLC = clc;
            Name = "EVGA CLC Fan";
            if (num > 1)
            {
                Name = Name + " #" + num;
            }
        }

        public override void update()
        {

        }

        public override int getMinSpeed()
        {
            return mCLC.getMinFanSpeed();
        }

        public override int getMaxSpeed()
        {
            return mCLC.getMaxFanSpeed();
        }

        public override void setSpeed(int value)
        {
            int min = this.getMinSpeed();
            int max = this.getMaxSpeed();

            if (value > max)
            {
                Value = max;
            }
            else if (value < min)
            {
                Value = min;
            }
            else
            {
                Value = value;
            }
            mCLC.setFanSpeed(Value);
        }
    }
}
