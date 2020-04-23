using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gigabyte.Engine.EnvironmentControl.CoolingDevice.Fan;
using Gigabyte.Engine.GraphicsCard.Amd;
using Gigabyte.GraphicsCard.Common;

namespace FanControl
{
    public class GigabyteAmdGpuFanControl : BaseControl
    {
        private string mName;
        private AmdRadeonGraphicsModule mModule = null;

        public GigabyteAmdGpuFanControl(AmdRadeonGraphicsModule module, int num) : base()
        {
            mName = "GPU Fan #" + num;
            mModule = module;
        }

        public override string getName()
        {
            return mName;
        }

        public override void update()
        {

        }

        public override int getMinSpeed()
        {
            var info = new GraphicsFanSpeedInfo();
            mModule.GetFanSpeedInfo(ref info);
            return info.MinPercent;
        }

        public override int getMaxSpeed()
        {
            var info = new GraphicsFanSpeedInfo();
            mModule.GetFanSpeedInfo(ref info);
            return info.MaxPercent;
        }

        public override int setSpeed(int value)
        {
            int min = this.getMinSpeed();
            int max = this.getMaxSpeed();

            if (value > max)
            {
                Value = max;
            }
            else if (value < min)
            {
                Value = min;
            }
            else
            {
                Value = value;
            }

            mModule.SetFanSpeed(Value);
            LastValue = Value;
            return Value;
        }
    }
}
