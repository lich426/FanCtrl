using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.InteropServices.WindowsRuntime;

namespace FanControl
{
    public class WinUSBController : USBController
    {
        private const byte ENDPOINT_TYPE_CTRL = 0x00;
        private const byte ENDPOINT_TYPE_ISO = 0x01;
        private const byte ENDPOINT_TYPE_BULK = 0x02;
        private const byte ENDPOINT_TYPE_INTR = 0x03;

        private const byte CTRL_TYPE_STANDARD = (0 << 5);
        private const byte CTRL_TYPE_CLASS = (1 << 5);
        private const byte CTRL_TYPE_VENDOR = (2 << 5);
        private const byte CTRL_TYPE_RESERVED = (3 << 5);

        private const byte CTRL_RECIPIENT_DEVICE = 0;
        private const byte CTRL_RECIPIENT_INTERFACE = 1;
        private const byte CTRL_RECIPIENT_ENDPOINT = 2;
        private const byte CTRL_RECIPIENT_OTHER = 3;

        private const byte USBXPRESS_REQUEST = 0x02;
        private const byte USBXPRESS_FLUSH_BUFFERS = 0x01;
        private const byte USBXPRESS_CLEAR_TO_SEND = 0x02;
        private const byte USBXPRESS_NOT_CLEAR_TO_SEND = 0x04;
        private const byte USBXPRESS_GET_PART_NUM = 0x08;

        private const byte USBXPRESS = CTRL_OUT | CTRL_TYPE_VENDOR | CTRL_RECIPIENT_DEVICE;

        private const byte CTRL_OUT = 0x00;
        private const byte CTRL_IN = 0x80;

        private byte mEndPointIn;
        private byte mEndPointOut;

        private bool mIsDeviceOpen = false;
        private IntPtr mDeviceHandle;

        private bool mThreadState = false;
        private Thread mThread = null;

        private List<List<byte[]>> mSendDataArrayList = new List<List<byte[]>>();

        private object mLock = new object();

        public WinUSBController(USBVendorID vendorID, USBProductID productID, byte endPointIn, byte endPointOut) : base(vendorID, productID)
        {
            mEndPointIn = endPointIn;
            mEndPointOut = endPointOut;
        }

        public override bool start()
        {
            Monitor.Enter(mLock);
            try
            {
                if (WinUSBController.initUSB() == false)
                {
                    Console.WriteLine("WinUSBController.start() : Failed init");
                    Monitor.Exit(mLock);
                    this.stop();
                    return false;
                }

                mDeviceHandle = WinUSBController.open((ushort)VendorID, (ushort)ProductID);
                mIsDeviceOpen = WinUSBController.isOpen(mDeviceHandle);
                if (mIsDeviceOpen == false)
                {
                    Console.WriteLine("WinUSBController.start() : Failed open, vendor(0x{0:X4}), product(0x{1:X4})", (ushort)VendorID, (ushort)ProductID);
                    Monitor.Exit(mLock);
                    this.stop();
                    return false;
                }

                int ret = WinUSBController.controlTransfer(mDeviceHandle, USBXPRESS, USBXPRESS_REQUEST, USBXPRESS_CLEAR_TO_SEND, 0, null, 0, 0);
                if (ret < 0)
                {
                    Console.WriteLine(String.Format("WinUSBController.start() : Failed controlTransfer({0})", ret));
                    Monitor.Exit(mLock);
                    this.stop();
                    return false;
                }

                mThreadState = true;
                mThread = new Thread(threadFunc);
                mThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("SiUSBController.start() : Failed catch({0})", e.Message);
                Monitor.Exit(mLock);
                this.stop();
                return false;
            }
            Monitor.Exit(mLock);
            return true;
        }

        public override void stop()
        {
            Monitor.Enter(mLock);
            mThreadState = false;
            if (mThread != null)
            {
                mThread.Join();
            }

            try
            {
                if(mIsDeviceOpen == true)
                {
                    WinUSBController.close(mDeviceHandle);
                    mIsDeviceOpen = false;
                }                
                WinUSBController.exitUSB();
            }
            catch { }
            Monitor.Exit(mLock);
        }

