using System;
using NvAPIWrapper.GPU;

namespace FanCtrl
{
    public class NvAPITemp : BaseSensor
    {
        public enum TEMPERATURE_TYPE
        {
            CORE = 0,
            HOTSPOT = 1,
            MEMORY = 2,
        };


        public delegate void LockBusHandler();
        public event LockBusHandler LockBus;
        public event LockBusHandler UnlockBus;

        private int mIndex = -1;
        private TEMPERATURE_TYPE mType = TEMPERATURE_TYPE.CORE;

        public NvAPITemp(string id, string name, int index, TEMPERATURE_TYPE type) : base(LIBRARY_TYPE.NvAPIWrapper)
        {
            ID = id;
            Name = name;
            mIndex = index;
            mType = type;
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

                if (mType == TEMPERATURE_TYPE.CORE)
                {
                    var e = gpuArray[mIndex].ThermalInformation.ThermalSensors.GetEnumerator();
                    while (e.MoveNext())
                    {
                        var value = e.Current;
                        Value = value.CurrentTemperature;
                        break;
                    }
                }
                else if (mType == TEMPERATURE_TYPE.HOTSPOT)
                {
                    float value = gpuArray[mIndex].ThermalInformation.HotSpotTemperature;
                    Value = (int)Math.Round((double)value);
                }
                else if (mType == TEMPERATURE_TYPE.MEMORY)
                {
                    float value = gpuArray[mIndex].ThermalInformation.MemoryJunctionTemperature;
                    Value = (int)Math.Round((double)value);
                }
            }
            catch { }
            UnlockBus();
        }
    }
}
