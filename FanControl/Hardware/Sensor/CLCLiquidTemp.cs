using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class CLCLiquidTemp : BaseSensor
    {
        private CLC mCLC = null;

        public CLCLiquidTemp(CLC clc) : base(SENSOR_TYPE.TEMPERATURE)
        {
            mCLC = clc;
            Name = "EVGA CLC Liquid";
        }

        public override string getString()
        {
            return Value + " ℃";
        }
        public override void update()
        {
            Value = mCLC.getLiquidTemp();
        }
    }
}
