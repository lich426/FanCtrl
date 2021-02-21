using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvAPIWrapper.GPU;

namespace FanCtrl
{
    public class NvAPITemp : BaseSensor
    {
        public delegate void LockBusHandler();
        public event LockBusHandler LockBus;
        public event LockBusHandler UnlockBus;

        private int mIndex = -1;

        public NvAPITemp(string id, string name, int index) : base(LIBRARY_TYPE.NvAPIWrapper)
        {
            ID = id;
            Name = name;
            mIndex = index;
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
            LockBus();
            try
            {
                var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                var e = gpuArray[mIndex].ThermalInformation.ThermalSensors.GetEnumerator();
                while (e.MoveNext())
                {
                    var value = e.Current;
                    Value = value.CurrentTemperature;
                    break;
                }
            }
            catch { }
            UnlockBus();
        }
    }
}
