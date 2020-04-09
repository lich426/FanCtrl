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
        }

        public override string getName()
        {
            return "NZXT Kraken Fan";
        }

        public override void update()
        {

        }

        public override int getMinSpeed()
        {
            return 25;
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
            else if (value < 25)
            {
                Value = 25;
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
