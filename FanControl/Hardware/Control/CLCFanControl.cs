using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class CLCFanControl : BaseControl
    {
        private CLC mCLC = null;

        public CLCFanControl(CLC clc) : base()
        {
            mCLC = clc;
            Name = "EVGA CLC Fan";
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

        public override int setSpeed(int value)
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
            LastValue = Value;
            return Value;
        }
    }
}
