using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class GigabyteAmdGpuTemp : BaseSensor
    {
        public delegate float OnGetGigabyteAmdTemperatureHandler(int index);

        public event OnGetGigabyteAmdTemperatureHandler onGetGigabyteAmdTemperatureHandler;

        private int mIndex = -1;

        public GigabyteAmdGpuTemp(string name, int index) : base(SENSOR_TYPE.TEMPERATURE)
        {
            Name = name;// module.ProductName;
            mIndex = index;
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
            float temp = onGetGigabyteAmdTemperatureHandler(mIndex);
            Value = (int)Math.Round(temp);
        }

    }
}
