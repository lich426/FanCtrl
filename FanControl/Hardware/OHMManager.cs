﻿using System;
using System.Collections.Generic;
using System.Threading;
using OpenHardwareMonitor.Hardware;

namespace FanControl
{
    public class OHMManager : IVisitor
    {
        private bool mIsStart = false;

        // LibreHardwareMonitorLib
        private Computer mComputer = null;

        public OHMManager() { }

        public void start()
        {
            if (mIsStart == true)
                return;
            mIsStart = true;

            mComputer = new Computer();
            mComputer.CPUEnabled = true;
            mComputer.GPUEnabled = true;
            mComputer.FanControllerEnabled = true;
            mComputer.MainboardEnabled = true;

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
            // CPU, GPU
            int cpuNum = 2;
            int gpuAmdNum = 2;
            int gpuNvidiaNum = 2;
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Length; i++)
            {
                if (hardwareArray[i].HardwareType == HardwareType.CPU)
                {
                    string name = hardwareArray[i].Name;
                    if (this.isExistTemp(ref sensorList, name) == true)
                    {
                        name = name + " #" + cpuNum++;
                    }
                    var sensor = new HardwareTemp(hardwareArray[i], name);
                    sensorList.Add(sensor);
                }

                else if (hardwareArray[i].HardwareType == HardwareType.GpuAti)
                {
                    string name = hardwareArray[i].Name;
                    if (this.isExistTemp(ref sensorList, name) == true)
                    {
                        name = name + " #" + gpuAmdNum++;
                    }
                    var sensor = new HardwareTemp(hardwareArray[i], name);
                    sensorList.Add(sensor);
                }

                else if (hardwareArray[i].HardwareType == HardwareType.GpuNvidia)
                {
                    string name = hardwareArray[i].Name;
                    if (this.isExistTemp(ref sensorList, name) == true)
                    {
                        name = name + " #" + gpuNvidiaNum++;
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

                    string name = sensorArray[j].Name.ToUpper();
                    if (name.Contains("CPU") == true)
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

                        string name = subSensorList[k].Name.ToUpper();
                        if (name.Contains("CPU") == true)
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
            int otherFanNum = 2;
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

                    if (hardwareArray[i].HardwareType == HardwareType.SuperIO)
                    {
                        var fan = new HardwareFanSpeed(sensorArray[j], "Fan #" + fanNum++);
                        fanList.Add(fan);
                    }
                    else
                    {
                        string name = sensorArray[j].Name;
                        if (this.isExistFan(ref fanList, name) == true)
                        {
                            name = name + " #" + otherFanNum++;
                        }
                        var fan = new HardwareFanSpeed(sensorArray[j], name);
                        fanList.Add(fan);
                    }
                }

                var subHardwareArray = hardwareArray[i].SubHardware;
                for (int j = 0; j < subHardwareArray.Length; j++)
                {
                    var subSensorArray = subHardwareArray[j].Sensors;
                    for (int k = 0; k < subSensorArray.Length; k++)
                    {
                        if (subSensorArray[k].SensorType != OpenHardwareMonitor.Hardware.SensorType.Fan)
                            continue;

                        if (subHardwareArray[j].HardwareType == HardwareType.SuperIO)
                        {
                            var fan = new HardwareFanSpeed(subSensorArray[k], "Fan #" + fanNum++);
                            fanList.Add(fan);
                        }
                        else
                        {
                            string name = subSensorArray[k].Name;
                            if (this.isExistFan(ref fanList, name) == true)
                            {
                                name = name + " #" + otherFanNum++;
                            }
                            var fan = new HardwareFanSpeed(subSensorArray[k], name);
                            fanList.Add(fan);
                        }
                    }
                }
            }
        }

        public void createGPUFan(ref List<BaseSensor> fanList)
        {
            int gpuFanNum = 1;
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Length; i++)
            {
                if (hardwareArray[i].HardwareType == HardwareType.GpuNvidia ||
                    hardwareArray[i].HardwareType == HardwareType.GpuAti)
                {
                    var sensorArray = hardwareArray[i].Sensors;
                    for (int j = 0; j < sensorArray.Length; j++)
                    {
                        if (sensorArray[j].SensorType == OpenHardwareMonitor.Hardware.SensorType.Control)
                        {
                            var fan = new HardwareFanSpeed(sensorArray[j], "GPU Fan #" + gpuFanNum++);
                            fanList.Add(fan);
                        }
                    }
                }
            }
        }

        public void createControl(ref List<BaseControl> controlList)
        {
            int fanNum = 1;
            int otherFanNum = 2;
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
                    if (sensorArray[j].SensorType != OpenHardwareMonitor.Hardware.SensorType.Control || sensorArray[j].Control == null)
                        continue;

                    if (hardwareArray[i].HardwareType == HardwareType.SuperIO)
                    {
                        var control = new HardwareControl(sensorArray[j], "Fan Control #" + fanNum++);
                        controlList.Add(control);
                    }
                    else
                    {
                        string name = sensorArray[j].Name;
                        if (this.isExistControl(ref controlList, name) == true)
                        {
                            name = name + " #" + otherFanNum++;
                        }
                        var control = new HardwareControl(sensorArray[j], name);
                        controlList.Add(control);
                    }
                }

                var subHardwareArray = hardwareArray[i].SubHardware;
                for (int j = 0; j < subHardwareArray.Length; j++)
                {
                    bool isExist2 = false;
                    var subSensorList = subHardwareArray[j].Sensors;
                    for (int k = 0; k < subSensorList.Length; k++)
                    {
                        if (subSensorList[k].SensorType != OpenHardwareMonitor.Hardware.SensorType.Control || subSensorList[k].Control == null)
                            continue;

                        if (subHardwareArray[j].HardwareType == HardwareType.SuperIO)
                        {
                            var control = new HardwareControl(subSensorList[k], "Fan Control #" + fanNum++);
                            controlList.Add(control);
                        }
                        else
                        {
                            string name = subSensorList[k].Name;
                            if (this.isExistControl(ref controlList, name) == true)
                            {
                                name = name + " #" + otherFanNum++;
                            }
                            var control = new HardwareControl(subSensorList[k], name);
                            controlList.Add(control);
                        }
                    }
                }
            }
        }

        public void createGPUFanControl(ref List<BaseControl> controlList)
        {
            int gpuFanNum = 1;
            var hardwareArray = mComputer.Hardware;
            for (int i = 0; i < hardwareArray.Length; i++)
            {
                if (hardwareArray[i].HardwareType == HardwareType.GpuNvidia ||
                    hardwareArray[i].HardwareType == HardwareType.GpuAti)
                {
                    var sensorArray = hardwareArray[i].Sensors;
                    for (int j = 0; j < sensorArray.Length; j++)
                    {
                        if (sensorArray[j].SensorType == OpenHardwareMonitor.Hardware.SensorType.Control)
                        {
                            var control = new HardwareControl(sensorArray[j], "GPU Fan Control #" + gpuFanNum++);
                            controlList.Add(control);
                        }
                    }
                }
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

        public void VisitParameter(IParameter parameter){}
    }
}