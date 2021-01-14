using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class OSDItem
    {
        public OSDUnitType UnitType { get; set; }

        public string ID { get; set; }

        public bool IsColor { get; set; } = false;

        public Color Color { get; set; } = Color.White;

        public string getOSDString(int digit)
        {
            try
            {
                var osdString = new StringBuilder();

                // Color prefix
                if (IsColor == true)
                {
                    osdString.Append("<C=");

                    var color = new byte[3];
                    color[0] = Color.R;
                    color[1] = Color.G;
                    color[2] = Color.B;
                    osdString.Append(Util.getHexString(color));

                    osdString.Append(">");
                }

                // Value prefix
                osdString.Append(string.Format("<A=-{0}>", digit));

                // Value
                var hardwareManager = HardwareManager.getInstance();

                if (UnitType == OSDUnitType.FPS)
                {
                    osdString.Append("<FR>");
                }
                else if (UnitType == OSDUnitType.Blank)
                {
                    osdString.Append(" ");
                }
                else
                {
                    var tempBaseMap = hardwareManager.TempBaseMap;
                    var fanBaseMap = hardwareManager.FanBaseMap;
                    var controlBaseMap = hardwareManager.ControlBaseMap;
                    var osdMap = hardwareManager.OSDSensorMap;
                    
                    if (tempBaseMap.ContainsKey(ID) == true)
                    {
                        var device = tempBaseMap[ID];
                        int value = device.Value;
                        value = (OptionManager.getInstance().IsFahrenheit == true) ? Util.getFahrenheit(value) : value;
                        osdString.Append(value.ToString());
                    }

                    else if (fanBaseMap.ContainsKey(ID) == true)
                    {
                        var device = fanBaseMap[ID];
                        int value = device.Value;
                        osdString.Append(value.ToString());
                    }

                    else if (controlBaseMap.ContainsKey(ID) == true)
                    {
                        var device = controlBaseMap[ID];
                        int value = device.Value;
                        osdString.Append(value.ToString());
                    }

                    else if (osdMap.ContainsKey(ID) == true)
                    {
                        var sensor = osdMap[ID];
                        osdString.Append(sensor.getString());
                    }

                    else
                    {
                        return "";
                    }
                }

                // Value postfix
                osdString.Append("<A>");

                // Unit prefix
                osdString.Append("<A1><S0>");

                // Unit
                osdString.Append(this.getUnitString());

                // Unit postfix
                osdString.Append("<S><A>");

                // Color postfix
                if (IsColor == true)
                {
                    osdString.Append("<C>");
                }

                return osdString.ToString();
            }
            catch { }
            return "";            
        }

        public OSDItem clone()
        {
            var item = new OSDItem();
            item.UnitType = this.UnitType;
            item.ID = this.ID;
            item.IsColor = this.IsColor;
            item.Color = Color.FromArgb(this.Color.R, this.Color.G, this.Color.B);
            return item;
        }

        public string getUnitString()
        {
            switch (UnitType)
            {
                case OSDUnitType.Temperature:
                    return (OptionManager.getInstance().IsFahrenheit == false) ? " 캜" : " 캟";

                case OSDUnitType.RPM:
                    return " RPM";

                case OSDUnitType.Percent:
                    return " %";

                case OSDUnitType.MHz:
                case OSDUnitType.kHz:
                    return " MHz";

                case OSDUnitType.KB:
                case OSDUnitType.GB:
                case OSDUnitType.MB:
                    return " MB";

                case OSDUnitType.MBPerSec:
                    return " MB/s";

                case OSDUnitType.Voltage:
                    return " V";

                case OSDUnitType.Power:
                    return " W";

                case OSDUnitType.FPS:
                    return " FPS";

                default:
                    return " ";
            }
        }
    }
}
