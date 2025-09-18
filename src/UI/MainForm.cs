using DarkUI.Config;
using DarkUI.Controls;
using FanCtrl.Resources;
using LibreHardwareMonitor.PawnIo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
    public partial class MainForm : ThemeForm
    {
        private bool mIsExit = false;
        private bool mIsFirstLoad = true;

        private List<Label> mTempLabelList = new List<Label>();
        private List<TextBox> mTempNameTextBoxList = new List<TextBox>();
        private List<Label> mFanLabelList = new List<Label>();
        private List<TextBox> mFanNameTextBoxList = new List<TextBox>();
        private List<NumericUpDownEx> mControlNumericUpDownList = new List<NumericUpDownEx>();
        private List<Label> mControlLabelList = new List<Label>();
        private List<TextBox> mControlNameTextBoxList = new List<TextBox>();

        private ControlForm mControlForm = null;

        private List<Icon> mFanIconList = new List<Icon>();
        private int mFanIconIndex = 0;
        private System.Windows.Forms.Timer mFanIconTimer = null;

        private bool mIsFirstShow = false;
        private bool mIsVisible = true;

        private bool mIsUserResize = true;

        private bool mIsWindowUpdate = false;

        private int mOriginWidth = 899;
        private int mOriginHeight = 169;
        private int mWidth = 899;
        private int mHeight = 169;

        private int mTempTop = 0;
        private int mTempLeft = 0;
        private int mTempWidth = 0;
        private int mTempHeight = 0;

        public MainForm()
        {
            if (read() == false)
            {
                write();
            }

            Util.setLanguage(OptionManager.getInstance().Language);

            if (PawnIo.IsInstalled)
            {
                if (PawnIo.Version() < new Version(2, 0, 0, 0))
                {
                    DialogResult result = MessageBox.Show(StringLib.PawnIO_update, "FanCtrl", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                        Util.InstallPawnIO();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show(StringLib.PawnIO_install, "FanCtrl", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                    Util.InstallPawnIO();
            }

            InitializeComponent();
            this.localizeComponent();

            HotkeyManager.getInstance().read();

            this.startHook();

            this.MinimumSize = new Size(mOriginWidth, mOriginHeight);

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
            mDonatePictureBox.MouseClick += onDonatePictureBoxClick;

            HardwareManager.getInstance().onUpdateCallback += onUpdate;

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
                mIsVisible = false;
                mIsFirstShow = false;
            }
        }

        private void localizeComponent()
        {
            this.Text = StringLib.Title + " v" + Application.ProductVersion;
            mTrayIcon.Text = StringLib.Title + " v" + Application.ProductVersion;
            mTempGroupBox.Text = StringLib.Temperature;
            mFanGroupBox.Text = StringLib.Fan_speed;
            mControlGroupBox.Text = StringLib.Fan_control;

            mLiquidctlButton.Text = StringLib.liquidctl_Setting;
            mReloadButton.Text = StringLib.Reload;
            mHotKeyButton.Text = StringLib.HotKey;
            mPluginButton.Text = StringLib.Plugin;
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

            FontFamily fontFamily = null;
            try
            {
                fontFamily = new FontFamily("Gulim");
            }
            catch
            {
                fontFamily = FontFamily.GenericSansSerif;
            }

            // Russian
            if (OptionManager.getInstance().Language == 5)
            {
                mFanControlButton.Font = new Font(fontFamily, 6.5f);
            }
            else
            {
                mFanControlButton.Font = new Font(fontFamily, 9.0f);
            }
        }

        protected void setTheme()
        {
            var type = OptionManager.getInstance().getNowTheme();
            if (type == THEME_TYPE.DARK)
            {
                ThemeProvider.Theme = new DarkTheme();
            }
            else
            {
                ThemeProvider.Theme = new LightTheme();
            }
            BackColor = ThemeProvider.Theme.Colors.GreyBackground;

            Theme.setTheme(this.Handle, (type == THEME_TYPE.DARK));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.SetStyle(ControlStyles.UserPaint |
                            ControlStyles.OptimizedDoubleBuffer |
                            ControlStyles.AllPaintingInWmPaint |
                            ControlStyles.SupportsTransparentBackColor, true);

            this.Resize += (s2, e2) =>
            {
                if (this.WindowState == FormWindowState.Minimized)
                    return;

                if (mIsUserResize == false)
                {
                    mIsUserResize = true;
                    return;
                }

                mWidth = this.Width;
                mHeight = this.Height;
                this.resizeForm();
            };

            this.ResizeEnd += (s2, e2) =>
            {
                mIsWindowUpdate = true;
                mTempLeft = this.Left;
                mTempTop = this.Top;
                mTempWidth = this.Width;
                mTempHeight = this.Height;
                this.write();
            };

            if (OptionManager.getInstance().IsMinimized == true)
            {
                this.BeginInvoke(new Action(delegate ()
                {
                    this.Close();
                }));
            }

            // reload
            this.reload();
        }

        private void reload()
        {
            this.Enabled = false;

            mLoadingPanel.Visible = true;
            mTrayIcon.ContextMenuStrip = null;

            mTempPanel.Controls.Clear();
            mFanPanel.Controls.Clear();
            mControlPanel.Controls.Clear();

            mTempLabelList.Clear();
            mTempNameTextBoxList.Clear();
            mFanLabelList.Clear();
            mFanNameTextBoxList.Clear();
            mControlNumericUpDownList.Clear();
            mControlLabelList.Clear();
            mControlNameTextBoxList.Clear();

            mLiquidctlButton.Enabled = OptionManager.getInstance().IsLiquidctl;
            mLiquidctlButton.Visible = OptionManager.getInstance().IsLiquidctl;

            mPluginButton.Enabled = OptionManager.getInstance().IsPlugin;
            mPluginButton.Visible = OptionManager.getInstance().IsPlugin;

            if (OptionManager.getInstance().IsPlugin == true)
            {
                mPluginButton.Left = (OptionManager.getInstance().IsLiquidctl == true) ? 257 : mLiquidctlButton.Left;
            }

            if (mFanIconTimer != null)
            {
                mFanIconTimer.Stop();
                mFanIconTimer.Dispose();
                mFanIconTimer = null;
            }
            
            mTrayIcon.Icon = mFanIconList[0];
            mFanIconIndex = 0;

            mWidth = mOriginWidth;
            mHeight = mOriginHeight;

            this.resizeForm();

            var task = new Task(delegate {
                int checkCount = (mIsFirstLoad == true) ? 3 : 0;
                mIsFirstLoad = false;
                while (true)
                {
                    // start hardware manager
                    HardwareManager.getInstance().start();

                    // set hardware name
                    bool isDifferent = false;
                    if (HardwareManager.getInstance().read(ref isDifferent) == false)
                        break;

                    if (isDifferent == true && checkCount > 0)
                    {
                        // restart
                        HardwareManager.getInstance().stop();
                        Util.sleep(50);
                        checkCount--;
                        continue;
                    }
                    break;
                }

                // set hardware name to file
                HardwareManager.getInstance().write();

                // read auto fan curve
                ControlManager.getInstance().read();

                // read osd data
                OSDManager.getInstance().read();

                // plugin start
                if (OptionManager.getInstance().IsPlugin == true && PluginManager.getInstance().IsStart == true)
                {
                    PluginManager.getInstance().start(PluginManager.getInstance().Port);
                }

                this.BeginInvoke(new Action(delegate ()
                {
                    this.onMainLoad();
                }));                
            });
            task.Start();
        }

        private void onMainLoad()
        {
            this.createComponent();
            this.ActiveControl = mFanControlButton;

            mEnableToolStripMenuItem.Checked = ControlManager.getInstance().IsEnable;
            mEnableOSDToolStripMenuItem.Checked = OSDManager.getInstance().IsEnable;
            mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.NORMAL);
            mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.SILENCE);
            mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.PERFORMANCE);
            mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.GAME);

            // startUpdate
            HardwareManager.getInstance().startUpdate();

            // start icon update
            mTrayIcon.ContextMenuStrip = mTrayMenuStrip;
            mFanIconTimer = new System.Windows.Forms.Timer();
            mFanIconTimer.Interval = 100;
            mFanIconTimer.Tick += onFanIconTimer;
            if (OptionManager.getInstance().IsAnimation == true)
            {
                mFanIconTimer.Start();
            }

            mLoadingPanel.Visible = false;

            if (mIsVisible == true)
            {
                mIsFirstShow = true;
                this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                                          (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
            }

            if (mIsWindowUpdate == true)
            {
                this.Left = mTempLeft;
                this.Top = mTempTop;
                this.Width = mTempWidth;
                this.Height = mTempHeight;
                mWidth = mTempWidth;
                mHeight = mTempHeight;
                resizeForm();
            }

            this.Enabled = true;
        }

        private void resizeForm()
        {
            // origin size
            int groupBoxHeight = 53;
            int panelHeight = 35;
            int madeLabelPoint1 = 78;
            int madeLabelPoint2 = 95;
            int donatePictureBoxPoint = 81;
            int buttonPoint = 71;

            int gapHeight = mHeight - mOriginHeight;

            groupBoxHeight = groupBoxHeight + gapHeight;
            panelHeight = panelHeight + gapHeight;
            madeLabelPoint1 = madeLabelPoint1 + gapHeight;
            madeLabelPoint2 = madeLabelPoint2 + gapHeight;
            donatePictureBoxPoint = donatePictureBoxPoint + gapHeight;
            buttonPoint = buttonPoint + gapHeight;

            mTempGroupBox.Height = groupBoxHeight;
            mFanGroupBox.Height = groupBoxHeight;
            mControlGroupBox.Height = groupBoxHeight;

            mTempPanel.Height = panelHeight;
            mFanPanel.Height = panelHeight;
            mControlPanel.Height = panelHeight;

            mMadeLabel1.Top = madeLabelPoint1;
            mMadeLabel2.Top = madeLabelPoint2;
            mDonatePictureBox.Top = donatePictureBoxPoint;

            mLiquidctlButton.Top = buttonPoint;
            mReloadButton.Top = buttonPoint;
            mHotKeyButton.Top = buttonPoint;
            mPluginButton.Top = buttonPoint;
            mOSDButton.Top = buttonPoint;
            mOptionButton.Top = buttonPoint;
            mFanControlButton.Top = buttonPoint;

            //////////////////////////////////////////////////////////

            int gapWidth = mWidth - mOriginWidth;
            int divide = gapWidth / 3;

            //mTempGroupBox.Left = 12 + divide;
            mFanGroupBox.Left = 290 + divide;
            mControlGroupBox.Left = 568 + (divide * 2);

            mTempGroupBox.Width = 272 + divide;
            mFanGroupBox.Width = 272 + divide;
            mControlGroupBox.Width = 306 + divide;

            mTempPanel.Width = 261 + divide;
            mFanPanel.Width = 260 + divide;
            mControlPanel.Width = 294 + divide;

            mPluginButton.Left = 257 + gapWidth;
            mLiquidctlButton.Left = 346 + gapWidth;
            mReloadButton.Left = 435 + gapWidth;
            mHotKeyButton.Left = 524 + gapWidth;
            mOSDButton.Left = 613 + gapWidth;
            mOptionButton.Left = 702 + gapWidth;
            mFanControlButton.Left = 791 + gapWidth;

            for (int i = 0; i < mTempNameTextBoxList.Count; i++)
            {
                mTempNameTextBoxList[i].Width = mTempPanel.Width - 70;
            }

            for (int i = 0; i < mFanNameTextBoxList.Count; i++)
            {
                mFanNameTextBoxList[i].Width = mFanPanel.Width - 90;
            }

            for (int i = 0; i < mControlNameTextBoxList.Count; i++)
            {
                mControlNameTextBoxList[i].Width = mControlPanel.Width - 95;
            }

            //////////////////////////////////////////////////////////

            mIsUserResize = false;
            this.Width = mWidth;
            this.Height = mHeight;
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
                mIsVisible = false;
                this.Visible = false;
                e.Cancel = true;
            }
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
            this.onTrayMenuShow(null, EventArgs.Empty);
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
            ControlManager.getInstance().ModeType = MODE_TYPE.NORMAL;
            mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.NORMAL);
            mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.SILENCE);
            mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.PERFORMANCE);
            mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.GAME);
            ControlManager.getInstance().write();
        }

        private void onTrayMenuSilenceClick(object sender, EventArgs e)
        {
            ControlManager.getInstance().ModeType = MODE_TYPE.SILENCE;
            mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.NORMAL);
            mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.SILENCE);
            mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.PERFORMANCE);
            mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.GAME);
            ControlManager.getInstance().write();
        }

        private void onTrayMenuPerformanceClick(object sender, EventArgs e)
        {
            ControlManager.getInstance().ModeType = MODE_TYPE.PERFORMANCE;
            mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.NORMAL);
            mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.SILENCE);
            mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.PERFORMANCE);
            mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.GAME);
            ControlManager.getInstance().write();
        }

        private void onTrayMenuGameClick(object sender, EventArgs e)
        {
            ControlManager.getInstance().ModeType = MODE_TYPE.GAME;
            mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.NORMAL);
            mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.SILENCE);
            mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.PERFORMANCE);
            mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.GAME);
            ControlManager.getInstance().write();
        }

        private void onTrayMenuShow(object sender, EventArgs e)
        {
            if (this.Enabled == false)
                return;

            mIsVisible = true;
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.Activate();

            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            if (mIsFirstShow == false)
            {
                this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                                          (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
                mIsFirstShow = true;
            }

            var type = OptionManager.getInstance().getNowTheme();
            Theme.setTheme(this.Handle, (type == THEME_TYPE.DARK));
        }

        private void onTrayMenuExit(object sender, EventArgs e)
        {
            if (mControlForm != null)
            {
                mControlForm.Close();
                mControlForm = null;
            }

            PluginManager.getInstance().stop();
            HardwareManager.getInstance().stop();

            if (mFanIconTimer != null)
            {
                mFanIconTimer.Stop();
                mFanIconTimer.Dispose();
                mFanIconTimer = null;
            }

            mTrayIcon.Visible = false;

            mIsExit = true;

            this.stopHook();

            Application.ExitThread();
            Application.Exit();
        }

        private void createComponent()
        {
            var hardwareManager = HardwareManager.getInstance();
            var controlManager = ControlManager.getInstance();

            int tempHeight = mTempPanel.Height;
            int fanHeight = mFanPanel.Height;
            int controlHeight = mControlPanel.Height;

            int fontPointY = 0;
            float fontSize = 9.0f;
            FontFamily fontFamily = null;
            try
            {
                fontFamily = new FontFamily("Gulim");
                fontPointY = -5;
            }
            catch
            {
                fontFamily = FontFamily.GenericSansSerif;
            }

            // temperature
            int pointY = 15;
            for (int i = 0; i < hardwareManager.TempList.Count(); i++)
            {
                var tempList = hardwareManager.TempList[i];
                if (tempList.Count == 0)
                    continue;

                var libLabel = new ThemeLibLabel(fontFamily, fontSize);
                libLabel.Location = new System.Drawing.Point(15, pointY);
                libLabel.Text = Define.cLibraryTypeString[i];                
                mTempPanel.Controls.Add(libLabel);

                var libLabel2 = new DarkLabel();
                libLabel2.Location = new System.Drawing.Point(5, pointY + 5);
                libLabel2.Size = new System.Drawing.Size(mTempPanel.Width - 30, 2);
                libLabel2.AutoSize = false;
                libLabel2.BorderStyle = BorderStyle.Fixed3D;
                mTempPanel.Controls.Add(libLabel2);

                pointY = pointY + 21;
                tempHeight = tempHeight + 21;

                for (int j = 0; j < tempList.Count; j++)
                {
                    var hardwareDevice = tempList[j];

                    var hardwareLabel = new ThemeHardwareLabel(fontFamily, fontSize);
                    hardwareLabel.Location = new System.Drawing.Point(25, pointY);
                    hardwareLabel.Text = hardwareDevice.Name;
                    mTempPanel.Controls.Add(hardwareLabel);

                    var hardwareLabel2 = new DarkLabel();
                    hardwareLabel2.Location = new System.Drawing.Point(20, pointY + 5);
                    hardwareLabel2.Size = new System.Drawing.Size(190, 2);
                    hardwareLabel2.AutoSize = false;
                    hardwareLabel2.BorderStyle = BorderStyle.Fixed3D;
                    mTempPanel.Controls.Add(hardwareLabel2);

                    pointY = pointY + 25;
                    tempHeight = tempHeight + 25;

                    for (int k = 0; k < hardwareDevice.DeviceList.Count; k++)
                    {
                        var device = hardwareDevice.DeviceList[k];

                        var label = new DarkLabel();
                        label.Location = new System.Drawing.Point(0, pointY);
                        label.Size = new System.Drawing.Size(45, 23);
                        label.Text = "";
                        label.AutoSize = false;
                        label.TextAlign = ContentAlignment.TopRight;
                        label.Font = new Font(fontFamily, fontSize, FontStyle.Regular);
                        mTempPanel.Controls.Add(label);
                        mTempLabelList.Add(label);

                        var textBox = new DarkTextBox();
                        textBox.Location = new System.Drawing.Point(label.Right + 2, label.Top + fontPointY);
                        textBox.Size = new System.Drawing.Size(mTempPanel.Width - 70, 23);
                        textBox.Multiline = false;
                        textBox.MaxLength = 40;
                        textBox.Text = device.Name;
                        textBox.Leave += (object sender, EventArgs e) =>
                        {
                            this.onNameTextBoxLeaves((TextBox)sender, NAME_TYPE.TEMPERATURE, ref mTempNameTextBoxList);
                        };
                        textBox.KeyDown += (object sender, KeyEventArgs e) =>
                        {
                            if (e.KeyCode == Keys.Enter)
                            {
                                this.onNameTextBoxLeaves((TextBox)sender, NAME_TYPE.TEMPERATURE, ref mTempNameTextBoxList);
                            }
                        };
                        mTempPanel.Controls.Add(textBox);
                        mTempNameTextBoxList.Add(textBox);

                        pointY = pointY + 25;
                        tempHeight = tempHeight + 25;
                    }

                    pointY = pointY + 10;
                    tempHeight = tempHeight + 10;
                }
            }

            tempHeight = tempHeight - 30;

            int maxHeight = Screen.PrimaryScreen.WorkingArea.Height - 200;
            if (tempHeight > maxHeight)
            {
                int gap = tempHeight - maxHeight;
                tempHeight = tempHeight - gap;
            }

            pointY = 15;
            for (int i = 0; i < hardwareManager.FanList.Count(); i++)
            {
                var fanList = hardwareManager.FanList[i];
                if (fanList.Count == 0)
                    continue;

                var libLabel = new ThemeLibLabel(fontFamily, fontSize);
                libLabel.Location = new System.Drawing.Point(15, pointY);
                libLabel.Text = Define.cLibraryTypeString[i];
                mFanPanel.Controls.Add(libLabel);

                var libLabel2 = new DarkLabel();
                libLabel2.Location = new System.Drawing.Point(5, pointY + 5);
                libLabel2.Size = new System.Drawing.Size(mFanPanel.Width - 30, 2);
                libLabel2.AutoSize = false;
                libLabel2.BorderStyle = BorderStyle.Fixed3D;
                mFanPanel.Controls.Add(libLabel2);

                pointY = pointY + 21;
                fanHeight = fanHeight + 21;

                for (int j = 0; j < fanList.Count; j++)
                {
                    var hardwareDevice = fanList[j];

                    var hardwareLabel = new ThemeHardwareLabel(fontFamily, fontSize);
                    hardwareLabel.Location = new System.Drawing.Point(25, pointY);
                    hardwareLabel.Text = hardwareDevice.Name;
                    mFanPanel.Controls.Add(hardwareLabel);

                    var hardwareLabel2 = new DarkLabel();
                    hardwareLabel2.Location = new System.Drawing.Point(20, pointY + 5);
                    hardwareLabel2.Size = new System.Drawing.Size(190, 2);
                    hardwareLabel2.AutoSize = false;
                    hardwareLabel2.BorderStyle = BorderStyle.Fixed3D;
                    mFanPanel.Controls.Add(hardwareLabel2);

                    pointY = pointY + 25;
                    fanHeight = fanHeight + 25;

                    for (int k = 0; k < hardwareDevice.DeviceList.Count; k++)
                    {
                        var device = hardwareDevice.DeviceList[k];

                        var label = new DarkLabel();
                        label.Location = new System.Drawing.Point(0, pointY);
                        label.Size = new System.Drawing.Size(67, 23);
                        label.Text = "";
                        label.AutoSize = false;
                        label.TextAlign = ContentAlignment.TopRight;
                        label.Font = new Font(fontFamily, fontSize, FontStyle.Regular);
                        mFanPanel.Controls.Add(label);
                        mFanLabelList.Add(label);

                        var textBox = new DarkTextBox();
                        textBox.Location = new System.Drawing.Point(label.Right + 2, label.Top + fontPointY);
                        textBox.Size = new System.Drawing.Size(mFanPanel.Width - 90, 23);
                        textBox.Multiline = false;
                        textBox.MaxLength = 40;
                        textBox.Text = device.Name;
                        textBox.Leave += (object sender, EventArgs e) =>
                        {
                            this.onNameTextBoxLeaves((TextBox)sender, NAME_TYPE.FAN, ref mFanNameTextBoxList);
                        };
                        textBox.KeyDown += (object sender, KeyEventArgs e) =>
                        {
                            if (e.KeyCode == Keys.Enter)
                            {
                                this.onNameTextBoxLeaves((TextBox)sender, NAME_TYPE.FAN, ref mFanNameTextBoxList);
                            }
                        };
                        mFanPanel.Controls.Add(textBox);
                        mFanNameTextBoxList.Add(textBox);

                        pointY = pointY + 25;
                        fanHeight = fanHeight + 25;
                    }

                    pointY = pointY + 10;
                    fanHeight = fanHeight + 10;
                }
            }

            fanHeight = fanHeight - 30;

            if (fanHeight > maxHeight)
            {
                int gap = fanHeight - maxHeight;
                fanHeight = fanHeight - gap;
            }

            // set height
            if (fanHeight > tempHeight)
            {
                tempHeight = fanHeight;
            }
            else
            {
                fanHeight = tempHeight;
            }

            pointY = 15;
            for (int i = 0; i < hardwareManager.ControlList.Count(); i++)
            {
                var controlList = hardwareManager.ControlList[i];
                if (controlList.Count == 0)
                    continue;

                var libLabel = new ThemeLibLabel(fontFamily, fontSize);
                libLabel.Location = new System.Drawing.Point(15, pointY);
                libLabel.Text = Define.cLibraryTypeString[i];
                mControlPanel.Controls.Add(libLabel);

                var libLabel2 = new DarkLabel();
                libLabel2.Location = new System.Drawing.Point(5, pointY + 5);
                libLabel2.Size = new System.Drawing.Size(mControlPanel.Width - 30, 2);
                libLabel2.AutoSize = false;
                libLabel2.BorderStyle = BorderStyle.Fixed3D;
                mControlPanel.Controls.Add(libLabel2);

                pointY = pointY + 21;
                controlHeight = controlHeight + 21;

                for (int j = 0; j < controlList.Count; j++)
                {
                    var hardwareDevice = controlList[j];

                    var hardwareLabel = new ThemeHardwareLabel(fontFamily, fontSize);
                    hardwareLabel.Location = new System.Drawing.Point(25, pointY);
                    hardwareLabel.Text = hardwareDevice.Name;
                    mControlPanel.Controls.Add(hardwareLabel);

                    var hardwareLabel2 = new DarkLabel();
                    hardwareLabel2.Location = new System.Drawing.Point(20, pointY + 5);
                    hardwareLabel2.Size = new System.Drawing.Size(190, 2);
                    hardwareLabel2.AutoSize = false;
                    hardwareLabel2.BorderStyle = BorderStyle.Fixed3D;
                    mControlPanel.Controls.Add(hardwareLabel2);

                    pointY = pointY + 25;
                    controlHeight = controlHeight + 25;

                    for (int k = 0; k < hardwareDevice.DeviceList.Count; k++)
                    {
                        var device = (BaseControl)hardwareDevice.DeviceList[k];

                        var number = new NumericUpDownEx();
                        number.Location = new System.Drawing.Point(10, pointY + fontPointY);
                        number.Size = new System.Drawing.Size(40, 23);
                        number.Maximum = 100;
                        number.Minimum = 0;
                        number.Value = 0;
                        number.Increment = 1;

                        int z = mControlNumericUpDownList.Count;
                        number.ValueChangedInput += (object sender, EventArgs e) =>
                        {
                            Console.WriteLine("ValueChangedInput : {0}", e.ToString());
                            var tempNumber = (NumericUpDownEx)sender;
                            var controlBaseList = hardwareManager.ControlBaseList;
                            var controlDevice = controlBaseList[z];
                            int originValue = controlDevice.Value;

                            int nowValue = Decimal.ToInt32(tempNumber.Value);
                            int minSpeed = controlBaseList[z].getMinSpeed();
                            int maxSpeed = controlBaseList[z].getMaxSpeed();

                            if (nowValue >= minSpeed && nowValue <= maxSpeed)
                            {
                                int changeValue = hardwareManager.addChangeValue(nowValue, controlDevice);
                                if (changeValue != originValue)
                                {
                                    tempNumber.ExValue = changeValue;
                                }
                                Console.WriteLine("numericIndex : " + z);
                            }
                            else
                            {
                                tempNumber.ExValue = originValue;
                                mToolTip.Show(minSpeed + " ≤  value ≤ " + maxSpeed, tempNumber, 2000);
                            }
                        };

                        mControlPanel.Controls.Add(number);
                        mControlNumericUpDownList.Add(number);

                        int minValue = device.getMinSpeed();
                        int maxValue = device.getMaxSpeed();
                        mToolTip.SetToolTip(number, minValue + " ≤  value ≤ " + maxValue);

                        var label = new DarkLabel();
                        label.Location = new System.Drawing.Point(number.Right + 1, pointY);
                        label.Size = new System.Drawing.Size(15, 23);
                        label.Text = "%";
                        label.Font = new Font(fontFamily, fontSize, FontStyle.Regular);
                        mControlPanel.Controls.Add(label);
                        mControlLabelList.Add(label);

                        var textBox2 = new DarkTextBox();
                        textBox2.Location = new System.Drawing.Point(label.Right + 7, label.Top + fontPointY);
                        textBox2.Size = new System.Drawing.Size(mControlPanel.Width - 95, 23);
                        textBox2.Multiline = false;
                        textBox2.MaxLength = 40;
                        textBox2.Text = device.Name;
                        textBox2.Leave += (object sender, EventArgs e) =>
                        {
                            this.onNameTextBoxLeaves((TextBox)sender, NAME_TYPE.CONTOL, ref mControlNameTextBoxList);
                        };
                        textBox2.KeyDown += (object sender, KeyEventArgs e) =>
                        {
                            if (e.KeyCode == Keys.Enter)
                            {
                                this.onNameTextBoxLeaves((TextBox)sender, NAME_TYPE.CONTOL, ref mControlNameTextBoxList);
                            }
                        };
                        mControlPanel.Controls.Add(textBox2);
                        mControlNameTextBoxList.Add(textBox2);

                        pointY = pointY + 25;
                        controlHeight = controlHeight + 25;
                    }

                    pointY = pointY + 10;
                    controlHeight = controlHeight + 10;
                }
            }

            controlHeight = controlHeight - 30;

            if (controlHeight > maxHeight)
            {
                int gap = controlHeight - maxHeight;
                controlHeight = controlHeight - gap;
            }

            // set height
            if (fanHeight > controlHeight)
            {
                controlHeight = fanHeight;
            }
            else
            {
                tempHeight = controlHeight;
                fanHeight = controlHeight;
            }

            int originPanelHeight = 35;
            int heightGap = tempHeight - originPanelHeight;
            mHeight = mHeight + heightGap;

            this.resizeForm();
        }

        private void onNameTextBoxLeaves(TextBox textBox, NAME_TYPE nameType, ref List<TextBox> nameTextBoxList)
        {
            try
            {
                string name = textBox.Text;
                int index = -1;
                for (int i = 0; i < nameTextBoxList.Count; i++)
                {
                    if (nameTextBoxList[i] == textBox)
                    {
                        index = i;
                        break;
                    }
                }

                if (index < 0)
                    return;

                BaseDevice device = null;
                if (nameType == NAME_TYPE.TEMPERATURE)
                {
                    device = HardwareManager.getInstance().TempBaseList[index];
                    PluginManager.getInstance().setTempDeviceName(device.ID, name);
                }
                else if (nameType == NAME_TYPE.FAN)
                {
                    device = HardwareManager.getInstance().FanBaseList[index];
                    PluginManager.getInstance().setFanDeviceName(device.ID, name);
                }
                else
                {
                    device = HardwareManager.getInstance().ControlBaseList[index];
                    PluginManager.getInstance().setControlDeviceName(device.ID, name);
                }

                string originName = device.Name;
                if (name.Length == 0)
                {
                    name = originName;
                }

                device.Name = name;
                textBox.Text = name;

                var osdSensorMap = HardwareManager.getInstance().OSDSensorMap;
                if (osdSensorMap.ContainsKey(device.ID) == true)
                {
                    var sensor = osdSensorMap[device.ID];
                    sensor.Name = name;
                }

                HardwareManager.getInstance().write();
            }
            catch { }
        }

        private void onUpdate()
        {
            if (this.Visible == false)
                return;

            this.BeginInvoke(new Action(delegate ()
            {
                var hardwareManager = HardwareManager.getInstance();

                for (int i = 0; i < hardwareManager.TempBaseList.Count; i++)
                {
                    var device = hardwareManager.TempBaseList[i];
                    mTempLabelList[i].Text = device.getString();
                }

                for (int i = 0; i < hardwareManager.FanBaseList.Count; i++)
                {
                    var device = hardwareManager.FanBaseList[i];
                    mFanLabelList[i].Text = device.getString();
                }

                for (int i = 0; i < hardwareManager.ControlBaseList.Count; i++)
                {
                    var device = hardwareManager.ControlBaseList[i];
                    if (mControlNumericUpDownList[i].Focused == false)
                    {
                        mControlNumericUpDownList[i].ExValue = device.Value;
                    }
                }

                if (mControlForm != null)
                    mControlForm.onUpdateTimer();
            }));
        }

        private void onHotKeyButtonClick(object sender, EventArgs e)
        {
            var form = new HotkeyForm(this);
            form.ShowDialog();
        }

        private void onOptionButtonClick(object sender, EventArgs e)
        {
            var form = new OptionForm();
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                HardwareManager.getInstance().restartTimer();

                // start icon update
                if (OptionManager.getInstance().IsAnimation == true)
                {
                    if (mFanIconTimer == null)
                    {
                        mFanIconTimer = new System.Windows.Forms.Timer();
                        mFanIconTimer.Interval = 100;
                        mFanIconTimer.Tick += onFanIconTimer;
                        mFanIconTimer.Start();
                    }
                }
                else
                {
                    if (mFanIconTimer != null)
                    {
                        mFanIconTimer.Stop();
                        mFanIconTimer.Dispose();
                        mFanIconTimer = null;
                    }

                    mTrayIcon.Icon = mFanIconList[0];
                    mFanIconIndex = 0;
                }
            }

            // Changed option data
            else if(result == DialogResult.Yes)
            {
                this.BeginInvoke(new Action(delegate ()
                {
                    this.localizeComponent();
                    PluginManager.getInstance().stop();
                    HardwareManager.getInstance().stop();
                    ControlManager.getInstance().reset();
                    OSDManager.getInstance().reset();

                    if (OptionManager.getInstance().IsHWInfo == false)
                    {
                        HWInfoManager.getInstance().reset();
                        HWInfoManager.getInstance().write();
                    }

                    this.setTheme();
                    this.reload();
                }));
            }

            // Reset option data
            else if (result == DialogResult.No)
            {
                this.BeginInvoke(new Action(delegate ()
                {
                    PluginManager.getInstance().stop();
                    HardwareManager.getInstance().stop();
                    HardwareManager.getInstance().write();
                    ControlManager.getInstance().reset();
                    ControlManager.getInstance().write();
                    OSDManager.getInstance().reset();
                    OSDManager.getInstance().write();
                    HWInfoManager.getInstance().reset();
                    HWInfoManager.getInstance().write();

                    this.reload();
                }));
            }
        }

        private void onFanControlButtonClick(object sender, EventArgs e)
        {
            mControlForm = new ControlForm();
            mControlForm.onApplyCallback += (sender2, e2) =>
            {
                mEnableToolStripMenuItem.Checked = ControlManager.getInstance().IsEnable;
                mNormalToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.NORMAL);
                mSilenceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.SILENCE);
                mPerformanceToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.PERFORMANCE);
                mGameToolStripMenuItem.Checked = (ControlManager.getInstance().ModeType == MODE_TYPE.GAME);
            };
            mControlForm.ShowDialog();
            mControlForm = null;
        }
        
        private void onDonatePictureBoxClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MainForm.onDonatePictureBoxClick()");

            string localString = StringLib.Localize;
            if (localString.CompareTo("ko") == 0)
            {
                var form = new DonateForm();
                form.ShowDialog();
            }
            else
            {
                System.Diagnostics.Process.Start("https://www.paypal.com/paypalme/lich426");
            }
        }

        private void onPluginButtonClick(object sender, EventArgs e)
        {
            var form = new PluginForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.onReloadButtonClick(null, EventArgs.Empty);
            }
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

        private void onReloadButtonClick(object sender, EventArgs e)
        {
            HardwareManager.getInstance().stop();
            ControlManager.getInstance().reset();
            OSDManager.getInstance().reset();

            if (OptionManager.getInstance().IsHWInfo == false)
            {
                HWInfoManager.getInstance().reset();
                HWInfoManager.getInstance().write();
            }

            this.reload();
        }

        private void onLiquidctlButtonClick(object sender, EventArgs e)
        {
            var form = new LiquidctlForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.onReloadButtonClick(null, EventArgs.Empty);
            }
        }

        private string mLocationFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "Location.json";

        private bool read()
        {
            try
            {
                var jsonString = File.ReadAllText(mLocationFileName);
                var rootObject = JObject.Parse(jsonString);

                mIsWindowUpdate = (rootObject.ContainsKey("IsWindowUpdate") == true) ? rootObject.Value<bool>("IsWindowUpdate") : false;
                mTempLeft = (rootObject.ContainsKey("Left") == true) ? rootObject.Value<int>("Left") : 0;
                mTempTop = (rootObject.ContainsKey("Top") == true) ? rootObject.Value<int>("Top") : 0;
                mTempWidth = (rootObject.ContainsKey("Width") == true) ? rootObject.Value<int>("Width") : 899;
                mTempHeight = (rootObject.ContainsKey("Height") == true) ? rootObject.Value<int>("Height") : 169;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void write()
        {
            try
            {
                var rootObject = new JObject();
                rootObject["IsWindowUpdate"] = mIsWindowUpdate;
                rootObject["Left"] = mTempLeft;
                rootObject["Top"] = mTempTop;
                rootObject["Width"] = mTempWidth;
                rootObject["Height"] = mTempHeight;

                File.WriteAllText(mLocationFileName, rootObject.ToString());
            }
            catch { }
        }

        private const int WM_POWERBROADCAST = 0x218;
        private const int PBT_APMQUERYSUSPEND = 0x0;
        private const int PBT_APMRESUMESUSPEND = 0x7;
        private const int PBT_APMRESUMEAUTOMATIC = 0x12;        
        private const int PBT_APMSUSPEND = 0x4;

        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                case WM_POWERBROADCAST:
                    switch (msg.WParam.ToInt32())
                    {
                        case PBT_APMQUERYSUSPEND:
                        case PBT_APMSUSPEND:
                            Console.WriteLine("MainForm.WndProc() : enter power saving mode({0})", msg.WParam.ToInt32());
                            break;
                        case PBT_APMRESUMEAUTOMATIC:
                        case PBT_APMRESUMESUSPEND:
                            Console.WriteLine("MainForm.WndProc() : exit power saving mode({0})", msg.WParam.ToInt32());
                            this.BeginInvoke(new Action(delegate ()
                            {
                                if (this.Enabled == false)
                                {
                                    Console.WriteLine("MainForm.WndProc() : Already reloading");
                                    return;
                                }

                                HardwareManager.getInstance().stop();
                                ControlManager.getInstance().reset();
                                OSDManager.getInstance().reset();

                                mIsFirstLoad = true;
                                this.reload();
                            }));
                            break;
                    }
                    break;
            }
            base.WndProc(ref msg);
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYUP = 0x105;
        public delegate int OnHookHandler(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int SetWindowsHookEx(int hookID, OnHookHandler handler, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(int hookID);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(int hookID, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        private static int sHookInstance = 0;
        private static OnHookHandler sOnHookHandler = onHookHandler;
        private static bool sIsSetHook = false;
        private static OnHookHandler sOnSetHookHandler = null;
        private static MainForm sMainForm = null;

        private static bool sIsCtrl = false;
        private static bool sIsAlt = false;
        private static bool sIsLShift = false;
        private static bool sIsRShift = false;

        private void startHook()
        {
            try
            {
                sMainForm = this;
                var hInstance = LoadLibrary("User32");
                sHookInstance = SetWindowsHookEx(WH_KEYBOARD_LL, sOnHookHandler, hInstance, 0);
            }
            catch
            {
                sHookInstance = 0;
            }
        }

        private void stopHook()
        {
            try
            {
                sMainForm = null;
                if (sHookInstance != 0)
                    UnhookWindowsHookEx(sHookInstance);
            }
            catch { }
            sHookInstance = 0;
        }

        public void setHookHandler(OnHookHandler onHookHandler)
        {
            sIsSetHook = true;
            sOnSetHookHandler = onHookHandler;
        }

        public void unHookHandler()
        {
            sIsSetHook = false;
            sOnSetHookHandler = null;
        }

        private static int onHookHandler(int code, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (sMainForm != null && sMainForm.Enabled == false)
                    return CallNextHookEx(sHookInstance, code, wParam, lParam);

                if (sIsSetHook == true && sOnSetHookHandler != null)
                {
                    if (sOnSetHookHandler(code, wParam, lParam) == 1)
                        return 1;
                }
                else
                {
                    if (code >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
                    {
                        int vkCode = Marshal.ReadInt32(lParam);
                        switch (vkCode)
                        {
                            case (int)Keys.LControlKey: sIsCtrl = true; break;
                            case (int)Keys.LShiftKey: sIsLShift = true; break;
                            case (int)Keys.RShiftKey: sIsRShift = true; break;
                            case (int)Keys.LMenu: sIsAlt = true; break;
                            default:
                                {
                                    var keyManager = HotkeyManager.getInstance();
                                    if (keyManager.mEnableFanControlData.mIsCtrl == sIsCtrl &&
                                        keyManager.mEnableFanControlData.mIsAlt == sIsAlt &&
                                        keyManager.mEnableFanControlData.mIsLShift == sIsLShift &&
                                        keyManager.mEnableFanControlData.mIsRShift == sIsRShift &&
                                        keyManager.mEnableFanControlData.mKey == vkCode &&
                                        keyManager.mEnableFanControlData.mKey > 0)
                                    {
                                        sMainForm.onTrayMenuEnableClick(null, null);
                                    }

                                    if (keyManager.mModeNormalData.mIsCtrl == sIsCtrl &&
                                        keyManager.mModeNormalData.mIsAlt == sIsAlt &&
                                        keyManager.mModeNormalData.mIsLShift == sIsLShift &&
                                        keyManager.mModeNormalData.mIsRShift == sIsRShift &&
                                        keyManager.mModeNormalData.mKey == vkCode &&
                                        keyManager.mModeNormalData.mKey > 0)
                                    {
                                        sMainForm.onTrayMenuNormalClick(null, null);
                                    }

                                    if (keyManager.mModeSilenceData.mIsCtrl == sIsCtrl &&
                                        keyManager.mModeSilenceData.mIsAlt == sIsAlt &&
                                        keyManager.mModeSilenceData.mIsLShift == sIsLShift &&
                                        keyManager.mModeSilenceData.mIsRShift == sIsRShift &&
                                        keyManager.mModeSilenceData.mKey == vkCode &&
                                        keyManager.mModeSilenceData.mKey > 0)
                                    {
                                        sMainForm.onTrayMenuSilenceClick(null, null);
                                    }

                                    if (keyManager.mModePerformanceData.mIsCtrl == sIsCtrl &&
                                        keyManager.mModePerformanceData.mIsAlt == sIsAlt &&
                                        keyManager.mModePerformanceData.mIsLShift == sIsLShift &&
                                        keyManager.mModePerformanceData.mIsRShift == sIsRShift &&
                                        keyManager.mModePerformanceData.mKey == vkCode &&
                                        keyManager.mModePerformanceData.mKey > 0)
                                    {
                                        sMainForm.onTrayMenuPerformanceClick(null, null);
                                    }

                                    if (keyManager.mModeGameData.mIsCtrl == sIsCtrl &&
                                        keyManager.mModeGameData.mIsAlt == sIsAlt &&
                                        keyManager.mModeGameData.mIsLShift == sIsLShift &&
                                        keyManager.mModeGameData.mIsRShift == sIsRShift &&
                                        keyManager.mModeGameData.mKey == vkCode &&
                                        keyManager.mModeGameData.mKey > 0)
                                    {
                                        sMainForm.onTrayMenuGameClick(null, null);
                                    }

                                    if (keyManager.mEnableOSDData.mIsCtrl == sIsCtrl &&
                                        keyManager.mEnableOSDData.mIsAlt == sIsAlt &&
                                        keyManager.mEnableOSDData.mIsLShift == sIsLShift &&
                                        keyManager.mEnableOSDData.mIsRShift == sIsRShift &&
                                        keyManager.mEnableOSDData.mKey == vkCode &&
                                        keyManager.mEnableOSDData.mKey > 0)
                                    {
                                        sMainForm.onTrayManuEnableOSDClick(null, null);
                                    }
                                }
                                break;
                        }
                    }

                    else if (code >= 0 && (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP))
                    {
                        int vkCode = Marshal.ReadInt32(lParam);
                        switch (vkCode)
                        {
                            case (int)Keys.LControlKey: sIsCtrl = false; break;
                            case (int)Keys.LShiftKey: sIsLShift = false; break;
                            case (int)Keys.RShiftKey: sIsRShift = false; break;
                            case (int)Keys.LMenu: sIsAlt = false; break;
                            default: break;
                        }
                    }
                }
            }
            catch { }
            return CallNextHookEx(sHookInstance, code, wParam, lParam);
        }

    }
}
