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
    public partial class LiquidctlCommandForm : Form
    {
        public string Command { get; set; }

        public LiquidctlCommandForm(string command)
        {
            InitializeComponent();
            this.localizeComponent();
            Command = command;
            mCommandTextBox.Text = command;
        }

        private void localizeComponent()
        {
            mOKButton.Text = StringLib.OK;
        }

        private void onOKButtonClick(object sender, EventArgs e)
        {
            Command = mCommandTextBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
