using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{    
    public class ControlData
    {
        public string ID { get; set; }

        private List<FanData> mFanDataList = new List<FanData>();
        public List<FanData> FanDataList {
            get { return mFanDataList; }
        }

        public ControlData(string id)
        {
            ID = id;
        }

        public ControlData clone()
        {
            var controlData = new ControlData(ID);
            for (int i = 0; i < mFanDataList.Count; i++)
                controlData.FanDataList.Add(mFanDataList[i].clone());
            return controlData;
        }
    }
}
