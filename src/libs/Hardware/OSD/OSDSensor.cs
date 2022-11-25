using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanCtrl
{
    public enum OSDUnitType : int
    {
        Temperature = 0,
        RPM,
        Percent,
        MHz,
        kHz,
        KB,
        MB,
        GB,
        MBPerSec,
        Voltage,
        Power,
        FPS,
        Blank,
        HWiNFO,
        Unknown,
    }

    public class OSDSensor
    {
        public string Prefix { get; set; }

        public string Name { get; set; }

        public string ID { get; set; }

        public OSDUnitType UnitType { get; set; }

        public double DoubleValue { get; set; }

        public int Value { get; set; }

        public OSDSensor(string id, string prefix, string name, OSDUnitType unitType)
        {
            ID = id;
            Prefix = prefix;
            Name = name;
            UnitType = unitType;
        }

        public virtual string getString()
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

        public virtual void update()
        {

        }
    }
}
