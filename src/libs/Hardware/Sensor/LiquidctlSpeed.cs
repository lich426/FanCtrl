using System;

namespace FanCtrl
{
    public class LiquidctlSpeed : BaseSensor
    {
        private LiquidctlStatusData mLiquidctlStatusData;

        public LiquidctlSpeed(LiquidctlStatusData statusData) : base(LIBRARY_TYPE.Liquidctl)
        {
            ID = string.Format("liquidctl/{0}/{1}/{2}/Fan", statusData.LiquidctlData.Description, statusData.LiquidctlData.Address, statusData.Key);
            Name = statusData.Key;
            mLiquidctlStatusData = statusData;
        }

        public override string getString()
        {
            return Value + " RPM";
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
