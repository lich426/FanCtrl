using NvAPIWrapper.GPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class NvAPIFanSpeed : BaseSensor
    {
        public delegate void LockBusHandler();
        public event LockBusHandler LockBus;
        public event LockBusHandler UnlockBus;

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
            LockBus();
            try
            {
                var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                var e = gpuArray[mIndex].CoolerInformation.Coolers.GetEnumerator();
                while (e.MoveNext())
                {
                    var value = e.Current;
                    if (value.CoolerId == mCooerID)
                    {
                        Value = value.CurrentFanSpeedInRPM;
                        break;
                    }
                }
            }
            catch { }
            UnlockBus();
        }
        
    }
}
