using System;
using System.Collections.Generic;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Linq;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using NvAPIWrapper;
using NvAPIWrapper.GPU;
using NvAPIWrapper.Native.GPU;

namespace FanCtrl
{
    public class HardwareManager
    {
        public string mHardwareFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "Hardware.json";

        // Singletone
        private HardwareManager() { }
        private static HardwareManager sManager = new HardwareManager();
        public static HardwareManager getInstance() { return sManager; }

        // Start state
        private bool mIsStart = false;

        // lock
        private object mLock = new object();

        // Mutex
        private Mutex mISABusMutex = null;
        private Mutex mSMBusMutex = null;
        private Mutex mPCIMutex = null;

        // Gigabyte
        private Gigabyte mGigabyte = null;

        // LibreHardwareMonitor
        private LHM mLHM = null;

        // OpenHardwareMonitor
        private OHM mOHM = null;

        // NZXT Kraken
        public List<Kraken> KrakenList { get; } = new List<Kraken>();
        public List<KrakenLCD> KrakenLCDList { get; } = new List<KrakenLCD>();


        // EVGA CLC
        public List<CLC> CLCList { get; } = new List<CLC>();

        // NZXT RGB & Fan Controller
        public List<RGBnFC> RGBnFCList { get; } = new List<RGBnFC>();

        // Temperature sensor
        public List<List<HardwareDevice>> TempList { get; } = new List<List<HardwareDevice>>();
        public List<BaseSensor> TempBaseList { get; } = new List<BaseSensor>();
        public Dictionary<string, BaseSensor> TempBaseMap { get; } = new Dictionary<string, BaseSensor>();

        // Fan
        public List<List<HardwareDevice>> FanList { get; } = new List<List<HardwareDevice>>();
        public List<BaseSensor> FanBaseList { get; } = new List<BaseSensor>();
        public Dictionary<string, BaseSensor> FanBaseMap { get; } = new Dictionary<string, BaseSensor>();

        // Control
        public List<List<HardwareDevice>> ControlList { get; } = new List<List<HardwareDevice>>();
        public List<BaseControl> ControlBaseList { get; } = new List<BaseControl>();
        public Dictionary<string, BaseControl> ControlBaseMap { get; } = new Dictionary<string, BaseControl>();

        // OSD sensor List
        public List<OSDSensor> OSDSensorList { get; } = new List<OSDSensor>();
        public Dictionary<string, OSDSensor> OSDSensorMap { get; } = new Dictionary<string, OSDSensor>();

        // next tick change value
        private List<int> mChangeValueList = new List<int>();
        private List<BaseControl> mChangeControlList = new List<BaseControl>();

        // update timer
        private System.Timers.Timer mUpdateTimer = null;

        public event UpdateTimerEventHandler onUpdateCallback;
        public delegate void UpdateTimerEventHandler();

#if MY_DEBUG
        private int mDebugUpdateCount = 10;
#endif

