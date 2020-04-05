using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZXTSharp.KrakenX;

namespace FanControl
{
    public class NZXTKrakenPumpSpeed : BaseSensor
    {
        private KrakenX mKrakenX = null;

        public NZXTKrakenPumpSpeed(KrakenX krakenX) : base(SENSOR_TYPE.FAN)
        {
            mKrakenX = krakenX;
        }

        public override string getName()
        {
            return "NZXT Kraken Pump";
        }

        public override string getString()
        {
            var valueString = string.Format("{0:D4}", Value);
            return valueString + " RPM (" + this.getName() + ")";
        }

        public override void update()
        {
            try
            {
                Value = mKrakenX.GetPumpSpeed();
            }
            catch (Exception e) { }
        }
    }
}
