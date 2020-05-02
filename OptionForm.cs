using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FanControl
{
    public partial class OptionForm : Form
    {
        public event EventHandler OnExitHandler;

        public OptionForm()
        {
            InitializeComponent();
            this.localizeComponent();

            mToolTip.SetToolTip(mIntervalTextBox, "100 ≤ value ≤ 5000");

            mLibraryRadioButton1.Click += onRadioClick;
            mLibraryRadioButton2.Click += onRadioClick;

            mIntervalTextBox.KeyPress += onTextBoxKeyPress;
            mIntervalTextBox.Text = OptionManager.getInstance().Interval.ToString();
            mMinimizeCheckBox.Checked = OptionManager.getInstance().IsMinimized;
            mStartupCheckBox.Checked = OptionManager.getInstance().IsStartUp;

            mLibraryRadioButton1.Checked = (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor);
            mLibraryRadioButton2.Checked = (OptionManager.getInstance().LibraryType == LibraryType.OpenHardwareMonitor);
        }

        private void localizeComponent()
        {
            this.Text = StringLib.Option;
            mIntervalGroupBox.Text = StringLib.Interval;
            mMinimizeCheckBox.Text = StringLib.Start_minimized;
            mStartupCheckBox.Text = StringLib.Start_with_Windows;
            mLibraryGroupBox.Text = StringLib.Library;
            mOKButton.Text = StringLib.OK;
        }

        private void onRadioClick(object sender, EventArgs e)
        {
            if(sender == mLibraryRadioButton1)
            {
                mLibraryRadioButton1.Checked = true;
                mLibraryRadioButton2.Checked = false;
            }
            else
            {
                mLibraryRadioButton1.Checked = false;
                mLibraryRadioButton2.Checked = true;
            }
        }

        private void onOKButtonClick(object sender, EventArgs e)
        {
            int interval = int.Parse(mIntervalTextBox.Text);
            if (interval < 100)
            {
                interval = 100;
            }
            else if (interval > 5000)
            {
                interval = 5000;
            }

            bool isRestart = false;

            // 변경
            if((OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor && mLibraryRadioButton2.Checked == true) ||
                OptionManager.getInstance().LibraryType == LibraryType.OpenHardwareMonitor && mLibraryRadioButton1.Checked == true)
            {
                var result = MessageBox.Show(StringLib.OptionRestart, StringLib.Option, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Cancel)
                    return;
                isRestart = true;
            }

            OptionManager.getInstance().Interval = interval;
            OptionManager.getInstance().IsMinimized = mMinimizeCheckBox.Checked;
            OptionManager.getInstance().IsStartUp = mStartupCheckBox.Checked;
            OptionManager.getInstance().LibraryType = (mLibraryRadioButton1.Checked == true) ? LibraryType.LibreHardwareMonitor : LibraryType.OpenHardwareMonitor;
            OptionManager.getInstance().write();

            if(isRestart == true)
            {
                ControlManager.getInstance().reset();
                ControlManager.getInstance().write();
                OnExitHandler(null, EventArgs.Empty);
                Program.restartProgram();
                return;
            }

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
