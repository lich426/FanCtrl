using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class KrakenPumpSpeed : BaseSensor
    {
        private Kraken mKraken = null;

        public KrakenPumpSpeed(Kraken kraken) : base(SENSOR_TYPE.FAN)
        {
            mKraken = kraken;
            Name = "NZXT Kraken Pump";
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }

        public override void update()
        {
            Value = mKraken.getPumpSpeed();
        }
    }
}
