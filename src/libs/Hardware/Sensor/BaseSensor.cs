using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class BaseSensor : BaseDevice
    {
        // Value
        public int Value { get; set; }

        public BaseSensor(LIBRARY_TYPE type)
        {
            Type = type;
            Value = 0;
        }

        public virtual string getString()
        {
            return "";
        }        
    }
}
