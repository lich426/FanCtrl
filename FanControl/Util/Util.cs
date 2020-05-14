using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class Util
    {
        public static void printHex(string hexString)
        {
            var dataArray = Util.getHexBytes(hexString);
            if (dataArray == null)
                return;
            Util.printHex(dataArray, dataArray.Length);
        }

        public static void printHex(byte[] dataArray)
        {
            if (dataArray == null)
                return;
            Util.printHex(dataArray, dataArray.Length);
        }

        public static void printHex(byte[] dataArray, int dataSize)
        {
            if (dataArray == null || dataArray.Length < dataSize)
                return;

            for (int i = 0; i < dataSize; i++)
            {
                Console.Write("{0:X2} ", dataArray[i]);
                if (i == 0)
                    continue;
                else if ((i + 1) % 16 == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }        

        public static byte[] getHexBytes(string hexString)
        {
            try
            {
                int length = hexString.Length;
                var bytes = new byte[length / 2];
                for (int i = 0; i < length; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
                }
                return bytes;
            }
            catch { }
            return null;
        }

        public static string getHexString(byte[] datas)
        {
            try
            {
                string hexString = string.Empty;
                hexString = string.Concat(Array.ConvertAll(datas, byt => byt.ToString("X2")));
                return hexString;
            }
            catch { }
            return "";
        }

        public static string getHexString(byte[] datas, int dataSize)
        {
            try
            {
                var array = new byte[dataSize];
                for (int i = 0; i < dataSize; i++)
                {
                    array[i] = datas[i];
                }
                return Util.getHexString(array);
            }
            catch { }
            return "";
        }

        public static bool isHex(char value)
        {
            if ((value >= 48 && value <= 57) ||
                (value >= 65 && value <= 70) ||
                (value >= 97 && value <= 102))
            {
                return true;
            }
            return false;
        }

        public static long getNowMS()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
