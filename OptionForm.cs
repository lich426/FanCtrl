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

            mNvApiCheckBox.Checked = OptionManager.getInstance().IsNvAPIWrapper;

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
            mNVIDIAGroupBox.Text = StringLib.NVIDIA_Library;
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

            var optionManager = OptionManager.getInstance();
            bool isRestart = false;

            // 변경
            if( (optionManager.LibraryType == LibraryType.LibreHardwareMonitor && mLibraryRadioButton2.Checked == true) ||
                (optionManager.LibraryType == LibraryType.OpenHardwareMonitor && mLibraryRadioButton1.Checked == true) ||
                (optionManager.IsNvAPIWrapper != mNvApiCheckBox.Checked))
            {
                var result = MessageBox.Show(StringLib.OptionRestart, StringLib.Option, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Cancel)
                    return;
                isRestart = true;
            }

            optionManager.Interval = interval;
            optionManager.IsMinimized = mMinimizeCheckBox.Checked;
            optionManager.IsStartUp = mStartupCheckBox.Checked;
            optionManager.IsNvAPIWrapper = mNvApiCheckBox.Checked;
            optionManager.LibraryType = (mLibraryRadioButton1.Checked == true) ? LibraryType.LibreHardwareMonitor : LibraryType.OpenHardwareMonitor;
            optionManager.write();

            if(isRestart == true)
            {
                ControlManager.getInstance().reset();
                ControlManager.getInstance().write();
                OnExitHandler(null, EventArgs.Empty);
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
