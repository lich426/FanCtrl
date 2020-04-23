using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gigabyte.Engine.EnvironmentControl.CoolingDevice.Fan;

namespace FanControl
{
    public class GigabyteFanControl : BaseControl
    {
        public delegate void OnSetSpeedHandler(int index, int value);
        public event OnSetSpeedHandler onSetSpeedCallback;

        private string mName;
        private int mIndex = -1;
        private int mMinSpeed = 0;
        private int mMaxSpeed = 100;

        public GigabyteFanControl(string name, int index, int value) : base()
        {
            mName = name;
            mIndex = index;
            Value = value;
            LastValue = Value;
            if (value < mMinSpeed)
                mMinSpeed = value;
        }

        public override string getName()
        {
            return mName;
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
            
            onSetSpeedCallback(mIndex, Value);

            LastValue = Value;
            return Value;
        }
    }
}
