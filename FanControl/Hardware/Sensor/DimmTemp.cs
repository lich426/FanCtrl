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
        public delegate void OnSetDimmTemperature(object sender, byte address);
        public event OnSetDimmTemperature onSetDimmTemperature;

        private byte mAddress = 0;

        public DimmTemp(string name, byte address) : base(SENSOR_TYPE.TEMPERATURE)
        {
            Name = name;
            mAddress = address;
        }

        public override string getString()
        {
            return Value + " ℃";
        }
        public override void update()
        {
            onSetDimmTemperature(this, mAddress);
        }

    }
}
