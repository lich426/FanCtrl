using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FanCtrl
{
    public class OSDManager
    {
        public string mOSDFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "OSD.json";

        private static OSDManager sManager = new OSDManager();
        public static OSDManager getInstance() { return sManager; }

        private Object mLock = new Object();

        private bool mIsUpdate = false;
        public bool IsUpdate
        {
            get
            {
                Monitor.Enter(mLock);
                bool isUpdate = mIsUpdate;
                Monitor.Exit(mLock);
                return isUpdate;
            }
            set
            {
                Monitor.Enter(mLock);
                mIsUpdate = value;
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

        private bool mIsTime = false;
        public bool IsTime
        {
            get
            {
                Monitor.Enter(mLock);
                bool isTime = mIsTime;
                Monitor.Exit(mLock);
                return isTime;
            }
            set
            {
                Monitor.Enter(mLock);
                mIsTime = value;
                Monitor.Exit(mLock);
            }
        }

        private List<OSDGroup> mGroupList = new List<OSDGroup>();

        private OSDManager()
        {

        }

        private void clear()
        {
            mIsEnable = false;
            mIsTime = false;
            mGroupList.Clear();
        }

        public void reset()
        {
            Monitor.Enter(mLock);
            this.clear();
            Monitor.Exit(mLock);
        }
        
        public void read()
        {
            Monitor.Enter(mLock);
            String jsonString;
            try
            {
                jsonString = File.ReadAllText(mOSDFileName);
            }
            catch
            {
                this.clear();
                Monitor.Exit(mLock);
                this.write();
                return;
            }

            try
            {
                var osdMap = HardwareManager.getInstance().OSDSensorMap;

                var rootObject = JObject.Parse(jsonString);

                mIsEnable = (rootObject.ContainsKey("IsEnable") == true) ? rootObject.Value<bool>("IsEnable") : false;
                mIsTime = (rootObject.ContainsKey("IsTime") == true) ? rootObject.Value<bool>("IsTime") : false;

                if (rootObject.ContainsKey("Group") == true)
                {
                    var groupList = rootObject.Value<JArray>("Group");
                    for(int i = 0; i < groupList.Count; i++)
                    {
                        var groupObject = (JObject)groupList[i];

                        string name = (groupObject.ContainsKey("name") == false) ? "" : groupObject.Value<string>("name");
                        bool isColor = (groupObject.ContainsKey("isColor") == false) ? false : groupObject.Value<bool>("isColor");
                        byte r = (groupObject.ContainsKey("r") == false) ? (byte)0xFF : groupObject.Value<byte>("r");
                        byte g = (groupObject.ContainsKey("g") == false) ? (byte)0xFF : groupObject.Value<byte>("g");
                        byte b = (groupObject.ContainsKey("b") == false) ? (byte)0xFF : groupObject.Value<byte>("b");
                        int digit = (groupObject.ContainsKey("digit") == false) ? 5 : groupObject.Value<int>("digit");

                        var group = new OSDGroup();
                        group.Name = name;
                        group.IsColor = isColor;
                        group.Color = Color.FromArgb(r, g, b);
                        group.Digit = digit;

                        if (groupObject.ContainsKey("item") == true)
                        {
                            var itemList = groupObject.Value<JArray>("item");
                            for(int j = 0; j < itemList.Count; j++)
                            {
                                var itemObject = (JObject)itemList[j];

                                var unitType = (itemObject.ContainsKey("unitType") == false) ? (int)OSDUnitType.Unknown : itemObject.Value<int>("unitType");
                                string id = (itemObject.ContainsKey("id") == false) ? "" : itemObject.Value<string>("id");
                                isColor = (itemObject.ContainsKey("isColor") == false) ? false : itemObject.Value<bool>("isColor");
                                r = (itemObject.ContainsKey("r") == false) ? (byte)0xFF : itemObject.Value<byte>("r");
                                g = (itemObject.ContainsKey("g") == false) ? (byte)0xFF : itemObject.Value<byte>("g");
                                b = (itemObject.ContainsKey("b") == false) ? (byte)0xFF : itemObject.Value<byte>("b");

                                if (unitType >= (int)OSDUnitType.Unknown || id.Length == 0)
                                    continue;

                                if (osdMap.ContainsKey(id) == false)
                                    continue;

                                var item = new OSDItem();
                                item.UnitType = (OSDUnitType)unitType;
                                item.ID = id;
                                item.IsColor = isColor;
                                item.Color = Color.FromArgb(r, g, b);
                                group.ItemList.Add(item);
                            }
                        }

                        mGroupList.Add(group);
                    }
                }
            }
            catch 
            {
                this.clear();
                Monitor.Exit(mLock);
                return;
            }

            Monitor.Exit(mLock);
        }

        public void write()
        {
            Monitor.Enter(mLock);
            try
            {
                var rootObject = new JObject();
                rootObject["IsEnable"] = mIsEnable;
                rootObject["IsTime"] = mIsTime;

                var groupList = new JArray();
                for(int i = 0; i < mGroupList.Count; i++)
                {
                    var group = mGroupList[i];

                    var groupObject = new JObject();
                    groupObject["name"] = group.Name;
                    groupObject["isColor"] = group.IsColor;
                    groupObject["r"] = group.Color.R;
                    groupObject["g"] = group.Color.G;
                    groupObject["b"] = group.Color.B;
                    groupObject["digit"] = group.Digit;

                    var itemList = new JArray();
                    for (int j = 0; j < group.ItemList.Count; j++)
                    {
                        var item = group.ItemList[j];

                        var itemObject = new JObject();
                        itemObject["unitType"] = (int)item.UnitType;
                        itemObject["id"] = item.ID;
                        itemObject["isColor"] = item.IsColor;
                        itemObject["r"] = item.Color.R;
                        itemObject["g"] = item.Color.G;
                        itemObject["b"] = item.Color.B;

                        itemList.Add(itemObject);
                    }
                    groupObject["item"] = itemList;

                    groupList.Add(groupObject);
                }
                rootObject["Group"] = groupList;

                File.WriteAllText(mOSDFileName, rootObject.ToString());
            }
            catch
            {
                this.clear();
            }
            Monitor.Exit(mLock);
        }

        public int getGroupCount()
        {
            Monitor.Enter(mLock);
            int count = mGroupList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public OSDGroup getGroup(int index)
        {
            Monitor.Enter(mLock);
            if(index >= mGroupList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }
            var group = mGroupList[index];
            Monitor.Exit(mLock);
            return group;
        }

        public void setGroupList(List<OSDGroup> groupList)
        {
            Monitor.Enter(mLock);
            mGroupList.Clear();
            for (int i = 0; i < groupList.Count; i++)
            {
                mGroupList.Add(groupList[i].clone());
            }
            Monitor.Exit(mLock);
        }

        public List<OSDGroup> getCloneGroupList()
        {
            Monitor.Enter(mLock);
            var groupList = new List<OSDGroup>();
            for (int i = 0; i < mGroupList.Count; i++)
            {
                groupList.Add(mGroupList[i].clone());
            }
            Monitor.Exit(mLock);
            return groupList;
        }
    }
}
