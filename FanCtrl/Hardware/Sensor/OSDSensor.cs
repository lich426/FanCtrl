using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;
using OpenHardwareMonitor.Hardware;

namespace FanCtrl
{
    public enum OSDLibraryType
    {
        LibreHardwareMonitor = 0,
        OpenHardwareMonitor = 1,
        NvApiWrapper = 2,
    }

    public class OSDSensor : BaseSensor
    {
        public delegate double OnOSDSensorUpdate(OSDLibraryType libraryType, int index, int subIndex);
        public event OnOSDSensorUpdate onOSDSensorUpdate;

        private LibreHardwareMonitor.Hardware.ISensor mLHMSensor = null;
        private OpenHardwareMonitor.Hardware.ISensor mOHMSensor = null;

        private int mIndex = 0;
        private int mSubIndex = 0;

        public OSDLibraryType LibraryType { get; set; }

        public OSDUnitType UnitType { get; set; }

        public double DoubleValue { get; set; }

        public OSDSensor(LibreHardwareMonitor.Hardware.ISensor sensor, OSDUnitType unitType, string name, int index) : base(SENSOR_TYPE.OSD)
        {
            mLHMSensor = sensor;
            LibraryType = OSDLibraryType.LibreHardwareMonitor;
            UnitType = unitType;
            Name = name;
            mIndex = index;
        }

        public OSDSensor(OpenHardwareMonitor.Hardware.ISensor sensor, OSDUnitType unitType, string name, int index) : base(SENSOR_TYPE.OSD)
        {
            mOHMSensor = sensor;
            LibraryType = OSDLibraryType.OpenHardwareMonitor;
            UnitType = unitType;
            Name = name;
            mIndex = index;
        }

        public OSDSensor(OSDUnitType unitType, string name, int index, int subIndex) : base(SENSOR_TYPE.OSD)
        {
            LibraryType = OSDLibraryType.NvApiWrapper;
            UnitType = unitType;
            Name = name;
            mIndex = index;
            mSubIndex = subIndex;
        }        

        public override string getString()
        {
            this.update();

            if (UnitType == OSDUnitType.Voltage)
            {
                return string.Format("{0:0.00}", DoubleValue);
            }
            else if (UnitType == OSDUnitType.kHz)
            {
                Value = (int)Math.Round(DoubleValue / 1000);
                return Value.ToString();
            }
            else if (UnitType == OSDUnitType.KB)
            {
                Value = (int)Math.Round(DoubleValue / 1024);
                return Value.ToString();
            }
            else if (UnitType == OSDUnitType.GB)
            {
                Value = (int)Math.Round(DoubleValue * 1024);
                return Value.ToString();
            }
            else if (UnitType == OSDUnitType.MBPerSec)
            {
                double value = Math.Round(DoubleValue / 1024 / 1024);
                return string.Format("{0}", value);
            }
            else
            {
                Value = (int)Math.Round(DoubleValue);
                return Value.ToString();
            }
        }
        public override void update()
        {
            if (mLHMSensor != null)
            {
                DoubleValue = (mLHMSensor.Value.HasValue == true) ? (double)mLHMSensor.Value : (double)Value;
            }

            else if (mOHMSensor != null)
            {
                DoubleValue = (mOHMSensor.Value.HasValue == true) ? (double)mOHMSensor.Value : (double)Value;
            }
            else
            {
                DoubleValue = onOSDSensorUpdate(LibraryType, mIndex, mSubIndex);
            }
        }
    }
}
