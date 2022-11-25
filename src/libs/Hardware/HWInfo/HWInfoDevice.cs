using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class HWInfoDevice
    {
        public string Name { get; set; }

        public string ID { get; set; }

        public double Value { get; set; } = 0.0;

        public string Unit { get; set; }

        public HWInfoDevice(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
