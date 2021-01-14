using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class GigabyteAmdGpuFanControl : BaseControl
    {
        public delegate void OnSetGigabyteAmdControlHandler(int index, int value);
        public event OnSetGigabyteAmdControlHandler onSetGigabyteAmdControlHandler;

        private int mIndex = -1;
        private int mMinSpeed = 0;
        private int mMaxSpeed = 100;

        public GigabyteAmdGpuFanControl(string id, string name, int index, int minSpeed, int maxSpeed) : base(LIBRARY_TYPE.Gigabyte)
        {
            ID = id;
            Name = name;
            mIndex = index;
            mMinSpeed = minSpeed;
            mMaxSpeed = maxSpeed;
        }

        public override void update()
        {

        }

        public override int getMinSpeed()
        {
            return mMinSpeed;
        }

        public override int getMaxSpeed()
        {
            return mMaxSpeed;
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

            onSetGigabyteAmdControlHandler(mIndex, Value);

            LastValue = Value;
            return Value;
        }
    }
}
