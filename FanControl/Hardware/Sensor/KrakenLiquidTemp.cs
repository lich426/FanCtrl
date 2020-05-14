using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class KrakenLiquidTemp : BaseSensor
    {
        private Kraken mKraken = null;

        public KrakenLiquidTemp(Kraken kraken) : base(SENSOR_TYPE.TEMPERATURE)
        {
            mKraken = kraken;
            Name = "NZXT Kraken Liquid";
        }

        public override string getString()
        {
            return Value + " ℃";
        }
        public override void update()
        {
            Value = mKraken.getLiquidTemp();
        }
    }
}
