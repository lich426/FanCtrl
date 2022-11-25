using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class PluginFanSpeed : BaseSensor
    {
        public PluginFanSpeed(string id, string name) : base(LIBRARY_TYPE.Plugin)
        {
            ID = id;
            Name = name;
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }

        public override void update()
        {
            /*
            try
            {
                var device = HWInfoManager.getInstance().getHWInfoDevice(mCategoryID, mDeviceID);
                if (device == null)
                    return;

                int value = (int)Math.Round(device.Value);
                if (Value != value)
                    Value = value;
            }
            catch { }
            */
        }
    }
}
