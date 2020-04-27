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
            Name = "NZXT Kraken Pump";
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
            mKrakenX.SetPumpSpeed(Value);
            LastValue = Value;
            return Value;
        }
    }
}
