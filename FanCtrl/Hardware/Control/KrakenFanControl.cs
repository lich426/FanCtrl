using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class KrakenFanControl : BaseControl
    {
        private Kraken mKraken = null;

        public KrakenFanControl(Kraken kraken, uint num) : base()
        {
            mKraken = kraken;
            Name = "NZXT Kraken Fan";
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
            return mKraken.getMinFanSpeed();
        }

        public override int getMaxSpeed()
        {
            return mKraken.getMaxFanSpeed();
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
            mKraken.setFanSpeed(Value);
            LastValue = Value;
            return Value;
        }
    }
}
