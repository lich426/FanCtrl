using Gigabyte.Engine.GraphicsCard.Nvidia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class GigabyteNvidiaFanSpeed : BaseSensor
    {
        private string mName;
        private NvidiaGeforceGraphicsModule mModule = null;
        
        public GigabyteNvidiaFanSpeed(NvidiaGeforceGraphicsModule module, int num) : base(SENSOR_TYPE.TEMPERATURE)
        {
            mName = "GPU Fan #" + num;
            mModule = module;
        }

        public override string getName()
        {
            return mName;
        }

        public override string getString()
        {
            var valueString = string.Format("{0:D4}", Value);
            return valueString + " RPM (" + this.getName() + ")";
        }

        public override void update()
        {
            float speed = 0.0f;
            mModule.GetFanSpeed(ref speed);
            Value = (int)Math.Round(speed);
        }
        
    }
}
