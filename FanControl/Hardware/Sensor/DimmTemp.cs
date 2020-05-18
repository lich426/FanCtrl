using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanControl
{
    public class DimmTemp : BaseSensor
    {
        public delegate void OnSetDimmTemperature(object sender, int busIndex, byte address);
        public event OnSetDimmTemperature onSetDimmTemperature;

        private int mBusIndex = 0;
        private byte mAddress = 0;

        public DimmTemp(string name, int busIndex, byte address) : base(SENSOR_TYPE.TEMPERATURE)
        {
            Name = name;
            mBusIndex = busIndex;
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
            onSetDimmTemperature(this, mBusIndex, mAddress);
        }

    }
}
