using System;
using System.Collections.Generic;
using System.Threading;
using OpenHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class OHM : IVisitor
    {
        private bool mIsStart = false;

        private Computer mComputer = null;

        public OHM() { }

        public void start()
        {
            if (mIsStart == true)
                return;
            mIsStart = true;

            mComputer = new Computer();
            mComputer.CPUEnabled = true;
            mComputer.RAMEnabled = true;
            mComputer.MainboardEnabled = true;
            mComputer.FanControllerEnabled = true;
            mComputer.GPUEnabled = true;

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

        public void createTemp(ref List<BaseSensor> sensorList)
        {
            bool isNvAPIWrapper = OptionManager.getInstance().IsNvAPIWrapper;

            // CPU, GPU
            int cpuNum = 2;
            int GpuAtiNum = 2;
            int gpuNvidiaNum = 2;
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Length; i++)
            {
                if (hardwareArray[i].HardwareType == HardwareType.CPU)
                {
                    string name = hardwareArray[i].Name;
                    while (this.isExistTemp(ref sensorList, name) == true)
                    {
                        name = hardwareArray[i].Name + " #" + cpuNum++;
                    }
                    var sensor = new HardwareTemp(hardwareArray[i], name);
                    sensorList.Add(sensor);
                }

                if (hardwareArray[i].HardwareType == HardwareType.GpuAti)
                {
                    string name = hardwareArray[i].Name;
                    while (this.isExistTemp(ref sensorList, name) == true)
                    {
                        name = hardwareArray[i].Name + " #" + GpuAtiNum++;
                    }
                    var sensor = new HardwareTemp(hardwareArray[i], name);
                    sensorList.Add(sensor);
                }

                else if (hardwareArray[i].HardwareType == HardwareType.GpuNvidia && isNvAPIWrapper == false)
                {
                    string name = hardwareArray[i].Name;
                    while (this.isExistTemp(ref sensorList, name) == true)
                    {
                        name = hardwareArray[i].Name + " #" + gpuNvidiaNum++;
                    }
                    var sensor = new HardwareTemp(hardwareArray[i], name);
                    sensorList.Add(sensor);
                }
            }
        }

        public void createMotherBoardTemp(ref List<BaseSensor> sensorList)
        {
            // Motherboard
            int num = 1;
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Length; i++)
            {
                if (hardwareArray[i].HardwareType == HardwareType.CPU ||
                    hardwareArray[i].HardwareType == HardwareType.GpuAti ||
                    hardwareArray[i].HardwareType == HardwareType.GpuNvidia)
                {
                    continue;
                }

                var sensorArray = hardwareArray[i].Sensors;
                for (int j = 0; j < sensorArray.Length; j++)
                {
                    if (sensorArray[j].SensorType != OpenHardwareMonitor.Hardware.SensorType.Temperature)
                        continue;

                    string originName = sensorArray[j].Name.ToUpper();
                    if (originName.Contains("CPU") == true)
                        continue;

                    var sensor = new HardwareMotherBoardTemp(sensorArray[j], "Motherboard #" + num++);
                    sensorList.Add(sensor);
                }

                var subHardwareArray = hardwareArray[i].SubHardware;
                for (int j = 0; j < subHardwareArray.Length; j++)
                {
                    var subSensorList = subHardwareArray[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType != OpenHardwareMonitor.Hardware.SensorType.Temperature)
                            continue;

                        string originName = subSensorList[k].Name.ToUpper();
                        if (originName.Contains("CPU") == true)
                            continue;

                        var sensor = new HardwareMotherBoardTemp(subSensorList[k], "Motherboard #" + num++);
                        sensorList.Add(sensor);
                    }
                }
            }
        }

        public void createFan(ref List<BaseSensor> fanList)
        {
            int fanNum = 1;
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Length; i++)
            {
                if (hardwareArray[i].HardwareType == HardwareType.GpuNvidia ||
                    hardwareArray[i].HardwareType == HardwareType.GpuAti)
                {
                    continue;
                }

                var sensorArray = hardwareArray[i].Sensors;
                for (int j = 0; j < sensorArray.Length; j++)
                {
                    if (sensorArray[j].SensorType != OpenHardwareMonitor.Hardware.SensorType.Fan)
                        continue;

                    var fan = new HardwareFanSpeed(sensorArray[j], "Fan #" + fanNum++);
                    fanList.Add(fan);
                }

                var subHardwareArray = hardwareArray[i].SubHardware;
                for (int j = 0; j < subHardwareArray.Length; j++)
                {
                    var subSensorArray = subHardwareArray[j].Sensors;
                    for (int k = 0; k < subSensorArray.Length; k++)
                    {
                        if (subSensorArray[k].SensorType != OpenHardwareMonitor.Hardware.SensorType.Fan)
                            continue;

                        var fan = new HardwareFanSpeed(subSensorArray[k], "Fan #" + fanNum++);
                        fanList.Add(fan);
                    }
                }
            }
        }

        public void createControl(ref List<BaseControl> controlList)
        {
            int fanNum = 1;
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Length; i++)
            {
                if (hardwareArray[i].HardwareType == HardwareType.GpuNvidia ||
                    hardwareArray[i].HardwareType == HardwareType.GpuAti)
                {
                    continue;
                }

                var sensorArray = hardwareArray[i].Sensors;
                for (int j = 0; j < sensorArray.Length; j++)
                {
                    if (sensorArray[j].SensorType != OpenHardwareMonitor.Hardware.SensorType.Control)
                        continue;

                    var control = new HardwareControl(sensorArray[j], "Fan Control #" + fanNum++);
                    controlList.Add(control);
                }

                var subHardwareArray = hardwareArray[i].SubHardware;
                for (int j = 0; j < subHardwareArray.Length; j++)
                {
                    var subSensorList = subHardwareArray[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType != OpenHardwareMonitor.Hardware.SensorType.Control)
                            continue;

                        var control = new HardwareControl(subSensorList[k], "Fan Control #" + fanNum++);
                        controlList.Add(control);
                    }
                }
            }
        }

        public void createGPUFan(ref List<BaseSensor> fanList)
        {
            bool isNvAPIWrapper = OptionManager.getInstance().IsNvAPIWrapper;

            int gpuFanNum = 1;
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Length; i++)
            {
                if ((hardwareArray[i].HardwareType == HardwareType.GpuNvidia && isNvAPIWrapper == false) ||
                    (hardwareArray[i].HardwareType == HardwareType.GpuAti))
                {
                    var sensorArray = hardwareArray[i].Sensors;
                    for (int j = 0; j < sensorArray.Length; j++)
                    {
                        if (sensorArray[j].SensorType == OpenHardwareMonitor.Hardware.SensorType.Fan)
                        {
                            var name = "GPU Fan #" + gpuFanNum++;
                            while (this.isExistFan(ref fanList, name) == true)
                            {
                                name = "GPU Fan #" + gpuFanNum++;
                            }

                            var fan = new HardwareFanSpeed(sensorArray[j], name);
                            fanList.Add(fan);
                        }
                    }

                    var subHardwareArray = hardwareArray[i].SubHardware;
                    for (int j = 0; j < subHardwareArray.Length; j++)
                    {
                        var subSensorList = subHardwareArray[j].Sensors;
                        for (int k = 0; k < subSensorList.Length; k++)
                        {
                            if (subSensorList[k].SensorType == OpenHardwareMonitor.Hardware.SensorType.Fan)
                            {
                                var name = "GPU Fan #" + gpuFanNum++;
                                while (this.isExistFan(ref fanList, name) == true)
                                {
                                    name = "GPU Fan #" + gpuFanNum++;
                                }

                                var fan = new HardwareFanSpeed(subSensorList[k], name);
                                fanList.Add(fan);
                            }
                        }
                    }
                }
            }
        }

        public void createGPUFanControl(ref List<BaseControl> controlList)
        {
            bool isNvAPIWrapper = OptionManager.getInstance().IsNvAPIWrapper;

            int gpuFanNum = 1;
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Length; i++)
            {
                if ((hardwareArray[i].HardwareType == HardwareType.GpuNvidia && isNvAPIWrapper == false) ||
                    (hardwareArray[i].HardwareType == HardwareType.GpuAti))
                {
                    var sensorArray = hardwareArray[i].Sensors;
                    for (int j = 0; j < sensorArray.Length; j++)
                    {
                        if (sensorArray[j].SensorType == OpenHardwareMonitor.Hardware.SensorType.Control)
                        {
                            var name = "GPU Fan Control #" + gpuFanNum++;
                            while (this.isExistControl(ref controlList, name) == true)
                            {
                                name = "GPU Fan Control #" + gpuFanNum++;
                            }

                            var control = new HardwareControl(sensorArray[j], name);
                            controlList.Add(control);
                        }
                    }

                    var subHardwareArray = hardwareArray[i].SubHardware;
                    for (int j = 0; j < subHardwareArray.Length; j++)
                    {
                        var subSensorList = subHardwareArray[j].Sensors;
                        for (int k = 0; k < subSensorList.Length; k++)
                        {
                            if (subSensorList[k].SensorType == OpenHardwareMonitor.Hardware.SensorType.Control)
                            {
                                var name = "GPU Fan Control #" + gpuFanNum++;
                                while (this.isExistControl(ref controlList, name) == true)
                                {
                                    name = "GPU Fan Control #" + gpuFanNum++;
                                }

                                var control = new HardwareControl(subSensorList[k], name);
                                controlList.Add(control);
                            }
                        }
                    }
                }
            }
        }

        public void createOSDSensor(ref List<OSDSensor> osdList)
        {
            try
            {
                bool isNvAPIWrapper = OptionManager.getInstance().IsNvAPIWrapper;
                var hardwareArray = mComputer.Hardware;
                for (int i = 0; i < hardwareArray.Length; i++)
                {
                    if (isNvAPIWrapper == true && hardwareArray[i].HardwareType == HardwareType.GpuNvidia)
                        continue;

                    var sensorArray = hardwareArray[i].Sensors;
                    this.setOSDSensor(sensorArray, SensorType.Load, ref osdList);
                    this.setOSDSensor(sensorArray, SensorType.Clock, ref osdList);
                    this.setOSDSensor(sensorArray, SensorType.Voltage, ref osdList);
                    this.setOSDSensor(sensorArray, SensorType.Data, ref osdList);
                    this.setOSDSensor(sensorArray, SensorType.SmallData, ref osdList);
                    this.setOSDSensor(sensorArray, SensorType.Power, ref osdList);
                    this.setOSDSensor(sensorArray, SensorType.Throughput, ref osdList);

                    var subHardwareArray = hardwareArray[i].SubHardware;
                    for (int j = 0; j < subHardwareArray.Length; j++)
                    {
                        var subSensorArray = subHardwareArray[j].Sensors;
                        this.setOSDSensor(subSensorArray, SensorType.Load, ref osdList);
                        this.setOSDSensor(subSensorArray, SensorType.Clock, ref osdList);
                        this.setOSDSensor(subSensorArray, SensorType.Voltage, ref osdList);
                        this.setOSDSensor(subSensorArray, SensorType.Data, ref osdList);
                        this.setOSDSensor(subSensorArray, SensorType.SmallData, ref osdList);
                        this.setOSDSensor(subSensorArray, SensorType.Power, ref osdList);
                        this.setOSDSensor(subSensorArray, SensorType.Throughput, ref osdList);
                    }
                }
            }
            catch { }
        }

        private void setOSDSensor(ISensor[] sensorArray, SensorType sensorType, ref List<OSDSensor> osdList)
        {
            var sensorList = new List<ISensor>();
            for (int i = 0; i < sensorArray.Length; i++)
            {
                if (sensorArray[i].SensorType != sensorType)
                {
                    continue;
                }
                sensorList.Add(sensorArray[i]);
            }

            for (int i = 0; i < sensorList.Count; i++)
            {
                var sensor = sensorList[i];

                int index = osdList.Count;
                OSDUnitType unitType = OSDUnitType.Unknown;
                string sensorName = "";
                switch (sensorList[i].SensorType)
                {
                    case SensorType.Voltage:
                        unitType = OSDUnitType.Voltage;
                        sensorName = "[Voltage]  ";
                        break;

                    case SensorType.Power:
                        unitType = OSDUnitType.Power;
                        sensorName = "[Power] ";
                        break;

                    case SensorType.Load:
                        unitType = OSDUnitType.Percent;
                        sensorName = "[Load] ";
                        break;

                    case SensorType.Clock:
                        unitType = OSDUnitType.MHz;
                        sensorName = "[Clock] ";
                        break;

                    case SensorType.Data:
                        unitType = OSDUnitType.GB;
                        sensorName = "[Data] ";
                        break;

                    case SensorType.SmallData:
                        unitType = OSDUnitType.MB;
                        sensorName = "[Data] ";
                        break;

                    case SensorType.Throughput:
                        unitType = OSDUnitType.MBPerSec;
                        sensorName = "[Throughput] ";
                        break;

                    default:
                        unitType = OSDUnitType.Unknown;
                        break;
                }

                if (unitType == OSDUnitType.Unknown)
                    continue;

                var osdSensor = new OSDSensor(sensorList[i], unitType, sensorName + sensorList[i].Name, index);
                osdList.Add(osdSensor);
            }
        }

        public void update()
        {
            mComputer.Accept(this);
        }

        private bool isExistTemp(ref List<BaseSensor> sensorList, string name)
        {
            for (int i = 0; i < sensorList.Count; i++)
            {
                if (sensorList[i].Name.Equals(name) == true)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isExistFan(ref List<BaseSensor> fanList, string name)
        {
            for (int i = 0; i < fanList.Count; i++)
            {
                if (fanList[i].Name.Equals(name) == true)
                {
                    return true;
                }
            }
            return false;
        }

        private bool isExistControl(ref List<BaseControl> controlList, string name)
        {
            for (int i = 0; i < controlList.Count; i++)
            {
                if (controlList[i].Name.Equals(name) == true)
                {
                    return true;
                }
            }
            return false;
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
