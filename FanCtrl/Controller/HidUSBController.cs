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
        private delegate void SendDelegate(byte[] data);

        public HidUSBController(USBVendorID vendorID, USBProductID productID) : base(vendorID, productID)
        {

        }

        public override bool start()
        {
            try
            {
                int venderID = (int)this.VendorID;
                int productID = (int)this.ProductID;
                HidDevice hidDevice = null;
                foreach (HidDevice dev in DeviceList.Local.GetHidDevices(venderID))
                {
                    if(dev.ProductID == productID)
                    {
                        hidDevice = dev;
                        break;
                    }
                }

                if (hidDevice == null)
                {
                    Console.WriteLine("HidUSBController.start() : could not find the device");
                    this.stop();
                    return false;
                }

                if (hidDevice.TryOpen(out mHidStream) == false)
                {
                    Console.WriteLine("HidUSBController.start() : could not open the device");
                    this.stop();
                    return false;
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
        }

        public override void send(byte[] buffer)
        {
            this.writeAsync(buffer);
        }

        public override void send(List<byte[]> bufferList)
        {
            for (int i = 0; i < bufferList.Count; i++)
            {
                this.writeAsync(bufferList[i]);
            }
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

        private async void writeAsync(byte[] buffer)
        {
            var sendDelegate = new SendDelegate(write);
            await Task.Factory.FromAsync(sendDelegate.BeginInvoke, sendDelegate.EndInvoke, buffer, null);
        }

        private void write(byte[] buffer)
        {
            try
            {
                if (mHidStream != null && mHidStream.CanWrite == true)
                {
                    mHidStream.Write(buffer);
                }
            }
            catch { }
        }
    }
}
