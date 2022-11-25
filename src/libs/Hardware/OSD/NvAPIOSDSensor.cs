using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvAPIWrapper.GPU;

namespace FanCtrl
{
    public class NvAPIOSDSensor : OSDSensor
    {
        public delegate void LockBusHandler();
        public event LockBusHandler LockBus;
        public event LockBusHandler UnlockBus;

        private int mIndex = 0;
        private int mSubIndex = 0;

        public NvAPIOSDSensor(string id, string prefix, string name, OSDUnitType unitType, int index, int subIndex) : base(id, prefix, name, unitType)
        {
            mIndex = index;
            mSubIndex = subIndex;
        }        

        public override void update()
        {
            LockBus();
            try
            {
                var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                var gpu = gpuArray[mIndex];
                switch (mSubIndex)
                {
                    case 0:
                        DoubleValue = (double)gpu.CurrentClockFrequencies.GraphicsClock.Frequency;
                        break;
                    case 1:
                        DoubleValue = (double)gpu.CurrentClockFrequencies.MemoryClock.Frequency;
                        break;
                    case 2:
                        DoubleValue = (double)gpu.CurrentClockFrequencies.ProcessorClock.Frequency;
                        break;
                    case 3:
                        DoubleValue = (double)gpu.CurrentClockFrequencies.VideoDecodingClock.Frequency;
                        break;
                    case 4:
                        DoubleValue = (double)gpu.UsageInformation.GPU.Percentage;
                        break;
                    case 5:
                        DoubleValue = (double)gpu.UsageInformation.FrameBuffer.Percentage;
                        break;
                    case 6:
                        DoubleValue = (double)gpu.UsageInformation.VideoEngine.Percentage;
                        break;
                    case 7:
                        DoubleValue = (double)gpu.UsageInformation.BusInterface.Percentage;
                        break;
                    case 8:
                        DoubleValue = (double)(((double)gpu.MemoryInformation.PhysicalFrameBufferSizeInkB - (double)gpu.MemoryInformation.CurrentAvailableDedicatedVideoMemoryInkB) / (double)gpu.MemoryInformation.PhysicalFrameBufferSizeInkB * 100.0);
                        break;
                    case 9:
                        DoubleValue = (double)gpu.MemoryInformation.CurrentAvailableDedicatedVideoMemoryInkB;
                        break;
                    case 10:
                        DoubleValue = (double)(gpu.MemoryInformation.PhysicalFrameBufferSizeInkB - gpu.MemoryInformation.CurrentAvailableDedicatedVideoMemoryInkB);
                        break;
                    case 11:
                        DoubleValue = (double)gpu.MemoryInformation.PhysicalFrameBufferSizeInkB;
                        break;
                    default:
                        DoubleValue = 0;
                        break;
                }

                //Console.WriteLine("power : {0}", gpu.PowerTopologyInformation.PowerTopologyEntries.)
            }
            catch { }
            UnlockBus();
        }
    }
}
