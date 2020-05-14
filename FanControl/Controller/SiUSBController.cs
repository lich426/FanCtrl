using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using SIUSBXP_DLL;

namespace FanControl
{
    public class SiUSBController : USBController
    {
        private bool mThreadState = false;
        private Thread mThread = null;

        private IntPtr mDeviceHandle;

        private List<List<byte[]>> mSendDataArrayList = new List<List<byte[]>>();

        private object mLock = new object();

        public SiUSBController(USBVendorID vendorID, USBProductID productID) : base(vendorID, productID)
        {

        }

        public override bool start()
        {
            Monitor.Enter(mLock);
            try
            {
                uint count = SiUSBController.count();
                if (count == 0)
                {
                    Console.WriteLine("SiUSBController.start() : USB device is zero");
                    Monitor.Exit(mLock);
                    this.stop();
                    return false;
                }

                Console.WriteLine("SiUSBController.start() : USB device count({0})", count);

                bool isDeviceOpen = false;
                for (uint i = 0; i < count; i++)
                {
                    var vidSB = new StringBuilder();
                    var pidSB = new StringBuilder();
                    if (SiUSBController.getVID(i, vidSB) > 0 && SiUSBController.getPID(i, pidSB) > 0)
                    {
                        string vidString = vidSB.ToString();
                        string pidString = pidSB.ToString();

                        Console.WriteLine("SiUSBController.start() : Device index({0}), VendorID(0x{1:X4}), ProductID(0x{2:X4})", i, vidString, pidString);
                        
                        var vidHex = Util.getHexBytes(vidString);
                        var pidHex = Util.getHexBytes(pidString);
                        Array.Reverse(vidHex);
                        Array.Reverse(pidHex);

                        ushort vID = BitConverter.ToUInt16(vidHex, 0);
                        ushort pID = BitConverter.ToUInt16(pidHex, 0);

                        if (vID == (ushort)VendorID && pID == (ushort)ProductID)
                        {
                            if (SiUSBController.open(i, ref mDeviceHandle) == true)
                            {
                                Console.WriteLine("SiUSBController.start() : Success open({0})", i);
                                isDeviceOpen = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("SiUSBController.start() : Failed open({0})", i);
                            }
                        }
                    }
                }

                if(isDeviceOpen == false)
                {
                    Console.WriteLine("SiUSBController.start() : Failed device open");
                    Monitor.Exit(mLock);
                    this.stop();
                    return false;
                }

                mThreadState = true;
                mThread = new Thread(threadFunc);
                mThread.Start();
                Monitor.Exit(mLock);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("SiUSBController.start() : Failed catch({0})", e.Message);
                Monitor.Exit(mLock);
                this.stop();
                return false;
            }
        }

        public override void stop()
        {
            Monitor.Enter(mLock);
            mThreadState = false;
            if (mThread != null)
            {
                mThread.Join();
                mThread = null;
            }

            try
            {
                SiUSBController.close(mDeviceHandle);
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
                    while(mSendDataArrayList.Count > 0)
                    {
                        var sendList = mSendDataArrayList[0];
                        mSendDataArrayList.RemoveAt(0);

                        // write
                        for (int i = 0; i < sendList.Count; i++)
                        {
                            Console.WriteLine("SiUSBController.threadFunc() : write()-----------------------------------------");
                            Util.printHex(sendList[i]);

                            int ret = SiUSBController.write(mDeviceHandle, sendList[i], (uint)sendList[i].Length, 500);
                            if (ret < 0)
                            {
                                Console.WriteLine(String.Format("SiUSBController.threadFunc() : Failed send({0})", ret));
                            }
                        }

                        // read
                        int ret2 = SiUSBController.read(mDeviceHandle, recvArray, (uint)recvArray.Length, 500);
                        if (ret2 <= 0)
                        {
                            Console.WriteLine(String.Format("SiUSBController.threadFunc() : Failed recv({0})", ret2));
                            Monitor.Exit(mLock);
                            Thread.Sleep(10);
                            continue;
                        }

                        this.onReport(recvArray, ret2);
                    }
                }
                catch { }
                Monitor.Exit(mLock);
                Thread.Sleep(10);
            }
        }

