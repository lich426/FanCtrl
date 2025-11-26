using System.Drawing;
using System.Windows.Forms;

namespace FanCtrl
{
    public partial class ThemeLibLabel : Label
    {
        public ThemeLibLabel(FontFamily fontFamily, float fontSize)
        {
            var type = OptionManager.getInstance().getNowTheme();
            this.AutoSize = true;
            this.ForeColor = (type == THEME_TYPE.DARK) ? Color.FromArgb(255, 197, 138, 249) : Color.Red;
            this.Font = new Font(fontFamily, fontSize, FontStyle.Bold);
        }
    }
}