        public void start()
        {
            Monitor.Enter(mLock);
            if (mIsStart == true)
            {
                Monitor.Exit(mLock);
                return;
            }
            mIsStart = true;

            string mutexName = "Global\\Access_ISABUS.HTP.Method";
            this.createBusMutex(mutexName, ref mISABusMutex);

            mutexName = "Global\\Access_SMBUS.HTP.Method";
            this.createBusMutex(mutexName, ref mSMBusMutex);

            mutexName = "Global\\Access_PCI";
            this.createBusMutex(mutexName, ref mPCIMutex);

            // create list
            for (int i = 0; i < (int)LIBRARY_TYPE.MAX; i++)
            {
                TempList.Add(new List<HardwareDevice>());
                FanList.Add(new List<HardwareDevice>());
                ControlList.Add(new List<HardwareDevice>());
            }

            // Gigabyte
            if (OptionManager.getInstance().IsGigabyte == true)
            {
                mGigabyte = new Gigabyte();
                mGigabyte.LockBus += lockBus;
                mGigabyte.UnlockBus += unlockBus;

                if (mGigabyte.start() == false)
                {
                    mGigabyte = null;
                }
                else
                {
                    var tempList = TempList[(int)LIBRARY_TYPE.Gigabyte];
                    mGigabyte.createTemp(ref tempList);

                    var fanList = FanList[(int)LIBRARY_TYPE.Gigabyte];
                    mGigabyte.createFan(ref fanList);

                    var controlList = ControlList[(int)LIBRARY_TYPE.Gigabyte];
                    mGigabyte.createControl(ref controlList);
                }
            }
            
            // LHM
            if (OptionManager.getInstance().IsLHM == true)
            {
                mLHM = new LHM();
                mLHM.start();

                var tempList = TempList[(int)LIBRARY_TYPE.LHM];
                mLHM.createTemp(ref tempList);

                var fanList = FanList[(int)LIBRARY_TYPE.LHM];
                mLHM.createFan(ref fanList);

                var controlList = ControlList[(int)LIBRARY_TYPE.LHM];
                mLHM.createControl(ref controlList);
            }

            // OHM
            if (OptionManager.getInstance().IsOHM == true)
            {
                mOHM = new OHM();
                mOHM.start();

                var tempList = TempList[(int)LIBRARY_TYPE.OHM];
                mOHM.createTemp(ref tempList);

                var fanList = FanList[(int)LIBRARY_TYPE.OHM];
                mOHM.createFan(ref fanList);

                var controlList = ControlList[(int)LIBRARY_TYPE.OHM];
                mOHM.createControl(ref controlList);
            }

            // NvAPIWrapper
            if(OptionManager.getInstance().IsNvAPIWrapper == true)
            {
                this.lockBus();
                try
                {
                    NVIDIA.Initialize();
                }
                catch { }

                try
                {
                    var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                    for (int i = 0; i < gpuArray.Length; i++)
                    {
                        var gpu = gpuArray[i];
                        var hardwareName = gpu.FullName;

                        // temperature
                        var id = string.Format("NvAPIWrapper/{0}/{1}/Temp", hardwareName, gpu.GPUId);
                        var name = "GPU Core";
                        var temp = new NvAPITemp(id, name, i, NvAPITemp.TEMPERATURE_TYPE.CORE);
                        temp.LockBus += lockBus;
                        temp.UnlockBus += unlockBus;

                        var tempDevice = new HardwareDevice(hardwareName);
                        tempDevice.addDevice(temp);

                        if (gpu.ThermalInformation.HasAnyThermalSensor == true)
                        {
                            if (gpu.ThermalInformation.HotSpotTemperature != 0)
                            {
                                id = string.Format("NvAPIWrapper/{0}/{1}/HotSpotTemp", hardwareName, gpu.GPUId);
                                name = "GPU Hot Spot";
                                temp = new NvAPITemp(id, name, i, NvAPITemp.TEMPERATURE_TYPE.HOTSPOT);
                                temp.LockBus += lockBus;
                                temp.UnlockBus += unlockBus;
                                tempDevice.addDevice(temp);
                            }

                            if (gpu.ThermalInformation.MemoryJunctionTemperature != 0)
                            {
                                id = string.Format("NvAPIWrapper/{0}/{1}/MemoryJunctionTemp", hardwareName, gpu.GPUId);
                                name = "GPU Memory Junction";
                                temp = new NvAPITemp(id, name, i, NvAPITemp.TEMPERATURE_TYPE.MEMORY);
                                temp.LockBus += lockBus;
                                temp.UnlockBus += unlockBus;
                                tempDevice.addDevice(temp);
                            }
                        }

                        var tempList = TempList[(int)LIBRARY_TYPE.NvAPIWrapper];
                        tempList.Add(tempDevice);

                        var fanDevice = new HardwareDevice(hardwareName);
                        var controlDevice = new HardwareDevice(hardwareName);

                        int num = 1;
                        var e = gpuArray[i].CoolerInformation.Coolers.GetEnumerator();
                        while (e.MoveNext())
                        {
                            var value = e.Current;
                            int coolerID = value.CoolerId;
                            int speed = value.CurrentLevel;
                            int minSpeed = value.DefaultMinimumLevel;
                            int maxSpeed = value.DefaultMaximumLevel;
                            CoolerPolicy policy = value.DefaultPolicy;

                            // fan
                            id = string.Format("NvAPIWrapper/{0}/{1}/Fan/{2}", hardwareName, gpu.GPUId, coolerID);
                            name = "GPU Fan #" + num;
                            var fan = new NvAPIFanSpeed(id, name, i, coolerID);
                            fan.LockBus += lockBus;
                            fan.UnlockBus += unlockBus;
                            fanDevice.addDevice(fan);

                            // control
                            id = string.Format("NvAPIWrapper/{0}/{1}/Control/{2}", hardwareName, gpu.GPUId, coolerID);
                            name = "GPU Fan #" + num;
                            var control = new NvAPIFanControl(id, name, i, coolerID, speed, minSpeed, maxSpeed, policy);
                            control.LockBus += lockBus;
                            control.UnlockBus += unlockBus;
                            controlDevice.addDevice(control);
                            num++;
                        }

                        if (fanDevice.DeviceList.Count > 0)
                        {
                            var fanList = FanList[(int)LIBRARY_TYPE.NvAPIWrapper];
                            fanList.Add(fanDevice);
                        }

                        if (controlDevice.DeviceList.Count > 0)
                        {
                            var controlList = ControlList[(int)LIBRARY_TYPE.NvAPIWrapper];
                            controlList.Add(controlDevice);
                        }
                    }
                }
                catch { }
                this.unlockBus();
            }

            // DIMM thermal sensor
            if (OptionManager.getInstance().IsDimm == true)
            {
                this.lockSMBus(0);
                if (SMBusController.open() == true)
                {
                    int num = 1;
                    var device = new HardwareDevice("DIMM");

                    // 0x18 ~ 0x1F
                    for (byte addr = 0x18; addr < 0x20; addr++)
                    {
                        byte data = SMBusController.smbusDetect(addr);
                        if (data == addr)
                        {
                            var id = string.Format("DIMM/0/{0}", addr);
                            var temp = new DimmTemp(id, "DIMM #" + num++, addr);
                            temp.LockBus += lockSMBus;
                            temp.UnlockBus += unlockSMBus;
                            device.addDevice(temp);
                        }
                        Util.sleep(10);
                    }

                    if (device.DeviceList.Count > 0)
                    {
                        var tempList = TempList[(int)LIBRARY_TYPE.DIMM];
                        tempList.Add(device);
                    }
                }
                this.unlockSMBus();
            }

            if (OptionManager.getInstance().IsKrakenLCD == true)
            {
                // Z3 LCD
                uint devCount = LibUSBController.getDeviceCount(USBVendorID.NZXT, USBProductID.KrakenZ3);
                for (uint i = 0; i < devCount; i++)
                {
                    var krakenLCD = new KrakenLCD();
                    if (krakenLCD.start(i, USBProductID.KrakenZ3) == true)
                    {
                        KrakenLCDList.Add(krakenLCD);

                        krakenLCD.send();
                    }
                }
            }

            // NZXT Kraken
            if (OptionManager.getInstance().IsKraken == true)
            {                
                try
                {
                    uint num = 1;
                    var defPumpDuty = 100;
                    var defFanDuty = 50;
                    var controlManager = ControlManager.getInstance();

                    // X2
                    var tempDevice = new HardwareDevice("NZXT Kraken X2");
                    var fanDevice = new HardwareDevice("NZXT Kraken X2");
                    var controlDevice = new HardwareDevice("NZXT Kraken X2");
                    uint devCount = HidUSBController.getDeviceCount(USBVendorID.NZXT, USBProductID.KrakenX2);
                    for (uint i = 0; i < devCount; i++)
                    {
                        var kraken = new Kraken(defPumpDuty, defFanDuty);
                        if (kraken.start(i, USBProductID.KrakenX2) == true)
                        {
                            KrakenList.Add(kraken);

                            var id = string.Format("NZXT/KrakenX2/{0}/Temp", i);
                            var temp = new KrakenLiquidTemp(id, kraken, num);
                            tempDevice.addDevice(temp);

                            id = string.Format("NZXT/KrakenX2/{0}/Fan", i);
                            var fan = new KrakenFanSpeed(id, kraken, num);
                            fanDevice.addDevice(fan);

                            id = string.Format("NZXT/KrakenX2/{0}/Pump", i);
                            var pump = new KrakenPumpSpeed(id, kraken, num);
                            fanDevice.addDevice(pump);

                            id = string.Format("NZXT/KrakenX2/{0}/Control/Fan", i);
                            var fanControl = new KrakenFanControl(id, kraken, num);
                            controlDevice.addDevice(fanControl);
                            this.addChangeValue(defFanDuty, fanControl, false);

                            id = string.Format("NZXT/KrakenX2/{0}/Control/Pump", i);
                            var pumpControl = new KrakenPumpControl(id, kraken, num);
                            controlDevice.addDevice(pumpControl);
                            this.addChangeValue(defPumpDuty, pumpControl, false);

                            num++;
                        }
                    }

                    if (tempDevice.DeviceList.Count > 0)
                    {
                        var tempList = TempList[(int)LIBRARY_TYPE.NZXT_Kraken];
                        tempList.Add(tempDevice);
                    }

                    if (fanDevice.DeviceList.Count > 0)
                    {
                        var fanList = FanList[(int)LIBRARY_TYPE.NZXT_Kraken];
                        fanList.Add(fanDevice);
                    }

                    if (controlDevice.DeviceList.Count > 0)
                    {
                        var controlList = ControlList[(int)LIBRARY_TYPE.NZXT_Kraken];
                        controlList.Add(controlDevice);
                    }

                    // X3
                    tempDevice = new HardwareDevice("NZXT Kraken X3");
                    fanDevice = new HardwareDevice("NZXT Kraken X3");
                    controlDevice = new HardwareDevice("NZXT Kraken X3");
                    devCount = HidUSBController.getDeviceCount(USBVendorID.NZXT, USBProductID.KrakenX3);
                    for (uint i = 0; i < devCount; i++)
                    {
                        var kraken = new Kraken(defPumpDuty, defFanDuty);
                        if (kraken.start(i, USBProductID.KrakenX3) == true)
                        {
                            KrakenList.Add(kraken);

                            var id = string.Format("NZXT/KrakenX3/{0}/Temp", i);
                            var temp = new KrakenLiquidTemp(id, kraken, num);
                            tempDevice.addDevice(temp);

                            id = string.Format("NZXT/KrakenX3/{0}/Pump", i);
                            var pump = new KrakenPumpSpeed(id, kraken, num);
                            fanDevice.addDevice(pump);

                            id = string.Format("NZXT/KrakenX3/{0}/Control/Pump", i);
                            var pumpControl = new KrakenPumpControl(id, kraken, num);
                            controlDevice.addDevice(pumpControl);
                            this.addChangeValue(defPumpDuty, pumpControl, false);

                            num++;
                        }
                    }

                    if (tempDevice.DeviceList.Count > 0)
                    {
                        var tempList = TempList[(int)LIBRARY_TYPE.NZXT_Kraken];
                        tempList.Add(tempDevice);
                    }

                    if (fanDevice.DeviceList.Count > 0)
                    {
                        var fanList = FanList[(int)LIBRARY_TYPE.NZXT_Kraken];
                        fanList.Add(fanDevice);
                    }

                    if (controlDevice.DeviceList.Count > 0)
                    {
                        var controlList = ControlList[(int)LIBRARY_TYPE.NZXT_Kraken];
                        controlList.Add(controlDevice);
                    }                                     

                    // Z3
                    tempDevice = new HardwareDevice("NZXT Kraken Z3");
                    fanDevice = new HardwareDevice("NZXT Kraken Z3");
                    controlDevice = new HardwareDevice("NZXT Kraken Z3");
                    devCount = HidUSBController.getDeviceCount(USBVendorID.NZXT, USBProductID.KrakenZ3);
                    for (uint i = 0; i < devCount; i++)
                    {
                        // 해당 모드에 기록되어있는 팬속도와 펌프속도의 최소값을 기본값으로 설정한다.
                        var ctrDataList = controlManager.getControlDataList(controlManager.ModeType);
                        foreach (var ctrData in ctrDataList)
                        {
                            foreach (var fanData in ctrData.FanDataList)
                            {
                                if (fanData.ID == string.Format("NZXT/KrakenZ3/{0}/Control/Fan", i))
                                {
                                    defFanDuty = fanData.ValueList.Min();
                                }
                                else if (fanData.ID == string.Format("NZXT/KrakenZ3/{0}/Control/Pump", i))
                                {
                                    defPumpDuty = fanData.ValueList.Min();
                                }
                            }
                        }

                        var kraken = new Kraken(defPumpDuty, defFanDuty);
                        if (kraken.start(i, USBProductID.KrakenZ3) == true)
                        {
                            KrakenList.Add(kraken);                            

                            var id = string.Format("NZXT/KrakenZ3/{0}/Temp", i);
                            var temp = new KrakenLiquidTemp(id, kraken, num);
                            tempDevice.addDevice(temp);

                            id = string.Format("NZXT/KrakenZ3/{0}/Fan", i);
                            var fan = new KrakenFanSpeed(id, kraken, num);
                            fanDevice.addDevice(fan);

                            id = string.Format("NZXT/KrakenZ3/{0}/Pump", i);
                            var pump = new KrakenPumpSpeed(id, kraken, num);
                            fanDevice.addDevice(pump);

                            id = string.Format("NZXT/KrakenZ3/{0}/Control/Fan", i);
                            var fanControl = new KrakenFanControl(id, kraken, num);
                            controlDevice.addDevice(fanControl);
                            this.addChangeValue(defFanDuty, fanControl, false);

                            id = string.Format("NZXT/KrakenZ3/{0}/Control/Pump", i);
                            var pumpControl = new KrakenPumpControl(id, kraken, num);
                            controlDevice.addDevice(pumpControl);
                            this.addChangeValue(defPumpDuty, pumpControl, false);

                            num++;
                        }
                    }

                    if (tempDevice.DeviceList.Count > 0)
                    {
                        var tempList = TempList[(int)LIBRARY_TYPE.NZXT_Kraken];
                        tempList.Add(tempDevice);
                    }

                    if (fanDevice.DeviceList.Count > 0)
                    {
                        var fanList = FanList[(int)LIBRARY_TYPE.NZXT_Kraken];
                        fanList.Add(fanDevice);
                    }

                    if (controlDevice.DeviceList.Count > 0)
                    {
                        var controlList = ControlList[(int)LIBRARY_TYPE.NZXT_Kraken];
                        controlList.Add(controlDevice);
                    }
                }
                catch { }
            }

            // EVGA CLC
            if (OptionManager.getInstance().IsCLC == true)
            {
                try
                {
                    uint num = 1;
                    uint clcIndex = 0;

                    // SiUSBController
                    var tempDevice = new HardwareDevice("EVGA CLC");
                    var fanDevice = new HardwareDevice("EVGA CLC");
                    var controlDevice = new HardwareDevice("EVGA CLC");
                    uint devCount = SiUSBController.getDeviceCount(USBVendorID.ASETEK, USBProductID.CLC);
                    for (uint i = 0; i < devCount; i++)
                    {
                        var clc = new CLC();
                        if (clc.start(true, clcIndex, i) == true)
                        {
                            CLCList.Add(clc);

                            var id = string.Format("EVGA/CLC/{0}/Temp", i);
                            var temp = new CLCLiquidTemp(id, clc, num);
                            tempDevice.addDevice(temp);

                            id = string.Format("EVGA/CLC/{0}/Fan", i);
                            var fan = new CLCFanSpeed(id, clc, num);
                            fanDevice.addDevice(fan);

                            id = string.Format("EVGA/CLC/{0}/Pump", i);
                            var pump = new CLCPumpSpeed(id, clc, num);
                            fanDevice.addDevice(pump);

                            id = string.Format("EVGA/CLC/{0}/Control/Fan", i);
                            var fanControl = new CLCFanControl(id, clc, num);
                            controlDevice.addDevice(fanControl);
                            this.addChangeValue(25, fanControl, false);

                            id = string.Format("EVGA/CLC/{0}/Control/Pump", i);
                            var pumpControl = new CLCPumpControl(id, clc, num);
                            controlDevice.addDevice(pumpControl);
                            this.addChangeValue(50, pumpControl, false);

                            clcIndex++;
                            num++;
                        }
                    }

                    if (tempDevice.DeviceList.Count > 0)
                    {
                        var tempList = TempList[(int)LIBRARY_TYPE.EVGA_CLC];
                        tempList.Add(tempDevice);
                    }

                    if (fanDevice.DeviceList.Count > 0)
                    {
                        var fanList = FanList[(int)LIBRARY_TYPE.EVGA_CLC];
                        fanList.Add(fanDevice);
                    }

                    if (controlDevice.DeviceList.Count > 0)
                    {
                        var controlList = ControlList[(int)LIBRARY_TYPE.EVGA_CLC];
                        controlList.Add(controlDevice);
                    }

                    if (WinUSBController.init() == true)
                    {
                        tempDevice = new HardwareDevice("EVGA CLC");
                        fanDevice = new HardwareDevice("EVGA CLC");
                        controlDevice = new HardwareDevice("EVGA CLC");

                        // WinUSBController
                        devCount = WinUSBController.getDeviceCount(USBVendorID.ASETEK, USBProductID.CLC);
                        for (uint i = 0; i < devCount; i++)
                        {
                            var clc = new CLC();
                            if (clc.start(false, clcIndex, i) == true)
                            {
                                CLCList.Add(clc);

                                var id = string.Format("EVGA/CLC/{0}/Temp", i);
                                var temp = new CLCLiquidTemp(id, clc, num);
                                tempDevice.addDevice(temp);

                                id = string.Format("EVGA/CLC/{0}/Fan", i);
                                var fan = new CLCFanSpeed(id, clc, num);
                                fanDevice.addDevice(fan);

                                id = string.Format("EVGA/CLC/{0}/Pump", i);
                                var pump = new CLCPumpSpeed(id, clc, num);
                                fanDevice.addDevice(pump);

                                id = string.Format("EVGA/CLC/{0}/Control/Fan", i);
                                var fanControl = new CLCFanControl(id, clc, num);
                                controlDevice.addDevice(fanControl);
                                this.addChangeValue(25, fanControl, false);

                                id = string.Format("EVGA/CLC/{0}/Control/Pump", i);
                                var pumpControl = new CLCPumpControl(id, clc, num);
                                controlDevice.addDevice(pumpControl);
                                this.addChangeValue(50, pumpControl, false);

                                clcIndex++;
                                num++;
                            }
                        }

                        if (tempDevice.DeviceList.Count > 0)
                        {
                            var tempList = TempList[(int)LIBRARY_TYPE.EVGA_CLC];
                            tempList.Add(tempDevice);
                        }

                        if (fanDevice.DeviceList.Count > 0)
                        {
                            var fanList = FanList[(int)LIBRARY_TYPE.EVGA_CLC];
                            fanList.Add(fanDevice);
                        }

                        if (controlDevice.DeviceList.Count > 0)
                        {
                            var controlList = ControlList[(int)LIBRARY_TYPE.EVGA_CLC];
                            controlList.Add(controlDevice);
                        }
                    }
                }
                catch { }
            }

            if (OptionManager.getInstance().IsRGBnFC == true)
            {
                try
                {
                    var fanDevice = new HardwareDevice("NZXT RGB & Fan Controller");
                    var controlDevice = new HardwareDevice("NZXT RGB & Fan Controller");
                    uint num = 1;
                    uint devCount = HidUSBController.getDeviceCount(USBVendorID.NZXT, USBProductID.RGBAndFanController);
                    for (uint i = 0; i < devCount; i++)
                    {
                        var rgb = new RGBnFC();
                        if (rgb.start(i) == true)
                        {
                            RGBnFCList.Add(rgb);

                            for (int j = 0; j < RGBnFC.MAX_FAN_COUNT; j++)
                            {
                                var id = string.Format("NZXT/RGBnFC/{0}/Fan/{1}", i, j);
                                var fan = new RGBnFCFanSpeed(id, rgb, j, num);
                                fanDevice.addDevice(fan);

                                id = string.Format("NZXT/RGBnFC/{0}/Control/{1}", i, j);
                                var control = new RGBnFCControl(id, rgb, j, num);
                                controlDevice.addDevice(control);
                                this.addChangeValue(control.getMinSpeed(), control, false);

                                num++;
                            }
                        }
                    }

                    if (fanDevice.DeviceList.Count > 0)
                    {
                        var fanList = FanList[(int)LIBRARY_TYPE.RGBnFC];
                        fanList.Add(fanDevice);
                    }

                    if (controlDevice.DeviceList.Count > 0)
                    {
                        var controlList = ControlList[(int)LIBRARY_TYPE.RGBnFC];
                        controlList.Add(controlDevice);
                    }                    
                }
                catch { }
            }

            if (OptionManager.getInstance().IsHWInfo == true)
            {
                try
                {
                    HWInfoManager.getInstance().start();

                    var tempList = TempList[(int)LIBRARY_TYPE.HWiNFO];
                    HWInfoManager.getInstance().createTemp(ref tempList);

                    var fanList = FanList[(int)LIBRARY_TYPE.HWiNFO];
                    HWInfoManager.getInstance().createFan(ref fanList);
                }
                catch { }
            }

            for (int i = 0; i < TempList.Count; i++)
            {
                var deviceList = TempList[i];
                for (int j = 0; j < deviceList.Count; j++)
                {
                    var device = deviceList[j];
                    for (int k = 0; k < device.DeviceList.Count; k++)
                    {
                        var temp = device.DeviceList[k];
                        TempBaseList.Add((BaseSensor)temp);
                        TempBaseMap.Add(temp.ID, (BaseSensor)temp);
                    }
                }
            }
            for (int i = 0; i < FanList.Count; i++)
            {
                var deviceList = FanList[i];
                for (int j = 0; j < deviceList.Count; j++)
                {
                    var device = deviceList[j];
                    for (int k = 0; k < device.DeviceList.Count; k++)
                    {
                        var fan = device.DeviceList[k];
                        FanBaseList.Add((BaseSensor)fan);
                        FanBaseMap.Add(fan.ID, (BaseSensor)fan);
                    }
                }
            }
            for (int i = 0; i < ControlList.Count; i++)
            {
                var deviceList = ControlList[i];
                for (int j = 0; j < deviceList.Count; j++)
                {
                    var device = deviceList[j];
                    for (int k = 0; k < device.DeviceList.Count; k++)
                    {
                        var control = device.DeviceList[k];
                        ControlBaseList.Add((BaseControl)control);
                        ControlBaseMap.Add(control.ID, (BaseControl)control);
                    }
                }
            }

            // osd sensor
            this.createOSDSensor();

            Monitor.Exit(mLock);
        }

