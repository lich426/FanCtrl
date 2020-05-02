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

        private List<string> mSensorNameList = new List<string>();
        private List<string> mFanNameList = new List<string>();
        private List<string> mFanControlNameList = new List<string>();

        private List<string> mOriginSensorNameList = new List<string>();
        private List<string> mOriginFanNameList = new List<string>();
        private List<string> mOriginFanControlNameList = new List<string>();

        private ControlManager()
        {
            mControlDataList[0] = new List<ControlData>();
            mControlDataList[1] = new List<ControlData>();
            mControlDataList[2] = new List<ControlData>();
            mControlDataList[3] = new List<ControlData>();
        }

        private void clear()
        {
            mIsEnable = false;
            for (int i = 0; i < mControlDataList.Length; i++)
                mControlDataList[i].Clear();

            mModeIndex = 0;

            for (int i = 0; i < mSensorNameList.Count; i++)
                mSensorNameList[i] = mOriginSensorNameList[i];

            for (int i = 0; i < mFanNameList.Count; i++)
                mFanNameList[i] = mOriginFanNameList[i];

            for (int i = 0; i < mFanControlNameList.Count; i++)
                mFanControlNameList[i] = mOriginFanControlNameList[i];
        }

        public void reset()
        {
            Monitor.Enter(mLock);
            mIsEnable = false;
            for (int i = 0; i < mControlDataList.Length; i++)
                mControlDataList[i].Clear();

            mModeIndex = 0;
            mSensorNameList.Clear();
            mFanNameList.Clear();
            mFanControlNameList.Clear();

            Monitor.Exit(mLock);
        }

        public void setNameCount(int type, int count)
        {
            Monitor.Enter(mLock);
            List<string> nameList = null;
            List<string> nameList2 = null;
            if (type == 0)
            {
                nameList = mSensorNameList;
                nameList2 = mOriginSensorNameList;
            }
            else if (type == 1)
            {
                nameList = mFanNameList;
                nameList2 = mOriginFanNameList;
            }
            else
            {
                nameList = mFanControlNameList;
                nameList2 = mOriginFanControlNameList;
            }

            nameList.Clear();
            nameList2.Clear();
            for (int i = 0; i < count; i++)
            {
                nameList.Add("");
                nameList2.Add("");
            }
            Monitor.Exit(mLock);
        }

        public int getNameCount(int type)
        {
            Monitor.Enter(mLock);
            List<string> nameList = null;
            if (type == 0) nameList = mSensorNameList;
            else if (type == 1) nameList = mFanNameList;
            else nameList = mFanControlNameList;

            int count = nameList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public string getName(int type, int index, bool isOrigin)
        {
            Monitor.Enter(mLock);
            List<string> nameList = null;
            if (type == 0)
            {
                nameList = (isOrigin == true) ? mOriginSensorNameList : mSensorNameList;
            }
            else if (type == 1)
            {
                nameList = (isOrigin == true) ? mOriginFanNameList : mFanNameList;
            }
            else
            {
                nameList = (isOrigin == true) ? mOriginFanControlNameList : mFanControlNameList;
            }

            if (index >= nameList.Count)
            {
                Monitor.Exit(mLock);
                return "";
            }
            var valueString = nameList[index];
            Monitor.Exit(mLock);
            return valueString;
        }

        public void setName(int type, int index, bool isOrigin, string nameString)
        {
            Monitor.Enter(mLock);
            List<string> nameList = null;
            if (type == 0)
            {
                nameList = (isOrigin == true) ? mOriginSensorNameList : mSensorNameList;
            }
            else if (type == 1)
            {
                nameList = (isOrigin == true) ? mOriginFanNameList : mFanNameList;
            }
            else
            {
                nameList = (isOrigin == true) ? mOriginFanControlNameList : mFanControlNameList;
            }

            if (index >= nameList.Count)
            {
                Monitor.Exit(mLock);
                return;
            }
            nameList[index] = nameString;

            if (isOrigin == false)
            {
                // sensor
                if(type == 0)
                {
                    for (int i = 0; i < mControlDataList.Length; i++)
                    {
                        for (int j = 0; j < mControlDataList[i].Count; j++)
                        {
                            var controlData = mControlDataList[i][j];
                            if (controlData.Index == index)
                            {
                                controlData.Name = nameString;
                                break;
                            }
                        }
                    }
                }

                // control
                else if(type == 2)
                {
                    for (int i = 0; i < mControlDataList.Length; i++)
                    {
                        for (int j = 0; j < mControlDataList[i].Count; j++)
                        {
                            var controlData = mControlDataList[i][j];                            
                            for(int k = 0; k < controlData.FanDataList.Count; k++)
                            {
                                var fanData = controlData.FanDataList[k];
                                if(fanData.Index == index)
                                {
                                    fanData.Name = nameString;
                                    break;
                                }
                            }
                        }
                    }
                }                
            }

            Monitor.Exit(mLock);
        }

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

        public int getControlDataCount(int modeIndex)
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

                // name
                if (rootObject.ContainsKey("name") == true)
                {
                    var nameObject = rootObject.Value<JObject>("name");

                    // sensor name
                    if (nameObject.ContainsKey("sensor") == true)
                    {
                        var sensorList = nameObject.Value<JArray>("sensor");
                        for(int i = 0; i <sensorList.Count; i++)
                        {
                            var nameString = sensorList[i].Value<string>();
                            mSensorNameList[i] = nameString;
                        }
                    }

                    // fan name
                    if (nameObject.ContainsKey("fan") == true)
                    {
                        var fanList = nameObject.Value<JArray>("fan");
                        for (int i = 0; i < fanList.Count; i++)
                        {
                            var nameString = fanList[i].Value<string>();
                            mFanNameList[i] = nameString;
                        }
                    }

                    // fan control name
                    if (nameObject.ContainsKey("control") == true)
                    {
                        var controlList = nameObject.Value<JArray>("control");
                        for (int i = 0; i < controlList.Count; i++)
                        {
                            var nameString = controlList[i].Value<string>();
                            mFanControlNameList[i] = nameString;
                        }
                    }
                }

                this.readData(rootObject, "control", ref mControlDataList[0]);
                this.readData(rootObject, "silence", ref mControlDataList[1]);
                this.readData(rootObject, "performance", ref mControlDataList[2]);
                this.readData(rootObject, "game", ref mControlDataList[3]);
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

                // name
                var nameObject = new JObject();

                // sensor name
                var sensorList = new JArray();
                for(int i = 0; i < mSensorNameList.Count; i++)
                {
                    sensorList.Add(mSensorNameList[i]);
                }
                nameObject["sensor"] = sensorList;

                // fan name
                var fanList = new JArray();
                for (int i = 0; i < mFanNameList.Count; i++)
                {
                    fanList.Add(mFanNameList[i]);
                }
                nameObject["fan"] = fanList;

                // fan control name
                var fanControlList = new JArray();
                for (int i = 0; i < mFanControlNameList.Count; i++)
                {
                    fanControlList.Add(mFanControlNameList[i]);
                }
                nameObject["control"] = fanControlList;

                rootObject["name"] = nameObject;

                this.writeData(rootObject, "control", ref mControlDataList[0]);
                this.writeData(rootObject, "silence", ref mControlDataList[1]);
                this.writeData(rootObject, "performance", ref mControlDataList[2]);
                this.writeData(rootObject, "game", ref mControlDataList[3]);

                File.WriteAllText(cControlFileName, rootObject.ToString());
            }
            catch
            {
                this.clear();
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
                    this.clear();
                    Monitor.Exit(mLock);
                    return false;
                }
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

        private bool checkData(ref List<ControlData> controlDataList)
        {
            for (int i = 0; i < controlDataList.Count; i++)
            {
                var controlData = controlDataList[i];
                string sensorName = mSensorNameList[controlData.Index];
                if (controlData.Name.Equals(sensorName) == false)
                {
                    return false;
                }

                var fanDataList = controlData.FanDataList;
                for (int j = 0; j < fanDataList.Count; j++)
                {
                    var fanData = fanDataList[j];
                    string fanControlName = mFanControlNameList[fanData.Index];
                    if (fanData.Name.Equals(fanControlName) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
