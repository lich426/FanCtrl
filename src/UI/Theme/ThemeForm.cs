using DarkUI.Forms;

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
