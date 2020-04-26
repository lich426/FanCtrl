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

namespace FanControl
{
    public class ControlManager
    {
        private const string cControlFileName = "Control.json";

        private static ControlManager sManager = new ControlManager();
        public static ControlManager getInstance() { return sManager; }

        private ControlManager()
        {
            mControlDataList[0] = new List<ControlData>();
            mControlDataList[1] = new List<ControlData>();
            mControlDataList[2] = new List<ControlData>();
            mControlDataList[3] = new List<ControlData>();
        }

        private Object mLock = new Object();

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

        private int mModeIndex = 0;
        public int ModeIndex
        {
            get
            {
                Monitor.Enter(mLock);
                int index = mModeIndex;
                Monitor.Exit(mLock);
                return index;
            }
            set
            {
                Monitor.Enter(mLock);
                mModeIndex = value;
                Monitor.Exit(mLock);
            }
        }

        private List<ControlData>[] mControlDataList = new List<ControlData>[4];

        public void setControlDataList(int modeIndex, List<ControlData> controlData)
        {
            Monitor.Enter(mLock);
            var selectedControlDataList = mControlDataList[modeIndex];
            selectedControlDataList.Clear();
            for (int i = 0; i < controlData.Count; i++)
                selectedControlDataList.Add(controlData[i].clone());
            Monitor.Exit(mLock);
        }

        public List<ControlData> getCloneControlDataList(int modeIndex)
        {
            Monitor.Enter(mLock);
            var selectedControlDataList = mControlDataList[modeIndex];
            var controlDataList = new List<ControlData>();
            for (int i = 0; i < selectedControlDataList.Count; i++)
                controlDataList.Add(selectedControlDataList[i].clone());
            Monitor.Exit(mLock);
            return controlDataList;
        }