        private void onReport(byte[] recvArray, int recvDataSize)
        {
            ///*
            Console.WriteLine("SiUSBController.onReport() -----------------------------------------");
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

        ///////////////////////////// SiUSBXp ///////////////////////////
        private static uint count()
        {
            try
            {
                uint dwNumDevices = 0;
                var status = SIUSBXP.SI_GetNumDevices(ref dwNumDevices);
                if (status != SIUSBXP.SI_SUCCESS || dwNumDevices == 0)
                    return 0;
                return dwNumDevices;
            }
            catch { }
            return 0;            
        }

        private static bool open(uint index, ref IntPtr deviceHandle)
        {
            try
            {
                if (index >= SiUSBController.count())
                    return false;
                var status = SIUSBXP.SI_Open(index, ref deviceHandle);
                return (status == SIUSBXP.SI_SUCCESS);
            }
            catch { }
            return false;            
        }

        private static void close(IntPtr deviceHandle)
        {
            try
            {
                SIUSBXP.SI_Close(deviceHandle);
            }
            catch { }
        }

        private static int getVID(uint index, StringBuilder builder)
        {
            try
            {
                if (index >= SiUSBController.count())
                    return -100;
                var status = SIUSBXP.SI_GetProductString(index, builder, SIUSBXP.SI_RETURN_VID);
                return (status == SIUSBXP.SI_SUCCESS) ? 1 : -1;
            }
            catch { }
            return -200;
        }
        
        private static int getPID(uint index, StringBuilder builder)
        {
            try
            {
                if (index >= SiUSBController.count())
                    return -100;
                var status = SIUSBXP.SI_GetProductString(index, builder, SIUSBXP.SI_RETURN_PID);
                return (status == SIUSBXP.SI_SUCCESS) ? 1 : -1;
            }
            catch { }
            return -200;
        }

        private static int write(IntPtr deviceHandle, byte[] pData, uint dataSize, uint timeout)
        {
            try
            {
                uint tmpReadTO = 0;
                uint tmpWriteTO = 0;
                int status = SIUSBXP.SI_SUCCESS;

                // Save timeout values.
                SIUSBXP.SI_GetTimeouts(ref tmpReadTO, ref tmpWriteTO);

                // Set a timeout for the write
                SIUSBXP.SI_SetTimeouts(0, timeout);

                // Write the data
                uint sendDataSize = 0;
                status = SIUSBXP.SI_Write(deviceHandle, pData, dataSize, ref sendDataSize, IntPtr.Zero);

                // Restore timeouts
                SIUSBXP.SI_SetTimeouts(tmpReadTO, tmpWriteTO);

                return (status == SIUSBXP.SI_SUCCESS) ? (int)sendDataSize : -1;
            }
            catch { }
            return -200;
        }

        private static int read(IntPtr deviceHandle, byte[] pData, uint dataSize, uint timeout)
        {
            try
            {
                uint tmpReadTO = 0;
                uint tmpWriteTO = 0;
                int status = SIUSBXP.SI_SUCCESS;

                // Save timeout values.
                SIUSBXP.SI_GetTimeouts(ref tmpReadTO, ref tmpWriteTO);

                // Set a timeout for the read
                SIUSBXP.SI_SetTimeouts(timeout, 0);

                // Read the data
                uint recvDataSize = 0;
                status = SIUSBXP.SI_Read(deviceHandle, pData, dataSize, ref recvDataSize, IntPtr.Zero);

                // Restore timeouts
                SIUSBXP.SI_SetTimeouts(tmpReadTO, tmpWriteTO);

                return (status == SIUSBXP.SI_SUCCESS) ? (int)recvDataSize : -1;
            }
            catch { }
            return -200;
        }
    }
}
