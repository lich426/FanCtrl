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
    public partial class MainForm : Form
    {
        private List<Label> mSensorLabelList = new List<Label>();
        private List<Label> mFanLabelList = new List<Label>();
        private List<TextBox> mControlTextBoxList = new List<TextBox>();
        private List<Label> mControlLabelList = new List<Label>();
        
        private FanControlForm mFanControlForm = null;

        private Timer mTimer = new Timer();

        public MainForm()
        {
            InitializeComponent();
            this.localizeComponent();

            this.FormClosing += onClosing;

            mTrayIcon.Visible = true;
            mTrayIcon.MouseDoubleClick += onTrayIconDBClicked;
            mTrayIcon.ContextMenuStrip = mTrayMenuStrip;

            if (OptionManager.getInstance().read() == false)
            {
                OptionManager.getInstance().write();
            }

            if(OptionManager.getInstance().IsMinimized == true)
            {
                this.Visible = false;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }

            HardwareManager.getInstance().start(OptionManager.getInstance().Interval);
            this.createComponent();
            this.ActiveControl = mFanControlButton;

            ControlManager.getInstance().read();
            if (ControlManager.getInstance().checkData() == false)
            {
                MessageBox.Show(StringLib.Not_Match);
            }

            mTimer.Tick += onUpdateTimer;
            mTimer.Interval = OptionManager.getInstance().Interval;
            mTimer.Start();
        }

        private void localizeComponent()
        {
            System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = StringLib.Title + " v" + version.Major + "." + version.Minor + "." + version.Build;
            mTrayIcon.Text = StringLib.Title;
            mTempGroupBox.Text = StringLib.Temperature;
            mFanGroupBox.Text = StringLib.Fan_speed;
            mControlGroupBox.Text = StringLib.Fan_control;
            mOptionButton.Text = StringLib.Option;
            mFanControlButton.Text = StringLib.Auto_Fan_Control;
            mMadeLabel.Text = StringLib.Made;
        }

        private void onClosing(object sender, FormClosingEventArgs e)
        {
            if (mFanControlForm != null)
            {
                mFanControlForm.Close();
                mFanControlForm = null;
            }

            this.Visible = false;
            e.Cancel = true;
        }

        private void onTrayIconDBClicked(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.Activate();

            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
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
            mTimer.Stop();

            if (mFanControlForm != null)
            {
                mFanControlForm.Close();
                mFanControlForm = null;
            }

            HardwareManager.getInstance().stop();

            mSensorLabelList.Clear();
            mFanLabelList.Clear();
            mControlTextBoxList.Clear();
            mControlLabelList.Clear();

            mTrayIcon.Visible = false;

            Application.ExitThread();
            Application.Exit();
        }        

        private void createComponent()
        {
            var hardwareManager = HardwareManager.getInstance();

            // temperature
            for (int i = 0; i < hardwareManager.SensorList.Count; i++)
            {
                var label = new Label();
                label.Location = new System.Drawing.Point(15, 25 + i * 25);
                label.Name = "sensorLabel" + i.ToString();
                label.Size = new System.Drawing.Size(mTempGroupBox.Width - 20, 23);
                label.Text = "";
                mTempGroupBox.Controls.Add(label);
                mSensorLabelList.Add(label);

                if (i < hardwareManager.SensorList.Count - 1)
                {
                    mTempGroupBox.Height = mTempGroupBox.Height + 25;
                }
            }

            // fan
            for (int i = 0; i < hardwareManager.FanList.Count; i++)
            {
                var label = new Label();
                label.Location = new System.Drawing.Point(15, 25 + i * 25);
                label.Name = "fanLabel" + i.ToString();
                label.Size = new System.Drawing.Size(mFanGroupBox.Width - 20, 23);
                label.Text = "";
                mFanGroupBox.Controls.Add(label);
                mFanLabelList.Add(label);

                if (i < hardwareManager.FanList.Count - 1)
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
            for (int i = 0; i < hardwareManager.ControlList.Count; i++)
            {
                var textBox = new TextBox();
                textBox.Location = new System.Drawing.Point(15, 20 + i * 25);
                textBox.Name = "controlTextBox" + i.ToString();
                textBox.Size = new System.Drawing.Size(40, 23);
                textBox.Multiline = false;
                textBox.MaxLength = 3;
                textBox.Text = "" + hardwareManager.ControlList[i].Value;
                textBox.KeyPress += onTextBoxKeyPress;
                textBox.TextChanged += onTextBoxChanges;
                mControlGroupBox.Controls.Add(textBox);
                mControlTextBoxList.Add(textBox);

                int minValue = hardwareManager.ControlList[i].getMinSpeed();
                int maxValue = hardwareManager.ControlList[i].getMaxSpeed();
                var tooltipString = minValue + " ≤  value ≤ " + maxValue;
                mToolTip.SetToolTip(textBox, tooltipString);

                var label = new Label();
                label.Location = new System.Drawing.Point(textBox.Width + 20, 25 + i * 25);
                label.Name = "controlLabel" + i.ToString();
                label.Size = new System.Drawing.Size(mControlGroupBox.Width - textBox.Left - textBox.Width - 20, 23);
                label.Text = "% (" + hardwareManager.ControlList[i].getName() + ")";
                mControlGroupBox.Controls.Add(label);
                mControlLabelList.Add(label);

                if (i < hardwareManager.ControlList.Count - 1)
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
            mOptionButton.Top = mFanGroupBox.Top + mFanGroupBox.Height + 6;            
            mFanControlButton.Top = mFanGroupBox.Top + mFanGroupBox.Height + 6;
            mMadeLabel.Top = mFanGroupBox.Top + mFanGroupBox.Height + 20;
            this.Height = mFanGroupBox.Height + mOptionButton.Height + 70;
        }

        private void onTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false)
            {
                e.Handled = true;
            }
        }

        private void onTextBoxChanges(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.Focused == false)
                return;

            int value = int.Parse(textBox.Text);
            for (int i = 0; i < mControlTextBoxList.Count; i++)
            {
                if (mControlTextBoxList[i].Equals(sender) == true)
                {
                    int minValue = HardwareManager.getInstance().ControlList[i].getMinSpeed();
                    int maxValue = HardwareManager.getInstance().ControlList[i].getMaxSpeed();

                    if(value >= minValue && value <= maxValue)
                    {
                        int changeValue = HardwareManager.getInstance().ControlList[i].setSpeed(value);
                        if (changeValue != value)
                        {
                            textBox.Text = changeValue.ToString();
                        }
                    }
                    break;
                }
            }
        }

        private void onUpdateTimer(object sender, EventArgs e)
        {
            var hardwareManager = HardwareManager.getInstance();
            var sensorList = hardwareManager.SensorList;
            var fanList = hardwareManager.FanList;
            var controlList = hardwareManager.ControlList;

            for (int i = 0; i < sensorList.Count; i++)
            {
                mSensorLabelList[i].Text = sensorList[i].getString();
            }

            for (int i = 0; i < fanList.Count; i++)
            {
                mFanLabelList[i].Text = fanList[i].getString();
            }

            for (int i = 0; i < controlList.Count; i++)
            {
                if (mControlTextBoxList[i].Focused == false)
                {
                    mControlTextBoxList[i].Text = controlList[i].Value.ToString();
                }
            }

            if (mFanControlForm != null)
                mFanControlForm.onUpdateTimer();
        }
        

        private void onOptionButtonClick(object sender, EventArgs e)
        {
            var form = new OptionForm();
            if(form.ShowDialog() == DialogResult.OK)
            {
                HardwareManager.getInstance().restartTimer(OptionManager.getInstance().Interval);
            }
        }

        private void onFanControlButtonClick(object sender, EventArgs e)
        {
            if(mFanControlForm != null)
            {
                if(mFanControlForm.IsDisposed == true)
                {
                    mFanControlForm = null;
                }
                else
                {
                    mFanControlForm.Focus();
                    return;
                }
            }

            mFanControlForm = new FanControlForm();
            mFanControlForm.StartPosition = FormStartPosition.Manual;
            mFanControlForm.Location = new Point(this.Location.X + 100, this.Location.Y + 100);

            mFanControlForm.Show(this);
        }
    }
}
