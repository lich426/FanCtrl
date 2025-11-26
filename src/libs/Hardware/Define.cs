
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
        Plugin,
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
            "Plugin",
            "On screen display",
        };
    }
}
