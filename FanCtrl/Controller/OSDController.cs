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
    class OSDController
    {
        public static bool update(string osdString)
        {
            try
            {
                return OSDController.updateOSD(osdString);
            }
            catch { }
            return false;
        }

        public static void release()
        {
            try
            {
                OSDController.releaseOSD();
            }
            catch { }
        }

        [DllImport("OSD.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool updateOSD(string osdString);

        [DllImport("OSD.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void releaseOSD();
    }
}
