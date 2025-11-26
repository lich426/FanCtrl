using System.Collections.Generic;

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
