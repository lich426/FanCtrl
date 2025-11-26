using System.Windows.Forms;

namespace FanCtrl
{
    public class HotkeyData
    {
        public bool mIsCtrl { get; set; } = false;
        public bool mIsAlt { get; set; } = false;
        public bool mIsLShift { get; set; } = false;
        public bool mIsRShift { get; set; } = false;

        public int mKey { get; set; } = (int)Keys.None;

        public HotkeyData()
        {
            this.reset();
        }

        public void reset()
        {
            mIsCtrl = false;
            mIsAlt = false;
            mIsLShift = false;
            mIsRShift = false;
            mKey = (int)Keys.None;
        }
    }
}
