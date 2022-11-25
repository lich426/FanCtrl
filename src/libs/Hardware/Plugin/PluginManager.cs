using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FanCtrl
{
    class PluginManager
    {
        public string mFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "Plugin.json";

        private static PluginManager sManager = new PluginManager();
        public static PluginManager getInstance() { return sManager; }

        private object mLock = new object();

        public bool IsStart { get; set; } = false;

        public int Port { get; set; } = 9989;

        private ServerController mServerController;

        private List<PluginDevice> mTempDeviceList = new List<PluginDevice>();
        private List<PluginDevice> mFanDeviceList = new List<PluginDevice>();
        private List<PluginDevice> mControlDeviceList = new List<PluginDevice>();

        private Dictionary<string, PluginTemp> mTempDictionary = new Dictionary<string, PluginTemp>();
        private Dictionary<string, PluginFanSpeed> mFanDictionary = new Dictionary<string, PluginFanSpeed>();
        private Dictionary<string, PluginControl> mControlDictionary = new Dictionary<string, PluginControl>();

        private List<PluginPacket> mPluginPacketList = new List<PluginPacket>();
        private Dictionary<string, PluginPacket> mPluginPacketDictionary = new Dictionary<string, PluginPacket>();

        public delegate void OnConnectHandler(Tuple<string, int> clientTuple);
        public event OnConnectHandler onConnectHandler;

        public delegate void OnDisconnectHandler(Tuple<string, int> clientTuple);
        public event OnDisconnectHandler onDisconnectHandler;

        private PluginManager()
        {
            this.read();
        }

        public bool start(int port)
        {
            this.stop();
            Monitor.Enter(mLock);
            mServerController = new ServerController();
            mServerController.onConnectHandler += (connector) =>
            {
                Monitor.Enter(mLock);
                string keyString = string.Format("{0}:{1}", connector.IPString, connector.Port);
                var packet = new PluginPacket(connector.IPString, connector.Port);
                mPluginPacketList.Add(packet);
                mPluginPacketDictionary.Add(keyString, packet);
                Monitor.Exit(mLock);

                onConnectHandler?.Invoke(new Tuple<string, int>(connector.IPString, connector.Port));
            };
            mServerController.onDisconnectHandler += (connector) =>
            {
                Monitor.Enter(mLock);
                string keyString = string.Format("{0}:{1}", connector.IPString, connector.Port);
                if (mPluginPacketDictionary.ContainsKey(keyString) == true)
                {
                    for (int i = 0; i < mPluginPacketList.Count; i++)
                    {
                        var packet = mPluginPacketList[i];
                        if (packet.IPString.CompareTo(connector.IPString) == 0 && packet.Port == connector.Port)
                        {
                            mPluginPacketList.RemoveAt(i);
                            break;
                        }
                    }
                    mPluginPacketDictionary.Remove(keyString);
                }
                Monitor.Exit(mLock);

                onDisconnectHandler?.Invoke(new Tuple<string, int>(connector.IPString, connector.Port));
            };
            mServerController.onRecvHandler += (connector, recvArray, recvDataSize) =>
            {
                Monitor.Enter(mLock);
                PluginPacket packet = null;
                string keyString = string.Format("{0}:{1}", connector.IPString, connector.Port);
                if (mPluginPacketDictionary.ContainsKey(keyString) == true)
                {
                    packet = mPluginPacketDictionary[keyString];
                }
                Monitor.Exit(mLock);

                if (packet != null)
                {
                    packet.addRecvData(recvArray, recvDataSize);

                    while (true)
                    {
                        string jsonString = "";
                        if (packet.processData(ref jsonString) == false)
                            break;

                        try
                        {
                            var rootObject = JObject.Parse(jsonString);

                            if (rootObject.ContainsKey("list") == true)
                            {
                                var list = rootObject.Value<JArray>("list");
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var obj = (JObject)list[i];

                                    if (obj.ContainsKey("key") == false ||
                                        obj.ContainsKey("type") == false ||
                                        obj.ContainsKey("value") == false)
                                    {
                                        continue;
                                    }

                                    string key = obj.Value<string>("key");
                                    int type = obj.Value<int>("type");
                                    double value = obj.Value<double>("value");

                                    switch(type)
                                    {
                                        // temperature
                                        case 0:
                                            {
                                                string id = string.Format("Plugin/{0}/Temp", key);
                                                if (mTempDictionary.TryGetValue(id, out var pluginTemp) == true)
                                                {
                                                    pluginTemp.Value = (int)Math.Round(value);
                                                }
                                            }
                                            break;

                                        // fan speed
                                        case 1:
                                            {
                                                string id = string.Format("Plugin/{0}/Fan", key);
                                                if (mFanDictionary.TryGetValue(id, out var pluginFan) == true)
                                                {
                                                    pluginFan.Value = (int)Math.Round(value);
                                                }
                                            }
                                            break;

                                        default:
                                            break;
                                    }

                                }
                            }

                        } 
                        catch (Exception e)
                        {
                            //Console.WriteLine("PluginManager.start() : {0}", jsonString);
                            Console.WriteLine("PluginManager.start() : {0}", e.Message);
                        }
                    }
                }
            };

            bool isOK = mServerController.start(port);
            if (isOK == false)
            {
                mServerController = null;
            }
            Monitor.Exit(mLock);
            return isOK;
        }

        public void stop()
        {
            Monitor.Enter(mLock);
            mPluginPacketList.Clear();
            if (mServerController != null)
            {
                mServerController.stop();
                mServerController = null;
            }
            Monitor.Exit(mLock);
        }

        public bool isStart()
        {
            Monitor.Enter(mLock);
            var isOK = false;
            if (mServerController != null)
            {
                isOK = mServerController.IsStart;
            }
            Monitor.Exit(mLock);
            return isOK;
        }

        public void sendAll(string id, int controlValue)
        {
            Monitor.Enter(mLock);
            try
            {
                var rootObject = new JObject();

                // Plugin/{key}/Control -> {key}
                var stringArray = id.Split('/');
                if (stringArray.Length < 3)
                {
                    Monitor.Exit(mLock);
                    return;
                }

                // json example
                // {
                //      "key" : "3",
                //      "type : 2,
                //      "value" : 50
                // }
                rootObject["key"] = stringArray[1];
                rootObject["type"] = 2;    // 2: fan control
                rootObject["value"] = controlValue;
                
                var sendBuffer = PluginPacket.getSendPacket(rootObject.ToString());
                mServerController.send(sendBuffer, sendBuffer.Length);
            }
            catch(Exception e)
            {
                Console.WriteLine("PluginManager.sendAll() : {0}", e.Message);
            }
            Monitor.Exit(mLock);
        }

        public void getClientList(List<Tuple<string, int>> clientList)
        {
            Monitor.Enter(mLock);
            for (int i = 0; i < mPluginPacketList.Count; i++)
            {
                clientList.Add(new Tuple<string, int>(mPluginPacketList[i].IPString, mPluginPacketList[i].Port));
            }
            Monitor.Exit(mLock);
        }

        public void getDeviceList(List<PluginDevice> tempDeviceList, List<PluginDevice> fanDeviceList, List<PluginDevice> controlDeviceList)
        {
            Monitor.Enter(mLock);
            for (int i = 0; i < mTempDeviceList.Count; i++)
            {
                var device = new PluginDevice(mTempDeviceList[i].Key, mTempDeviceList[i].Name);
                tempDeviceList.Add(device);
            }
            for (int i = 0; i < mFanDeviceList.Count; i++)
            {
                var device = new PluginDevice(mFanDeviceList[i].Key, mFanDeviceList[i].Name);
                fanDeviceList.Add(device);
            }
            for (int i = 0; i < mControlDeviceList.Count; i++)
            {
                var device = new PluginDevice(mControlDeviceList[i].Key, mControlDeviceList[i].Name);
                controlDeviceList.Add(device);
            }
            Monitor.Exit(mLock);
        }

        public void setDeviceList(List<PluginDevice> tempDeviceList, List<PluginDevice> fanDeviceList, List<PluginDevice> controlDeviceList)
        {
            Monitor.Enter(mLock);
            mTempDeviceList.Clear();
            mFanDeviceList.Clear();
            mControlDeviceList.Clear();
            var tempDeviceDictionary = new Dictionary<string, PluginDevice>();
            var fanDeviceDictionary = new Dictionary<string, PluginDevice>();
            var controlDeviceDictionary = new Dictionary<string, PluginDevice>();

            for (int i = 0; i < tempDeviceList.Count; i++)
            {
                if (tempDeviceDictionary.ContainsKey(tempDeviceList[i].Key) == false)
                {
                    tempDeviceDictionary.Add(tempDeviceList[i].Key, tempDeviceList[i]);
                    mTempDeviceList.Add(tempDeviceList[i]);
                }
            }
            for (int i = 0; i < fanDeviceList.Count; i++)
            {
                if (fanDeviceDictionary.ContainsKey(fanDeviceList[i].Key) == false)
                {
                    fanDeviceDictionary.Add(fanDeviceList[i].Key, fanDeviceList[i]);
                    mFanDeviceList.Add(fanDeviceList[i]);
                }
            }
            for (int i = 0; i < controlDeviceList.Count; i++)
            {
                if (controlDeviceDictionary.ContainsKey(controlDeviceList[i].Key) == false)
                {
                    controlDeviceDictionary.Add(controlDeviceList[i].Key, controlDeviceList[i]);
                    mControlDeviceList.Add(controlDeviceList[i]);
                }
            }
            Monitor.Exit(mLock);

            this.write();
        }

        public void setTempDeviceName(string id, string name)
        {
            Monitor.Enter(mLock);
            var splitArray = id.Split('/');
            if (splitArray.Length >= 3)
            {
                string keyString = splitArray[1];
                for (int i = 0; i < mTempDeviceList.Count; i++)
                {
                    if (mTempDeviceList[i].Key.CompareTo(keyString) == 0)
                    {
                        mTempDeviceList[i].Name = name;
                        break;
                    }
                }
            }
            Monitor.Exit(mLock);
            this.write();
        }

        public void setFanDeviceName(string id, string name)
        {
            Monitor.Enter(mLock);
            var splitArray = id.Split('/');
            if (splitArray.Length >= 3)
            {
                string keyString = splitArray[1];
                for (int i = 0; i < mFanDeviceList.Count; i++)
                {
                    if (mFanDeviceList[i].Key.CompareTo(keyString) == 0)
                    {
                        mFanDeviceList[i].Name = name;
                        break;
                    }
                }
            }
            Monitor.Exit(mLock);
            this.write();
        }

        public void setControlDeviceName(string id, string name)
        {
            Monitor.Enter(mLock);
            var splitArray = id.Split('/');
            if (splitArray.Length >= 3)
            {
                string keyString = splitArray[1];
                for (int i = 0; i < mControlDeviceList.Count; i++)
                {
                    if (mControlDeviceList[i].Key.CompareTo(keyString) == 0)
                    {
                        mControlDeviceList[i].Name = name;
                        break;
                    }
                }
            }
            Monitor.Exit(mLock);
            this.write();
        }

        public void createTemp(ref List<HardwareDevice> deviceList)
        {
            Monitor.Enter(mLock);
            mTempDictionary.Clear();
            var hardwareDevice = new HardwareDevice("Plugin");
            for (int i = 0; i < mTempDeviceList.Count; i++)
            {
                var device = mTempDeviceList[i];
                string id = string.Format("Plugin/{0}/Temp", device.Key);
                var temp = new PluginTemp(id, device.Name);
                hardwareDevice.addDevice(temp);
                mTempDictionary.Add(id, temp);
            }
            if (hardwareDevice.DeviceList.Count > 0)
            {
                deviceList.Add(hardwareDevice);
            }
            Monitor.Exit(mLock);
        }

        public void createFan(ref List<HardwareDevice> deviceList)
        {
            Monitor.Enter(mLock);            
            mFanDictionary.Clear();
            var hardwareDevice = new HardwareDevice("Plugin");
            for (int i = 0; i < mFanDeviceList.Count; i++)
            {
                var device = mFanDeviceList[i];
                string id = string.Format("Plugin/{0}/Fan", device.Key);
                var fan = new PluginFanSpeed(id, device.Name);
                hardwareDevice.addDevice(fan);
                mFanDictionary.Add(id, fan);
            }
            if (hardwareDevice.DeviceList.Count > 0)
            {
                deviceList.Add(hardwareDevice);
            }
            Monitor.Exit(mLock);
        }

        public void createControl(ref List<HardwareDevice> deviceList)
        {
            Monitor.Enter(mLock);
            mControlDictionary.Clear();
            var hardwareDevice = new HardwareDevice("Plugin");
            for (int i = 0; i < mControlDeviceList.Count; i++)
            {
                var device = mControlDeviceList[i];
                string id = string.Format("Plugin/{0}/Control", device.Key);
                var control = new PluginControl(id, device.Name);
                hardwareDevice.addDevice(control);
                mControlDictionary.Add(id, control);
            }
            if (hardwareDevice.DeviceList.Count > 0)
            {
                deviceList.Add(hardwareDevice);
            }
            Monitor.Exit(mLock);
        }

        public bool read()
        {
            Monitor.Enter(mLock);
            string jsonString;
            try
            {
                jsonString = File.ReadAllText(mFileName);
            }
            catch
            {
                Monitor.Exit(mLock);
                this.write();
                return false;
            }

            try
            {
                mTempDeviceList.Clear();
                mFanDeviceList.Clear();
                mControlDeviceList.Clear();

                var tempDeviceDictionary = new Dictionary<string, PluginDevice>();
                var fanDeviceDictionary = new Dictionary<string, PluginDevice>();
                var controlDeviceDictionary = new Dictionary<string, PluginDevice>();

                var rootObject = JObject.Parse(jsonString);

                if (rootObject.ContainsKey("port") == true)
                {
                    Port = rootObject.Value<int>("port");
                }

                if (rootObject.ContainsKey("IsStart") == true)
                {
                    IsStart = rootObject.Value<bool>("IsStart");
                }

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
                            string id = jobject.Value<string>("key");
                            string name = jobject.Value<string>("name");
                            var device = new PluginDevice(id, name);
                            if (tempDeviceDictionary.ContainsKey(id) == false)
                            {
                                tempDeviceDictionary.Add(id, device);
                                mTempDeviceList.Add(device);
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
                            string id = jobject.Value<string>("key");
                            string name = jobject.Value<string>("name");
                            var device = new PluginDevice(id, name);
                            if (fanDeviceDictionary.ContainsKey(id) == false)
                            {
                                fanDeviceDictionary.Add(id, device);
                                mFanDeviceList.Add(device);
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
                            string id = jobject.Value<string>("key");
                            string name = jobject.Value<string>("name");
                            var device = new PluginDevice(id, name);
                            if (controlDeviceDictionary.ContainsKey(id) == false)
                            {
                                controlDeviceDictionary.Add(id, device);
                                mControlDeviceList.Add(device);
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

                // port
                rootObject["port"] = Port;

                // start
                rootObject["IsStart"] = IsStart;

                // name
                var nameObject = new JObject();

                // temp name
                var tempList = new JArray();
                for (int i = 0; i < mTempDeviceList.Count; i++)
                {
                    var device = mTempDeviceList[i];
                    var jobject = new JObject();
                    jobject["key"] = device.Key;
                    jobject["name"] = device.Name;
                    tempList.Add(jobject);
                }
                nameObject["temp"] = tempList;

                // fan name
                var fanList = new JArray();
                for (int i = 0; i < mFanDeviceList.Count; i++)
                {
                    var device = mFanDeviceList[i];
                    var jobject = new JObject();
                    jobject["key"] = device.Key;
                    jobject["name"] = device.Name;
                    fanList.Add(jobject);
                }
                nameObject["fan"] = fanList;

                // control name
                var controlList = new JArray();
                for (int i = 0; i < mControlDeviceList.Count; i++)
                {
                    var device = mControlDeviceList[i];
                    var jobject = new JObject();
                    jobject["key"] = device.Key;
                    jobject["name"] = device.Name;
                    controlList.Add(jobject);
                }
                nameObject["control"] = controlList;

                rootObject["name"] = nameObject;

                File.WriteAllText(mFileName, rootObject.ToString());
            }
            catch { }
            Monitor.Exit(mLock);
        }
    }
}
