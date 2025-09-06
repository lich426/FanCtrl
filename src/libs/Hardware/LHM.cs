using System;
using System.Collections.Generic;
using System.Threading;
using LibreHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class LHM : IVisitor
    {
        private const string mIDPrefixTemperature = "LHM/Temp";
        private const string mIDPrefixFan = "LHM/Fan";
        private const string mIDPrefixControl = "LHM/Control";
        private const string mIDPrefixOSD = "LHM/OSD";

        private bool mIsStart = false;

        private Computer mComputer = null;

        public LHM() { }

        public void start()
        {
            if (mIsStart == true)
                return;
            mIsStart = true;

            mComputer = new Computer();
            mComputer.IsCpuEnabled = OptionManager.getInstance().IsLHMCpu;            
            mComputer.IsMotherboardEnabled = OptionManager.getInstance().IsLHMMotherboard;
            mComputer.IsControllerEnabled = OptionManager.getInstance().IsLHMContolled;
            mComputer.IsGpuEnabled = OptionManager.getInstance().IsLHMGpu;
            mComputer.IsStorageEnabled = OptionManager.getInstance().IsLHMStorage;
            mComputer.IsMemoryEnabled = OptionManager.getInstance().IsLHMMemory;
            mComputer.IsBatteryEnabled = false;
            mComputer.IsNetworkEnabled = false;
            mComputer.IsRing0Enabled = true;
            mComputer.IsPsuEnabled = false;

            mComputer.Open();
            mComputer.Accept(this);
        }

        public void stop()
        {
            if (mIsStart == false)
                return;
            mIsStart = false;

            if (mComputer != null)
            {
                mComputer.Close();
                mComputer = null;
            }
        }

        public void createTemp(ref List<HardwareDevice> deviceList)
        {
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Count; i++)
            {
                string hardwareName = (hardwareArray[i].Name.Length > 0) ? hardwareArray[i].Name : "Unknown";
                var device = new HardwareDevice(hardwareName);

                var sensorArray = hardwareArray[i].Sensors;
                for (int j = 0; j < sensorArray.Length; j++)
                {
                    if (sensorArray[j].SensorType != SensorType.Temperature)
                        continue;

                    string id = string.Format("{0}{1}", mIDPrefixTemperature, sensorArray[j].Identifier.ToString());
                    string name = (sensorArray[j].Name.Length > 0) ? sensorArray[j].Name : "Temperature";
                    var sensor = new LHMTemp(id, sensorArray[j], name);
                    device.addDevice(sensor);
                }

                var subHardwareArray = hardwareArray[i].SubHardware;
                for (int j = 0; j < subHardwareArray.Length; j++)
                {
                    var subSensorList = subHardwareArray[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType != SensorType.Temperature)
                            continue;

                        string id = string.Format("{0}{1}", mIDPrefixTemperature, subSensorList[k].Identifier.ToString());
                        string name = (subSensorList[k].Name.Length > 0) ? subSensorList[k].Name : "Temperature";
                        var sensor = new LHMTemp(id, subSensorList[k], name);
                        device.addDevice(sensor);
                    }
                }

                if (device.DeviceList.Count > 0)
                {
                    deviceList.Add(device);
                }
            }
        }

        public void createFan(ref List<HardwareDevice> deviceList)
        {
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Count; i++)
            {
                string hardwareName = (hardwareArray[i].Name.Length > 0) ? hardwareArray[i].Name : "Unknown";
                var device = new HardwareDevice(hardwareName);

                var sensorArray = hardwareArray[i].Sensors;
                for (int j = 0; j < sensorArray.Length; j++)
                {
                    if (sensorArray[j].SensorType != SensorType.Fan)
                        continue;

                    string id = string.Format("{0}{1}", mIDPrefixFan, sensorArray[j].Identifier.ToString());
                    string name = (sensorArray[j].Name.Length > 0) ? sensorArray[j].Name : "Fan";
                    var sensor = new LHMFanSpeed(id, sensorArray[j], name);
                    device.addDevice(sensor);
                }

                var subHardwareArray = hardwareArray[i].SubHardware;
                for (int j = 0; j < subHardwareArray.Length; j++)
                {
                    var subSensorList = subHardwareArray[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType != SensorType.Fan)
                            continue;

                        string id = string.Format("{0}{1}", mIDPrefixFan, subSensorList[k].Identifier.ToString());
                        string name = (subSensorList[k].Name.Length > 0) ? subSensorList[k].Name : "Fan";
                        var sensor = new LHMFanSpeed(id, subSensorList[k], name);
                        device.addDevice(sensor);
                    }
                }

                if (device.DeviceList.Count > 0)
                {
                    deviceList.Add(device);
                }
            }
        }

        public void createControl(ref List<HardwareDevice> deviceList)
        {
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Count; i++)
            {
                string hardwareName = (hardwareArray[i].Name.Length > 0) ? hardwareArray[i].Name : "Unknown";
                var device = new HardwareDevice(hardwareName);

                var sensorArray = hardwareArray[i].Sensors;
                for (int j = 0; j < sensorArray.Length; j++)
                {
                    if (sensorArray[j].SensorType != SensorType.Control)
                        continue;

                    string id = string.Format("{0}{1}", mIDPrefixControl, sensorArray[j].Identifier.ToString());
                    string name = (sensorArray[j].Name.Length > 0) ? sensorArray[j].Name : "Control";
                    var sensor = new LHMControl(id, sensorArray[j], name);
                    device.addDevice(sensor);
                }

                var subHardwareArray = hardwareArray[i].SubHardware;
                for (int j = 0; j < subHardwareArray.Length; j++)
                {
                    var subSensorList = subHardwareArray[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType != SensorType.Control)
                            continue;

                        string id = string.Format("{0}{1}", mIDPrefixControl, subSensorList[k].Identifier.ToString());
                        string name = (subSensorList[k].Name.Length > 0) ? subSensorList[k].Name : "Control";
                        var sensor = new LHMControl(id, subSensorList[k], name);
                        device.addDevice(sensor);
                    }
                }

                if (device.DeviceList.Count > 0)
                {
                    deviceList.Add(device);
                }
            }
        }

        public void createOSDSensor(List<OSDSensor> osdList, Dictionary<string, OSDSensor> osdMap)
        {
            try
            {
                var hardwareArray = mComputer.Hardware;
                for (int i = 0; i < hardwareArray.Count; i++)
                {
                    var sensorArray = hardwareArray[i].Sensors;
                    this.setOSDSensor(sensorArray, SensorType.Load, osdList, osdMap);
                    this.setOSDSensor(sensorArray, SensorType.Clock, osdList, osdMap);
                    this.setOSDSensor(sensorArray, SensorType.Voltage, osdList, osdMap);
                    this.setOSDSensor(sensorArray, SensorType.Data, osdList, osdMap);
                    this.setOSDSensor(sensorArray, SensorType.SmallData, osdList, osdMap);
                    this.setOSDSensor(sensorArray, SensorType.Power, osdList, osdMap);
                    this.setOSDSensor(sensorArray, SensorType.Throughput, osdList, osdMap);

                    var subHardwareArray = hardwareArray[i].SubHardware;
                    for (int j = 0; j < subHardwareArray.Length; j++)
                    {
                        var subSensorArray = subHardwareArray[j].Sensors;
                        this.setOSDSensor(subSensorArray, SensorType.Load, osdList, osdMap);
                        this.setOSDSensor(subSensorArray, SensorType.Clock, osdList, osdMap);
                        this.setOSDSensor(subSensorArray, SensorType.Voltage, osdList, osdMap);
                        this.setOSDSensor(subSensorArray, SensorType.Data, osdList, osdMap);
                        this.setOSDSensor(subSensorArray, SensorType.SmallData, osdList, osdMap);
                        this.setOSDSensor(subSensorArray, SensorType.Power, osdList, osdMap);
                        this.setOSDSensor(subSensorArray, SensorType.Throughput, osdList, osdMap);
                    }
                }
            }
            catch { }
        }

        private void setOSDSensor(ISensor[] sensorArray, SensorType sensorType, List<OSDSensor> osdList, Dictionary<string, OSDSensor> osdMap)
        {
            for (int i = 0; i < sensorArray.Length; i++)
            {
                var sensor = sensorArray[i];
                if (sensorArray[i].SensorType != sensorType)
                {
                    continue;
                }                

                OSDUnitType unitType = OSDUnitType.Unknown;
                string prefix = "";
                switch (sensorArray[i].SensorType)
                {
                    case SensorType.Voltage:
                        unitType = OSDUnitType.Voltage;
                        prefix = "[Voltage] ";
                        break;

                    case SensorType.Power:
                        unitType = OSDUnitType.Power;
                        prefix = "[Power] ";
                        break;

                    case SensorType.Load:
                        unitType = OSDUnitType.Percent;
                        prefix = "[Load] ";
                        break;

                    case SensorType.Clock:
                        unitType = OSDUnitType.MHz;
                        prefix = "[Clock] ";
                        break;

                    case SensorType.Data:
                        unitType = OSDUnitType.GB;
                        prefix = "[Data] ";
                        break;

                    case SensorType.SmallData:
                        unitType = OSDUnitType.MB;
                        prefix = "[Data] ";
                        break;

                    case SensorType.Throughput:
                        unitType = OSDUnitType.MBPerSec;
                        prefix = "[Throughput] ";
                        break;

                    default:
                        unitType = OSDUnitType.Unknown;
                        break;
                }

                if (unitType == OSDUnitType.Unknown)
                    continue;

                string id = string.Format("{0}{1}", mIDPrefixOSD, sensor.Identifier.ToString());
                var osdSensor = new LHMOSDSensor(id, prefix, sensorArray[i].Name, unitType, sensor);
                osdList.Add(osdSensor);
                osdMap.Add(id, osdSensor);
            }
        }

        public void update()
        {
            mComputer.Accept(this);
        }
                        
        /////////////////////////// Visitor ///////////////////////////
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }

        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware)
                subHardware.Accept(this);
        }

        public void VisitSensor(ISensor sensor) { }

        public void VisitParameter(IParameter parameter){}
    }
}
