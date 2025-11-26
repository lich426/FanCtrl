using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Diagnostics;

namespace FanCtrl
{
    public class LiquidctlManager
    {
        public string LiquidctlFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "liquidctl.json";

        // liquidctl path
        public string LiquidctlPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\plugin\\liquidctl.exe";

        //private const string mIDPrefix = "liquidctl";
        //private const string mIDPrefixOSD = "liquidctl/OSD";

        // Singletone
        private LiquidctlManager() {}
        private static LiquidctlManager sManager = new LiquidctlManager();
        public static LiquidctlManager getInstance() { return sManager; }

        // Start state
        private bool mIsStart = false;

        // lock
        private object mLock = new object();

        // update timer
        private System.Timers.Timer mUpdateTimer = null;

        private bool mIsCommand = false;

        private List<LiquidctlData> mLiquidctlDataList = new List<LiquidctlData>();

        private List<LiquidctlControl> mLiquidctlControlList = new List<LiquidctlControl>();

        private List<string> mLiquidctlCommandList = new List<string>();

        public void start()
        {
            Monitor.Enter(mLock);
            if (mIsStart == true)
            {
                Monitor.Exit(mLock);
                return;
            }

            try
            {
                this.initialize();

                var jsonString = getStatus();
                var rootArray = JArray.Parse(jsonString);
                for (int i = 0; i < rootArray.Count; i++)
                {
                    var data = (JObject)rootArray[i];
                    var liquildctlData = new LiquidctlData();
                    liquildctlData.Bus = (data.ContainsKey("bus") == true) ? data.Value<string>("bus") : "";
                    liquildctlData.Address = (data.ContainsKey("address") == true) ? data.Value<string>("address") : "";
                    liquildctlData.Description = (data.ContainsKey("description") == true) ? data.Value<string>("description") : "";

                    var statusList = data.Value<JArray>("status");
                    for (int j = 0; j < statusList.Count; j++)
                    {
                        var tempData = (JObject)statusList[j];
                        var statusData = new LiquidctlStatusData(liquildctlData);

                        statusData.Key = (tempData.ContainsKey("key") == true) ? tempData.Value<string>("key") : "";
                        statusData.Unit = (tempData.ContainsKey("unit") == true) ? tempData.Value<string>("unit") : "";
                        statusData.Value = (tempData.ContainsKey("value") == true) ? tempData.GetValue("value").ToString() : "";

                        if (statusData.Unit.CompareTo("¡ÆC") == 0)
                        {
                            statusData.Type = LIQUID_STATUS_DATA_TYPE.TEMPERATURE;
                        }
                        else if (statusData.Unit.CompareTo("rpm") == 0)
                        {
                            statusData.Type = LIQUID_STATUS_DATA_TYPE.FAN;
                        }

                        Console.WriteLine("key : {0}, unit : {1}, type : {2}", statusData.Key, statusData.Unit, statusData.Value);

                        liquildctlData.addStatusData(statusData);
                    }
                    mLiquidctlDataList.Add(liquildctlData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Monitor.Exit(mLock);
                return;
            }

            this.read();

            mIsCommand = false;
            mUpdateTimer = new System.Timers.Timer();
            mUpdateTimer.Interval = OptionManager.getInstance().Interval;
            mUpdateTimer.Elapsed += onUpdateTimer;
            mUpdateTimer.Start();
            mIsStart = true;
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

            if (mUpdateTimer != null)
            {
                mUpdateTimer.Stop();
                mUpdateTimer.Dispose();
                mUpdateTimer = null;
            }

            mLiquidctlDataList.Clear();
            mLiquidctlControlList.Clear();
            mLiquidctlCommandList.Clear();

            mIsStart = false;
            Monitor.Exit(mLock);
        }

        private void onUpdateTimer(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(mLock) == false)
                return;

            try
            {
                var jsonString = getStatus();
                var rootArray = JArray.Parse(jsonString);
                for (int i = 0; i < rootArray.Count; i++)
                {
                    var data = (JObject)rootArray[i];
                    var bus = (data.ContainsKey("bus") == true) ? data.Value<string>("bus") : "";
                    var address = (data.ContainsKey("address") == true) ? data.Value<string>("address") : "";

                    LiquidctlData liquidctlData = null;
                    for (int j = 0; j < mLiquidctlDataList.Count; j++)
                    {
                        if (mLiquidctlDataList[j].Bus.CompareTo(bus) == 0 && mLiquidctlDataList[j].Address.CompareTo(address) == 0)
                        {
                            liquidctlData = mLiquidctlDataList[j];
                            break;
                        }
                    }
                    if (liquidctlData != null)
                    {
                        var statusList = data.Value<JArray>("status");
                        for (int j = 0; j < statusList.Count; j++)
                        {
                            var tempData = (JObject)statusList[j];
                            var value = (tempData.ContainsKey("value") == true) ? tempData.GetValue("value").ToString() : "";
                            liquidctlData.getStatusData(j).Value = value;

                            //Console.WriteLine("key : {0}, unit : {1}, type : {2}", liquidctlData.StatusDataList[j].Key, liquidctlData.StatusDataList[j].Unit, liquidctlData.StatusDataList[j].Value);
                        }
                    }
                }
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.Message);
            }

            if (mIsCommand == false)
            {
                mIsCommand = true;
                string liquidctlPath = LiquidctlPath;
                for (int i = 0; i < mLiquidctlCommandList.Count; i++)
                {
                    Thread.Sleep(10);
                    var p = new Process();
                    p.StartInfo.FileName = liquidctlPath;
                    p.StartInfo.Arguments = mLiquidctlCommandList[i];
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    if (p.Start() == true)
                    {
                        p.Close();
                    }
                }
            }

            Monitor.Exit(mLock);
        }

