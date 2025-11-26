
namespace FanCtrl
{
    public class RGBnFCControl : BaseControl
    {
        private RGBnFC mRGBnFC = null;
        private int mIndex = 0;

        public RGBnFCControl(string id, RGBnFC fc, int index, uint num) : base(LIBRARY_TYPE.RGBnFC)
        {
            ID = id;
            mRGBnFC = fc;
            mIndex = index;
            Name = "NZXT RGB＆Fan #" + num;
        }

        public override void update()
        {

        }

        public override int getMinSpeed()
        {
            return mRGBnFC.getMinFanSpeed(mIndex);
        }

        public override int getMaxSpeed()
        {
            return mRGBnFC.getMaxFanSpeed(mIndex);
        }

        public override void setSpeed(int value)
        {
            int min = this.getMinSpeed();
            int max = this.getMaxSpeed();

            if (value > max)
            {
                Value = max;
            }
            else if (value < min)
            {
                Value = min;
            }
            else
            {
                Value = value;
            }
            mRGBnFC.setFanSpeed(mIndex, Value);
        }
    }
}
