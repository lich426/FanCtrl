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

namespace FanCtrl
{
    public partial class OptionForm : Form
    {
        public event EventHandler OnExitHandler;

        public OptionForm()
        {
            InitializeComponent();
            this.localizeComponent();

            mToolTip.SetToolTip(mIntervalTextBox, "100 ≤ value ≤ 5000");
            mToolTip.SetToolTip(mStartupDelayTextBox, "0 ≤ value ≤ 59");

            mIntervalTextBox.Text = OptionManager.getInstance().Interval.ToString();
            mIntervalTextBox.KeyPress += onTextBoxKeyPress;
            mIntervalTextBox.Leave += onTextBoxLeaves;

            mGigabyteCheckBox.Checked = OptionManager.getInstance().IsGigabyte;

            mLibraryRadioButton1.Click += onRadioClick;
            mLibraryRadioButton2.Click += onRadioClick;
            mLibraryRadioButton1.Checked = (OptionManager.getInstance().LibraryType == LibraryType.LibreHardwareMonitor);
            mLibraryRadioButton2.Checked = (OptionManager.getInstance().LibraryType == LibraryType.OpenHardwareMonitor);

            mDimmCheckBox.Checked = OptionManager.getInstance().IsDimm;

            mNvApiCheckBox.Checked = OptionManager.getInstance().IsNvAPIWrapper;

            mKrakenCheckBox.Checked = OptionManager.getInstance().IsKraken;
            mKrakenButton.Enabled = (HardwareManager.getInstance().getKrakenList().Count > 0);

            mCLCCheckBox.Checked = OptionManager.getInstance().IsCLC;            
            mCLCButton.Enabled = (HardwareManager.getInstance().getCLCList().Count > 0);

            mRGBnFCCheckBox.Checked = OptionManager.getInstance().IsRGBnFC;
            mRGBnFCButton.Enabled = (HardwareManager.getInstance().getRGBnFCList().Count > 0);

            mFahrenheitCheckBox.Checked = OptionManager.getInstance().IsFahrenheit;
            mAnimationCheckBox.Checked = OptionManager.getInstance().IsAnimation;
            mMinimizeCheckBox.Checked = OptionManager.getInstance().IsMinimized;

            mStartupDelayTextBox.Text = OptionManager.getInstance().DelayTime.ToString();
            mStartupCheckBox.Checked = OptionManager.getInstance().IsStartUp;
        }

        private void localizeComponent()
        {
            this.Text = StringLib.Option;
            mIntervalGroupBox.Text = StringLib.Interval;
            mKrakenButton.Text = StringLib.Lighting;
            mCLCButton.Text = StringLib.Lighting;
            mRGBnFCButton.Text = StringLib.Lighting;
            mAnimationCheckBox.Text = StringLib.Tray_Icon_animation;
            mFahrenheitCheckBox.Text = StringLib.Fahrenheit;
            mMinimizeCheckBox.Text = StringLib.Start_minimized;
            mStartupCheckBox.Text = StringLib.Start_with_Windows;
            mStartupDelayLabel.Text = StringLib.Delay_Time;
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

            int delayTime = int.Parse(mStartupDelayTextBox.Text);
            if (delayTime < 0)
            {
                delayTime = 0;
            }
            else if (delayTime > 59)
            {
                delayTime = 59;
            }

            var optionManager = OptionManager.getInstance();
            bool isRestart = false;

            // 변경
            if ((optionManager.IsGigabyte != mGigabyteCheckBox.Checked) ||
                (optionManager.LibraryType == LibraryType.LibreHardwareMonitor && mLibraryRadioButton2.Checked == true) ||
                (optionManager.LibraryType == LibraryType.OpenHardwareMonitor && mLibraryRadioButton1.Checked == true) ||
                (optionManager.IsDimm != mDimmCheckBox.Checked) ||
                (optionManager.IsNvAPIWrapper != mNvApiCheckBox.Checked) ||
                (optionManager.IsKraken != mKrakenCheckBox.Checked) ||
                (optionManager.IsCLC != mCLCCheckBox.Checked) ||
                (optionManager.IsRGBnFC != mRGBnFCCheckBox.Checked))
            {
                var result = MessageBox.Show(StringLib.OptionRestart, StringLib.Option, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Cancel)
                    return;
                isRestart = true;
            }

            optionManager.Interval = interval;
            optionManager.IsGigabyte = mGigabyteCheckBox.Checked;
            optionManager.LibraryType = (mLibraryRadioButton1.Checked == true) ? LibraryType.LibreHardwareMonitor : LibraryType.OpenHardwareMonitor;
            optionManager.IsDimm = mDimmCheckBox.Checked;
            optionManager.IsNvAPIWrapper = mNvApiCheckBox.Checked;
            optionManager.IsKraken = mKrakenCheckBox.Checked;
            optionManager.IsCLC = mCLCCheckBox.Checked;
            optionManager.IsRGBnFC = mRGBnFCCheckBox.Checked;
            optionManager.IsFahrenheit = mFahrenheitCheckBox.Checked;
            optionManager.IsAnimation = mAnimationCheckBox.Checked;
            optionManager.IsMinimized = mMinimizeCheckBox.Checked;
            optionManager.DelayTime = delayTime;
            optionManager.IsStartUp = false;
            optionManager.IsStartUp = mStartupCheckBox.Checked;            
            optionManager.write();

            if (isRestart == true)
            {
                ControlManager.getInstance().reset();
                ControlManager.getInstance().write();

                OSDManager.getInstance().reset();
                OSDManager.getInstance().write();

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
            if (textBox == mIntervalTextBox)
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
                mIntervalTextBox.Text = interval.ToString();
            }
            else if (textBox == mStartupDelayTextBox)
            {
                int delay = int.Parse(mStartupDelayTextBox.Text);
                if (delay < 0)
                {
                    delay = 0;
                }
                else if (delay > 59)
                {
                    delay = 59;
                }
                mStartupDelayTextBox.Text = delay.ToString();
            }         
        }

