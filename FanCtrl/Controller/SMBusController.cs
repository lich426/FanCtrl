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

namespace FanCtrl
{
    class SMBusController
    {
        public static ushort[] sManufacturerID = {
            0x1B09,     // On Semiconductor
            0x0054,     // MicroChip
            0x104A,     // ST
            0x00B3,     // RENESAS
            0x1131,     // NXP
            0x1C85,     // ABLIC

        };

        enum CPU_TYPE
        {
            AMD,
            INTEL,
            UNKNOWN,
        };


        private static object sLock = new object();
        private static CPU_TYPE sCpuType = CPU_TYPE.UNKNOWN;

        public static bool open()
        {
            Monitor.Enter(sLock);
            if (Ring0.IsOpen == true)
            {
                Monitor.Exit(sLock);
                return true;
            }               

            try
            {
                Ring0.Open();
                if (Ring0.IsOpen == true)
                {
                    try
                    {
                        string wmiQuery = "SELECT * FROM Win32_PnPSignedDriver WHERE Description LIKE '%%SMBUS%%' OR Description LIKE '%%SM BUS%%'";
                        var searcher = new ManagementObjectSearcher(wmiQuery);
                        var collection = searcher.Get();
                        string manufacturer = "";
                        foreach (var obj in collection)
                        {
                            manufacturer = obj["Manufacturer"].ToString().ToUpper();
                            if (manufacturer.Equals("INTEL") == true)
                            {
                                wmiQuery = "SELECT * FROM Win32_PnPAllocatedResource";
                                string deviceID = obj["DeviceID"].ToString().Substring(4, 33);

                                var searcher2 = new ManagementObjectSearcher(wmiQuery);
                                var collection2 = searcher2.Get();
                                foreach (var obj2 in collection2)
                                {
                                    string dependent = obj2["Dependent"].ToString();
                                    string antecedent = obj2["Antecedent"].ToString();

                                    if (dependent.IndexOf(deviceID) >= 0 && antecedent.IndexOf("Port") >= 0)
                                    {
                                        var antecedentArray = antecedent.Split('=');
                                        if (antecedentArray.Length >= 2)
                                        {
                                            string addressString = antecedentArray[1].Replace("\"", "");
                                            if (addressString.Length > 0)
                                            {
                                                ushort startAddress = ushort.Parse(addressString);
                                                SMBusIntel.setSMBAddress(startAddress);
                                                sCpuType = CPU_TYPE.INTEL;
                                            }
                                        }
                                    }
                                }
                            }

                            else if (manufacturer.Equals("ADVANCED MICRO DEVICES, INC") == true)
                            {
                                sCpuType = CPU_TYPE.AMD;
                            }
                            break;
                        }
                    }
                    catch
                    {
                        Monitor.Exit(sLock);
                        return false;
                    }
                    Monitor.Exit(sLock);
                    return true;
                }
            }
            catch { }
            Monitor.Exit(sLock);
            return false;
        }

        public static void close()
        {
            Monitor.Enter(sLock);
            if (Ring0.IsOpen == false)
            {
                Monitor.Exit(sLock);
                return;
            }

            try
            {
                Ring0.Close();
            }
            catch { }
            Monitor.Exit(sLock);
        }

        public static byte smbusDetect(byte address)
        {
            Monitor.Enter(sLock);
            if (Ring0.IsOpen == false)
            {
                Monitor.Exit(sLock);
                return 0x00;
            }

            byte addr = 0;
            try
            {
                if (sCpuType == CPU_TYPE.AMD)
                {
                    addr = SMbusAmd.smbDetect(address);
                }
                else if(sCpuType == CPU_TYPE.INTEL)
                {
                    addr = SMBusIntel.smbDetect(address);
                }
                Monitor.Exit(sLock);
                return addr;
            }
            catch { }
            Monitor.Exit(sLock);
            return 0x00;
        }

        public static ushort smbusWordData(byte address, byte offset)
        {
            Monitor.Enter(sLock);
            if (Ring0.IsOpen == false)
            {
                Monitor.Exit(sLock);
                return 0;
            }

            ushort data = 0;
            try
            {
                if (sCpuType == CPU_TYPE.AMD)
                {
                    data = SMbusAmd.getWord(address, offset);
                }
                else if (sCpuType == CPU_TYPE.INTEL)
                {
                    data = SMBusIntel.getWord(address, offset);
                }
                Monitor.Exit(sLock);
                return data;
            }
            catch { }
            Monitor.Exit(sLock);
            return 0;
        }
    }
}
