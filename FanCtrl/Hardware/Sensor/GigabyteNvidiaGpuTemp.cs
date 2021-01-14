using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class GigabyteNvidiaGpuTemp : BaseSensor
    {
        public delegate float OnGetGigabyteNvidiaTemperatureHandler(int index);

        public event OnGetGigabyteNvidiaTemperatureHandler onGetGigabyteNvidiaTemperatureHandler;

        private int mIndex = -1;

        public GigabyteNvidiaGpuTemp(string id, string name, int index) : base(LIBRARY_TYPE.Gigabyte)
        {
            ID = id;
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
            float temp = onGetGigabyteNvidiaTemperatureHandler(mIndex);
            Value = (int)Math.Round(temp);
        }

    }
}
