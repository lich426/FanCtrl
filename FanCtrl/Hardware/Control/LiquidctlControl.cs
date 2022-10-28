using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class LiquidctlControl : BaseControl
    {
        public string DeviceName { get; set; }

        public string Address { get; set; }

        public string ChannelName { get; set; }

        public LiquidctlControl(int index, string deviceName, string name, string address, string channelName) : base(LIBRARY_TYPE.Liquidctl)
        {
            ID = string.Format("liquidctl/{0}/{1}/Control/{2}", deviceName, address, index);
            DeviceName = deviceName;
            Name = name;
            Address = address;
            ChannelName = channelName;
            Value = LastValue = NextValue = 50;
        }

        public override int setSpeed(int value)
        {
            if (Address.Length == 0 || ChannelName.Length == 0)
                return Value;

            Task.Run(() => {
                var p = new Process();
                p.StartInfo.FileName = LiquidctlManager.getInstance().LiquidctlPath;
                p.StartInfo.Arguments = string.Format("--address \"{0}\" set {1} speed {2}", Address, ChannelName, NextValue);
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                if (p.Start() == true)
                {
                    p.Close();
                }
            });

            Value = LastValue = NextValue = value;
            return value;
        }
    }
}
