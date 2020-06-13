using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public enum OSDItemType : int
    {
        Sensor = 0,
        Fan = 1,
        Control = 2,
        Predefined = 3,
    }

    public enum OSDUnitType : int
    {
        Temperature = 0,
        RPM = 1,
        Percent = 2,
        //MHz = 3,
        //MB = 4,
        //FPS = 5,
    }

    public class OSDItem
    {
        public OSDItemType ItemType { get; set; }

        public OSDUnitType UnitType { get; set; }

        public int Index { get; set; } = 0;

        public bool IsColor { get; set; } = false;

        public Color Color { get; set; } = Color.White;

        public string getOSDString()
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
                osdString.Append("<A0>");

                // Value
                var hardwareManager = HardwareManager.getInstance();
                if (ItemType == OSDItemType.Sensor)
                {
                    var sensor = hardwareManager.getSensor(Index);
                    if (sensor == null)
                        return "";
                    int value = sensor.Value;
                    value = (OptionManager.getInstance().IsFahrenheit == true) ? Util.getFahrenheit(value) : value;
                    osdString.Append(value.ToString());
                }
                else if (ItemType == OSDItemType.Fan)
                {
                    var fan = hardwareManager.getFan(Index);
                    if (fan == null)
                        return "";
                    int value = fan.Value;
                    osdString.Append(value.ToString());
                }
                else if (ItemType == OSDItemType.Control)
                {
                    var control = hardwareManager.getControl(Index);
                    if (control == null)
                        return "";
                    int value = control.Value;
                    osdString.Append(value.ToString());
                }
                /*
                else if (ItemType == OSDItemType.Predefined)
                {

                }
                */
                else
                {
                    return "";
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
            item.ItemType = this.ItemType;
            item.UnitType = this.UnitType;
            item.Index = this.Index;
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

                    /*
                case OSDUnitType.MHz:
                    return " MHz";

                case OSDUnitType.MB:
                    return " MB";

                case OSDUnitType.FPS:
                    return " FPS";
                    */
                default:
                    return " ";
            }
        }
    }
}
