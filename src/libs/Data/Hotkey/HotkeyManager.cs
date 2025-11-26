using System.IO;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FanCtrl
{
    public class HotkeyManager
    {
        private string mHotkeyFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "Hotkey.json";

        private static HotkeyManager sManager = new HotkeyManager();
        public static HotkeyManager getInstance() { return sManager; }

        public HotkeyData mEnableFanControlData { get; set; } = new HotkeyData();
        public HotkeyData mModeNormalData { get; set; } = new HotkeyData();
        public HotkeyData mModeSilenceData { get; set; } = new HotkeyData();
        public HotkeyData mModePerformanceData { get; set; } = new HotkeyData();
        public HotkeyData mModeGameData { get; set; } = new HotkeyData();
        public HotkeyData mEnableOSDData { get; set; } = new HotkeyData();

        private HotkeyManager()
        {
            this.reset();
        }

        public void reset()
        {
            mEnableFanControlData.reset();
            mModeNormalData.reset();
            mModeSilenceData.reset();
            mModePerformanceData.reset();
            mModeGameData.reset();
            mEnableOSDData.reset();
        }

        public bool read()
        {
            try 
            {
                var jsonString = File.ReadAllText(mHotkeyFileName);                
                var rootObject = JObject.Parse(jsonString);

                this.readData(rootObject, "mEnableFanControlData", mEnableFanControlData);
                this.readData(rootObject, "mModeNormalData", mModeNormalData);
                this.readData(rootObject, "mModeSilenceData", mModeSilenceData);
                this.readData(rootObject, "mModePerformanceData", mModePerformanceData);
                this.readData(rootObject, "mModeGameData", mModeGameData);
                this.readData(rootObject, "mEnableOSDData", mEnableOSDData);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void readData(JObject rootObject, string keyString, HotkeyData HotkeyData)
        {
            if (rootObject.ContainsKey(keyString) == false)
                return;

            var hotKeyObject = rootObject.Value<JObject>(keyString);
            bool isCtrl = (hotKeyObject.ContainsKey("mIsCtrl") == true) ? hotKeyObject.Value<bool>("mIsCtrl") : false;
            bool isAlt = (hotKeyObject.ContainsKey("mIsAlt") == true) ? hotKeyObject.Value<bool>("mIsAlt") : false;
            bool isLShift = (hotKeyObject.ContainsKey("mIsLShift") == true) ? hotKeyObject.Value<bool>("mIsLShift") : false;
            bool isRShift = (hotKeyObject.ContainsKey("mIsRShift") == true) ? hotKeyObject.Value<bool>("mIsRShift") : false;
            int key = (hotKeyObject.ContainsKey("mKey") == true) ? hotKeyObject.Value<int>("mKey") : 0;

            HotkeyData.mIsCtrl = isCtrl;
            HotkeyData.mIsAlt = isAlt;
            HotkeyData.mIsLShift = isLShift;
            HotkeyData.mIsRShift = isRShift;
            HotkeyData.mKey = key;
        }

        public void write()
        {
            try
            {
                var rootObject = new JObject();

                this.writeData(rootObject, "mEnableFanControlData", mEnableFanControlData);
                this.writeData(rootObject, "mModeNormalData", mModeNormalData);
                this.writeData(rootObject, "mModeSilenceData", mModeSilenceData);
                this.writeData(rootObject, "mModePerformanceData", mModePerformanceData);
                this.writeData(rootObject, "mModeGameData", mModeGameData);
                this.writeData(rootObject, "mEnableOSDData", mEnableOSDData);

                File.WriteAllText(mHotkeyFileName, rootObject.ToString());
            }
            catch {}
        }

        private void writeData(JObject rootObject, string keyString, HotkeyData HotkeyData)
        {
            var tempObject = new JObject();
            tempObject["mIsCtrl"] = HotkeyData.mIsCtrl;
            tempObject["mIsAlt"] = HotkeyData.mIsAlt;
            tempObject["mIsLShift"] = HotkeyData.mIsLShift;
            tempObject["mIsRShift"] = HotkeyData.mIsRShift;
            tempObject["mKey"] = HotkeyData.mKey;

            rootObject[keyString] = tempObject;
        }
    }
}
