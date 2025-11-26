
namespace FanCtrl
{
    public class KrakenLiquidTemp : BaseSensor
    {
        private Kraken mKraken = null;

        public KrakenLiquidTemp(string id, Kraken kraken, uint num) : base(LIBRARY_TYPE.NZXT_Kraken)
        {
            ID = id;
            mKraken = kraken;
            Name = "NZXT Kraken Liquid";
            if (num > 1)
            {
                Name = Name + " #" + num;
            }
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
