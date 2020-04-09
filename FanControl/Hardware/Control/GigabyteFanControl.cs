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
        private string mName;
        private int mIndex = -1;
        private SmartGuardianFanControlModule mGigabyteSmartGuardianFanControlModule = null;

        public GigabyteFanControl(string name, int index, SmartGuardianFanControlModule module) : base()
        {
            mName = name;
            mIndex = index;
            mGigabyteSmartGuardianFanControlModule = module;
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
            return 30;
        }

        public override int getMaxSpeed()
        {
            return 100;
        }

        public override int setSpeed(int value)
        {
            if (value > 100)
            {
                Value = 100;
            }
            else if (value < 30)
            {
                Value = 30;
            }
            else
            {
                Value = value;
            }

            if (mGigabyteSmartGuardianFanControlModule != null && mGigabyteSmartGuardianFanControlModule.IsDisposed == false)
            {
                mGigabyteSmartGuardianFanControlModule.SetCalibrationPwm(mIndex, (byte)Value);
            }
            LastValue = Value;
            return Value;
        }
    }
}
