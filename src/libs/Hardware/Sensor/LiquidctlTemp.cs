using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class LiquidctlTemp : BaseSensor
    {
        private LiquidctlStatusData mLiquidctlStatusData;

        public LiquidctlTemp(LiquidctlStatusData statusData) : base(LIBRARY_TYPE.Liquidctl)
        {
            ID = string.Format("liquidctl/{0}/{1}/{2}/Temp", statusData.LiquidctlData.Description, statusData.LiquidctlData.Address, statusData.Key);
            Name = statusData.Key;
            mLiquidctlStatusData = statusData;
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
            try
            {
                double value = double.Parse(mLiquidctlStatusData.Value);
                Value = (int)Math.Round(value);
            }
            catch { }
        }
    }
}
