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
        LHM,
        NvAPIWrapper,
        DIMM,
        NZXT_Kraken,
        EVGA_CLC,
        RGBnFC,
        HWiNFO,
        Liquidctl,
        OSD,

        MAX,
    };

    public class Define
    {
        public static string[] cLibraryTypeString = {
            "LibreHardwareMonitor",
            "NvAPIWrapper",
            "DIMM",
            "NZXT Kraken",
            "EVGA CLC",
            "NZXT RGB&FanController",
            "HWiNFO",
            "liquidctl",
            "On screen display",
        };
    }
}
