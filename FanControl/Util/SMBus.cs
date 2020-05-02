using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FanControl
{
    class SMBus
    {
        private static bool sIsOpen = false;
        public static bool IsOpen { 
            get {
                Monitor.Enter(sLock);
                bool isOpen = sIsOpen;
                Monitor.Exit(sLock);
                return isOpen;
            }
            set {
                Monitor.Enter(sLock);
                sIsOpen = value;
                Monitor.Exit(sLock);
            }
        }

        private static object sLock = new object();

        public static bool open()
        {
            Monitor.Enter(sLock);
            if (sIsOpen == true)
            {
                Monitor.Exit(sLock);
                return true;
            }               

            try
            {
                sIsOpen = (SMBus.openSMBus() > 0);
            }
            catch
            {
                sIsOpen = false;
            }
            Monitor.Exit(sLock);
            return sIsOpen;
        }

        public static void close()
        {
            Monitor.Enter(sLock);
            if (sIsOpen == false)
            {
                Monitor.Exit(sLock);
                return;
            }

            try
            {
                SMBus.closeSMBus();
            }
            catch { }
            sIsOpen = false;
            Monitor.Exit(sLock);
        }

        public static byte[] i2cDetect()
        {
            Monitor.Enter(sLock);
            if (sIsOpen == false)
            {
                Monitor.Exit(sLock);
                return null;
            }

            try
            {
                var pByteArrayData = SMBus.getI2CDetect();
                var retData = SMBus.getBytes(pByteArrayData);
                SMBus.deleteByteArrayData(pByteArrayData);
                Monitor.Exit(sLock);
                return retData;
            }
            catch { }
            Monitor.Exit(sLock);
            return null;
        }

        public static byte[] i2cByteData(byte address, int length)
        {
            Monitor.Enter(sLock);
            if (sIsOpen == false)
            {
                Monitor.Exit(sLock);
                return null;
            }

            try
            {
                var pByteArrayData = SMBus.getI2CByteData(address, length);
                var retData = SMBus.getBytes(pByteArrayData);
                SMBus.deleteByteArrayData(pByteArrayData);
                Monitor.Exit(sLock);
                return retData;
            }
            catch { }
            Monitor.Exit(sLock);
            return null;
        }

        public static ushort[] i2cWordData(byte address, int length)
        {
            Monitor.Enter(sLock);
            if (sIsOpen == false)
            {
                Monitor.Exit(sLock);
                return null;
            }

            try
            {
                var pByteArrayData = SMBus.getI2CWordData(address, length);
                var datas = SMBus.getBytes(pByteArrayData);
                SMBus.deleteByteArrayData(pByteArrayData);

                var retData = new ushort[length];
                for (int i = 0; i < length; i++)
                {
                    retData[i] = BitConverter.ToUInt16(datas, i * 2);
                }
                Monitor.Exit(sLock);
                return retData;
            }
            catch { }
            Monitor.Exit(sLock);
            return null;
        }

        private static byte[] getBytes(IntPtr pByteArrayData)
        {
            var datas = SMBus.getData(pByteArrayData);
            int dataSize = SMBus.getDataSize(pByteArrayData);
            var retData = new byte[dataSize];
            Marshal.Copy(datas, retData, 0, dataSize);            
            return retData;
        }

        [DllImport("SMBus.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int openSMBus();

        [DllImport("SMBus.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void closeSMBus();

        [DllImport("SMBus.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getI2CDetect();

        [DllImport("SMBus.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getI2CByteData(byte address, int length);

        [DllImport("SMBus.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getI2CWordData(byte address, int length);

        [DllImport("SMBus.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getData(IntPtr pByteArrayData);

        [DllImport("SMBus.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int getDataSize(IntPtr pByteArrayData);

        [DllImport("SMBus.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void deleteByteArrayData(IntPtr pByteArrayData);
    }
}
