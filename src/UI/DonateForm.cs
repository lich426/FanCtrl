using FanCtrl.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
