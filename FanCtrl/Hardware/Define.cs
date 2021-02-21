using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading;

namespace FanCtrl
{
    public enum LIBRARY_TYPE
    {
        Gigabyte,
        LHM,
        OHM,
        NvAPIWrapper,
        DIMM,
        NZXT_Kraken,
        EVGA_CLC,
        RGBnFC,
        HWiNFO,
        OSD,

        MAX,
    };

    public class Define
    {
        public static string[] cLibraryTypeString = {
            "Gigabyte",
            "LibreHardwareMonitor",
            "OpenHardwareMonitor",
            "NvAPIWrapper",
            "DIMM",
            "NZXT Kraken",
            "EVGA CLC",
            "NZXT RGB&FanController",
            "HWiNFO",
            "On screen display",
        };
    }
}
