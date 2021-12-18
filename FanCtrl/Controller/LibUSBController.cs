using LibUsbDotNet;
using LibUsbDotNet.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class LibUSBController : USBController
    {
        private UsbDevice mUsbDevice;
        private UsbEndpointReader mEpReader;
        private UsbEndpointWriter mEpWriter;
        private UsbRegDeviceList mRegDevices;

        private delegate void SendDelegate();

        private uint mIndex = 0;
        private bool mIsSend = false;
        private List<byte[]> mSendArrayList = new List<byte[]>();
        private object mSendArrayListLock = new object();

        public LibUSBController(USBVendorID vendorID, USBProductID productID) : base(vendorID, productID)
        {
            mRegDevices = UsbDevice.AllDevices;
            UsbDevice.UsbErrorEvent += OnUsbGlobalErrorEvent;
        }

        public static uint getDeviceCount(USBVendorID vendorID, USBProductID productID)
        {
            uint count = 0;
            try
            {
                var regDevices = UsbDevice.AllDevices;
                int venderID2 = (int)vendorID;
                int productID2 = (int)productID;
                foreach (UsbRegistry reg in regDevices)
                {
                    if (reg.Pid == productID2 && reg.Vid == venderID2)
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
            bool bRtn = false;

            this.stop();
            int idx = (int)index;
            if (mRegDevices[idx].Open(out mUsbDevice))
            {
                bRtn = true;
                IUsbDevice wholeUsbDevice = mUsbDevice as IUsbDevice;
                if (!ReferenceEquals(wholeUsbDevice, null))
                {
                    // Select config #1
                    wholeUsbDevice.SetConfiguration(1);

                    // Claim interface #0.
                    wholeUsbDevice.ClaimInterface(0);
                }

                if (bRtn)
                {
                    mEpReader = mUsbDevice.OpenEndpointReader((ReadEndpointID)(0x01 | 0x81));
                    mEpWriter = mUsbDevice.OpenEndpointWriter((WriteEndpointID)0x02);
                    mEpReader.DataReceived += OnDataReceivedEvent;
                    mEpReader.DataReceivedEnabled = true;
                    mEpReader.Flush();
                }
            }

            if (bRtn)
            {
                // tsStatus.Text = "Device Opened.";
            }
            else
            {
                // tsStatus.Text = "Device Failed to Opened!";
                if (!ReferenceEquals(mUsbDevice, null))
                {
                    if (mUsbDevice.IsOpen) mUsbDevice.Close();
                    mUsbDevice = null;
                }
            }
            return bRtn;
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

        private async void writeAsync()
        {
            Monitor.Enter(mSendArrayListLock);
            mIsSend = true;
            Monitor.Exit(mSendArrayListLock);
            var sendDelegate = new SendDelegate(write);
            await Task.Factory.FromAsync(sendDelegate.BeginInvoke, sendDelegate.EndInvoke, null);
        }

        private void write()
        {
            Monitor.Enter(mSendArrayListLock);
            try
            {                
                if (mEpWriter != null)
                {
                    int uiTransmitted;
                    for (int i = 0; i < mSendArrayList.Count; i++)
                    {
                        if (mEpWriter.Write(mSendArrayList[i], 1000, out uiTransmitted) == ErrorCode.None)
                        {
                            Console.WriteLine("OnDataReceivedEvent : OK({0} byte)", uiTransmitted);
                        }
                    }
                }
            }
            catch
            {
                Monitor.Exit(mSendArrayListLock);
                return;
            }
            mSendArrayList.Clear();
            mIsSend = false;
            Monitor.Exit(mSendArrayListLock);
        }

        public override void stop()
        {
            if (mUsbDevice != null)
            {
                try
                {
                    if (mUsbDevice.IsOpen)
                    {
                        if (mEpReader != null)
                        {
                            mEpReader.DataReceivedEnabled = false;
                            mEpReader.DataReceived -= OnDataReceivedEvent;
                            mEpReader.Dispose();
                            mEpReader = null;
                        }
                        if (mEpWriter != null)
                        {
                            mEpWriter.Abort();
                            mEpWriter.Dispose();
                            mEpWriter = null;
                        }

                        IUsbDevice wholeUsbDevice = mUsbDevice as IUsbDevice;
                        if (!ReferenceEquals(wholeUsbDevice, null))
                        {
                            // Release interface #0.
                            wholeUsbDevice.ReleaseInterface(0);
                        }

                        mUsbDevice.Close();
                        mUsbDevice = null;
                    }
                }
                catch { }

                Monitor.Enter(mSendArrayListLock);
                mIsSend = false;
                mSendArrayList.Clear();
                Monitor.Exit(mSendArrayListLock);
            }
        }

        private void OnDataReceivedEvent(object sender, EndpointDataEventArgs e)
        {
            Console.WriteLine("OnDataReceivedEvent : OK");
        }

        private void OnUsbGlobalErrorEvent(object sender, UsbError e)
        {
            Console.WriteLine("OnUsbGlobalErrorEvent : OK");
        }
    }
}
