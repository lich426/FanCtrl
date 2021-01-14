using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;
using OpenHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class NvAPIOSDSensor : OSDSensor
    {
        public delegate double OnNvAPIOSDSensorUpdate(int index, int subIndex);
        public event OnNvAPIOSDSensorUpdate onNvAPIOSDSensorUpdate;

        private int mIndex = 0;
        private int mSubIndex = 0;

        public NvAPIOSDSensor(string id, string prefix, string name, OSDUnitType unitType, int index, int subIndex) : base(id, prefix, name, unitType)
        {
            mIndex = index;
            mSubIndex = subIndex;
        }        

        public override void update()
        {
            if (onNvAPIOSDSensorUpdate != null)
            {
                DoubleValue = onNvAPIOSDSensorUpdate(mIndex, mSubIndex);
            }            
        }
    }
}