        public int Count(int modeIndex)
        {
            Monitor.Enter(mLock);
            var selectedControlDataList = mControlDataList[modeIndex];
            int count = selectedControlDataList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public ControlData getControlData(int modeIndex, int index)
        {
            Monitor.Enter(mLock);
            var selectedControlDataList = mControlDataList[modeIndex];
            if (index >= selectedControlDataList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }

            var controlData = selectedControlDataList[index];
            Monitor.Exit(mLock);
            return controlData;
        }

        public bool read()
        {
            Monitor.Enter(mLock);

            for (int i = 0; i < mControlDataList.Length; i++)
                mControlDataList[i].Clear();

            String jsonString;
            try
            {
                jsonString = File.ReadAllText(cControlFileName);
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

                if (rootObject.ContainsKey("enable") == false)
                {
                    mIsEnable = false;
                }
                else
                {
                    mIsEnable = rootObject.Value<bool>("enable");
                }

                if (rootObject.ContainsKey("modeIndex") == false)
                {
                    mModeIndex = 0;
                }
                else
                {
                    mModeIndex = rootObject.Value<int>("modeIndex");
                    if (mModeIndex < 0 || mModeIndex > 3)
                        mModeIndex = 0;
                }

                this.readData(rootObject, "control", ref mControlDataList[0]);
                this.readData(rootObject, "silence", ref mControlDataList[1]);
                this.readData(rootObject, "performance", ref mControlDataList[2]);
                this.readData(rootObject, "game", ref mControlDataList[3]);
            }
            catch
            {
                mIsEnable = false;
                for (int i = 0; i < mControlDataList.Length; i++)
                    mControlDataList[i].Clear();
                Monitor.Exit(mLock);
                return false;
            }
            Monitor.Exit(mLock);
            return true;
        }

        private void readData(JObject rootObject, string keyString, ref List<ControlData> controlDataList)
        {
            if (rootObject.ContainsKey(keyString) == false)
                return;

            var controlList = rootObject.Value<JArray>(keyString);
            for (int i = 0; i < controlList.Count; i++)
            {
                var controlObject = (JObject)controlList[i];

                if (controlObject.ContainsKey("index") == false ||
                    controlObject.ContainsKey("name") == false)
                {
                    continue;
                }

                int sensorIndex = controlObject.Value<int>("index");
                string sensorName = controlObject.Value<string>("name");

                var controlData = new ControlData(sensorIndex, sensorName);

                // FanData
                var fanList = controlObject.Value<JArray>("fan");
                for (int j = 0; j < fanList.Count; j++)
                {
                    var fanObject = (JObject)fanList[j];

                    if (fanObject.ContainsKey("index") == false ||
                        fanObject.ContainsKey("name") == false)
                    {
                        continue;
                    }

                    int fanIndex = fanObject.Value<int>("index");
                    string fanName = fanObject.Value<string>("name");
                    bool isStep = (fanObject.ContainsKey("step") == true) ? fanObject.Value<bool>("step") : true;
                    int hysteresis = (fanObject.ContainsKey("hysteresis") == true) ? fanObject.Value<int>("hysteresis") : 0;

                    var fanData = new FanData(fanIndex, fanName, isStep, hysteresis);

                    // Percent value
                    var valueList = fanObject.Value<JArray>("value");

                    // fan value list is 21
                    if (valueList.Count == FanData.MAX_FAN_VALUE_SIZE)
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
                rootObject["enable"] = mIsEnable;
                rootObject["modeIndex"] = mModeIndex;

                this.writeData(rootObject, "control", ref mControlDataList[0]);
                this.writeData(rootObject, "silence", ref mControlDataList[1]);
                this.writeData(rootObject, "performance", ref mControlDataList[2]);
                this.writeData(rootObject, "game", ref mControlDataList[3]);

                File.WriteAllText(cControlFileName, rootObject.ToString());
            }
            catch
            {
                mIsEnable = false;
                for (int i = 0; i < mControlDataList.Length; i++)
                    mControlDataList[i].Clear();
            }
            Monitor.Exit(mLock);
        }

        private void writeData(JObject rootObject, string keyString, ref List<ControlData> controlDataList)
        {
            var controlList = new JArray();
            for (int i = 0; i < controlDataList.Count; i++)
            {
                var controlData = controlDataList[i];
                if (controlData.FanDataList.Count == 0)
                    continue;

                var controlObject = new JObject();
                controlObject["name"] = controlData.Name;
                controlObject["index"] = controlData.Index;

                var fanList = new JArray();
                for (int j = 0; j < controlData.FanDataList.Count; j++)
                {
                    var fanData = controlData.FanDataList[j];

                    var fanObject = new JObject();
                    fanObject["name"] = fanData.Name;
                    fanObject["index"] = fanData.Index;
                    fanObject["step"] = fanData.IsStep;
                    fanObject["hysteresis"] = fanData.Hysteresis;

                    var valueList = new JArray();
                    for (int k = 0; k < FanData.MAX_FAN_VALUE_SIZE; k++)
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

        public bool checkData()
        {
            Monitor.Enter(mLock);
            try
            {
                if(this.checkData(ref mControlDataList[0]) == false ||
                    this.checkData(ref mControlDataList[1]) == false ||
                    this.checkData(ref mControlDataList[2]) == false ||
                    this.checkData(ref mControlDataList[3]) == false)
                {
                    for (int i = 0; i < mControlDataList.Length; i++)
                        mControlDataList[i].Clear();
                    Monitor.Exit(mLock);
                    return false;
                }
            }
            catch
            {
                for (int i = 0; i < mControlDataList.Length; i++)
                    mControlDataList[i].Clear();
                Monitor.Exit(mLock);
                return false;
            }
            Monitor.Exit(mLock);
            return true;
        }

        private bool checkData(ref List<ControlData> controlDataList)
        {
            HardwareManager hardwareManager = HardwareManager.getInstance();
            for (int i = 0; i < controlDataList.Count; i++)
            {
                var controlData = controlDataList[i];
                if (controlData.Name.Equals(hardwareManager.getSensor(controlData.Index).getName()) == false)
                {
                    return false;
                }

                var fanDataList = controlData.FanDataList;
                for (int j = 0; j < fanDataList.Count; j++)
                {
                    var fanData = fanDataList[j];
                    if (fanData.Name.Equals(hardwareManager.getControl(fanData.Index).getName()) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
