using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class GigabyteNvidiaFanSpeed : BaseSensor
    {
        public delegate float OnGetGigabyteNvidiaFanSpeedHandler(int index);

        public event OnGetGigabyteNvidiaFanSpeedHandler onGetGigabyteNvidiaFanSpeedHandler;

        private int mIndex = -1;

        public GigabyteNvidiaFanSpeed(string id, string name, int index) : base(LIBRARY_TYPE.Gigabyte)
        {
            ID = id;
            Name = name;
            mIndex = index;
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }

        public override void update()
        {
            float speed = onGetGigabyteNvidiaFanSpeedHandler(mIndex);
            Value = (int)Math.Round(speed);
        }
        
    }
}