        public void stop()
        {
            Monitor.Enter(mLock);
            if (mIsStart == false)
            {
                Monitor.Exit(mLock);
                return;
            }
            mIsStart = false;

            if (mUpdateTimer != null)
            {
                mUpdateTimer.Stop();
                mUpdateTimer.Dispose();
                mUpdateTimer = null;
            }

            // restore fan control
            for (int i = 0; i < ControlBaseList.Count; i++)
            {
                var control = ControlBaseList[i];
                control.setAuto();
            }

            if (mGigabyte != null)
            {
                mGigabyte.stop();
                mGigabyte = null;
            }

            if (mLHM != null)
            {
                mLHM.stop();
                mLHM = null;
            }

            if (mOHM != null)
            {
                mOHM.stop();
                mOHM = null;
            }

            for (int i = 0; i < KrakenList.Count; i++)
            {
                try
                {
                    KrakenList[i].stop();
                }
                catch { }
            }
            KrakenList.Clear();

            for (int i = 0; i < CLCList.Count; i++)
            {
                try
                {
                    CLCList[i].stop();
                }
                catch { }
            }
            CLCList.Clear();

            for (int i = 0; i < RGBnFCList.Count; i++)
            {
                try
                {
                    RGBnFCList[i].stop();
                }
                catch { }
            }
            RGBnFCList.Clear();

            if (OptionManager.getInstance().IsNvAPIWrapper == true)
            {
                try
                {
                    NVIDIA.Unload();
                }
                catch { }
            }                

            HWInfoManager.getInstance().stop();

            mChangeControlList.Clear();
            mChangeValueList.Clear();

            TempList.Clear();
            FanList.Clear();
            ControlList.Clear();

            TempBaseList.Clear();
            FanBaseList.Clear();
            ControlBaseList.Clear();

            TempBaseMap.Clear();
            FanBaseMap.Clear();
            ControlBaseMap.Clear();

            OSDSensorList.Clear();
            OSDSensorMap.Clear();

            SMBusController.close();

            if (mISABusMutex != null)
            {
                mISABusMutex.Close();
                mISABusMutex = null;
            }

            if (mSMBusMutex != null)
            {
                mSMBusMutex.Close();
                mSMBusMutex = null;
            }

            if (mPCIMutex != null)
            {
                mPCIMutex.Close();
                mPCIMutex = null;
            }

            OSDController.release();
            WinUSBController.exit();         

            Monitor.Exit(mLock);
        }

