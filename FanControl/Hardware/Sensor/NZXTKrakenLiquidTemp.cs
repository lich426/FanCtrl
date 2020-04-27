using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZXTSharp.KrakenX;

namespace FanControl
{
    public class NZXTKrakenLiquidTemp : BaseSensor
    {
        private KrakenX mKrakenX = null;

        public NZXTKrakenLiquidTemp(KrakenX krakenX) : base(SENSOR_TYPE.TEMPERATURE)
        {
            mKrakenX = krakenX;
            Name = "NZXT Kraken Liquid";
        }

        public override string getString()
        {
            return Value + " ℃";
        }
        public override void update()
        {
            Value = mKrakenX.GetLiquidTemp();
        }
    }
}
