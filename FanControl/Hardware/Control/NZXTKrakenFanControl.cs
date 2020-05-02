using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZXTSharp.KrakenX;

namespace FanControl
{
    public class NZXTKrakenFanControl : BaseControl
    {
        private KrakenX mKrakenX = null;

        public NZXTKrakenFanControl(KrakenX krakenX) : base()
        {
            mKrakenX = krakenX;
            Name = "NZXT Kraken Fan";
        }

        public override void update()
        {

        }

        public override int getMinSpeed()
        {
            return mKrakenX.getMinFanSpeed();
        }

        public override int getMaxSpeed()
        {
            return mKrakenX.getMaxFanSpeed();
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
            mKrakenX.SetFanSpeed(Value);
            LastValue = Value;
            return Value;
        }
    }
}
