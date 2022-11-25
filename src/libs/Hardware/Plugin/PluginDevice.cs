using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class PluginDevice
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public PluginDevice(string key, string name)
        {
            Key = key;
            Name = name;
        }
    }
}
