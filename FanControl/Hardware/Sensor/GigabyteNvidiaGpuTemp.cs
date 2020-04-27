using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gigabyte.Engine.GraphicsCard.Nvidia;

namespace FanControl
{
    public class GigabyteNvidiaGpuTemp : BaseSensor
    {
        private NvidiaGeforceGraphicsModule mModule = null;

        public GigabyteNvidiaGpuTemp(string name, NvidiaGeforceGraphicsModule module) : base(SENSOR_TYPE.TEMPERATURE)
        {
            Name = name;// module.ProductName;
            mModule = module;
        }

        public override string getString()
        {
            return Value + " ℃";
        }
        public override void update()
        {
            float temp = 0.0f;
            mModule.GetTemperature(ref temp);
            Value = (int)Math.Round(temp);
        }

    }
}
