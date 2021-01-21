using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class DimmTemp : BaseSensor
    {
        public delegate void OnSetDimmTemperature(object sender, byte address);
        public event OnSetDimmTemperature onSetDimmTemperature;

        private byte mAddress = 0;

        public DimmTemp(string id, string name, byte address) : base(LIBRARY_TYPE.DIMM)
        {
            ID = id;
            Name = name;
            mAddress = address;
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
            onSetDimmTemperature(this, mAddress);
        }

    }
}
