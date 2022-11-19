using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FanCtrl
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

        public static int getFahrenheit(int celsius)
        {
            return (int)Math.Round(((double)celsius * 9 / 5) + 32);
        }

        public static int getCelsius(int fahrenheit)
        {
            return (int)Math.Round(((double)fahrenheit - 32) * 5 / 9);
        }

        public static void sleep(int ms)
        {
            try
            {
                Thread.Sleep(ms);
            }
            catch { }
        }

        public static void sleep(ref bool isEnd, int ms)
        {
            try
            {
                if (ms <= 0)
                    return;

                if (ms < 10)
                {
                    Thread.Sleep(ms);
                    return;
                }

                while (isEnd == true)
                {
                    ms = ms - 10;
                    if (ms == 0)
                    {
                        break;
                    }
                    else if (ms < 10)
                    {
                        Thread.Sleep(ms);
                        break;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            catch { }
        }

        public static void setLanguage(int language)
        {
            // Language
            switch (language)
            {
                case 1:
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ko-KR");
                    break;

                case 2:
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ja-JP");
                    break;

                default:
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                    break;
            }
        }
    }
}
