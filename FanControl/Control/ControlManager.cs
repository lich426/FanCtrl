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

        private ControlManager() {}

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

        private List<ControlData> mControlDataList = new List<ControlData>();
        
        public void setControlDataList(List<ControlData> controlData)
        {
            Monitor.Enter(mLock);
            mControlDataList.Clear();
            for (int i = 0; i < controlData.Count; i++)
                mControlDataList.Add(controlData[i].clone());
            Monitor.Exit(mLock);
        }

        public List<ControlData> getCloneControlDataList()
        {
            Monitor.Enter(mLock);
            var controlDataList = new List<ControlData>();
            for (int i = 0; i < mControlDataList.Count; i++)
                controlDataList.Add(mControlDataList[i].clone());
            Monitor.Exit(mLock);
            return controlDataList;
        }

        public int Count()
        {
            Monitor.Enter(mLock);
            int count = mControlDataList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public ControlData getControlData(int index)
        {
            Monitor.Enter(mLock);
            if(index >= mControlDataList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }

            var controlData = mControlDataList[index];
            Monitor.Exit(mLock);
            return controlData;
        }

        public bool read()
        {
            Monitor.Enter(mLock);
            mControlDataList.Clear();

            String jsonString;
            try
            {
                jsonString = File.ReadAllText(cControlFileName);
            }
            catch(Exception e)
            {
                mIsEnable = false;
                Monitor.Exit(mLock);
                this.write();
                return true;
            }

            try
            {
                var rootObject = JObject.Parse(jsonString);
                mIsEnable = rootObject.Value<bool>("enable");
                                
                var controlList = rootObject.Value<JArray>("control");
                for(int i =0; i < controlList.Count; i++)
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
                    for(int j = 0; j < fanList.Count; j++)
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
                        if(valueList.Count == FanData.MAX_FAN_VALUE_SIZE)
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
                    mControlDataList.Add(controlData);
                }
            }
            catch (Exception e)
            {
                mIsEnable = false;
                mControlDataList.Clear();
                Monitor.Exit(mLock);
                return false;
            }
            Monitor.Exit(mLock);
            return true;
        }

        public void write()
        {
            Monitor.Enter(mLock);
            try
            {
                var rootObject = new JObject();
                rootObject["enable"] = mIsEnable;

                var controlList = new JArray();
                for (int i = 0; i < mControlDataList.Count; i++)
                {
                    var controlData = mControlDataList[i];
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

                rootObject["control"] = controlList;

                File.WriteAllText(cControlFileName, rootObject.ToString());
            }
            catch (Exception e)
            {
                mIsEnable = false;
                mControlDataList.Clear();
            }
            Monitor.Exit(mLock);
        }

        public bool checkData()
        {
            Monitor.Enter(mLock);
            try
            {                
                HardwareManager hardwareManager = HardwareManager.getInstance();

                var sensorList = hardwareManager.SensorList;
                var controlList = hardwareManager.ControlList;

                for (int i = 0; i < mControlDataList.Count; i++)
                {
                    var controlData = mControlDataList[i];
                    if(controlData.Name.Equals(sensorList[controlData.Index].getName()) == false)
                    {
                        mControlDataList.Clear();
                        Monitor.Exit(mLock);
                        return false;
                    }

                    var fanDataList = controlData.FanDataList;
                    for(int j = 0; j < fanDataList.Count; j++)
                    {
                        var fanData = fanDataList[j];
                        if(fanData.Name.Equals(controlList[fanData.Index].getName()) == false)
                        {
                            mControlDataList.Clear();
                            Monitor.Exit(mLock);
                            return false;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                mControlDataList.Clear();
                Monitor.Exit(mLock);
                return false;
            }
            Monitor.Exit(mLock);
            return true;
        }
    }
}
