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
        [DllImport("OSD.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool updateOSD(string osdString);

        [DllImport("OSD.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void releaseOSD();
    }
}