        private void onKrakenButtonClick(object sender, EventArgs e)
        {
            var deviceList = HardwareManager.getInstance().getKrakenList();
            if (deviceList.Count == 1)
            {
                var form = new LightingForm(deviceList[0], 1);
                form.ShowDialog();
            }
            else
            {
                var menu = new ContextMenu();
                for (int i = 0; i < deviceList.Count; i++)
                {
                    int index = i;
                    var item = new MenuItem(string.Format("{0}", i + 1), (sender2, e2) =>
                    {
                        var form = new LightingForm(deviceList[index], index + 1);
                        form.ShowDialog();
                    });
                    menu.MenuItems.Add(item);
                }

                var point = mKrakenButton.PointToClient(Control.MousePosition);
                menu.Show(mKrakenButton, point);
            }
        }

        private void onCLCButtonClick(object sender, EventArgs e)
        {
            var deviceList = HardwareManager.getInstance().getCLCList();
            if (deviceList.Count == 1)
            {
                var form = new LightingForm(deviceList[0], 1);
                form.ShowDialog();
            }
            else
            {
                var menu = new ContextMenu();
                for (int i = 0; i < deviceList.Count; i++)
                {
                    int index = i;
                    var item = new MenuItem(string.Format("{0}", i + 1), (sender2, e2) =>
                    {
                        var form = new LightingForm(deviceList[index], index + 1);
                        form.ShowDialog();
                    });
                    menu.MenuItems.Add(item);
                }

                var point = mCLCButton.PointToClient(Control.MousePosition);
                menu.Show(mCLCButton, point);
            }
        }

        private void onRGBnFCButtonClick(object sender, EventArgs e)
        {
            var deviceList = HardwareManager.getInstance().getRGBnFCList();
            if (deviceList.Count == 1)
            {
                var form = new LightingForm(deviceList[0], 1);
                form.ShowDialog();
            }
            else
            {
                var menu = new ContextMenu();
                for (int i = 0; i < deviceList.Count; i++)
                {
                    int index = i;
                    var item = new MenuItem(string.Format("{0}", i + 1), (sender2, e2) =>
                    {
                        var form = new LightingForm(deviceList[index], index + 1);
                        form.ShowDialog();
                    });
                    menu.MenuItems.Add(item);
                }

                var point = mRGBnFCButton.PointToClient(Control.MousePosition);
                menu.Show(mRGBnFCButton, point);
            }
        }
    }
}
