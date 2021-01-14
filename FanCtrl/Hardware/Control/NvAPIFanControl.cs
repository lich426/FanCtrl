using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class NvAPIFanControl : BaseControl
    {
        public delegate void OnSetNvAPIControlHandler(int index, int coolerID, int value);
        public event OnSetNvAPIControlHandler onSetNvAPIControlHandler;

        private int mIndex = 0;
        private int mCoolerID = 0;
        private int mMinSpeed = 0;
        private int mMaxSpeed = 100;

        public NvAPIFanControl(string id, string name, int index, int coolerID, int value, int minSpeed, int maxSpeed) : base(LIBRARY_TYPE.NvAPIWrapper)
        {
            ID = id;
            Name = name;
            mIndex = index;
            mCoolerID = coolerID;
            Value = value;
            LastValue = value;
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

            onSetNvAPIControlHandler(mIndex, mCoolerID, Value);

            LastValue = Value;
            return Value;
        }
    }
}
