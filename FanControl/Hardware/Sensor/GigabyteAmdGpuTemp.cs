using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gigabyte.Engine.GraphicsCard.Amd;

namespace FanControl
{
    public class GigabyteAmdGpuTemp : BaseSensor
    {
        private AmdRadeonGraphicsModule mModule = null;

        public GigabyteAmdGpuTemp(string name, AmdRadeonGraphicsModule module) : base(SENSOR_TYPE.TEMPERATURE)
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
