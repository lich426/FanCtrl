using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanControl
{
    public class GigabyteTemp : BaseSensor
    {
        public delegate float OnGetGigabyteTemperatureHandler(int index);

        public event OnGetGigabyteTemperatureHandler onGetGigabyteTemperatureHandler;

        private int mIndex = -1;

        public GigabyteTemp(string name, int index) : base(SENSOR_TYPE.TEMPERATURE)
        {
            Name = name;
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
            float temp = onGetGigabyteTemperatureHandler(mIndex);
            Value = (int)Math.Round(temp);
        }

    }
}
