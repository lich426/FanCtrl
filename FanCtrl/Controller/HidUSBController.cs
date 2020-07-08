using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using HidSharp;

namespace FanCtrl
{
    public class HidUSBController : USBController
    {
        // HidDevice
        private HidStream mHidStream = null;

        private delegate void RecvDelegate();
        private delegate void SendDelegate();

        private bool mIsSend = false;
        private List<byte[]> mSendArrayList = new List<byte[]>();
        private object mSendArrayListLock = new object();

        public HidUSBController(USBVendorID vendorID, USBProductID productID) : base(vendorID, productID)
        {

        }

        public static uint getDeviceCount(USBVendorID vendorID, USBProductID productID)
        {
            uint count = 0;
            try
            {                
                int venderID2 = (int)vendorID;
                int productID2 = (int)productID;
                foreach (HidDevice dev in DeviceList.Local.GetHidDevices(venderID2))
                {
                    if (dev.ProductID == productID2)
                    {
                        count++;
                    }
                }                
            }
            catch { }
            return count;
        }

        public override bool start(uint index)
        {
            try
            {
                uint i = 0;
                int venderID = (int)this.VendorID;
                int productID = (int)this.ProductID;
                foreach (HidDevice dev in DeviceList.Local.GetHidDevices(venderID))
                {
                    if (dev.ProductID == productID)
                    {
                        if (i == index)
                        {
                            if (dev.TryOpen(out mHidStream) == false)
                            {
                                Console.WriteLine("HidUSBController.start() : could not open the device");
                                this.stop();
                                return false;
                            }
                            break;
                        }
                        i++;
                    }
                }

                this.readAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("HidUSBController.start() : Failed catch\nerror : {0}", e.Message);
                this.stop();
                return false;
            }
            return true;
        }

        public override void stop()
        {
            try
            {
                if (mHidStream != null)
                {
                    mHidStream.Close();
                }
            }
            catch { }

            try
            {
                Monitor.Enter(mSendArrayListLock);
                mSendArrayList.Clear();
                Monitor.Exit(mSendArrayListLock);
            }
            catch { }
        }

        public override void send(byte[] buffer)
        {
            Monitor.Enter(mSendArrayListLock);
            mSendArrayList.Add(buffer);
            if (mIsSend == false)
            {
                this.writeAsync();
            }
            Monitor.Exit(mSendArrayListLock);            
        }

        public override void send(List<byte[]> bufferList)
        {
            Monitor.Enter(mSendArrayListLock);
            for (int i = 0; i < bufferList.Count; i++)
            {
                mSendArrayList.Add(bufferList[i]);
            }
            if (mIsSend == false)
            {
                this.writeAsync();
            }
            Monitor.Exit(mSendArrayListLock);
        }

        private async void readAsync()
        {
            var recvDelegate = new RecvDelegate(read);
            await Task.Factory.FromAsync(recvDelegate.BeginInvoke, recvDelegate.EndInvoke, null);
        }

        private void read()
        {
            try
            {
                if (mHidStream != null && mHidStream.CanRead == true)
                {
                    var buffer = mHidStream.Read();
                    this.onRecv(buffer, buffer.Length);
                    this.readAsync();
                }
            }
            catch { }
        }

        private async void writeAsync()
        {
            mIsSend = true;
            var sendDelegate = new SendDelegate(write);
            await Task.Factory.FromAsync(sendDelegate.BeginInvoke, sendDelegate.EndInvoke, null);
        }

        private void write()
        {
            Monitor.Enter(mSendArrayListLock);
            try
            {
                if (mHidStream != null && mHidStream.CanWrite == true)
                {
                    for (int i = 0; i < mSendArrayList.Count; i++)
                    {
                        mHidStream.Write(mSendArrayList[i]);
                    }
                }
            }
            catch { }
            mSendArrayList.Clear();
            mIsSend = false;
            Monitor.Exit(mSendArrayListLock);
        }
    }
}
