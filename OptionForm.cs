using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FanControl
{
    public partial class OptionForm : Form
    {
        public OptionForm()
        {
            InitializeComponent();
            this.localizeComponent();

            mToolTip.SetToolTip(mIntervalTextBox, "100 ≤ value ≤ 9999");

            mIntervalTextBox.KeyPress += onTextBoxKeyPress;
            mIntervalTextBox.Text = OptionManager.getInstance().Interval.ToString();
            mMinimizeCheckBox.Checked = OptionManager.getInstance().IsMinimized;
            mStartupCheckBox.Checked = OptionManager.getInstance().IsStartUp;
        }

        private void localizeComponent()
        {
            this.Text = StringLib.Option;
            mIntervalGroupBox.Text = StringLib.Interval;
            mMinimizeCheckBox.Text = StringLib.Start_minimized;
            mStartupCheckBox.Text = StringLib.Start_with_Windows;
            mOKButton.Text = StringLib.OK;
        }

        private void onOKButtonClick(object sender, EventArgs e)
        {
            int interval = int.Parse(mIntervalTextBox.Text);
            if (interval < 100)
            {
                interval = 100;
            }
            else if (interval > 9999)
            {
                interval = 9999;
            }

            OptionManager.getInstance().Interval = interval;
            OptionManager.getInstance().IsMinimized = mMinimizeCheckBox.Checked;
            OptionManager.getInstance().IsStartUp = mStartupCheckBox.Checked;
            OptionManager.getInstance().write();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void onTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false)
            {
                e.Handled = true;
            }
        }
    }
}
