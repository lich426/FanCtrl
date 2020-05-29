using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class KrakenPumpControl : BaseControl
    {
        private Kraken mKraken = null;

        public KrakenPumpControl(Kraken kraken) : base()
        {
            mKraken = kraken;
            Name = "NZXT Kraken Pump";
        }

        public override void update()
        {

        }

        public override int getMinSpeed()
        {
            return mKraken.getMinPumpSpeed();
        }

        public override int getMaxSpeed()
        {
            return mKraken.getMaxPumpSpeed();
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
            mKraken.setPumpSpeed(Value);
            LastValue = Value;
            return Value;
        }
    }
}
