using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public enum SENSOR_TYPE
    {
        // temperature
        TEMPERATURE,

        // fan speed
        FAN,

        UNKNOWN,
    };

    public class BaseSensor
    {
        // Sensor type
        public SENSOR_TYPE Type { get; }

        // Value
        public int Value { get; set; }

        public string Name { get; set; }

        public BaseSensor(SENSOR_TYPE type)
        {
            Type = type;
            Value = 0;
        }

        public virtual string getString()
        {
            return "";
        }

        public virtual void update()
        {

        }
        
    }
}
