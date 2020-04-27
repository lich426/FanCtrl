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
            Name = "NZXT Kraken Pump";
        }

        public override string getString()
        {
            var valueString = string.Format("{0:D4}", Value);
            return valueString + " RPM";
        }

        public override void update()
        {
            Value = mKrakenX.GetPumpSpeed();
        }
    }
}
