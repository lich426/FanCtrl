using NvAPIWrapper.GPU;
using NvAPIWrapper.Native.GPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class NvAPIFanControl : BaseControl
    {
        public delegate void LockBusHandler();
        public event LockBusHandler LockBus;
        public event LockBusHandler UnlockBus;

        private int mIndex = 0;
        private int mCoolerID = 0;
        
        private int mMinSpeed = 0;
        private int mMaxSpeed = 100;

        private CoolerPolicy mDefaultCoolerPolicy = CoolerPolicy.None;

        public NvAPIFanControl(string id, string name, int index, int coolerID, int value, int minSpeed, int maxSpeed, CoolerPolicy defaultCoolerPolicy) : base(LIBRARY_TYPE.NvAPIWrapper)
        {
            ID = id;
            Name = name;
            mIndex = index;
            mCoolerID = coolerID;
            Value = value;
            LastValue = value;
            mMinSpeed = minSpeed;
            mMaxSpeed = maxSpeed;
            IsSetSpeed = true;
            mDefaultCoolerPolicy = defaultCoolerPolicy;
            Console.WriteLine("NvAPIFanControl.NvAPIFanControl() : defaultCoolerPolicy({0})", (int)defaultCoolerPolicy);
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
                    var cur = e.Current;
                    if (cur.CoolerId == mCoolerID)
                    {
                        LastValue = Value = cur.CurrentLevel;
                        break;
                    }
                }
            }
            catch { }
            UnlockBus();
        }

        public override int getMinSpeed()
        {
            return mMinSpeed;
        }

        public override int getMaxSpeed()
        {
            return mMaxSpeed;
        }

        public override int setSpeed(int value)
        {
            if (value > mMaxSpeed)
            {
                Value = mMaxSpeed;
            }
            else if (value < mMinSpeed)
            {
                Value = mMinSpeed;
            }
            else
            {
                Value = value;
            }

            LockBus();
            try
            {
                var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                var info = gpuArray[mIndex].CoolerInformation;
                info.SetCoolerSettings(mCoolerID, Value);
                IsSetSpeed = true;
                Console.WriteLine("NvAPIFanControl.setSpeed() : {0}, {1}", mCoolerID, Value);
            }
            catch { }
            UnlockBus();

            LastValue = Value;
            return Value;
        }

        public override void setAuto()
        {
            if (IsSetSpeed == false)
                return;

            LockBus();
            try
            {
                var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                var info = gpuArray[mIndex].CoolerInformation;

                //info.RestoreCoolerSettingsToDefault(mCoolerID);
                info.SetCoolerSettings(mCoolerID, mDefaultCoolerPolicy);
                //info.SetCoolerSettings(mCoolerID, NvAPIWrapper.Native.GPU.CoolerPolicy.None);
                IsSetSpeed = false;
                Console.WriteLine("NvAPIFanControl.setAuto() : {0}", mCoolerID);
            }
            catch { }
            UnlockBus();
        }
    }
}