        private void initialize()
        {
            try
            {
                var p = new Process();
                p.StartInfo.FileName = LiquidctlPath;
                p.StartInfo.Arguments = "initialize all";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                if (p.Start() == true)
                {
                    var jsonString = p.StandardOutput.ReadToEnd();
                    p.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string getStatus()
        {
            try
            {
                var p = new Process();
                p.StartInfo.FileName = LiquidctlPath;
                p.StartInfo.Arguments = "status --json";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                if (p.Start() == true)
                {
                    var jsonString = p.StandardOutput.ReadToEnd();
                    p.Close();

                    //Console.WriteLine(output);
                    return jsonString;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }

        public void createTemp(ref List<HardwareDevice> deviceList)
        {
            Monitor.Enter(mLock);
            for (int i = 0; i < mLiquidctlDataList.Count; i++)
            {
                var liquidctlData = mLiquidctlDataList[i];
                var hardwareDevice = new HardwareDevice(liquidctlData.Description);

                int count = liquidctlData.getStatusCount();
                for (int j = 0; j < count; j++)
                {
                    var statusData = liquidctlData.getStatusData(j);
                    if (statusData.Type == LIQUID_STATUS_DATA_TYPE.TEMPERATURE)
                    {
                        var temp = new LiquidctlTemp(statusData);
                        hardwareDevice.addDevice(temp);
                    }
                }

                if (hardwareDevice.DeviceList.Count > 0)
                {
                    deviceList.Add(hardwareDevice);
                }
            }
            Monitor.Exit(mLock);
        }

        public void createFan(ref List<HardwareDevice> deviceList)
        {
            Monitor.Enter(mLock);
            for (int i = 0; i < mLiquidctlDataList.Count; i++)
            {
                var liquidctlData = mLiquidctlDataList[i];
                var hardwareDevice = new HardwareDevice(liquidctlData.Description);

                int count = liquidctlData.getStatusCount();
                for (int j = 0; j < count; j++)
                {
                    var statusData = liquidctlData.getStatusData(j);
                    if (statusData.Type == LIQUID_STATUS_DATA_TYPE.FAN)
                    {
                        var temp = new LiquidctlSpeed(statusData);
                        hardwareDevice.addDevice(temp);
                    }
                }

                if (hardwareDevice.DeviceList.Count > 0)
                {
                    deviceList.Add(hardwareDevice);
                }
            }
            Monitor.Exit(mLock);
        }

        public void createControl(ref List<HardwareDevice> deviceList)
        {
            Monitor.Enter(mLock);
            for (int i = 0; i < mLiquidctlDataList.Count; i++)
            {
                var liquidctlData = mLiquidctlDataList[i];
                var hardwareDevice = new HardwareDevice(liquidctlData.Description);

                for (int j = 0; j < mLiquidctlControlList.Count; j++)
                {
                    var control = mLiquidctlControlList[j];
                    if (control.Address.CompareTo(liquidctlData.Address) == 0)
                    {
                        hardwareDevice.addDevice(control);
                    }
                }

                if (hardwareDevice.DeviceList.Count > 0)
                {
                    deviceList.Add(hardwareDevice);
                }
            }
            Monitor.Exit(mLock);
        }

        public int getLiquidctlDataCount()
        {
            Monitor.Enter(mLock);
            int count = mLiquidctlDataList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public LiquidctlData getLiquidctlData(int index)
        {
            Monitor.Enter(mLock);
            if (index >= mLiquidctlDataList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }
            var data = mLiquidctlDataList[index];
            Monitor.Exit(mLock);
            return data;
        }

        public int getLiquidctlControlCount()
        {
            Monitor.Enter(mLock);
            int count = mLiquidctlControlList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public LiquidctlControl getLiquidctlControl(int index)
        {
            Monitor.Enter(mLock);
            if (index >= mLiquidctlControlList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }
            var data = mLiquidctlControlList[index];
            Monitor.Exit(mLock);
            return data;
        }

        public int getLiquidctlCommandCount()
        {
            Monitor.Enter(mLock);
            int count = mLiquidctlCommandList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public string getLiquidctlCommand(int index)
        {
            Monitor.Enter(mLock);
            if (index >= mLiquidctlCommandList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }
            var data = mLiquidctlCommandList[index];
            Monitor.Exit(mLock);
            return data;
        }

        public void read()
        {
            Monitor.Enter(mLock);
            string jsonString;
            try
            {
                jsonString = File.ReadAllText(LiquidctlFileName);
            }
            catch
            {
                Monitor.Exit(mLock);
                this.write(null, null);
                return;
            }

            try
            {
                var rootObject = JObject.Parse(jsonString);
                LiquidctlPath = (rootObject.ContainsKey("path") == true) ? rootObject.Value<string>("path") : LiquidctlPath;

                // list
                mLiquidctlControlList.Clear();
                if (rootObject.ContainsKey("list") == true)
                {
                    var controlArray = rootObject.Value<JArray>("list");
                    for (int i = 0; i < controlArray.Count; i++)
                    {
                        var controlObject = (JObject)controlArray[i];
                        var deviceName = (controlObject.ContainsKey("deviceName") == true) ? controlObject.Value<string>("deviceName") : "";
                        var name = (controlObject.ContainsKey("name") == true) ? controlObject.Value<string>("name") : "";
                        var channel = (controlObject.ContainsKey("channel") == true) ? controlObject.Value<string>("channel") : "";
                        var address = (controlObject.ContainsKey("address") == true) ? controlObject.Value<string>("address") : "";

                        var control = new LiquidctlControl(i, deviceName, name, address, channel);
                        mLiquidctlControlList.Add(control);
                    }
                }

                mLiquidctlCommandList.Clear();
                if (rootObject.ContainsKey("command") == true)
                {
                    var commandArray = rootObject.Value<JArray>("command");
                    for (int i = 0; i < commandArray.Count; i++)
                    {
                        var commandObject = (JObject)commandArray[i];
                        var command = (commandObject.ContainsKey("cmd") == true) ? commandObject.Value<string>("cmd") : "";
                        mLiquidctlCommandList.Add(command);
                    }
                }
            }
            catch
            {
                mLiquidctlDataList.Clear();
                mLiquidctlControlList.Clear();
                mLiquidctlCommandList.Clear();
            }
            Monitor.Exit(mLock);
        }

        public void write(List<LiquidctlControl> tempControlList, List<string> tempCommandList)
        {
            Monitor.Enter(mLock);
            try
            {
                var controlList = (tempControlList != null) ? tempControlList : mLiquidctlControlList;
                var commandList = (tempCommandList != null) ? tempCommandList : mLiquidctlCommandList;

                var rootObject = new JObject();

                if (LiquidctlPath.Length == 0)
                {
                    LiquidctlPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\liquidctl.exe";
                }
                rootObject["path"] = LiquidctlPath;

                var controlArray = new JArray();
                for (int i = 0; i < controlList.Count; i++)
                {
                    var control = controlList[i];
                    var controlObject = new JObject();
                    controlObject["id"] = control.ID;
                    controlObject["deviceName"] = control.DeviceName;
                    controlObject["name"] = control.Name;
                    controlObject["channel"] = control.ChannelName;
                    controlObject["address"] = control.Address;
                    controlArray.Add(controlObject);
                }
                rootObject["list"] = controlArray;

                var commandArray = new JArray();
                for (int i = 0; i < commandList.Count; i++)
                {
                    var command = commandList[i];
                    var commandObject = new JObject();
                    commandObject["cmd"] = command;
                    commandArray.Add(commandObject);
                }
                rootObject["command"] = commandArray;

                File.WriteAllText(LiquidctlFileName, rootObject.ToString());
            }
            catch { }
            Monitor.Exit(mLock);
        }
    }
}
