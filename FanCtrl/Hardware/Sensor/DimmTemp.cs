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
        public delegate bool LockSMBusHandler(int ms);
        public delegate void UnlockSMBusHandler();
        public event LockSMBusHandler LockBus;
        public event UnlockSMBusHandler UnlockBus;

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
            if (LockBus(10) == false)
                return;

            ushort data = SMBusController.smbusWordData(mAddress, 0x05);
            UnlockBus();

            if (data > 0)
            {
                var temp = BitConverter.GetBytes(data);
                Array.Reverse(temp);

                double value = 0.0f;
                byte upper = (byte)(temp[1] & 0x1F);
                byte lower = temp[0];
                                
                if ((upper & 0x10) == 0x10) { }    // negative
                else
                {
                    upper = (byte)(upper & 0x0F);
                    value = (double)((double)upper * 16 + (double)lower / 16);
                    Value = (int)value;
                }
            }
            Util.sleep(10);
        }

    }
}
