using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class KrakenFanSpeed : BaseSensor
    {
        private Kraken mKraken = null;

        public KrakenFanSpeed(Kraken kraken) : base(SENSOR_TYPE.FAN)
        {
            mKraken = kraken;
            Name = "NZXT Kraken Fan";
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }
        public override void update()
        {
            Value = mKraken.getFanSpeed();
        }
    }
}
