using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
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
            if (OptionManager.getInstance().IsFahrenheit == true)
                return Util.getFahrenheit(Value) + " °F";
            else
                return Value + " °C";
        }
        public override void update()
        {
            Value = mKraken.getLiquidTemp();
        }
    }
}
