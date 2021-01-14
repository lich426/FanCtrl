using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class GigabyteFanControl : BaseControl
    {
        public delegate void OnSetGigabyteControlHandler(int index, int value);
        public event OnSetGigabyteControlHandler onSetGigabyteControlHandler;

        private int mIndex = -1;
        private int mMinSpeed = 0;
        private int mMaxSpeed = 100;

        public GigabyteFanControl(string id, string name, int index, int value) : base(LIBRARY_TYPE.Gigabyte)
        {
            ID = id;
            Name = name;
            mIndex = index;
            Value = value;
            LastValue = Value;
            if (value < mMinSpeed)
                mMinSpeed = value;
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
            if (value > mMaxSpeed)
            {
                Value = mMaxSpeed;
            }
            else if (value < mMinSpeed)
            {
                Value = mMinSpeed;
            }
            else
            {
                Value = value;
            }

            onSetGigabyteControlHandler(mIndex, Value);

            LastValue = Value;
            return Value;
        }
    }
}
