using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class HWInfoOSDSensor : OSDSensor
    {
        private string mCategoryID = "";
        private string mDeviceID = "";

        public HWInfoOSDSensor(string id, string prefix, string name, OSDUnitType unitType, string categoryID, string deviceID) : base(id, prefix, name, unitType)
        {
            mCategoryID = categoryID;
            mDeviceID = deviceID;
        }

        public string getUnitString()
        {
            try
            {
                var device = HWInfoManager.getInstance().getHWInfoDevice(mCategoryID, mDeviceID);
                if (device == null)
                    return "";

                return device.Unit;
            }
            catch { }
            return "";
        }

        public override string getString()
        {
            this.update();

            try
            {
                var device = HWInfoManager.getInstance().getHWInfoDevice(mCategoryID, mDeviceID);
                if (device == null)
                    return Value.ToString();

                // Voltage
                if (device.Unit.Equals("V") == true)
                {
                    return string.Format("{0:0.00}", DoubleValue);
                }

                // Yes/No
                else if(device.Unit.Equals("Yes/No") == true)
                {
                    Value = (int)Math.Round(DoubleValue);
                    return (Value > 0) ? "Yes" : "No";
                }

                else
                {
                    Value = (int)Math.Round(DoubleValue);
                    return Value.ToString();
                }
            }
            catch { }
            return "";
        }

        public override void update()
        {
            try
            {
                var device = HWInfoManager.getInstance().getHWInfoDevice(mCategoryID, mDeviceID);
                if (device == null)
                    return;

                DoubleValue = device.Value;

                if (device.Unit.Equals("°C") == true || device.Unit.Equals("°F") == true)
                {
                    if (OptionManager.getInstance().IsFahrenheit == true && device.Unit.Equals("°C") == true)
                    {
                        int value = (int)Math.Round(DoubleValue);
                        DoubleValue = (double)Util.getFahrenheit(value);
                    }

                    else if (OptionManager.getInstance().IsFahrenheit == false && device.Unit.Equals("°F") == true)
                    {
                        int value = (int)Math.Round(DoubleValue);
                        DoubleValue = (double)Util.getCelsius(value);
                    }
                }
            }
            catch { }
        }
    }
}
