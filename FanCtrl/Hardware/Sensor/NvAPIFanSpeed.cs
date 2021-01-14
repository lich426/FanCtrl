using Gigabyte.Engine.EnvironmentControl.CoolingDevice.Fan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class NvAPIFanSpeed : BaseSensor
    {
        public delegate int OnGetNvAPIFanSpeedHandler(int index, int coolerID);

        public event OnGetNvAPIFanSpeedHandler onGetNvAPIFanSpeedHandler;

        private int mIndex = 0;
        private int mCooerID = 0;
        
        public NvAPIFanSpeed(string id, string name, int index, int coolerID) : base(LIBRARY_TYPE.NvAPIWrapper)
        {
            ID = id;
            Name = name;
            mIndex = index;
            mCooerID = coolerID;
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }

        public override void update()
        {
            Value = onGetNvAPIFanSpeedHandler(mIndex, mCooerID);
        }
        
    }
}
