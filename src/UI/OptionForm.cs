using FanCtrl.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FanCtrl
{
    public partial class OptionForm : Form
    {
        public OptionForm()
        {
            InitializeComponent();
            this.localizeComponent();

            if (Screen.PrimaryScreen.WorkingArea.Height < this.Height)
            {
                this.AutoScroll = true;
                this.AutoSizeMode = AutoSizeMode.GrowOnly;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.Width = this.Width + 10;
                this.Height = this.Height - 100;

                this.MinimumSize = new Size(this.Width, 300);
                this.MaximumSize = new Size(this.Width, Int32.MaxValue);
            }            

            mToolTip.SetToolTip(mIntervalTextBox, "100 ≤ value ≤ 5000");
            mToolTip.SetToolTip(mStartupDelayTextBox, "0 ≤ value ≤ 59");

            mIntervalTextBox.Text = OptionManager.getInstance().Interval.ToString();
            mIntervalTextBox.KeyPress += onTextBoxKeyPress;
            mIntervalTextBox.Leave += onTextBoxLeaves;

            mLHMCheckBox.CheckedChanged += (object sender, EventArgs e) =>
            {
                mLHMCPUCheckBox.Enabled = mLHMCheckBox.Checked;
                mLHMMBCheckBox.Enabled = mLHMCheckBox.Checked;
                mLHMGPUCheckBox.Enabled = mLHMCheckBox.Checked;
                mLHMControllerCheckBox.Enabled = mLHMCheckBox.Checked;
                mLHMStorageCheckBox.Enabled = mLHMCheckBox.Checked;
                mLHMMemoryCheckBox.Enabled = mLHMCheckBox.Checked;
            };
            mLHMCheckBox.Checked = OptionManager.getInstance().IsLHM;
            mLHMCPUCheckBox.Checked = OptionManager.getInstance().IsLHMCpu;
            mLHMMBCheckBox.Checked = OptionManager.getInstance().IsLHMMotherboard;
            mLHMGPUCheckBox.Checked = OptionManager.getInstance().IsLHMGpu;
            mLHMControllerCheckBox.Checked = OptionManager.getInstance().IsLHMContolled;
            mLHMStorageCheckBox.Checked = OptionManager.getInstance().IsLHMStorage;
            mLHMMemoryCheckBox.Checked = OptionManager.getInstance().IsLHMMemory;
            if (mLHMCheckBox.Checked == false)
            {
                mLHMCPUCheckBox.Enabled = false;
                mLHMMBCheckBox.Enabled = false;
                mLHMGPUCheckBox.Enabled = false;
                mLHMControllerCheckBox.Enabled = false;
                mLHMStorageCheckBox.Enabled = false;
                mLHMMemoryCheckBox.Enabled = false;
            }

            mNvApiCheckBox.Checked = OptionManager.getInstance().IsNvAPIWrapper;

            mDimmCheckBox.Checked = OptionManager.getInstance().IsDimm;
            
            mKrakenCheckBox.Checked = OptionManager.getInstance().IsKraken;
            mKrakenButton.Enabled = (HardwareManager.getInstance().KrakenList.Count > 0);

            mCLCCheckBox.Checked = OptionManager.getInstance().IsCLC;            
            mCLCButton.Enabled = (HardwareManager.getInstance().CLCList.Count > 0);

            mRGBnFCCheckBox.Checked = OptionManager.getInstance().IsRGBnFC;
            mRGBnFCButton.Enabled = (HardwareManager.getInstance().RGBnFCList.Count > 0);

            mHWInfoCheckBox.Checked = OptionManager.getInstance().IsHWInfo;

            mLiquidctlCheckBox.Checked = OptionManager.getInstance().IsLiquidctl;

            mPluginCheckBox.Checked = OptionManager.getInstance().IsPlugin;

            mLanguageComboBox.Items.Add(StringLib.English);
            mLanguageComboBox.Items.Add(StringLib.Korean);
            mLanguageComboBox.Items.Add(StringLib.Japanese);
            mLanguageComboBox.Items.Add(StringLib.French);
            mLanguageComboBox.SelectedIndex = OptionManager.getInstance().Language;

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
            mPluginCheckBox.Text = StringLib.Plugin;
            mAnimationCheckBox.Text = StringLib.Tray_Icon_animation;
            mLanguageLabel.Text = StringLib.Language;
            mFahrenheitCheckBox.Text = StringLib.Fahrenheit;
            mMinimizeCheckBox.Text = StringLib.Start_minimized;
            mStartupCheckBox.Text = StringLib.Start_with_Windows;
            mStartupDelayLabel.Text = StringLib.Delay_Time;
            mLibraryGroupBox.Text = StringLib.Library;
            mResetButton.Text = StringLib.Reset;
            mOKButton.Text = StringLib.OK;
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

            if ((optionManager.IsLHM != mLHMCheckBox.Checked) ||
                (mLHMCheckBox.Checked == true && optionManager.IsLHMCpu != mLHMCPUCheckBox.Checked) ||
                (mLHMCheckBox.Checked == true && optionManager.IsLHMMotherboard != mLHMMBCheckBox.Checked) ||
                (mLHMCheckBox.Checked == true && optionManager.IsLHMGpu != mLHMGPUCheckBox.Checked) ||
                (mLHMCheckBox.Checked == true && optionManager.IsLHMContolled != mLHMControllerCheckBox.Checked) ||
                (mLHMCheckBox.Checked == true && optionManager.IsLHMStorage != mLHMStorageCheckBox.Checked) ||
                (mLHMCheckBox.Checked == true && optionManager.IsLHMMemory != mLHMMemoryCheckBox.Checked) ||

                (optionManager.IsNvAPIWrapper != mNvApiCheckBox.Checked) ||
                (optionManager.IsDimm != mDimmCheckBox.Checked) ||
                (optionManager.IsKraken != mKrakenCheckBox.Checked) ||
                (optionManager.IsCLC != mCLCCheckBox.Checked) ||
                (optionManager.IsRGBnFC != mRGBnFCCheckBox.Checked) ||
                (optionManager.IsHWInfo != mHWInfoCheckBox.Checked) ||
                (optionManager.IsLiquidctl != mLiquidctlCheckBox.Checked) ||
                (optionManager.IsPlugin != mPluginCheckBox.Checked) ||
                (optionManager.Language != mLanguageComboBox.SelectedIndex))
            {
                var result = MessageBox.Show(StringLib.OptionChange, StringLib.Option, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Cancel)
                    return;

                isRestart = true;
            }

            optionManager.Interval = interval;

            optionManager.IsLHM = mLHMCheckBox.Checked;
            optionManager.IsLHMCpu = mLHMCPUCheckBox.Checked;
            optionManager.IsLHMMotherboard = mLHMMBCheckBox.Checked;
            optionManager.IsLHMGpu = mLHMGPUCheckBox.Checked;
            optionManager.IsLHMContolled = mLHMControllerCheckBox.Checked;
            optionManager.IsLHMStorage = mLHMStorageCheckBox.Checked;
            optionManager.IsLHMMemory = mLHMMemoryCheckBox.Checked;

            optionManager.IsNvAPIWrapper = mNvApiCheckBox.Checked;

            optionManager.IsDimm = mDimmCheckBox.Checked;
            
            optionManager.IsKraken = mKrakenCheckBox.Checked;

            optionManager.IsCLC = mCLCCheckBox.Checked;

            optionManager.IsRGBnFC = mRGBnFCCheckBox.Checked;

            optionManager.IsHWInfo = mHWInfoCheckBox.Checked;

            optionManager.IsLiquidctl = mLiquidctlCheckBox.Checked;

            optionManager.IsPlugin = mPluginCheckBox.Checked;

            optionManager.Language = mLanguageComboBox.SelectedIndex;
            optionManager.IsFahrenheit = mFahrenheitCheckBox.Checked;
            optionManager.IsAnimation = mAnimationCheckBox.Checked;
            optionManager.IsMinimized = mMinimizeCheckBox.Checked;
            optionManager.DelayTime = delayTime;
            optionManager.IsStartUp = mStartupCheckBox.Checked;            
            optionManager.write();

            if (isRestart == true)
            {
                this.DialogResult = DialogResult.Yes;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }

            // Language
            Util.setLanguage(optionManager.Language);

            this.Close();
        }

        private void onResetButtonClick(object sender, EventArgs e)
        {
            var result = MessageBox.Show(StringLib.OptionReset, StringLib.Option, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Cancel)
                return;

            OptionManager.getInstance().reset();            
            OptionManager.getInstance().write();
            this.DialogResult = DialogResult.No;
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
            var deviceList = HardwareManager.getInstance().KrakenList;
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
            var deviceList = HardwareManager.getInstance().CLCList;
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
            var deviceList = HardwareManager.getInstance().RGBnFCList;
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
