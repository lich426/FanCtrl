using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class GigabyteNvidiaGpuTemp : BaseSensor
    {
        public delegate float OnGetGigabyteNvidiaTemperatureHandler(int index);

        public event OnGetGigabyteNvidiaTemperatureHandler onGetGigabyteNvidiaTemperatureHandler;

        private int mIndex = -1;

        public GigabyteNvidiaGpuTemp(string name, int index) : base(SENSOR_TYPE.TEMPERATURE)
        {
            Name = name;// module.ProductName;
            mIndex = index;
        }

        public override string getString()
        {
            return Value + " ℃";
        }
        public override void update()
        {
            float temp = onGetGigabyteNvidiaTemperatureHandler(mIndex);
            Value = (int)Math.Round(temp);
        }

    }
}
