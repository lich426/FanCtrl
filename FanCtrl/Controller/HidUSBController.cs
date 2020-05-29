using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using HidLibrary;

namespace FanCtrl
{
    public class HidUSBController : USBController
    {
        // HidDevice
        private HidDevice mHidDevice = null;

        public HidUSBController(USBVendorID vendorID, USBProductID productID) : base(vendorID, productID)
        {

        }

        public override bool start()
        {
            try
            {
                mHidDevice = HidDevices.Enumerate((int)VendorID, (int)ProductID).FirstOrDefault();
                mHidDevice.OpenDevice();
                mHidDevice.ReadReport(onReport);
            }
            catch
            {
                Console.WriteLine("HidUSBController.start() : Failed catch");
                this.stop();
                return false;
            }
            return true;
        }

        public override void stop()
        { 
            if (mHidDevice != null)
            {
                try
                {
                    mHidDevice.CloseDevice();
                }
                catch { }
            }
        }
        
        private void onReport(HidReport report)
        {
            try
            {
                var dataArray = report.Data;
                //Console.WriteLine("onReport.onReport() -----------------------------------------");
                //Util.printHex(dataArray, dataArray.Length);

                this.onRecv(dataArray, dataArray.Length);
                mHidDevice.ReadReport(onReport);
            }
            catch { }
        }

        public override void send(byte[] buffer)
        {
            try
            {
                mHidDevice.WriteAsync(buffer);
            }
            catch { }
        }

        public override void send(List<byte[]> bufferList)
        {
            for(int i = 0; i < bufferList.Count; i++)
            {
                try
                {
                    mHidDevice.WriteAsync(bufferList[i]);
                }
                catch { }
            }            
        }
    }
}
