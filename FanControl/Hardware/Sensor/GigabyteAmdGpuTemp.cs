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
        private string mName;
        private AmdRadeonGraphicsModule mModule = null;

        public GigabyteAmdGpuTemp(AmdRadeonGraphicsModule module) : base(SENSOR_TYPE.TEMPERATURE)
        {
            mName = module.ProductName;
            mModule = module;
        }

        public override string getName()
        {
            return mName;
        }

        public override string getString()
        {
            return Value + " ℃ (" + this.getName() + ")";
        }
        public override void update()
        {
            float temp = 0.0f;
            mModule.GetTemperature(ref temp);
            Value = (int)Math.Round(temp);
        }

    }
}
