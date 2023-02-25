using HidSharp.Reports;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class HWInfoManager
    {
        public string mHWInfoFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "HWiNFO.json";

        private const string mIDPrefix = "HWiNFO";
        private const string mIDPrefixOSD = "HWiNFO/OSD";
        private const int MAX_CONNECT_TIMEOUT = 2000;

        private bool mIsStart = false;

        private string mIPAddress = "127.0.0.1";
        private ushort mPort = 27007;

        private TcpClient mTCPClient = null;
        private Thread mThread = null;

        private object mLock = new object();
        private Dictionary<string, HWInfoCategory> mHWInfoCategoryMap = new Dictionary<string, HWInfoCategory>();
        private List<HWInfoCategory> mHWInfoCategoryList = new List<HWInfoCategory>();

        private bool mIsFirstRecv = false;

        private bool mIsVersionMore734 = false;

        private HWInfoManager() { }
        private static HWInfoManager sManager = new HWInfoManager();
        public static HWInfoManager getInstance() { return sManager; }

        public void start()
        {
            if (mIsStart == true)
                return;

            this.read();

            mIsStart = true;
            mThread = new Thread(new ThreadStart(threadFunction));
            mThread.Start();

            // waiting first recv
            for (int i = 0; i < 100; i++)
            {
                if (mIsFirstRecv == true)
                    break;
                Util.sleep(10);
            }
        }

        public void stop()
        {
            if (mIsStart == false)
                return;

            mIsStart = false;

            this.write();

            this.disconnect();

            try
            {
                if (mThread != null)
                {
                    mThread.Join();
                    mThread = null;
                }

            } catch{ }

            Monitor.Enter(mLock);
            mIsFirstRecv = false;
            mHWInfoCategoryMap.Clear();
            mHWInfoCategoryList.Clear();
            Monitor.Exit(mLock);
        }        

        private bool connect()
        {
            try
            {
                if (mTCPClient != null && mTCPClient.Connected == true)
                {
                    return true;
                }

                Console.WriteLine("HWInfo.connect() : try connect to {0}:{1}", mIPAddress, mPort);
                mTCPClient = new TcpClient();
                var result = mTCPClient.BeginConnect(mIPAddress, mPort, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(MAX_CONNECT_TIMEOUT, true);
                if (success == true)
                {
                    if (mTCPClient.Connected == true)
                    {
                        mTCPClient.EndConnect(result);

                        mTCPClient.GetStream().WriteTimeout = 2000;
                        mTCPClient.GetStream().ReadTimeout = 2000;

                        Console.WriteLine("HWInfo.connect() : connected({0}:{1})", mIPAddress, mPort);
                        return true;
                    }
                }
                mTCPClient.Close();
                mTCPClient.Dispose();
                mTCPClient = null;
                Console.WriteLine("HWInfo.connect() : connection failed({0}:{1})", mIPAddress, mPort);
            }
            catch { }
            return false;
        }

        private void disconnect()
        {
            try
            {
                if (mTCPClient != null)
                {
                    mTCPClient.Close();
                    mTCPClient.Dispose();
                    mTCPClient = null;
                    Console.WriteLine("HWInfo.disconnect() : disconnect({0}:{1})", mIPAddress, mPort);
                }
            }
            catch { }
        }

        private bool recv(int length, List<byte> recvList)
        {
            try
            {
                var stream = mTCPClient.GetStream();
                var datas = new byte[length];

                long lastTime = 0;
                while (true)
                {
                    if (stream.DataAvailable == false)
                    {
                        if (lastTime == 0)
                        {
                            lastTime = Util.getNowMS();
                        }
                        else
                        {
                            long nowTime = Util.getNowMS();
                            long delayTime = nowTime - lastTime;
                            if (delayTime >= 100)
                                return false;
                        }

                        //Console.WriteLine("HWInfo.recv() : dataAvailable false, lastTime({0})", lastTime);
                        Util.sleep(1);
                        continue;
                    }
                    lastTime = 0;

                    int nRead = stream.Read(datas, 0, length);
                    if (nRead == 0)
                    {
                        return false;
                    }

                    recvList.AddRange(datas.Take(length));

                    if (nRead >= length)
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private bool getVersion(List<byte> recvList)
        {
            //Console.WriteLine("HWInfo.getVersion()");

            var stream = mTCPClient.GetStream();

            // request
            var sendDatas = new byte[128];
            sendDatas[0] = 0x43;
            sendDatas[1] = 0x52;
            sendDatas[2] = 0x57;
            sendDatas[3] = 0x48;
            sendDatas[4] = 0x01;
            stream.Write(sendDatas, 0, sendDatas.Length);

            // reply1
            int recvLength = 132;
            var tempList = new List<byte>();
            if (this.recv(recvLength, tempList) == false)
                return false;

            if (tempList[0] == 0x52 && tempList[1] == 0x52 && tempList[2] == 0x57 && tempList[3] == 0x48 && tempList[4] == 0x01)
            {
                var temp = new byte[] { tempList[12], tempList[13], tempList[14], tempList[15] };
                recvLength = BitConverter.ToInt32(temp, 0);
            }
            else
            {
                return false;
            }

            // reply2
            recvList.Clear();
            if (this.recv(recvLength, recvList) == false)
                return false;

            if (recvList[0] == 0x50 && recvList[1] == 0x52 && recvList[2] == 0x57 && recvList[3] == 0x48 && recvList[4] == 0x01)
            {
                try
                {
                    var tempList2 = new List<byte>();
                    for (int i = 40; i < recvLength; i++)
                    {
                        if (recvList[i] == 0x00)
                            break;
                        tempList2.Add(recvList[i]);
                    }

                    // ex) HWiNFO64 v7.40-5000
                    string hwinfoVersion = Encoding.Default.GetString(tempList2.ToArray());
                    var splitArray = hwinfoVersion.Split(' ');
                    if (splitArray.Length == 2)
                    {
                        var splitArray2 = splitArray[1].Split('-');
                        if (splitArray2.Length == 2)
                        {
                            var versionString = splitArray2[0].Replace("v", "");
                            double version = double.Parse(versionString);
                            if (version >= 7.34)
                            {
                                mIsVersionMore734 = true;
                            }
                        }
                    }
                    
                }
                catch { }
                return true;
            }
            return false;
        }

        private bool getDeviceData(List<byte> recvList)
        {
            //Console.WriteLine("HWInfo.getDeviceData()");
            var stream = mTCPClient.GetStream();

            // request
            var sendDatas = new byte[128];
            sendDatas[0] = 0x43;
            sendDatas[1] = 0x52;
            sendDatas[2] = 0x57;
            sendDatas[3] = 0x48;
            sendDatas[4] = 0x02;
            stream.Write(sendDatas, 0, sendDatas.Length);

            // reply1
            int recvLength = 132;
            var tempList = new List<byte>();
            if (this.recv(recvLength, tempList) == false)
                return false;

            if (tempList[0] == 0x52 && tempList[1] == 0x52 && tempList[2] == 0x57 && tempList[3] == 0x48 && tempList[4] == 0x02)
            {
                var temp = new byte[] { tempList[12], tempList[13], tempList[14], tempList[15] };
                recvLength = BitConverter.ToInt32(temp, 0);
            }
            else
            {
                return false;
            }

            // reply2
            recvList.Clear();
            if (this.recv(recvLength, recvList) == false)
                return false;

            if (recvList[0] == 0x50 && recvList[1] == 0x52 && recvList[2] == 0x57 && recvList[3] == 0x48 && recvList[4] == 0x02)
            {
                return true;
            }
            return false;
        }

        private void threadFunction()
        {
            while (mIsStart == true)
            {
                if (this.connect() == false)
                {
                    Util.sleep(ref mIsStart, 500);
                    continue;
                }

                try
                {
                    var stream = mTCPClient.GetStream();

                    var recvList = new List<byte>();
                    if (this.getVersion(recvList) == false)
                    {
                        this.disconnect();
                        Util.sleep(ref mIsStart, 500);
                        continue;
                    }

                    bool isDeviceData = true;
                    while (mIsStart == true && isDeviceData == true)
                    {
                        if (this.getDeviceData(recvList) == false)
                        {
                            isDeviceData = false;
                            break;
                        }

                        //Console.WriteLine("HWInfo.threadFunction() : {0}", recvList.Count);

                        // processData
                        this.processData(recvList);

                        Util.sleep(ref mIsStart, 500);
                    }

                    if (isDeviceData == false)
                    {
                        this.disconnect();
                        Util.sleep(ref mIsStart, 500);
                        continue;
                    }
                }
                catch
                {
                    this.disconnect();
                    Util.sleep(ref mIsStart, 500);
                    continue;
                }
            }
        }

        private void processData(List<byte> recvList)
        {
            try
            {
                int index = 60;
                bool isValue = false;

                var tempCategoryList = new List<HWInfoCategory>();
                while (index < recvList.Count)
                {
                    if (isValue == false)
                    {
                        // category value1
                        var datas = new byte[] { recvList[index], recvList[index + 1], recvList[index + 2], recvList[index + 3] };
                        uint id1 = BitConverter.ToUInt32(datas, 0);
                        if (id1 < 100)
                        {
                            isValue = true;
                            continue;
                        }
                        index = index + 4;

                        // category value2
                        datas = new byte[] { recvList[index], recvList[index + 1], recvList[index + 2], recvList[index + 3] };
                        uint id2 = BitConverter.ToUInt32(datas, 0);
                        index = index + 4;

                        // category name
                        var tempList = new List<byte>();
                        for (int i = 0; i < 128; i++)
                        {
                            if (recvList[index + i] == 0x00)
                                break;
                            tempList.Add(recvList[index + i]);
                        }
                        string name = Encoding.Default.GetString(tempList.ToArray());
                        index = index + 128;

                        // category name (repeat)
                        index = index + 128;

                        // pass packet
                        if (mIsVersionMore734 == true)
                        {
                            index = index + 128;
                        }                        

                        Monitor.Enter(mLock);
                        string categoryID = string.Format("{0}/{1}/{2}/{3}", mIDPrefix, id1, id2, name);
                        HWInfoCategory category = null;
                        if (mHWInfoCategoryMap.ContainsKey(categoryID) == false)
                        {
                            category = new HWInfoCategory(categoryID, name);
                            mHWInfoCategoryMap.Add(categoryID, category);
                            mHWInfoCategoryList.Add(category);
                        }
                        else
                        {
                            category = mHWInfoCategoryMap[categoryID];
                        }
                        tempCategoryList.Add(category);
                        Monitor.Exit(mLock);
                        //Console.WriteLine("HWInfo.processData() : {0}, {1}, {2}", id1, id2, name);
                    }
                    else
                    {
                        // device value1
                        var datas = new byte[] { recvList[index], recvList[index + 1], recvList[index + 2], recvList[index + 3] };
                        uint deviceValue1 = BitConverter.ToUInt32(datas, 0);
                        index = index + 4;

                        // category intex (uint)
                        datas = new byte[] { recvList[index], recvList[index + 1], recvList[index + 2], recvList[index + 3] };
                        int categoryIndex = BitConverter.ToInt32(datas, 0);
                        index = index + 4;

                        // device value2
                        datas = new byte[] { recvList[index], recvList[index + 1], recvList[index + 2], recvList[index + 3] };
                        uint deviceValue2 = BitConverter.ToUInt32(datas, 0);
                        index = index + 4;

                        // device name
                        var tempList = new List<byte>();
                        for (int i = 0; i < 128; i++)
                        {
                            if (recvList[index + i] == 0x00)
                                break;
                            tempList.Add(recvList[index + i]);
                        }
                        string name = Encoding.Default.GetString(tempList.ToArray());
                        index = index + 128;

                        // device name (repeat)
                        index = index + 128;

                        // unit
                        tempList = new List<byte>();
                        for (int i = 0; i < 16; i++)
                        {
                            if (recvList[index + i] == 0x00)
                                break;
                            tempList.Add(recvList[index + i]);
                        }
                        string unit = Encoding.Default.GetString(tempList.ToArray());
                        index = index + 16;

                        // current
                        datas = new byte[] { recvList[index], recvList[index + 1], recvList[index + 2], recvList[index + 3], recvList[index + 4], recvList[index + 5], recvList[index + 6], recvList[index + 7] };
                        double current = BitConverter.ToDouble(datas, 0);
                        index = index + 8;

                        // minimum
                        datas = new byte[] { recvList[index], recvList[index + 1], recvList[index + 2], recvList[index + 3], recvList[index + 4], recvList[index + 5], recvList[index + 6], recvList[index + 7] };
                        double minimum = BitConverter.ToDouble(datas, 0);
                        index = index + 8;

                        // maximum
                        datas = new byte[] { recvList[index], recvList[index + 1], recvList[index + 2], recvList[index + 3], recvList[index + 4], recvList[index + 5], recvList[index + 6], recvList[index + 7] };
                        double maximum = BitConverter.ToDouble(datas, 0);
                        index = index + 8;

                        // average
                        datas = new byte[] { recvList[index], recvList[index + 1], recvList[index + 2], recvList[index + 3], recvList[index + 4], recvList[index + 5], recvList[index + 6], recvList[index + 7] };
                        double average = BitConverter.ToDouble(datas, 0);
                        index = index + 8;

                        // pass packet
                        if (mIsVersionMore734 == true)
                        {
                            index = index + 144;
                        }                        

                        try
                        {
                            if (categoryIndex < tempCategoryList.Count && name.Length > 0)
                            {
                                var category = tempCategoryList[categoryIndex];
                                string deviceID = string.Format("{0}/{1}/{2}/{3}", category.ID, deviceValue1, deviceValue2, name);
                                category.update(deviceID, name, current, unit);
                            }
                        }
                        catch { }

                        //Console.WriteLine("HWInfo.processData() : name({0}), value({1}, {2}, {3}), cur({4}{5})", name, deviceValue1, categoryIndex, deviceValue2, current, unit);
                    }
                }

                if (mIsFirstRecv == false)
                {
                    mIsFirstRecv = true;
                    this.write();
                }
            }
            catch { }
        }

        public void createTemp(ref List<HardwareDevice> deviceList)
        {
            Monitor.Enter(mLock);
            for (int i = 0; i < mHWInfoCategoryList.Count; i++)
            {
                var category = mHWInfoCategoryList[i];
                var hardwareDevice = new HardwareDevice(category.Name);
                
                int deviceCount = category.getDeviceCount();
                for (int j = 0; j < deviceCount; j++)
                {
                    var device = category.getDevice(j);
                    if (device == null)
                        continue;

                    if (device.Unit.Equals("°C") == true || device.Unit.Equals("°F") == true)
                    {
                        var temp = new HWInfoTemp(device.ID, device.Name, category.ID, device.ID);
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
            for (int i = 0; i < mHWInfoCategoryList.Count; i++)
            {
                var category = mHWInfoCategoryList[i];
                var hardwareDevice = new HardwareDevice(category.Name);

                int deviceCount = category.getDeviceCount();
                for (int j = 0; j < deviceCount; j++)
                {
                    var device = category.getDevice(j);
                    if (device == null)
                        continue;

                    if (device.Unit.Equals("RPM") == true)
                    {
                        var fan = new HWInfoFanSpeed(device.ID, device.Name, category.ID, device.ID);
                        hardwareDevice.addDevice(fan);
                    }
                }

                if (hardwareDevice.DeviceList.Count > 0)
                {
                    deviceList.Add(hardwareDevice);
                }
            }
            Monitor.Exit(mLock);
        }

        public void createOSDSensor(List<OSDSensor> osdList, Dictionary<string, OSDSensor> osdMap)
        {
            Monitor.Enter(mLock);
            for (int i = 0; i < mHWInfoCategoryList.Count; i++)
            {
                var category = mHWInfoCategoryList[i];

                int deviceCount = category.getDeviceCount();
                for (int j = 0; j < deviceCount; j++)
                {
                    var device = category.getDevice(j);
                    if (device == null)
                        continue;

                    string prefix = "[HWiNFO] ";
                    string id = string.Format("{0}/{1}", mIDPrefixOSD, device.ID);
                    var osdSensor = new HWInfoOSDSensor(id, prefix, device.Name, OSDUnitType.HWiNFO, category.ID, device.ID);
                    osdList.Add(osdSensor);
                    osdMap.Add(id, osdSensor);
                }
            }
            Monitor.Exit(mLock); 
        }

        public HWInfoDevice getHWInfoDevice(string categoryID, string deviceID)
        {
            Monitor.Enter(mLock);
            if (mHWInfoCategoryMap.ContainsKey(categoryID) == false)
            {
                Monitor.Exit(mLock);
                return null;
            }

            var category = mHWInfoCategoryMap[categoryID];
            var device = category.getDevice(deviceID);
            Monitor.Exit(mLock);
            return device;
        }

        public void read()
        {
            Monitor.Enter(mLock);
            String jsonString;
            try
            {
                jsonString = File.ReadAllText(mHWInfoFileName);
            }
            catch
            {
                Monitor.Exit(mLock);
                this.write();
                return;
            }

            try
            {
                var rootObject = JObject.Parse(jsonString);

                // categoryList
                if (rootObject.ContainsKey("list") == true)
                {
                    var categoryList = rootObject.Value<JArray>("list");
                    for (int i = 0; i < categoryList.Count; i++)
                    {
                        var categoryObject = (JObject)categoryList[i];

                        if (categoryObject.ContainsKey("id") == false ||
                            categoryObject.ContainsKey("name") == false ||
                            categoryObject.ContainsKey("list") == false)
                        {
                            continue;
                        }

                        string id = categoryObject.Value<string>("id");
                        string name = categoryObject.Value<string>("name");
                        var deviceList = categoryObject.Value<JArray>("list");

                        var category = new HWInfoCategory(id, name);
                        mHWInfoCategoryMap.Add(id, category);
                        mHWInfoCategoryList.Add(category);

                        for (int j = 0; j < deviceList.Count; j++)
                        {
                            var deviceObject = (JObject)deviceList[j];

                            if (deviceObject.ContainsKey("id") == false ||
                                deviceObject.ContainsKey("name") == false ||
                                deviceObject.ContainsKey("unit") == false)
                            {
                                continue;
                            }

                            string deviceID = deviceObject.Value<string>("id");
                            string deviceName = deviceObject.Value<string>("name");
                            string deviceUnit = deviceObject.Value<string>("unit");

                            category.update(deviceID, deviceName, 0.0, deviceUnit);
                        }
                    }
                }
            }
            catch
            {
                mHWInfoCategoryMap.Clear();
                mHWInfoCategoryList.Clear();
            }
            Monitor.Exit(mLock);
        }

        public void write()
        {
            Monitor.Enter(mLock);
            try
            {
                var rootObject = new JObject();

                // category
                var categoryList = new JArray();
                for (int i = 0; i < mHWInfoCategoryList.Count; i++)
                {
                    var category = mHWInfoCategoryList[i];
                    var categoryObject = new JObject();
                    categoryObject["id"] = category.ID;
                    categoryObject["name"] = category.Name;

                    // device
                    var deviceList = new JArray();
                    int deviceCount = category.getDeviceCount();
                    for (int j = 0; j < deviceCount; j++)
                    {
                        var device = category.getDevice(j);
                        if (device == null)
                            continue;

                        var deviceObject = new JObject();
                        deviceObject["id"] = device.ID;
                        deviceObject["name"] = device.Name;
                        deviceObject["unit"] = device.Unit;

                        deviceList.Add(deviceObject);
                    }
                    categoryObject["list"] = deviceList;

                    categoryList.Add(categoryObject);
                }
                rootObject["list"] = categoryList;

                File.WriteAllText(mHWInfoFileName, rootObject.ToString());
            }
            catch { }
            Monitor.Exit(mLock);
        }

        public void reset()
        {
            Monitor.Enter(mLock);
            mHWInfoCategoryMap.Clear();
            mHWInfoCategoryList.Clear();
            Monitor.Exit(mLock);
        }
    }
}
