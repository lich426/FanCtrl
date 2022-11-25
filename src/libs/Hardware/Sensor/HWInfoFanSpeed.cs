using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class HWInfoFanSpeed : BaseSensor
    {
        private string mCategoryID = "";
        private string mDeviceID = "";

        public HWInfoFanSpeed(string id, string name, string categoryID, string deviceID) : base(LIBRARY_TYPE.HWiNFO)
        {
            ID = id;
            Name = name;
            mCategoryID = categoryID;
            mDeviceID = deviceID;
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }

        public override void update()
        {
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
        }

    }
}
