using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FanCtrl
{
    public enum NAME_TYPE
    {
        TEMPERATURE,
        FAN,
        CONTOL,
    }

    public enum MODE_TYPE
    {
        NORMAL,
        SILENCE,
        PERFORMANCE,
        GAME,
    }

    public class ControlManager
    {
        public string mControlFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "Control.json";

        private static ControlManager sManager = new ControlManager();
        public static ControlManager getInstance() { return sManager; }

        private Object mLock = new Object();

        private int mWidth = 748;
        public int Width
        {
            get
            {
                Monitor.Enter(mLock);
                int width = mWidth;
                Monitor.Exit(mLock);
                return width;
            }
            set
            {
                Monitor.Enter(mLock);
                mWidth = value;
                Monitor.Exit(mLock);
            }
        }

        private int mHeight = 414;
        public int Height
        {
            get
            {
                Monitor.Enter(mLock);
                int height = mHeight;
                Monitor.Exit(mLock);
                return height;
            }
            set
            {
                Monitor.Enter(mLock);
                mHeight = value;
                Monitor.Exit(mLock);
            }
        }

        private bool mIsMaximize = false;
        public bool IsMaximize
        {
            get
            {
                Monitor.Enter(mLock);
                bool isMaximize = mIsMaximize;
                Monitor.Exit(mLock);
                return isMaximize;
            }
            set
            {
                Monitor.Enter(mLock);
                mIsMaximize = value;
                Monitor.Exit(mLock);
            }
        }

        private bool mIsEnable = false;
        public bool IsEnable
        {
            get
            {
                Monitor.Enter(mLock);
                bool isEnable = mIsEnable;
                Monitor.Exit(mLock);
                return isEnable;
            }
            set
            {
                Monitor.Enter(mLock);
                mIsEnable = value;
                Monitor.Exit(mLock);
            }
        }

        private MODE_TYPE mModeType = MODE_TYPE.NORMAL;
        public MODE_TYPE ModeType
        {
            get
            {
                Monitor.Enter(mLock);
                MODE_TYPE type = mModeType;
                Monitor.Exit(mLock);
                return type;
            }
            set
            {
                Monitor.Enter(mLock);
                mModeType = value;
                Monitor.Exit(mLock);
            }
        }

        private List<ControlData>[] mControlDataList = new List<ControlData>[4];

        private ControlManager()
        {
            mControlDataList[0] = (new List<ControlData>());
            mControlDataList[1] = (new List<ControlData>());
            mControlDataList[2] = (new List<ControlData>());
            mControlDataList[3] = (new List<ControlData>());
        }

        private void clear()
        {
            mIsEnable = false;
            for (int i = 0; i < mControlDataList.Length; i++)
            {
                mControlDataList[i].Clear();
            }
            mModeType = MODE_TYPE.NORMAL;
        }

        public void reset()
        {
            Monitor.Enter(mLock);
            this.clear();
            Monitor.Exit(mLock);
        }

        public List<ControlData> getControlDataList(MODE_TYPE modeType)
        {
            Monitor.Enter(mLock);
            int modeIndex = (int)modeType;
            var controlDataList = mControlDataList[modeIndex];
            Monitor.Exit(mLock);
            return controlDataList;
        }

        public void setControlDataList(MODE_TYPE modeType, List<ControlData> controlDataList)
        {
            Monitor.Enter(mLock);
            int modeIndex = (int)modeType;
            var selectedControlDataList = mControlDataList[modeIndex];
            selectedControlDataList.Clear();
            for (int i = 0; i < controlDataList.Count; i++)
            {
                selectedControlDataList.Add(controlDataList[i].clone());
            }
            Monitor.Exit(mLock);
        }

        public List<ControlData> getCloneControlDataList(MODE_TYPE modeType)
        {
            Monitor.Enter(mLock);
            int modeIndex = (int)modeType;
            var selectedControlDataList = mControlDataList[modeIndex];
            var controlDataList = new List<ControlData>();
            for (int i = 0; i < selectedControlDataList.Count; i++)
            {
                controlDataList.Add(selectedControlDataList[i].clone());
            }
            Monitor.Exit(mLock);
            return controlDataList;
        }

        public bool read()
        {
            Monitor.Enter(mLock);

            for (int i = 0; i < mControlDataList.Length; i++)
            {
                mControlDataList[i].Clear();
            }

            string jsonString;
            try
            {
                jsonString = File.ReadAllText(mControlFileName);
            }
            catch
            {
                mIsEnable = false;
                Monitor.Exit(mLock);
                this.write();
                return true;
            }

            try
            {
                var rootObject = JObject.Parse(jsonString);

                mWidth = (rootObject.ContainsKey("width") == true) ? rootObject.Value<int>("width") : mWidth;
                mHeight = (rootObject.ContainsKey("height") == true) ? rootObject.Value<int>("height") : mHeight;
                mIsMaximize = (rootObject.ContainsKey("maximize") == true) ? rootObject.Value<bool>("maximize") : mIsMaximize;
                mIsEnable = (rootObject.ContainsKey("enable") == true) ? rootObject.Value<bool>("enable") : mIsEnable;

                if (rootObject.ContainsKey("modeIndex") == false)
                {
                    mModeType = MODE_TYPE.NORMAL;
                }
                else
                {
                    int modeIndex = rootObject.Value<int>("modeIndex");
                    if (modeIndex < 0 || modeIndex > 3)
                        modeIndex = 0;
                    mModeType = (MODE_TYPE)modeIndex;
                }

                this.readData(rootObject, "control", mControlDataList[0]);
                this.readData(rootObject, "silence", mControlDataList[1]);
                this.readData(rootObject, "performance", mControlDataList[2]);
                this.readData(rootObject, "game", mControlDataList[3]);
            }
            catch
            {
                this.clear();
                Monitor.Exit(mLock);
                return false;
            }
            Monitor.Exit(mLock);
            return true;
        }

        private void readData(JObject rootObject, string keyString, List<ControlData> controlDataList)
        {
            if (rootObject.ContainsKey(keyString) == false)
                return;

            var tempBaseMap = HardwareManager.getInstance().TempBaseMap;
            var controlBaseMap = HardwareManager.getInstance().ControlBaseMap;

            var controlList = rootObject.Value<JArray>(keyString);
            for (int i = 0; i < controlList.Count; i++)
            {
                var controlObject = (JObject)controlList[i];
                if (controlObject.ContainsKey("id") == false)
                {
                    continue;
                }

                string id = controlObject.Value<string>("id");

                // check temperature sensor
                if (tempBaseMap.ContainsKey(id) == false)
                {
                    continue;
                }

                var controlData = new ControlData(id);

                // FanData
                var fanList = controlObject.Value<JArray>("fan");
                for (int j = 0; j < fanList.Count; j++)
                {
                    var fanObject = (JObject)fanList[j];
                    if (fanObject.ContainsKey("id") == false)
                    {
                        continue;
                    }

                    string fanID = fanObject.Value<string>("id");

                    // check control sensor
                    if (controlBaseMap.ContainsKey(fanID) == false)
                    {
                        continue;
                    }

                    bool isStep = (fanObject.ContainsKey("step") == true) ? fanObject.Value<bool>("step") : true;
                    int hysteresis = (fanObject.ContainsKey("hysteresis") == true) ? fanObject.Value<int>("hysteresis") : 0;
                    if (hysteresis <= 0) hysteresis = 0;
                    else if(hysteresis > 20)  hysteresis = 20;

                    int unit = (fanObject.ContainsKey("unit") == true) ? fanObject.Value<int>("unit") : 1;
                    if (unit <= 0) unit = 0;
                    else if (unit >= 2) unit = 2;

                    int auto = (fanObject.ContainsKey("auto") == true) ? fanObject.Value<int>("auto") : 0;
                    if (auto <= 0) auto = 0;
                    else if (auto >= 100) auto = 100;

                    if (unit == 0) { }
                    else if (unit == 1) auto = auto / 5 * 5;
                    else auto = auto / 10 * 10;

                    int delayTime = (fanObject.ContainsKey("delayTime") == true) ? fanObject.Value<int>("delayTime") : 0;

                    var fanData = new FanData(fanID, (FanValueUnit)unit, isStep, hysteresis, auto, delayTime);

                    // Percent value
                    var valueList = fanObject.Value<JArray>("value");

                    // fan value list
                    if (valueList.Count == fanData.getMaxFanValue())
                    {
                        for (int k = 0; k < valueList.Count; k++)
                        {
                            int value = valueList[k].Value<int>();
                            fanData.ValueList[k] = value;
                        }

                        // add fan data
                        controlData.FanDataList.Add(fanData);
                    }
                }

                // add control data
                controlDataList.Add(controlData);
            }
        }

        public void write()
        {
            Monitor.Enter(mLock);
            try
            {
                var rootObject = new JObject();

                rootObject["width"] = mWidth;
                rootObject["height"] = mHeight;
                rootObject["maximize"] = mIsMaximize;
                rootObject["enable"] = mIsEnable;
                rootObject["modeIndex"] = (int)mModeType;

                this.writeData(rootObject, "control", mControlDataList[0]);
                this.writeData(rootObject, "silence", mControlDataList[1]);
                this.writeData(rootObject, "performance", mControlDataList[2]);
                this.writeData(rootObject, "game", mControlDataList[3]);

                File.WriteAllText(mControlFileName, rootObject.ToString());
            }
            catch
            {
                this.clear();
            }
            Monitor.Exit(mLock);
        }

        private void writeData(JObject rootObject, string keyString, List<ControlData> controlDataList)
        {
            var controlList = new JArray();
            for (int i = 0; i < controlDataList.Count; i++)
            {
                var controlData = controlDataList[i];
                if (controlData.FanDataList.Count == 0)
                    continue;

                var controlObject = new JObject();
                controlObject["id"] = controlData.ID;
             
                var fanList = new JArray();
                for (int j = 0; j < controlData.FanDataList.Count; j++)
                {
                    var fanData = controlData.FanDataList[j];

                    var fanObject = new JObject();
                    fanObject["id"] = fanData.ID;
                    fanObject["step"] = fanData.IsStep;
                    fanObject["hysteresis"] = fanData.Hysteresis;
                    fanObject["unit"] = (int)fanData.Unit;
                    fanObject["auto"] = fanData.Auto;
                    fanObject["delayTime"] = fanData.DelayTime;

                    var valueList = new JArray();
                    for (int k = 0; k < fanData.getMaxFanValue(); k++)
                    {
                        int value = fanData.ValueList[k];
                        valueList.Add(value);
                    }

                    fanObject["value"] = valueList;
                    fanList.Add(fanObject);
                }

                controlObject["fan"] = fanList;
                controlList.Add(controlObject);
            }

            rootObject[keyString] = controlList;
        }
    }
}
