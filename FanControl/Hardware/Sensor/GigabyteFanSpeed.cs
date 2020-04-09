using Gigabyte.Engine.EnvironmentControl.CoolingDevice.Fan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class GigabyteFanSpeed : BaseSensor
    {
        private string mName;
        private int mIndex = -1;
        private SmartGuardianFanControlModule mGigabyteSmartGuardianFanControlModule = null;
        
        public GigabyteFanSpeed(string name, int index, SmartGuardianFanControlModule module) : base(SENSOR_TYPE.TEMPERATURE)
        {
            mName = name;
            mIndex = index;
            mGigabyteSmartGuardianFanControlModule = module;
        }

        public override string getName()
        {
            return mName;
        }

        public override string getString()
        {
            var valueString = string.Format("{0:D4}", Value);
            return valueString + " RPM (" + this.getName() + ")";
        }

        public override void update()
        {
            if (mGigabyteSmartGuardianFanControlModule != null && mGigabyteSmartGuardianFanControlModule.IsDisposed == false)
            {
                List<float> pTemperatureDatas = new List<float>();
                List<float> pFanSpeedDatas = new List<float>();
                mGigabyteSmartGuardianFanControlModule.GetHardwareMonitorDatas(ref pTemperatureDatas, ref pFanSpeedDatas);
                Value = (int)Math.Round(pFanSpeedDatas[mIndex]);
            }
        }
        
    }
}