        public void startUpdate()
        {
            Monitor.Enter(mLock);
            if (mIsStart == false)
            {
                Monitor.Exit(mLock);
                return;
            }

            mUpdateTimer = new System.Timers.Timer();
            mUpdateTimer.Interval = OptionManager.getInstance().Interval;
            mUpdateTimer.Elapsed += onUpdateTimer;
            mUpdateTimer.Start();

            Monitor.Exit(mLock);
        }

        public void restartTimer()
        {
            Monitor.Enter(mLock);
            if (mIsStart == false)
            {
                Monitor.Exit(mLock);
                return;
            }

            if (mUpdateTimer != null)
            {
                mUpdateTimer.Stop();
                mUpdateTimer.Dispose();
                mUpdateTimer = null;
            }

            mUpdateTimer = new System.Timers.Timer();
            mUpdateTimer.Interval = OptionManager.getInstance().Interval;
            mUpdateTimer.Elapsed += onUpdateTimer;
            mUpdateTimer.Start();
            Monitor.Exit(mLock);
        }

        private void createBusMutex(string mutexName, ref Mutex mutex)
        {
            try
            {
                //mutex permissions set to everyone to allow other software to access the hardware
                //otherwise other monitoring software cant access
                var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
                var securitySettings = new MutexSecurity();
                securitySettings.AddAccessRule(allowEveryoneRule);
                mutex = new Mutex(false, mutexName, out _, securitySettings);
            }
            catch (UnauthorizedAccessException)
            {
                try
                {
                    mutex = Mutex.OpenExisting(mutexName, MutexRights.Synchronize);
                }
                catch { }
            }
        }

