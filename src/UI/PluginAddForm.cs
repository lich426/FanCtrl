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
    public partial class PluginAddForm : Form
    {
        public string KeyString { get; set; }

        public string NameString { get; set; }

        public PluginAddForm(string key, string name)
        {
            InitializeComponent();
            this.localizeComponent();
            mKeyTextBox.Text = key;
            mNameTextBox.Text = name;
        }

        private void localizeComponent()
        {
            this.Text = StringLib.Plugin;
            mOKButton.Text = StringLib.OK;
        }

        private void onOKButtonClick(object sender, EventArgs e)
        {
            KeyString = mKeyTextBox.Text;
            NameString = mNameTextBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
