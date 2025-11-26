
namespace FanCtrl
{
    public class KrakenPumpControl : BaseControl
    {
        private Kraken mKraken = null;

        public KrakenPumpControl(string id, Kraken kraken, uint num) : base(LIBRARY_TYPE.NZXT_Kraken)
        {
            ID = id;
            mKraken = kraken;
            Name = "NZXT Kraken Pump";
            if (num > 1)
            {
                Name = Name + " #" + num;
            }
        }

        public override void update()
        {

        }

        public override int getMinSpeed()
        {
            return mKraken.getMinPumpSpeed();
        }

        public override int getMaxSpeed()
        {
            return mKraken.getMaxPumpSpeed();
        }

        public override void setSpeed(int value)
        {
            int min = this.getMinSpeed();
            int max = this.getMaxSpeed();

            if (value > max)
            {
                Value = max;
            }
            else if (value < min)
            {
                Value = min;
            }
            else
            {
                Value = value;
            }
            mKraken.setPumpSpeed(Value);
        }
    }
}
