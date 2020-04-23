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
            HardwareManager.getInstance().onUpdateCallback += onUpdate;
            HardwareManager.getInstance().start();
            this.createComponent();
            this.ActiveControl = mFanControlButton;

            ControlManager.getInstance().read();
            if (ControlManager.getInstance().checkData() == false)
            {
                MessageBox.Show(StringLib.Not_Match);
            }
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
            if (mFanControlForm != null)
            {
                mFanControlForm.Close();
                mFanControlForm = null;
            }

            HardwareManager.getInstance().stop();
            
            mTrayIcon.Visible = false;

            Application.ExitThread();
            Application.Exit();
        }        

        private void createComponent()
        {
            var hardwareManager = HardwareManager.getInstance();

            // temperature
            for (int i = 0; i < hardwareManager.getSensorCount(); i++)
            {
                var label = new Label();
                label.Location = new System.Drawing.Point(15, 25 + i * 25);
                label.Name = "sensorLabel" + i.ToString();
                label.Size = new System.Drawing.Size(mTempGroupBox.Width - 18, 23);
                label.Text = "";
                mTempGroupBox.Controls.Add(label);
                mSensorLabelList.Add(label);

                if (i < hardwareManager.getSensorCount() - 1)
                {
                    mTempGroupBox.Height = mTempGroupBox.Height + 25;
                }
            }

            // fan
            for (int i = 0; i < hardwareManager.getFanCount(); i++)
            {
                var label = new Label();
                label.Location = new System.Drawing.Point(15, 25 + i * 25);
                label.Name = "fanLabel" + i.ToString();
                label.Size = new System.Drawing.Size(mFanGroupBox.Width - 18, 23);
                label.Text = "";
                mFanGroupBox.Controls.Add(label);
                mFanLabelList.Add(label);

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
                textBox.Location = new System.Drawing.Point(15, 20 + i * 25);
                textBox.Name = "controlTextBox" + i.ToString();
                textBox.Size = new System.Drawing.Size(40, 23);
                textBox.Multiline = false;
                textBox.MaxLength = 3;
                textBox.Text = "" + hardwareManager.getControl(i).Value;
                textBox.KeyPress += onTextBoxKeyPress;
                textBox.TextChanged += onTextBoxChanges;
                mControlGroupBox.Controls.Add(textBox);
                mControlTextBoxList.Add(textBox);

                int minValue = hardwareManager.getControl(i).getMinSpeed();
                int maxValue = hardwareManager.getControl(i).getMaxSpeed();
                var tooltipString = minValue + " ≤  value ≤ " + maxValue;
                mToolTip.SetToolTip(textBox, tooltipString);

                var label = new Label();
                label.Location = new System.Drawing.Point(textBox.Width + 20, 25 + i * 25);
                label.Name = "controlLabel" + i.ToString();
                label.Size = new System.Drawing.Size(mControlGroupBox.Width - textBox.Left - textBox.Width - 20, 23);
                label.Text = "% (" + hardwareManager.getControl(i).getName() + ")";
                mControlGroupBox.Controls.Add(label);
                mControlLabelList.Add(label);

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

        private void onUpdate()
        {
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

                if (mFanControlForm != null && mFanControlForm.IsDisposed == false)
                    mFanControlForm.onUpdateTimer();
            }));
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
