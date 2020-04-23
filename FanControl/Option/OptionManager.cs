using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FanControl
{
    public class OptionManager
    {
        private const string cOptionFileName = "Option.json";

        private static OptionManager sManager = new OptionManager();
        public static OptionManager getInstance() { return sManager; }

        private StartupControl mStartupControl = new StartupControl();

        private OptionManager()
        {
            Interval = 1000;
            IsMinimized = false;
        }
        
        public int Interval { get; set; }

        public bool IsMinimized { get; set; }

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

        public bool read()
        {
            try 
            {
                var jsonString = File.ReadAllText(cOptionFileName);                
                var rootObject = JObject.Parse(jsonString);                
                Interval = rootObject.Value<int>("interval");
                IsMinimized = rootObject.Value<bool>("minimized");
                IsStartUp = rootObject.Value<bool>("startup");
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
                rootObject["interval"] = Interval;
                rootObject["minimized"] = IsMinimized;
                rootObject["startup"] = IsStartUp;
                File.WriteAllText(cOptionFileName, rootObject.ToString());
            }
            catch {}
        }

    }
}
