using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading;

namespace FanCtrl
{
    public enum USBDeviceType
    {
        Kraken = 0,
        CLC,
        RGBnFC,
    };

    public class USBDevice
    {
        protected object mLock = new object();
        protected string mFileName;
        protected System.Timers.Timer mTimer = new System.Timers.Timer();
        protected USBController mUSBController = null;

        public USBDevice(USBDeviceType type)
        {
            DeviceType = type;
        }

        public USBDeviceType DeviceType { get; }

        protected bool mIsSendCustomData = false;
        protected List<byte[]> mCustomDataList = new List<byte[]>();

        protected virtual bool readFile()
        {
            return false;
        }

        public virtual void writeFile()
        {

        }

        public void setCustomDataList(List<string> hexStringList)
        {
            Monitor.Enter(mLock);
            mCustomDataList.Clear();
            if (hexStringList.Count == 0)
            {
                Monitor.Exit(mLock);
                return;
            }
            for (int i = 0; i < hexStringList.Count; i++)
            {
                mCustomDataList.Add(Util.getHexBytes(hexStringList[i]));
            }
            mIsSendCustomData = (mCustomDataList.Count > 0);
            Monitor.Exit(mLock);
        }

        public List<string> getCustomDataList()
        {
            Monitor.Enter(mLock);
            var hexStringList = new List<string>();
            for (int i = 0; i < mCustomDataList.Count; i++)
            {
                hexStringList.Add(Util.getHexString(mCustomDataList[i]));
            }
            Monitor.Exit(mLock);
            return hexStringList;
        }
    }
}
