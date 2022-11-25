using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public enum LIQUID_STATUS_DATA_TYPE
    {
        UNKNOWN,
        TEMPERATURE,
        FAN,
        CONTOL,
    }

    public class LiquidctlStatusData
    {
        public LiquidctlData LiquidctlData { get; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Unit { get; set; }

        public LIQUID_STATUS_DATA_TYPE Type { get; set; } = LIQUID_STATUS_DATA_TYPE.UNKNOWN;

        public LiquidctlStatusData(LiquidctlData data)
        {
            LiquidctlData = data;
        }
    }
}
