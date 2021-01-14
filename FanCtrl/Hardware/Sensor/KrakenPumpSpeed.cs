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

        public KrakenPumpSpeed(string id, Kraken kraken, uint num) : base(LIBRARY_TYPE.NZXT_Kraken)
        {
            ID = id;
            mKraken = kraken;
            Name = "NZXT Kraken Pump";
            if (num > 1)
            {
                Name = Name + " #" + num;
            }
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
