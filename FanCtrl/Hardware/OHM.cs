using System;
using System.Collections.Generic;
using System.Threading;
using OpenHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class OHM : IVisitor
    {
        private const string mIDPrefixTemperature = "OHM/Temp";
        private const string mIDPrefixFan = "OHM/Fan";
        private const string mIDPrefixControl = "OHM/Control";
        private const string mIDPrefixOSD = "OHM/OSD";        

        private bool mIsStart = false;

        private Computer mComputer = null;

        public OHM() { }

        public void start()
        {
            if (mIsStart == true)
                return;
            mIsStart = true;

            mComputer = new Computer();
            mComputer.CPUEnabled = OptionManager.getInstance().IsOHMCpu;
            mComputer.MainboardEnabled = OptionManager.getInstance().IsOHMMotherboard;
            mComputer.FanControllerEnabled = OptionManager.getInstance().IsOHMContolled;
            mComputer.GPUEnabled = OptionManager.getInstance().IsOHMGpu;
            mComputer.HDDEnabled = OptionManager.getInstance().IsOHMStorage;
            mComputer.RAMEnabled = OptionManager.getInstance().IsOHMMemory;

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
            for (int i = 0; i < hardwareArray.Length; i++)
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
                    var sensor = new OHMTemp(id, sensorArray[j], name);
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
                        var sensor = new OHMTemp(id, subSensorList[k], name);
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
            for (int i = 0; i < hardwareArray.Length; i++)
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
                    var sensor = new OHMFanSpeed(id, sensorArray[j], name);
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
                        var sensor = new OHMFanSpeed(id, subSensorList[k], name);
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
            for (int i = 0; i < hardwareArray.Length; i++)
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
                    var sensor = new OHMControl(id, sensorArray[j], name);
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
                        var sensor = new OHMControl(id, subSensorList[k], name);
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
                for (int i = 0; i < hardwareArray.Length; i++)
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
                var osdSensor = new OHMOSDSensor(id, prefix, sensorArray[i].Name, unitType, sensor);
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

        public void VisitParameter(IParameter parameter) { }
    }
}
