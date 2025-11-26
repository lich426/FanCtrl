using System.Collections.Generic;
using System.Threading;

namespace FanCtrl
{
    public class HWInfoCategory
    {
        public string Name { get; set; }

        public string ID { get; set; }

        private object mLock = new object();
        private Dictionary<string, HWInfoDevice> mHWInfoDeviceMap = new Dictionary<string, HWInfoDevice>();
        private List<HWInfoDevice> mHWInfoDeviceList = new List<HWInfoDevice>();

        public HWInfoCategory(string id, string name)
        {
            ID = id;
            Name = name;
        }

        public void update(string deviceID, string name, double value, string unit)
        {
            Monitor.Enter(mLock);
            HWInfoDevice device = null;
            if (mHWInfoDeviceMap.ContainsKey(deviceID) == false)
            {
                device = new HWInfoDevice(deviceID, name);
                mHWInfoDeviceMap.Add(deviceID, device);
                mHWInfoDeviceList.Add(device);
            }
            else
            {
                device = mHWInfoDeviceMap[deviceID];
            }
            device.Value = value;
            device.Unit = unit;
            Monitor.Exit(mLock);
        }

        public int getDeviceCount()
        {
            Monitor.Enter(mLock);
            int count = mHWInfoDeviceList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public HWInfoDevice getDevice(int index)
        {
            Monitor.Enter(mLock);
            if (index >= mHWInfoDeviceList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }

            var device = mHWInfoDeviceList[index];
            Monitor.Exit(mLock);
            return device;
        }

        public HWInfoDevice getDevice(string id)
        {
            Monitor.Enter(mLock);
            if (mHWInfoDeviceMap.ContainsKey(id) == false)
            {
                Monitor.Exit(mLock);
                return null;
            }

            var device = mHWInfoDeviceMap[id];
            Monitor.Exit(mLock);
            return device;
        }
    }
}
