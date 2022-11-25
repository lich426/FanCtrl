using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading;

namespace FanCtrl
{
    public class HardwareDevice
    {
        // device name
        public string Name { get; set; }

        // device list
        public List<BaseDevice> DeviceList { get; } = new List<BaseDevice>();

        public HardwareDevice(string name)
        {
            Name = name;
        }

        public void addDevice(BaseDevice device)
        {
            DeviceList.Add(device);
        }
    }
}
