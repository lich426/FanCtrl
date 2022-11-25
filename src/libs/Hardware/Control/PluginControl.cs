using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class PluginControl : BaseControl
    {
        public PluginControl(string id, string name) : base(LIBRARY_TYPE.Plugin)
        {
            ID = id;
            Name = name;
            Value = NextValue = 50;
        }

        public override void update()
        {
            
        }

        public override int getMinSpeed()
        {
            return 0;
        }

        public override int getMaxSpeed()
        {
            return 100;
        }

        public override void setSpeed(int value)
        {
            Value = value;
            PluginManager.getInstance().sendAll(ID, Value);
        }

        public override void setAuto()
        {

        }
    }
}
