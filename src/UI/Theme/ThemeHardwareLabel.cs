using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FanCtrl
{
    public partial class ThemeHardwareLabel : Label
    {
        public ThemeHardwareLabel(FontFamily fontFamily, float fontSize)
        {
            var type = OptionManager.getInstance().getNowTheme();
            this.AutoSize = true;
            this.Height = 20;
            this.ForeColor = (type == THEME_TYPE.DARK) ? Color.FromArgb(255, 138, 180, 248) : Color.Blue;
            this.Font = new Font(fontFamily, fontSize, FontStyle.Regular);
        }
    }
}