        private void threadFunc()
        {
            var recvArray = new byte[32];
            while (mThreadState == true)
            {
                if (Monitor.TryEnter(mLock) == false)
                {
                    Thread.Sleep(10);
                    continue;
                }
                try
                {
                    if(mSendDataArrayList.Count == 0)
                    {
                        Monitor.Exit(mLock);
                        Thread.Sleep(10);
                        continue;
                    }

                    int ret = this.beginTransaction();
                    if (ret < 0)
                    {
                        Console.WriteLine(String.Format("WinUSBController.threadFunc() : Failed beginTransaction({0})", ret));
                        Monitor.Exit(mLock);
                        Thread.Sleep(10);
                        continue;
                    }

                    var sendList = mSendDataArrayList[0];
                    mSendDataArrayList.RemoveAt(0);

                    for(int i = 0; i < sendList.Count; i++)
                    {
                        ret = this.write(sendList[i]);
                        if (ret < 0)
                        {
                            Console.WriteLine(String.Format("WinUSBController.threadFunc() : Failed write({0})", ret));
                        }
                    }                    

                    ret = this.endTransactionRead(recvArray);
                    if (ret < 0)
                    {
                        Console.WriteLine(String.Format("WinUSBController.threadFunc() : Failed endTransactionRead({0})", ret));
                        Monitor.Exit(mLock);
                        Thread.Sleep(10);
                        continue;
                    }

                    this.onReport(recvArray, ret);                    
                }
                catch { }
                Monitor.Exit(mLock);
                Thread.Sleep(10);
            }
        }

        private int beginTransaction()
        {
            // Claim Interface
            int ret = WinUSBController.claimInterface(mDeviceHandle, 0);
            if (ret < 0)
            {
                Console.WriteLine(String.Format("WinUSBController.beginTransaction() : Failed claimInterface({0})", ret));
                return ret;
            }

            ret = WinUSBController.controlTransfer(mDeviceHandle, USBXPRESS, USBXPRESS_REQUEST, USBXPRESS_FLUSH_BUFFERS, 0, null, 0, 0);
            if (ret < 0)
            {
                Console.WriteLine(String.Format("WinUSBController.beginTransaction() : Failed controlTransfer({0})", ret));
                WinUSBController.releaseInterface(mDeviceHandle, 0);
                return ret;
            }
            return ret;
        }

        private int endTransactionRead(byte[] recvArray)
        {
            int ret = WinUSBController.bulkTransfer(mDeviceHandle,
                                                    mEndPointIn,
                                                    recvArray,
                                                    recvArray.Length,
                                                    500);

            if (ret < 0)
            {
                Console.WriteLine(String.Format("WinUSBController.endTransactionRead() : Failed bulkTransfer({0})", ret));
            }

            int ret2 = WinUSBController.releaseInterface(mDeviceHandle, 0);
            if (ret2 < 0)
            {
                Console.WriteLine(String.Format("WinUSBController.endTransactionRead() : Failed releaseInterface({0})", ret2));
            }
            return ret;
        }

        private int write(byte[] sendArray)
        {
            int ret = WinUSBController.bulkTransfer(mDeviceHandle,
                                                    mEndPointOut,
                                                    sendArray,
                                                    sendArray.Length,
                                                    500);

            if (ret < 0)
            {
                Console.WriteLine(String.Format("WinUSBController.write() : Failed bulkTransfer({0})", ret));
            }
            return ret;
        }


        private void onReport(byte[] recvArray, int recvDataSize)
        {
            ///*
            Console.WriteLine("WinUSBController.onReport() -----------------------------------------");
            Util.printHex(recvArray, recvDataSize);
            //*/
            this.onRecv(recvArray, recvDataSize);
        }

        public override void send(List<byte[]> bufferList)
        {
            Monitor.Enter(mLock);
            mSendDataArrayList.Add(bufferList);
            Monitor.Exit(mLock);
        }

        [DllImport("libusb.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool initUSB();

        [DllImport("libusb.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void exitUSB();

        [DllImport("libusb.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr open(ushort vendorID, ushort productID);

        [DllImport("libusb.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool isOpen(IntPtr deviceHandle);

        [DllImport("libusb.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void close(IntPtr deviceHandle);

        [DllImport("libusb.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int setConfiguration(IntPtr deviceHandle, int configuration);

        [DllImport("libusb.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int claimInterface(IntPtr deviceHandle, int interfaceNumber);

        [DllImport("libusb.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int releaseInterface(IntPtr deviceHandle, int interfaceNumber);

        [DllImport("libusb.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int bulkTransfer(IntPtr deviceHandle, byte endpoint, byte[] pData, int dataSize, uint timeout);

        [DllImport("libusb.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int controlTransfer(IntPtr deviceHandle, byte bmRequestType, byte bRequest, ushort wValue, ushort wIndex, byte[] pData, int dataSize, uint timeout);
    }
}
