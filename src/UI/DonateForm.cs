using FanCtrl.Resources;

namespace FanCtrl
{
    public partial class DonateForm : ThemeForm
    {
        public DonateForm()
        {
            InitializeComponent();
            this.localizeComponent();
        }

        private void localizeComponent()
        {
            this.Text = StringLib.Donate;
        }
    }
}