        private void lockBus()
        {
            try
            {
                mISABusMutex.WaitOne();
            }
            catch { }
            try
            {
                mPCIMutex.WaitOne();
            }
            catch { }            
        }

        private void unlockBus()
        {
            try
            {
                mISABusMutex.ReleaseMutex();
            }
            catch { }
            try
            {
                mPCIMutex.ReleaseMutex();
            }
            catch { }
        }

        private bool lockSMBus(int ms)
        {
            try
            {
                if (ms <= 0)
                    return mSMBusMutex.WaitOne();
                return mSMBusMutex.WaitOne(ms, false);
            }
            catch { }            
            return false;
        }

        private void unlockSMBus()
        {
            try
            {
                mSMBusMutex.ReleaseMutex();
            }
            catch { }
        }        
        
        private void createOSDSensor()
        {
            // Temp
            for (int i = 0; i < TempBaseList.Count; i++)
            {
                var device = TempBaseList[i];
                string id = device.ID;
                string prefix = "[" + StringLib.Temperature + "] ";
                string name = device.Name;
                var osdSensor = new OSDSensor(id, prefix, name, OSDUnitType.Temperature);
                OSDSensorList.Add(osdSensor);
                OSDSensorMap.Add(id, osdSensor);
            }

            // Fan
            for (int i = 0; i < FanBaseList.Count; i++)
            {
                var device = FanBaseList[i];
                string id = device.ID;
                string prefix = "[" + StringLib.Fan_speed + "] ";
                string name = device.Name;
                var osdSensor = new OSDSensor(id, prefix, name, OSDUnitType.RPM);
                OSDSensorList.Add(osdSensor);
                OSDSensorMap.Add(id, osdSensor);
            }

            // Control
            for (int i = 0; i < ControlBaseList.Count; i++)
            {
                var device = ControlBaseList[i];
                string id = device.ID;
                string prefix = "[" + StringLib.Fan_control + "] ";
                string name = device.Name;
                var osdSensor = new OSDSensor(id, prefix, name, OSDUnitType.Percent);
                OSDSensorList.Add(osdSensor);
                OSDSensorMap.Add(id, osdSensor);
            }

            // Framerate
            string id2 = "OSD/Framerate";
            string prefix2 = "[" + StringLib.ETC + "] ";
            string name2 = "Framerate";
            var osdSensor2 = new OSDSensor(id2, prefix2, name2,  OSDUnitType.FPS);
            OSDSensorList.Add(osdSensor2);
            OSDSensorMap.Add(id2, osdSensor2);

            // Blank
            id2 = "OSD/Blank";
            name2 = "Blank";
            osdSensor2 = new OSDSensor(id2, prefix2, name2, OSDUnitType.Blank);
            OSDSensorList.Add(osdSensor2);
            OSDSensorMap.Add(id2, osdSensor2);

            //////////////// other sensor ////////////////
            // LHM
            if (OptionManager.getInstance().IsLHM == true && mLHM != null)
            {
                mLHM.createOSDSensor(OSDSensorList, OSDSensorMap);
            }

            // OHM
            if (OptionManager.getInstance().IsOHM == true && mOHM != null)
            {
                mOHM.createOSDSensor(OSDSensorList, OSDSensorMap);
            }

            // NvAPIWrapper
            if (OptionManager.getInstance().IsNvAPIWrapper == true)
            {
                this.lockBus();
                try
                {
                    string idPrefix = "NvAPIWrapper/OSD";
                    var gpuArray = PhysicalGPU.GetPhysicalGPUs();
                    for (int i = 0; i < gpuArray.Length; i++)
                    {
                        int subIndex = 0;

                        string id = string.Format("{0}/{1}/{2}", idPrefix, gpuArray[i].GPUId, subIndex);
                        string prefix = "[Clock] ";
                        string name = "GPU Graphics";
                        var osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.kHz, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);

                        id = string.Format("{0}/{1}/{2}", idPrefix, i, subIndex);
                        prefix = "[Clock] ";
                        name = "GPU Memory";
                        osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.kHz, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);

                        id = string.Format("{0}/{1}/{2}", idPrefix, i, subIndex);
                        prefix = "[Clock] ";
                        name = "GPU Processor";
                        osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.kHz, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);

                        id = string.Format("{0}/{1}/{2}", idPrefix, i, subIndex);
                        prefix = "[Clock] ";
                        name = "GPU Video Decoding";
                        osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.kHz, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);

