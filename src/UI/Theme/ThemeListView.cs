using DarkUI.Config;
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
    public partial class ThemeListView : ListView
    {
        public ThemeListView()
        {
            this.BackColor = ThemeProvider.Theme.Colors.GreyBackground;
            this.ForeColor = ThemeProvider.Theme.Colors.LightText;
        }
    }
}
