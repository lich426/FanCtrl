using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FanCtrl
{
    class SMbusAmd
    {
        public static byte smbDetect(byte addr)
        {
            Ring0.WriteSmbus(SMB_HSTADD, (addr << 1) | SMB_WRITE);
            Ring0.WriteSmbus(SMB_HSTCNT, 0);
            if (transaction() == true)
            {
                ushort data = getWord(addr, 0x06);
                var temp = BitConverter.GetBytes(data);
                Array.Reverse(temp);

                ushort manufacturerID = BitConverter.ToUInt16(temp, 0);

                for (int i = 0; i < SMBusController.sManufacturerID.Length; i++)
                {
                    if (manufacturerID == SMBusController.sManufacturerID[i])
                        return addr;
                }
            }
            return 0x00;
        }

        public static ushort getWord(byte addr, byte command)
        {
            Ring0.WriteSmbus(SMB_HSTADD, (addr << 1) | SMB_READ);
            Ring0.WriteSmbus(SMB_HSTCMD, command);

            Ring0.WriteSmbus(SMB_HSTCNT, 0x0C);

            transaction();

            ushort temp = (ushort)(Ring0.ReadSmbus(SMB_HSTDAT0) + (Ring0.ReadSmbus(SMB_HSTDAT1) << 8));
            return temp;
        }

        private static bool transaction()
        {
            byte temp = (byte)Ring0.ReadSmbus(SMB_HSTSTS);

            if (temp != 0x00)
            {
                Ring0.WriteSmbus(SMB_HSTSTS, temp);

                temp = (byte)Ring0.ReadSmbus(SMB_HSTSTS);

                if (temp != 0x00)
                {
                    return false;
                }
            }

            temp = (byte)Ring0.ReadSmbus(SMB_HSTCNT);
            Ring0.WriteSmbus(SMB_HSTCNT, (byte)(temp | 0x040));

            temp = 0;
            int timeout = 0;
            int MAX_TIMEOUT = 5000;
            while ((++timeout < MAX_TIMEOUT) && temp <= 1)
            {
                temp = (byte)Ring0.ReadSmbus(SMB_HSTSTS);
            }

            if (timeout == MAX_TIMEOUT || (temp & 0x10) > 0 || (temp & 0x08) > 0 || (temp & 0x04) > 0)
            {
                return false;
            }

            temp = (byte)Ring0.ReadSmbus(SMB_HSTSTS);
            if (temp != 0x00)
            {
                Ring0.WriteSmbus(SMB_HSTSTS, temp);
            }

            return true;
        }

        private const byte SMB_READ = 0x01;
        private const byte SMB_WRITE = 0x00;

        private const ushort SMB_ADDRESS = 0x0B00;

        private const ushort SMB_HSTSTS = (0 + SMB_ADDRESS);
        //private const ushort SMB_HSLVSTS = (1 + SMB_ADDRESS);
        private const ushort SMB_HSTCNT = (2 + SMB_ADDRESS);
        private const ushort SMB_HSTCMD = (3 + SMB_ADDRESS);
        private const ushort SMB_HSTADD = (4 + SMB_ADDRESS);
        private const ushort SMB_HSTDAT0 = (5 + SMB_ADDRESS);
        private const ushort SMB_HSTDAT1 = (6 + SMB_ADDRESS);
        //private const ushort SMB_BLKDAT = (7 + SMB_ADDRESS);
        //private const ushort SMB_SLVCNT = (8 + SMB_ADDRESS);
        //private const ushort SMB_SHDWCMD = (9 + SMB_ADDRESS);
        //private const ushort SMB_SLVEVT = (0xA + SMB_ADDRESS);
        //private const ushort SMB_SLVDAT = (0xC + SMB_ADDRESS);
    }
}
