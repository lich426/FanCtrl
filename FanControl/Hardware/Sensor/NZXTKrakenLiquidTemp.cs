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
        }

        public override string getName()
        {
            return "NZXT Kraken Liquid";
        }

        public override string getString()
        {
            return Value + " ℃ (" + this.getName() + ")";
        }
        public override void update()
        {
            try
            {
                var temp = mKrakenX.GetLiquidTemp();
                Value = (temp != null) ? (int)temp : 0;
            }
            catch (Exception e) { }
        }
    }
}
