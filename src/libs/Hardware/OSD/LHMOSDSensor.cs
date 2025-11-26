using LibreHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class LHMOSDSensor : OSDSensor
    {
        private ISensor mSensor = null;

        public LHMOSDSensor(string id, string prefix, string name, OSDUnitType unitType, ISensor sensor) : base(id, prefix, name, unitType)
        {
            mSensor = sensor;
        }  
        
        public override void update()
        {
            if (mSensor != null)
            {
                DoubleValue = (mSensor.Value.HasValue == true) ? (double)mSensor.Value : (double)Value;
            }
        }
    }
}
