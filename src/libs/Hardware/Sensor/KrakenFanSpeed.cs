
namespace FanCtrl
{
    public class KrakenFanSpeed : BaseSensor
    {
        private Kraken mKraken = null;

        public KrakenFanSpeed(string id, Kraken kraken, uint num) : base(LIBRARY_TYPE.NZXT_Kraken)
        {
            ID = id;
            mKraken = kraken;
            Name = "NZXT Kraken Fan";
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
            Value = mKraken.getFanSpeed();
        }
    }
}
