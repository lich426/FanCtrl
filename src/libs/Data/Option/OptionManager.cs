using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace FanCtrl
{
    public class OptionManager
    {
        private string mOptionFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "Option.json";

        private static OptionManager sManager = new OptionManager();
        public static OptionManager getInstance() { return sManager; }

        private StartupControl mStartupControl = new StartupControl();

        private OptionManager()
        {
            this.reset();
            if (read() == false)
            {
                write();
            }
        }
        
        public int Interval { get; set; }

        // LibreHardwareMonitor
        public bool IsLHM { get; set; }
        public bool IsLHMCpu { get; set; }
        public bool IsLHMMotherboard { get; set; }
        public bool IsLHMGpu { get; set; }
        public bool IsLHMContolled { get; set; }
        public bool IsLHMStorage { get; set; }
        public bool IsLHMMemory { get; set; }

        // NvApiWrapper
        public bool IsNvAPIWrapper { get; set; }

        // Dimm
        public bool IsDimm { get; set; }

        // NZXT Kraken X2, X3
        public bool IsKraken { get; set; }

        // EVGA CLC
        public bool IsCLC { get; set; }

        // NZXT Fan&Contoller
        public bool IsRGBnFC { get; set; }

        // HWiNFO
        public bool IsHWInfo { get; set; }

        // liquidctl
        public bool IsLiquidctl { get; set; }

        // Plugin
        public bool IsPlugin { get; set; }

        // Other options
        public int Language { get; set; }

        public THEME_TYPE Theme { get; set; }

        public bool IsAnimation { get; set; }

        public bool IsFahrenheit { get; set; }

        public bool IsMinimized { get; set; }

        public int DelayTime
        {
            get
            {
                return mStartupControl.DelayTime;
            }
            set
            {

                mStartupControl.DelayTime = value;
            }
        }

        public bool IsStartUp
        {
            get
            {
                return mStartupControl.Startup;
            }
            set
            {

                mStartupControl.Startup = value;
            }
        }

        public void reset()
        {
            Interval = 1000;

            IsLHM = true;
            IsLHMCpu = true;
            IsLHMMotherboard = true;
            IsLHMGpu = true;
            IsLHMContolled = true;
            IsLHMStorage = true;
            IsLHMMemory = true;

            IsNvAPIWrapper = false;
            IsDimm = false;
            IsKraken = false;
            IsCLC = false;
            IsRGBnFC = false;
            IsHWInfo = false;
            IsLiquidctl = false;
            IsPlugin = false;

            Language = getSystemLocale();
            Theme = 0;
            IsAnimation = true;
            IsFahrenheit = false;
            IsMinimized = false;
        }

        public bool read()
        {
            try 
            {
                var jsonString = File.ReadAllText(mOptionFileName);                
                var rootObject = JObject.Parse(jsonString);

                Interval = (rootObject.ContainsKey("Interval") == true) ? rootObject.Value<int>("Interval") : 1000;

                IsLHM = (rootObject.ContainsKey("IsLHM") == true) ? rootObject.Value<bool>("IsLHM") : true;
                IsLHMCpu = (rootObject.ContainsKey("IsLHMCpu") == true) ? rootObject.Value<bool>("IsLHMCpu") : true;
                IsLHMMotherboard = (rootObject.ContainsKey("IsLHMMotherboard") == true) ? rootObject.Value<bool>("IsLHMMotherboard") : true;
                IsLHMGpu = (rootObject.ContainsKey("IsLHMGpu") == true) ? rootObject.Value<bool>("IsLHMGpu") : true;
                IsLHMContolled = (rootObject.ContainsKey("IsLHMContolled") == true) ? rootObject.Value<bool>("IsLHMContolled") : true;
                IsLHMStorage = (rootObject.ContainsKey("IsLHMStorage") == true) ? rootObject.Value<bool>("IsLHMStorage") : true;
                IsLHMMemory = (rootObject.ContainsKey("IsLHMMemory") == true) ? rootObject.Value<bool>("IsLHMMemory") : true;

                IsNvAPIWrapper = (rootObject.ContainsKey("IsNvAPIWrapper") == true) ? rootObject.Value<bool>("IsNvAPIWrapper") : false;
                IsDimm = (rootObject.ContainsKey("IsDimm") == true) ? rootObject.Value<bool>("IsDimm") : false;
                IsKraken = (rootObject.ContainsKey("IsKraken") == true) ? rootObject.Value<bool>("IsKraken") : false;
                IsCLC = (rootObject.ContainsKey("IsCLC") == true) ? rootObject.Value<bool>("IsCLC") : false;
                IsRGBnFC = (rootObject.ContainsKey("IsRGBnFC") == true) ? rootObject.Value<bool>("IsRGBnFC") : false;
                IsHWInfo = (rootObject.ContainsKey("IsHWInfo") == true) ? rootObject.Value<bool>("IsHWInfo") : false;
                IsLiquidctl = (rootObject.ContainsKey("IsLiquidctl") == true) ? rootObject.Value<bool>("IsLiquidctl") : false;
                IsPlugin = (rootObject.ContainsKey("IsPlugin") == true) ? rootObject.Value<bool>("IsPlugin") : false;

                Language = (rootObject.ContainsKey("Language") == true) ? rootObject.Value<int>("Language") : this.getSystemLocale();
                Theme = (rootObject.ContainsKey("Theme") == true) ? (THEME_TYPE)rootObject.Value<int>("Theme") : THEME_TYPE.SYSTEM;
                IsAnimation = (rootObject.ContainsKey("IsAnimation") == true) ? rootObject.Value<bool>("IsAnimation") : true;
                IsFahrenheit = (rootObject.ContainsKey("IsFahrenheit") == true) ? rootObject.Value<bool>("IsFahrenheit") : false;
                IsMinimized = (rootObject.ContainsKey("IsMinimized") == true) ? rootObject.Value<bool>("IsMinimized") : false;

                DelayTime = (rootObject.ContainsKey("DelayTime") == true) ? rootObject.Value<int>("DelayTime") : 0;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void write()
        {
            try
            {
                var rootObject = new JObject();
                rootObject["Interval"] = Interval;
                
                rootObject["IsLHM"] = IsLHM;
                rootObject["IsLHMCpu"] = IsLHMCpu;
                rootObject["IsLHMMotherboard"] = IsLHMMotherboard;
                rootObject["IsLHMGpu"] = IsLHMGpu;
                rootObject["IsLHMContolled"] = IsLHMContolled;
                rootObject["IsLHMStorage"] = IsLHMStorage;
                rootObject["IsLHMMemory"] = IsLHMMemory;

                rootObject["IsNvAPIWrapper"] = IsNvAPIWrapper;
                rootObject["IsDimm"] = IsDimm;                
                rootObject["IsKraken"] = IsKraken;
                rootObject["IsCLC"] = IsCLC;
                rootObject["IsRGBnFC"] = IsRGBnFC;
                rootObject["IsHWInfo"] = IsHWInfo;
                rootObject["IsLiquidctl"] = IsLiquidctl;
                rootObject["IsPlugin"] = IsPlugin;

                rootObject["Language"] = Language;
                rootObject["Theme"] = (int)Theme;
                rootObject["IsAnimation"] = IsAnimation;
                rootObject["IsFahrenheit"] = IsFahrenheit;
                rootObject["IsMinimized"] = IsMinimized;

                rootObject["DelayTime"] = DelayTime;

                File.WriteAllText(mOptionFileName, rootObject.ToString());
            }
            catch {}
        }

        public int getSystemLocale()
        {
            var name = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            if (name.CompareTo("ko") == 0)
            {
                return 1;
            }
            else if (name.CompareTo("ja") == 0)
            {
                return 2;
            }
            else if (name.CompareTo("fr") == 0)
            {
                return 3;
            }
            else if (name.CompareTo("es") == 0)
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }

        public THEME_TYPE getNowTheme()
        {
            if (this.Theme == THEME_TYPE.SYSTEM)
            {
                var key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize");
                var registryValueObject = key?.GetValue("AppsUseLightTheme");
                if (registryValueObject == null)
                {
                    return THEME_TYPE.LIGHT;
                }
                var registryValue = (int)registryValueObject;
                return (registryValue > 0) ? THEME_TYPE.LIGHT : THEME_TYPE.DARK;
            }
            return this.Theme;
        }
    }
}
