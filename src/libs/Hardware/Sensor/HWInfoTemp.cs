using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class HWInfoTemp : BaseSensor
    {
        private string mCategoryID = "";
        private string mDeviceID = "";

        public HWInfoTemp(string id, string name, string categoryID, string deviceID) : base(LIBRARY_TYPE.HWiNFO)
        {
            ID = id;
            Name = name;
            mCategoryID = categoryID;
            mDeviceID = deviceID;
        }

        public override string getString()
        {
            if (OptionManager.getInstance().IsFahrenheit == true)
                return Util.getFahrenheit(Value) + " °F";
            else
                return Value + " °C";
        }
        public override void update()
        {
            try
            {
                var device = HWInfoManager.getInstance().getHWInfoDevice(mCategoryID, mDeviceID);
                if (device == null)
                    return;

                int value = (int)Math.Round(device.Value);
                if (device.Unit.Equals("°C") == true)
                {
                    if (Value != value)
                        Value = value;
                }
                else
                {
                    value = Util.getCelsius(value);
                    if (Value != value)
                        Value = value;
                }
            }
            catch { }
        }
    }
}