                        id = string.Format("{0}/{1}/{2}", idPrefix, i, subIndex);
                        prefix = "[Load] ";
                        name = "GPU Core";
                        osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.Percent, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);

                        id = string.Format("{0}/{1}/{2}", idPrefix, i, subIndex);
                        prefix = "[Load] ";
                        name = "GPU Frame Buffer";
                        osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.Percent, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);

                        id = string.Format("{0}/{1}/{2}", idPrefix, i, subIndex);
                        prefix = "[Load] ";
                        name = "GPU Video Engine";
                        osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.Percent, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);

                        id = string.Format("{0}/{1}/{2}", idPrefix, i, subIndex);
                        prefix = "[Load] ";
                        name = "GPU Bus Interface";
                        osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.Percent, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);

                        id = string.Format("{0}/{1}/{2}", idPrefix, i, subIndex);
                        prefix = "[Load] ";
                        name = "GPU Memory";
                        osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.Percent, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);

                        id = string.Format("{0}/{1}/{2}", idPrefix, i, subIndex);
                        prefix = "[Data] ";
                        name = "GPU Memory Free";
                        osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.KB, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);

                        id = string.Format("{0}/{1}/{2}", idPrefix, i, subIndex);
                        prefix = "[Data] ";
                        name = "GPU Memory Used";
                        osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.KB, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);

                        id = string.Format("{0}/{1}/{2}", idPrefix, i, subIndex);
                        prefix = "[Data] ";
                        name = "GPU Memory Total";
                        osdSensor = new NvAPIOSDSensor(id, prefix, name, OSDUnitType.KB, i, subIndex++);
                        osdSensor.LockBus += lockBus;
                        osdSensor.UnlockBus += unlockBus;
                        OSDSensorList.Add(osdSensor);
                        OSDSensorMap.Add(id, osdSensor);
                    }
                }
                catch { }
                this.unlockBus();
            }

            if (OptionManager.getInstance().IsHWInfo == true)
            {
                HWInfoManager.getInstance().createOSDSensor(OSDSensorList, OSDSensorMap);
            }
        }

        private void onUpdateTimer(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(mLock) == false)
                return;

