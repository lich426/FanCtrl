using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{    
    public class ControlData
    {
        public int Index { get; set; }

        public string Name { get; set; }

        private List<FanData> mFanDataList = new List<FanData>();
        public List<FanData> FanDataList {
            get { return mFanDataList; }
        }

        public ControlData(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public ControlData clone()
        {
            var controlData = new ControlData(Index, Name);
            for (int i = 0; i < mFanDataList.Count; i++)
                controlData.FanDataList.Add(mFanDataList[i].clone());
            return controlData;
        }
    }
}
