using DarkUI.Config;
using DarkUI.Forms;
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
    public partial class ThemeForm : DarkForm
    {
        public ThemeForm()
        {
            var type = OptionManager.getInstance().getNowTheme();
            Theme.setTheme(this.Handle, (type == THEME_TYPE.DARK));
        }
    }
}
