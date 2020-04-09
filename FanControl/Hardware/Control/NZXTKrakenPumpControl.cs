using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZXTSharp.KrakenX;

namespace FanControl
{
    public class NZXTKrakenPumpControl : BaseControl
    {
        private KrakenX mKrakenX = null;

        public NZXTKrakenPumpControl(KrakenX krakenX) : base()
        {
            mKrakenX = krakenX;
        }

        public override string getName()
        {
            return "NZXT Kraken Pump";
        }

        public override void update()
        {

        }

        public override int getMinSpeed()
        {
            return 50;
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
            else if (value < 50)
            {
                Value = 50;
            }
            else
            {
                Value = value;
            }
            mKrakenX.SetPumpSpeed(Value);
            LastValue = Value;
            return Value;
        }
    }
}
