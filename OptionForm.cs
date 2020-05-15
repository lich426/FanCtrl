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

            mIntervalTextBox.Text = OptionManager.getInstance().Interval.ToString();
            mIntervalTextBox.KeyPress += onTextBoxKeyPress;
            mIntervalTextBox.Leave += onTextBoxLeaves;

            mGigabyteCheckBox.Checked = OptionManager.getInstance().IsGigabyte;

            mLibraryRadioButton1.Click += onRadioClick;
            mLibraryRadioButton2.Click += onRadioClick;
            mLibraryRadioButton1.Checked = (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor);
            mLibraryRadioButton2.Checked = (OptionManager.getInstance().LibraryType == LibraryType.OpenHardwareMonitor);
            
            mNvApiCheckBox.Checked = OptionManager.getInstance().IsNvAPIWrapper;

            mKrakenCheckBox.Checked = OptionManager.getInstance().IsKraken;
            mCLCCheckBox.Checked = OptionManager.getInstance().IsCLC;

            mKrakenButton.Enabled = (HardwareManager.getInstance().getKraken() != null);
            mCLCButton.Enabled = (HardwareManager.getInstance().getCLC() != null);            

            mAnimationCheckBox.Checked = OptionManager.getInstance().IsAnimation;
            mMinimizeCheckBox.Checked = OptionManager.getInstance().IsMinimized;
            mStartupCheckBox.Checked = OptionManager.getInstance().IsStartUp;
        }

        private void localizeComponent()
        {
            this.Text = StringLib.Option;
            mIntervalGroupBox.Text = StringLib.Interval;
            mKrakenButton.Text = StringLib.Lighting;
            mCLCButton.Text = StringLib.Lighting;
            mAnimationCheckBox.Text = StringLib.Tray_Icon_animation;
            mMinimizeCheckBox.Text = StringLib.Start_minimized;
            mStartupCheckBox.Text = StringLib.Start_with_Windows;
            mLibraryGroupBox.Text = StringLib.Library;
            mOKButton.Text = StringLib.OK;
        }

        private void onRadioClick(object sender, EventArgs e)
        {
            if (sender == mLibraryRadioButton1)
            {
                mLibraryRadioButton1.Checked = true;
                mLibraryRadioButton2.Checked = false;
            }
            else if (sender == mLibraryRadioButton2)
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
            if ((optionManager.IsGigabyte != mGigabyteCheckBox.Checked) ||
                (optionManager.LibraryType == LibraryType.LibreHardwareMonitor && mLibraryRadioButton2.Checked == true) ||
                (optionManager.LibraryType == LibraryType.OpenHardwareMonitor && mLibraryRadioButton1.Checked == true) ||
                (optionManager.IsNvAPIWrapper != mNvApiCheckBox.Checked) ||
                (optionManager.IsKraken != mKrakenCheckBox.Checked) ||
                (optionManager.IsCLC != mCLCCheckBox.Checked))
            {
                var result = MessageBox.Show(StringLib.OptionRestart, StringLib.Option, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Cancel)
                    return;
                isRestart = true;
            }

            optionManager.Interval = interval;
            optionManager.IsGigabyte = mGigabyteCheckBox.Checked;
            optionManager.LibraryType = (mLibraryRadioButton1.Checked == true) ? LibraryType.LibreHardwareMonitor : LibraryType.OpenHardwareMonitor;
            optionManager.IsNvAPIWrapper = mNvApiCheckBox.Checked;
            optionManager.IsKraken = mKrakenCheckBox.Checked;
            optionManager.IsCLC = mCLCCheckBox.Checked;
            optionManager.IsAnimation = mAnimationCheckBox.Checked;
            optionManager.IsMinimized = mMinimizeCheckBox.Checked;
            optionManager.IsStartUp = mStartupCheckBox.Checked;
            optionManager.write();

            if (isRestart == true)
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

        private void onTextBoxLeaves(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            int interval = int.Parse(mIntervalTextBox.Text);
            if (interval < 100)
            {
                interval = 100;
            }
            else if (interval > 5000)
            {
                interval = 5000;
            }
            mIntervalTextBox.Text = interval.ToString();
        }

        private void onKrakenButtonClick(object sender, EventArgs e)
        {
            var form = new LightingForm(HardwareManager.getInstance().getKraken());
            form.ShowDialog();
        }

        private void onCLCButtonClick(object sender, EventArgs e)
        {
            var form = new LightingForm(HardwareManager.getInstance().getCLC());
            form.ShowDialog();
        }
    }
}
