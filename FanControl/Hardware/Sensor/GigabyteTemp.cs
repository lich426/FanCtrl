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
        private int mIndex = -1;

        public GigabyteTemp(string name, int index) : base(SENSOR_TYPE.TEMPERATURE)
        {
            Name = name;
            mIndex = index;
        }

        public override string getString()
        {
            return Value + " ℃";
        }
        public override void update()
        {
            float temp = HardwareManager.getInstance().GigabyteTemperatureList[mIndex];
            Value = (int)Math.Round(temp);
        }

    }
}
