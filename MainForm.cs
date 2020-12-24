using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace FanCtrl
{
    public partial class MainForm : Form
    {
        private bool mIsExit = false;

        private List<Label> mSensorLabelList = new List<Label>();
        private List<TextBox> mSensorNameTextBoxList = new List<TextBox>();
        private List<Label> mFanLabelList = new List<Label>();
        private List<TextBox> mFanNameTextBoxList = new List<TextBox>();
        private List<TextBox> mControlTextBoxList = new List<TextBox>();
        private List<Label> mControlLabelList = new List<Label>();
        private List<TextBox> mControlNameTextBoxList = new List<TextBox>();        

        private ControlForm mControlForm = null;

        private List<Icon> mFanIconList = new List<Icon>();
        private int mFanIconIndex = 0;
        private System.Windows.Forms.Timer mFanIconTimer = new System.Windows.Forms.Timer();

        private int mDeviceCheckCount = 0;
        private System.Timers.Timer mDeviceCheckTimer = new System.Timers.Timer();
        private object mDeviceCheckTimerLock = new object();

        public MainForm()
        {
            InitializeComponent();
            this.localizeComponent();

            this.FormClosing += onClosing;

            mFanIconList.Add(Properties.Resources.fan_1);
            mFanIconList.Add(Properties.Resources.fan_2);
            mFanIconList.Add(Properties.Resources.fan_3);
            mFanIconList.Add(Properties.Resources.fan_4);
            mFanIconList.Add(Properties.Resources.fan_5);
            mFanIconList.Add(Properties.Resources.fan_6);
            mFanIconList.Add(Properties.Resources.fan_7);
            mFanIconList.Add(Properties.Resources.fan_8);

            mTrayIcon.Icon = mFanIconList[0];
            mTrayIcon.Visible = true;
            mTrayIcon.MouseDoubleClick += onTrayIconDBClicked;
            mTrayIcon.ContextMenuStrip = mTrayMenuStrip;

            mDonatePictureBox.MouseClick += onDonatePictureBoxClick;

            if (OptionManager.getInstance().read() == false)
            {
                OptionManager.getInstance().write();
            }

            if (OptionManager.getInstance().Interval < 100)
            {
                OptionManager.getInstance().Interval = 100;
            }
            else if (OptionManager.getInstance().Interval > 5000)
            {
                OptionManager.getInstance().Interval = 5000;
            }

            if (OptionManager.getInstance().IsMinimized == true)
            {
                this.Visible = false;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Enabled = false;
            mLoadingPanel.Visible = true;
            mDeviceCheckTimer.Interval = 100;
            mDeviceCheckTimer.Elapsed += (object sender2, ElapsedEventArgs e2) =>
            {
                if (Monitor.TryEnter(mDeviceCheckTimerLock) == false)
                    return;

                mDeviceCheckCount++;

                bool isErrorMessage = false;
                if (checkDevice() == false)
                {
                    if (mDeviceCheckCount >= 5)
                    {
                        isErrorMessage = true;                     
                    }
                    else
                    {
                        HardwareManager.getInstance().stop();
                        ControlManager.getInstance().reset();
                        Monitor.Exit(mDeviceCheckTimerLock);
                        return;
                    }
                }

                this.BeginInvoke(new Action(delegate ()
                {
                    // OSDManager
                    OSDManager.getInstance().read();

                    this.createComponent();
                    this.ActiveControl = mFanControlButton;

                    mEnableToolStripMenuItem.Checked = ControlManager.getInstance().IsEnable;
                    mEnableOSDToolStripMenuItem.Checked = OSDManager.getInstance().IsEnable;
                    mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 0);
                    mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 1);
                    mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 2);
                    mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 3);

                    // startUpdate
                    HardwareManager.getInstance().startUpdate();

                    // start icon update
                    mFanIconTimer.Interval = 100;
                    mFanIconTimer.Tick += onFanIconTimer;
                    if (OptionManager.getInstance().IsAnimation == true)
                    {
                        mFanIconTimer.Start();
                    }

                    mLoadingPanel.Visible = false;
                    this.Enabled = true;

                    if (isErrorMessage == true)
                    {
                        MessageBox.Show(StringLib.Not_Match, StringLib.Error);
                    }
                }));
                
                mDeviceCheckTimer.Stop();
                Monitor.Exit(mDeviceCheckTimerLock);
            };
            mDeviceCheckTimer.Start();

            if (OptionManager.getInstance().IsMinimized == true)
            {
                this.BeginInvoke(new Action(delegate ()
                {
                    this.Close();
                }));
            }
        }        

        private void localizeComponent()
        {
            this.Text = StringLib.Title + " v" + Application.ProductVersion;
            mTrayIcon.Text = StringLib.Title;
            mTempGroupBox.Text = StringLib.Temperature;
            mFanGroupBox.Text = StringLib.Fan_speed;
            mControlGroupBox.Text = StringLib.Fan_control;
            mOptionButton.Text = StringLib.Option;
            mFanControlButton.Text = StringLib.Auto_Fan_Control;
            mMadeLabel1.Text = StringLib.Made1;
            mMadeLabel2.Text = StringLib.Made2;

            mEnableToolStripMenuItem.Text = StringLib.Enable_automatic_fan_control;
            mEnableOSDToolStripMenuItem.Text = StringLib.Enable_OSD;
            mNormalToolStripMenuItem.Text = StringLib.Normal;
            mSilenceToolStripMenuItem.Text = StringLib.Silence;
            mPerformanceToolStripMenuItem.Text = StringLib.Performance;
            mGameToolStripMenuItem.Text = StringLib.Game;
            mShowToolStripMenuItem.Text = StringLib.Show;
            mExitToolStripMenuItem.Text = StringLib.Exit;
        }

        private void onClosing(object sender, FormClosingEventArgs e)
        {
            if (mControlForm != null)
            {
                mControlForm.Close();
                mControlForm = null;
            }

            if (mIsExit == false)
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }

        private bool checkDevice()
        {
            var hardwareManager = HardwareManager.getInstance();
            var controlManager = ControlManager.getInstance();

            hardwareManager.onUpdateCallback += onUpdate;
            hardwareManager.start();

            // name
            controlManager.setNameCount(0, hardwareManager.getSensorCount());
            controlManager.setNameCount(1, hardwareManager.getFanCount());
            controlManager.setNameCount(2, hardwareManager.getControlCount());

            for (int i = 0; i < hardwareManager.getSensorCount(); i++)
            {
                var temp = hardwareManager.getSensor(i);
                controlManager.setName(0, i, true, temp.Name);
                controlManager.setName(0, i, false, temp.Name);
            }

            for (int i = 0; i < hardwareManager.getFanCount(); i++)
            {
                var temp = hardwareManager.getFan(i);
                controlManager.setName(1, i, true, temp.Name);
                controlManager.setName(1, i, false, temp.Name);
            }

            for (int i = 0; i < hardwareManager.getControlCount(); i++)
            {
                var temp = hardwareManager.getControl(i);
                controlManager.setName(2, i, true, temp.Name);
                controlManager.setName(2, i, false, temp.Name);
            }

            if (controlManager.read() == false || controlManager.checkData() == false)
            {
                return false;
            }
            return true;
        }

        private void onFanIconTimer(object sender, EventArgs e)
        {
            if (ControlManager.getInstance().IsEnable == false || OptionManager.getInstance().IsAnimation == false)
                return;

            if (mFanIconIndex >= mFanIconList.Count)
                mFanIconIndex = 0;
            mTrayIcon.Icon = mFanIconList[mFanIconIndex++];
        }

        private void onTrayIconDBClicked(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.Activate();

            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
        }

        private void onTrayMenuEnableClick(object sender, EventArgs e)
        {
            ControlManager.getInstance().IsEnable = !ControlManager.getInstance().IsEnable;
            mEnableToolStripMenuItem.Checked = ControlManager.getInstance().IsEnable;
            ControlManager.getInstance().write();
        }

        private void onTrayManuEnableOSDClick(object sender, EventArgs e)
        {
            OSDManager.getInstance().IsEnable = !OSDManager.getInstance().IsEnable;
            mEnableOSDToolStripMenuItem.Checked = OSDManager.getInstance().IsEnable;
            OSDManager.getInstance().write();
        }

        private void onTrayMenuNormalClick(object sender, EventArgs e)
        {
            ControlManager.getInstance().ModeIndex = 0;
            mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 0);
            mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 1);
            mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 2);
            mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 3);
            ControlManager.getInstance().write();
        }

        private void onTrayMenuSilenceClick(object sender, EventArgs e)
        {
            ControlManager.getInstance().ModeIndex = 1;
            mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 0);
            mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 1);
            mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 2);
            mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 3);
            ControlManager.getInstance().write();
        }

        private void onTrayMenuPerformanceClick(object sender, EventArgs e)
        {
            ControlManager.getInstance().ModeIndex = 2;
            mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 0);
            mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 1);
            mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 2);
            mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 3);
            ControlManager.getInstance().write();
        }

        private void onTrayMenuGameClick(object sender, EventArgs e)
        {
            ControlManager.getInstance().ModeIndex = 3;
            mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 0);
            mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 1);
            mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 2);
            mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 3);
            ControlManager.getInstance().write();
        }

        private void onTrayMenuShow(object sender, EventArgs e)
        {            
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.Activate();

            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
        }

        private void onTrayMenuExit(object sender, EventArgs e)
        {
            if (mControlForm != null)
            {
                mControlForm.Close();
                mControlForm = null;
            }

            HardwareManager.getInstance().stop();

            mFanIconTimer.Stop();
            mTrayIcon.Visible = false;

            mIsExit = true;
            Application.ExitThread();
            Application.Exit();
        }

        private void onRestartProgram(object sender, EventArgs e)
        {
            if (mControlForm != null)
            {
                mControlForm.Close();
                mControlForm = null;
            }

            HardwareManager.getInstance().stop();

            mFanIconTimer.Stop();
            mTrayIcon.Visible = false;

            Program.releaseMutex();
            Program.executeProgram();

            mIsExit = true;
            Environment.Exit(0);
        }

        private void createComponent()
        {
            var hardwareManager = HardwareManager.getInstance();
            var controlManager = ControlManager.getInstance();

            // temperature
            for (int i = 0; i < hardwareManager.getSensorCount(); i++)
            {
                var label = new Label();
                label.Location = new System.Drawing.Point(3, 25 + i * 25);
                label.Name = "sensorLabel" + i.ToString();
                label.Size = new System.Drawing.Size(40, 23);
                label.Text = "";
                label.AutoSize = false;
                label.TextAlign = ContentAlignment.TopRight;
                mTempGroupBox.Controls.Add(label);
                mSensorLabelList.Add(label);

                var textBox = new TextBox();
                textBox.Location = new System.Drawing.Point(label.Left + label.Width + 5, label.Top - 5);
                textBox.Name = "sensorName" + i.ToString();
                textBox.Size = new System.Drawing.Size(mTempGroupBox.Width - 60, 23);
                textBox.Multiline = false;
                textBox.MaxLength = 40;
                textBox.Text = controlManager.getName(0, i, false);
                textBox.Leave += onSensorNameTextBoxLeaves;
                mTempGroupBox.Controls.Add(textBox);
                mSensorNameTextBoxList.Add(textBox);

                if (i < hardwareManager.getSensorCount() - 1)
                {
                    mTempGroupBox.Height = mTempGroupBox.Height + 25;
                }
            }

            // fan
            for (int i = 0; i < hardwareManager.getFanCount(); i++)
            {
                var label = new Label();
                label.Location = new System.Drawing.Point(10, 25 + i * 25);
                label.Name = "fanLabel" + i.ToString();
                label.Size = new System.Drawing.Size(60, 23);
                label.Text = "";
                label.AutoSize = false;
                label.TextAlign = ContentAlignment.TopRight;
                mFanGroupBox.Controls.Add(label);
                mFanLabelList.Add(label);

                var textBox = new TextBox();
                textBox.Location = new System.Drawing.Point(label.Left + label.Width + 5, label.Top - 5);
                textBox.Name = "fanName" + i.ToString();
                textBox.Size = new System.Drawing.Size(mFanGroupBox.Width - 85, 23);
                textBox.Multiline = false;
                textBox.MaxLength = 40;
                textBox.Text = controlManager.getName(1, i, false);
                textBox.Leave += onFanNameTextBoxLeaves;
                mFanGroupBox.Controls.Add(textBox);
                mFanNameTextBoxList.Add(textBox);

                if (i < hardwareManager.getFanCount() - 1)
                {
                    mFanGroupBox.Height = mFanGroupBox.Height + 25;
                }
            }

            // set groupbox height
            if (mFanGroupBox.Height > mTempGroupBox.Height)
                mTempGroupBox.Height = mFanGroupBox.Height;
            else
                mFanGroupBox.Height = mTempGroupBox.Height;

            // control
            for (int i = 0; i < hardwareManager.getControlCount(); i++)
            {
                var textBox = new TextBox();
                textBox.Location = new System.Drawing.Point(10, 20 + i * 25);
                textBox.Name = "controlTextBox" + i.ToString();
                textBox.Size = new System.Drawing.Size(40, 23);
                textBox.Multiline = false;
                textBox.MaxLength = 3;
                textBox.Text = "" + hardwareManager.getControl(i).Value;
                textBox.KeyPress += onControlTextBoxKeyPress;
                textBox.TextChanged += onControlTextBoxChanges;
                mControlGroupBox.Controls.Add(textBox);
                mControlTextBoxList.Add(textBox);

                int minValue = hardwareManager.getControl(i).getMinSpeed();
                int maxValue = hardwareManager.getControl(i).getMaxSpeed();
                var tooltipString = minValue + " ≤  value ≤ " + maxValue;
                mToolTip.SetToolTip(textBox, tooltipString);

                var label = new Label();
                label.Location = new System.Drawing.Point(textBox.Left + textBox.Width + 2, 25 + i * 25);
                label.Name = "controlLabel" + i.ToString();
                label.Size = new System.Drawing.Size(15, 23);
                label.Text = "%";
                mControlGroupBox.Controls.Add(label);
                mControlLabelList.Add(label);

                var textBox2 = new TextBox();
                textBox2.Location = new System.Drawing.Point(label.Left + label.Width + 5, label.Top - 5);
                textBox2.Name = "controlName" + i.ToString();
                textBox2.Size = new System.Drawing.Size(mControlGroupBox.Width - 85, 23);
                textBox2.Multiline = false;
                textBox2.MaxLength = 40;
                textBox2.Text = controlManager.getName(2, i, false);
                textBox2.Leave += onFanControlNameTextBoxLeaves;
                mControlGroupBox.Controls.Add(textBox2);
                mControlNameTextBoxList.Add(textBox2);

                if (i < hardwareManager.getControlCount() - 1)
                {
                    mControlGroupBox.Height = mControlGroupBox.Height + 25;
                }
            }

            // set groupbox height
            if (mFanGroupBox.Height > mControlGroupBox.Height)
            {
                mControlGroupBox.Height = mFanGroupBox.Height;
            }
            else
            {
                mTempGroupBox.Height = mControlGroupBox.Height;
                mFanGroupBox.Height = mControlGroupBox.Height;
            }            

            // position
            mOSDButton.Top = mFanGroupBox.Top + mFanGroupBox.Height + 10;
            mOptionButton.Top = mFanGroupBox.Top + mFanGroupBox.Height + 10;
            mFanControlButton.Top = mFanGroupBox.Top + mFanGroupBox.Height + 10;
            mMadeLabel1.Top = mFanGroupBox.Top + mFanGroupBox.Height + 15;
            mMadeLabel2.Top = mFanGroupBox.Top + mFanGroupBox.Height + 32;
            mDonatePictureBox.Top = mFanGroupBox.Top + mFanGroupBox.Height + 17;
            this.Height = mFanGroupBox.Height + mOptionButton.Height + 70;            
        }

        private void onControlTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false)
            {
                e.Handled = true;
            }
        }

        private void onControlTextBoxChanges(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.Focused == false)
                return;

            var hardwareManager = HardwareManager.getInstance();
            int value = int.Parse(textBox.Text);
            for (int i = 0; i < mControlTextBoxList.Count; i++)
            {
                if (mControlTextBoxList[i].Equals(sender) == true)
                {
                    int minValue = hardwareManager.getControl(i).getMinSpeed();
                    int maxValue = hardwareManager.getControl(i).getMaxSpeed();

                    if(value >= minValue && value <= maxValue)
                    {
                        int changeValue = hardwareManager.addChangeValue(value, hardwareManager.getControl(i));
                        if (changeValue != value)
                        {
                            textBox.Text = changeValue.ToString();
                        }
                    }
                    break;
                }
            }
        }

        private void onSensorNameTextBoxLeaves(object sender, EventArgs e)
        {
            this.onNameTextBoxLeaves((TextBox)sender, 0, ref mSensorNameTextBoxList);
        }

        private void onFanNameTextBoxLeaves(object sender, EventArgs e)
        {
            this.onNameTextBoxLeaves((TextBox)sender, 1, ref mFanNameTextBoxList);
        }

        private void onFanControlNameTextBoxLeaves(object sender, EventArgs e)
        {
            this.onNameTextBoxLeaves((TextBox)sender, 2, ref mControlNameTextBoxList);
        }

        private void onNameTextBoxLeaves(TextBox textBox, int type, ref List<TextBox> nameTextBoxList)
        {
            var controlManager = ControlManager.getInstance();
            int index = -1;
            int num = 2;
            string name = textBox.Text;

            while (true)
            {
                bool isExist = false;
                for (int i = 0; i < nameTextBoxList.Count; i++)
                {
                    if (nameTextBoxList[i] == textBox)
                    {
                        if (name.Length == 0)
                        {
                            textBox.Text = controlManager.getName(type, i, false);
                            return;
                        }

                        index = i;
                        continue;
                    }

                    else if (nameTextBoxList[i].Text.Equals(name) == true)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (isExist == true)
                {
                    name = textBox.Text + " #" + num++;
                    continue;
                }

                textBox.Text = name;
                controlManager.setName(type, index, false, name);
                break;
            }
            controlManager.write();
        }

        private void onUpdate()
        {
            if (this.Visible == false)
                return;

            this.BeginInvoke(new Action(delegate ()
            {
                var hardwareManager = HardwareManager.getInstance();
                
                for (int i = 0; i < hardwareManager.getSensorCount(); i++)
                {
                    var sensor = hardwareManager.getSensor(i);
                    if (sensor == null)
                        break;

                    mSensorLabelList[i].Text = sensor.getString();
                }

                for (int i = 0; i < hardwareManager.getFanCount(); i++)
                {
                    var fan = hardwareManager.getFan(i);
                    if (fan == null)
                        break;

                    mFanLabelList[i].Text = fan.getString();
                }

                for (int i = 0; i < hardwareManager.getControlCount(); i++)
                {
                    var control = hardwareManager.getControl(i);
                    if (control == null)
                        break;

                    if (mControlTextBoxList[i].Focused == false)
                    {
                        mControlTextBoxList[i].Text = control.Value.ToString();
                    }
                }

                if (mControlForm != null)
                    mControlForm.onUpdateTimer();
            }));
        }
        

        private void onOptionButtonClick(object sender, EventArgs e)
        {
            var form = new OptionForm();
            form.OnExitHandler += onRestartProgram;
            if (form.ShowDialog() == DialogResult.OK)
            {
                HardwareManager.getInstance().restartTimer(OptionManager.getInstance().Interval);

                // start icon update
                if (OptionManager.getInstance().IsAnimation == true)
                {
                    mFanIconTimer.Start();
                }
                else
                {
                    mFanIconTimer.Stop();
                }
            }
        }

        private void onFanControlButtonClick(object sender, EventArgs e)
        {
            mControlForm = new ControlForm();
            mControlForm.onApplyCallback += (sender2, e2) =>
            {
                mEnableToolStripMenuItem.Checked = ControlManager.getInstance().IsEnable;
                mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 0);
                mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 1);
                mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 2);
                mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeIndex == 3);
            };
            mControlForm.ShowDialog();
            mControlForm = null;
        }
        
        private void onDonatePictureBoxClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MainForm.onDonatePictureBoxClick()");
            System.Diagnostics.Process.Start("https://www.buymeacoffee.com/lich");
        }

        private void onOSDButtonClick(object sender, EventArgs e)
        {
            var form = new OSDForm();
            form.onApplyCallback += (sender2, e2) =>
            {
                mEnableOSDToolStripMenuItem.Checked = OSDManager.getInstance().IsEnable;
            };
            form.ShowDialog();
        }
    }
}
