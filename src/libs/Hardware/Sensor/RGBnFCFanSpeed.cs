using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class RGBnFCFanSpeed : BaseSensor
    {
        private RGBnFC mRGBnFC = null;
        private int mIndex = 0;

        public RGBnFCFanSpeed(string id, RGBnFC fc, int index, uint num) : base(LIBRARY_TYPE.RGBnFC)
        {
            ID = id;
            mRGBnFC = fc;
            mIndex = index;
            Name = "NZXT RGB＆Fan #" + num;
        }

        public override string getString()
        {
            return Value.ToString() + " RPM";
        }
        public override void update()
        {
            Value = mRGBnFC.getFanSpeed(mIndex);
        }
    }
}