#if MY_DEBUG
            if (ControlManager.getInstance().ModeType == MODE_TYPE.PERFORMANCE)
            {
                if (mDebugUpdateCount <= 0)
                {
                    Monitor.Exit(mLock);
                    return;
                }
                mDebugUpdateCount--;
            }
            else
            {
                mDebugUpdateCount = 10;
            }
#endif

            try
            {
                if (mGigabyte != null)
                {
                    mGigabyte.update();
                }
            }
            catch { }

            try
            {
                if (mLHM != null)
                {
                    mLHM.update();
                }
            }
            catch { }

            try
            {
                if (mOHM != null)
                {
                    mOHM.update();
                }
            }
            catch { }

            try
            {
                for (int i = 0; i < (int)LIBRARY_TYPE.MAX; i++)
                {
                    var tempList = TempList[i];
                    for (int j = 0; j < tempList.Count; j++)
                    {
                        var deviceList = tempList[j].DeviceList;
                        for (int k = 0; k < deviceList.Count; k++)
                        {
                            var temp = deviceList[k];
                            temp.update();
                        }
                    }

                    var fanList = FanList[i];
                    for (int j = 0; j < fanList.Count; j++)
                    {
                        var deviceList = fanList[j].DeviceList;
                        for (int k = 0; k < deviceList.Count; k++)
                        {
                            var fan = deviceList[k];
                            fan.update();
                        }
                    }

                    var controlList = ControlList[i];
                    for (int j = 0; j < controlList.Count; j++)
                    {
                        var deviceList = controlList[j].DeviceList;
                        for (int k = 0; k < deviceList.Count; k++)
                        {
                            var control = deviceList[k];
                            control.update();
                        }
                    }
                }
            }
            catch { }

            try
            {
                // change value
                bool isExistChange = false;
                if (mChangeValueList.Count > 0)
                {
                    for (int i = 0; i < mChangeControlList.Count; i++)
                    {
                        isExistChange = true;
                        mChangeControlList[i].setSpeed(mChangeValueList[i]);
                    }
                    mChangeControlList.Clear();
                    mChangeValueList.Clear();
                }

                // Control
                var manualControlDictionary = new Dictionary<string, BaseControl>();
                var autoControlDictionary = new Dictionary<string, BaseControl>();

                var controlManager = ControlManager.getInstance();
                if (controlManager.IsEnable == true && isExistChange == false)
                {
                    var controlDataList = controlManager.getControlDataList(controlManager.ModeType);
                    for (int i = 0; i < controlDataList.Count; i++)
                    {
                        var controlData = controlDataList[i];
                        if (controlData == null)
                            break;

                        string tempID = controlData.ID;
                        if (TempBaseMap.ContainsKey(tempID) == false)
                            continue;

                        var tempDevice = TempBaseMap[tempID];
                        int temperature = tempDevice.Value;

                        for (int j = 0; j < controlData.FanDataList.Count; j++)
                        {
                            var fanData = controlData.FanDataList[j];

                            string fanID = fanData.ID;
                            if (ControlBaseMap.ContainsKey(fanID) == false)
                                continue;

                            var controlDevice = ControlBaseMap[fanID];

                            bool isAuto = false;
                            int percent = fanData.getValue(temperature, ref isAuto);

                            // auto mode
                            if (isAuto == true)
                            {
                                autoControlDictionary.Add(fanID, controlDevice);
                            }

                            // manual mode
                            else
                            {
                                // remove auto mode control
                                autoControlDictionary.Remove(fanID);

                                if (manualControlDictionary.ContainsKey(fanID) == false)
                                {
                                    manualControlDictionary.Add(fanID, controlDevice);
                                    controlDevice.NextValue = percent;
                                }
                                else
                                {
                                    controlDevice.NextValue = (controlDevice.NextValue >= percent) ? controlDevice.NextValue : percent;
                                }
                            }
                        }
                    }

                    foreach (var keyPair in manualControlDictionary)
                    {
                        var control = keyPair.Value;

                        // remove auto mode control
                        autoControlDictionary.Remove(control.ID);

                        // Console.WriteLine("manual mode : name({0}), value({1}), nextvalue({2})", control.Name, control.Value, control.NextValue);

                        if (control.Value == control.NextValue)
                            continue;
                        control.setSpeed(control.NextValue);
                    }

                    foreach (var keyPair in autoControlDictionary)
                    {
                        var control = keyPair.Value;
                        // Console.WriteLine("auto mode : name({0})", control.Name);
                        control.setAuto();
                    }
                }
            }
            catch { }            

            // onUpdateCallback
            onUpdateCallback();

            var osdManager = OSDManager.getInstance();
            if (osdManager.IsEnable == true)
            {
                var osdHeaderString = "<A0=-5><A1=5><S0=50>\r";

                var osdString = new StringBuilder();
                if (osdManager.IsTime == true)
                {
                    osdString.Append(DateTime.Now.ToString("HH:mm:ss") + "\n");
                }

                int maxNameLength = 0;
                for (int i = 0; i < osdManager.getGroupCount(); i++)
                {
                    var group = osdManager.getGroup(i);
                    if (group == null)
                        break;
                    if (group.Name.Length > maxNameLength)
                        maxNameLength = group.Name.Length;
                }

                for (int i = 0; i < osdManager.getGroupCount(); i++)
                {
                    var group = osdManager.getGroup(i);
                    if (group == null)
                        break;
                    osdString.Append(group.getOSDString(maxNameLength));
                }

                if (osdString.ToString().Length > 0)
                {
                    var sendString = osdHeaderString + osdString.ToString();
                    OSDController.update(sendString);
                    osdManager.IsUpdate = true;
                }
            }
            else
            {
                if (osdManager.IsUpdate == true)
                {
                    OSDController.release();
                    osdManager.IsUpdate = false;
                }
            }
            Monitor.Exit(mLock);
        }

        public int addChangeValue(int value, BaseControl control, bool isLock = true)
        {
            if (isLock == true)
            {
                Monitor.Enter(mLock);
            }            
            if (value < control.getMinSpeed())
            {
                value = control.getMinSpeed();
            }
            else if(value > control.getMaxSpeed())
            {
                value = control.getMaxSpeed();
            }
            mChangeValueList.Add(value);
            mChangeControlList.Add(control);

            if (isLock == true)
            {
                Monitor.Exit(mLock);
            }
            return value;
        }

        public bool read(ref bool isDifferent)
        {
            Monitor.Enter(mLock);
            String jsonString;
            try
            {
                jsonString = File.ReadAllText(mHardwareFileName);
            }
            catch
            {
                Monitor.Exit(mLock);
                this.write();
                return false;
            }

            try
            {
                var rootObject = JObject.Parse(jsonString);

                // name
                if (rootObject.ContainsKey("name") == true)
                {
                    var nameObject = rootObject.Value<JObject>("name");

                    // temperature name
                    if (nameObject.ContainsKey("temp") == true)
                    {
                        var list = nameObject.Value<JArray>("temp");
                        for (int i = 0; i < list.Count; i++)
                        {
                            var jobject = list[i];
                            string id = jobject.Value<string>("id");
                            string name = jobject.Value<string>("name");

                            if (TempBaseMap.ContainsKey(id) == false)
                            {
                                isDifferent = true;
                                continue;
                            }

                            var device = TempBaseMap[id];
                            device.Name = name;

                            if (OSDSensorMap.ContainsKey(id) == true)
                            {
                                var sensor = OSDSensorMap[id];
                                sensor.Name = name;
                            }
                        }
                    }

                    // fan name
                    if (nameObject.ContainsKey("fan") == true)
                    {
                        var list = nameObject.Value<JArray>("fan");
                        for (int i = 0; i < list.Count; i++)
                        {
                            var jobject = list[i];
                            string id = jobject.Value<string>("id");
                            string name = jobject.Value<string>("name");

                            if (FanBaseMap.ContainsKey(id) == false)
                            {
                                isDifferent = true;
                                continue;
                            }

                            var device = FanBaseMap[id];
                            device.Name = name;

                            if (OSDSensorMap.ContainsKey(id) == true)
                            {
                                var sensor = OSDSensorMap[id];
                                sensor.Name = name;
                            }
                        }
                    }

                    // control name
                    if (nameObject.ContainsKey("control") == true)
                    {
                        var list = nameObject.Value<JArray>("control");
                        for (int i = 0; i < list.Count; i++)
                        {
                            var jobject = list[i];
                            string id = jobject.Value<string>("id");
                            string name = jobject.Value<string>("name");

                            if (ControlBaseMap.ContainsKey(id) == false)
                            {
                                isDifferent = true;
                                continue;
                            }

                            var device = ControlBaseMap[id];
                            device.Name = name;

                            if (OSDSensorMap.ContainsKey(id) == true)
                            {
                                var sensor = OSDSensorMap[id];
                                sensor.Name = name;
                            }
                        }
                    }
                }
            }
            catch
            {
                Monitor.Exit(mLock);
                return false;
            }
            Monitor.Exit(mLock);
            return true;
        }

        public void write()
        {
            Monitor.Enter(mLock);
            try
            {
                var rootObject = new JObject();

                // name
                var nameObject = new JObject();

                // temp name
                var tempList = new JArray();
                for (int i = 0; i < TempBaseList.Count; i++)
                {
                    var device = TempBaseList[i];
                    var jobject = new JObject();
                    jobject["id"] = device.ID;
                    jobject["name"] = device.Name;
                    tempList.Add(jobject);
                }
                nameObject["temp"] = tempList;

                // fan name
                var fanList = new JArray();
                for (int i = 0; i < FanBaseList.Count; i++)
                {
                    var device = FanBaseList[i];
                    var jobject = new JObject();
                    jobject["id"] = device.ID;
                    jobject["name"] = device.Name;
                    fanList.Add(jobject);
                }
                nameObject["fan"] = fanList;

                // control name
                var controlList = new JArray();
                for (int i = 0; i < ControlBaseList.Count; i++)
                {
                    var device = ControlBaseList[i];
                    var jobject = new JObject();
                    jobject["id"] = device.ID;
                    jobject["name"] = device.Name;
                    controlList.Add(jobject);
                }
                nameObject["control"] = controlList;

                rootObject["name"] = nameObject;

                File.WriteAllText(mHardwareFileName, rootObject.ToString());
            }
            catch { }
            Monitor.Exit(mLock);
        }
    }
}
