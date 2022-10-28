using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class LiquidctlData
    {
        public string Bus { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        private List<LiquidctlStatusData> mStatusDataList = new List<LiquidctlStatusData>();
        private object mLock = new object();

        public int getStatusCount()
        {
            Monitor.Enter(mLock);
            int count = mStatusDataList.Count;
            Monitor.Exit(mLock);
            return count;
        }

        public LiquidctlStatusData getStatusData(int index)
        {
            Monitor.Enter(mLock);
            if (index >= mStatusDataList.Count)
            {
                Monitor.Exit(mLock);
                return null;
            }
            var data = mStatusDataList[index];
            Monitor.Exit(mLock);
            return data;
        }

        public void addStatusData(LiquidctlStatusData data)
        {
            Monitor.Enter(mLock);
            mStatusDataList.Add(data);
            Monitor.Exit(mLock);
        }
    }
}
