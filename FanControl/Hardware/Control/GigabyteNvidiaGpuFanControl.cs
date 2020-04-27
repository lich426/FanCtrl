using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gigabyte.Engine.EnvironmentControl.CoolingDevice.Fan;
using Gigabyte.Engine.GraphicsCard.Nvidia;
using Gigabyte.GraphicsCard.Common;

namespace FanControl
{
    public class GigabyteNvidiaGpuFanControl : BaseControl
    {
        private NvidiaGeforceGraphicsModule mModule = null;

        public GigabyteNvidiaGpuFanControl(NvidiaGeforceGraphicsModule module, int num) : base()
        {
            Name = "GPU Fan #" + num;
            mModule = module;
            var info = new GraphicsCoolerSetting();
            mModule.GetFanSpeedInfo(ref info);
            info.Support = true;
            info.Manual = true;
        }

        public override void update()
        {

        }

        public override int getMinSpeed()
        {
            var info = new GraphicsCoolerSetting();
            mModule.GetFanSpeedInfo(ref info);
            return (int)Math.Ceiling(info.Config.Minimum);
        }

        public override int getMaxSpeed()
        {
            var info = new GraphicsCoolerSetting();
            mModule.GetFanSpeedInfo(ref info);
            return (int)Math.Ceiling(info.Config.Maximum);
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

            mModule.SetFanSpeed((float)Value);
            LastValue = Value;
            return Value;
        }
    }
}
