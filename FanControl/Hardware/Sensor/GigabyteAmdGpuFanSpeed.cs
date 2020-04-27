using Gigabyte.Engine.GraphicsCard.Amd;
using Gigabyte.GraphicsCard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class GigabyteAmdGpuFanSpeed : BaseSensor
    {
        private AmdRadeonGraphicsModule mModule = null;

        public GigabyteAmdGpuFanSpeed(AmdRadeonGraphicsModule module, int num) : base(SENSOR_TYPE.TEMPERATURE)
        {
            Name = "GPU Fan #" + num;
            mModule = module;
        }

        public override string getString()
        {
            var valueString = string.Format("{0:D4}", Value);
            return valueString + " RPM";
        }

        public override void update()
        {
            float speed = 0.0f;
            FanSpeedType type = FanSpeedType.RPM;
            mModule.GetFanSpeed(ref speed, ref type);
            Value = (int)Math.Round(speed);
        }
        
    }
}
