using DarkUI.Config;
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
