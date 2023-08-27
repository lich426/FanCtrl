using FanCtrl.Resources;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FanCtrl
{
    public partial class HotkeyForm : ThemeForm
    {
        private const int WM_KEYDOWN = 0x100;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYUP = 0x105;

        private MainForm mMainForm = null;

        private bool mIsCtrl = false;
        private bool mIsAlt = false;
        private bool mIsLShift = false;
        private bool mIsRShift = false;

        private int mKey = (int)Keys.None;

        public HotkeyForm(MainForm mainForm)
        {
            InitializeComponent();
            this.localizeComponent();

            this.setTextBoxText(mEnableFanControlTextBox, HotkeyManager.getInstance().mEnableFanControlData);
            this.setTextBoxText(mModeNormalTextBox, HotkeyManager.getInstance().mModeNormalData);
            this.setTextBoxText(mModeSilenceTextBox, HotkeyManager.getInstance().mModeSilenceData);
            this.setTextBoxText(mModePerformanceTextBox, HotkeyManager.getInstance().mModePerformanceData);
            this.setTextBoxText(mModeGameTextBox, HotkeyManager.getInstance().mModeGameData);
            this.setTextBoxText(mEnableOSDTextBox, HotkeyManager.getInstance().mEnableOSDData);

            mIsCtrl = false;
            mIsAlt = false;
            mIsLShift = false;
            mIsRShift = false;
            mKey = (int)Keys.None;

            mMainForm = mainForm;
            mMainForm.setHookHandler(onHookHandler);
            this.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                mMainForm.unHookHandler();
            };

            mEnableFanControlTextBox.GotFocus += onTextBoxGotFocus;
            mModeNormalTextBox.GotFocus += onTextBoxGotFocus;
            mModeSilenceTextBox.GotFocus += onTextBoxGotFocus;
            mModePerformanceTextBox.GotFocus += onTextBoxGotFocus;
            mModeGameTextBox.GotFocus += onTextBoxGotFocus;
            mEnableOSDTextBox.GotFocus += onTextBoxGotFocus;

            mEnableFanControlTextBox.Leave += onTextBoxLeave;
            mModeNormalTextBox.Leave += onTextBoxLeave;
            mModeSilenceTextBox.Leave += onTextBoxLeave;
            mModePerformanceTextBox.Leave += onTextBoxLeave;
            mModeGameTextBox.Leave += onTextBoxLeave;
            mEnableOSDTextBox.Leave += onTextBoxLeave;
        }

        private void localizeComponent()
        {
            this.Text = StringLib.HotKey;
            mFanControlGroupBox.Text = StringLib.Fan_control;
            mEnableFanControlLabel.Text = StringLib.Enable_automatic_fan_control + " :";
            mModeNormalLabel.Text = StringLib.Mode + " - " + StringLib.Normal + " :";
            mModeSilenceLabel.Text = StringLib.Mode + " - " + StringLib.Silence + " :";
            mModePerformanceLabel.Text = StringLib.Mode + " - " + StringLib.Performance + " :";
            mModeGameLabel.Text = StringLib.Mode + " - " + StringLib.Game + " :";
            mEnableOSDLabel.Text = StringLib.Enable_OSD + " :";

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
                mEnableFanControlLabel.Font = new Font(fontFamily, 6.5f);
                mEnableOSDLabel.Font = new Font(fontFamily, 6.5f);
            }
        }

        private void setTextBoxText(TextBox textBox, HotkeyData keyData)
        {
            mIsCtrl = keyData.mIsCtrl;
            mIsAlt = keyData.mIsAlt;
            mIsLShift = keyData.mIsLShift;
            mIsRShift = keyData.mIsRShift;
            mKey = keyData.mKey;
            if (mKey == (int)Keys.None)
            {
                textBox.Text = "NONE";
            }
            else
            {
                textBox.Text = this.getSubKeyString() + this.getKeyString((Keys)mKey);
            }
        }

        private string getSubKeyString()
        {
            var kc = new KeysConverter();
            string keyString = "";
            if (mIsCtrl == true)
            {
                keyString = "CTRL + ";
            }
            if (mIsAlt == true)
            {
                keyString = keyString + "ALT + ";
            }
            if (mIsLShift == true)
            {
                keyString = keyString + "LShift + ";
            }
            if (mIsRShift == true)
            {
                keyString = keyString + "RShift + ";
            }
            return keyString;
        }

        private string getKeyString(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.Oemtilde: return "`";
                case Keys.OemMinus: return "-";
                case Keys.Oemplus: return "+";
                case Keys.OemOpenBrackets: return "[";
                case Keys.OemCloseBrackets: return "]";
                case Keys.Oem5: return "\\";
                case Keys.Oem1: return ";";
                case Keys.Oem7: return "'";
                case Keys.Oemcomma: return ",";
                case Keys.OemPeriod: return ".";
                case Keys.OemQuestion: return "/";
                case Keys.Divide: return "/";
                case Keys.Multiply: return "*";
                case Keys.Subtract: return "-";
                case Keys.Add: return "+";
                default:
                    break;
            }

            var kc = new KeysConverter();
            return kc.ConvertToString(keyCode);
        }

        private int onHookHandler(int code, IntPtr wParam, IntPtr lParam)
        {
            if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                TextBox textBox = null;
                if (mEnableFanControlTextBox.Focused == true)
                    textBox = mEnableFanControlTextBox;
                else if (mModeNormalTextBox.Focused == true)
                    textBox = mModeNormalTextBox;
                else if (mModeSilenceTextBox.Focused == true)
                    textBox = mModeSilenceTextBox;
                else if (mModePerformanceTextBox.Focused == true)
                    textBox = mModePerformanceTextBox;
                else if (mModeGameTextBox.Focused == true)
                    textBox = mModeGameTextBox;
                else if (mEnableOSDTextBox.Focused == true)
                    textBox = mEnableOSDTextBox;

                if (textBox == null || code < 0)
                    return 0;

                int vkCode = Marshal.ReadInt32(lParam);

                switch (vkCode)
                {
                    // pass window key
                    case (int)Keys.LWin:
                    case (int)Keys.RWin:
                        return 1;

                    // esc
                    case (int)Keys.Escape:
                        mKey = (int)Keys.None;
                        textBox.Text = "NONE";

                        mIsCtrl = false;
                        mIsAlt = false;
                        mIsLShift = false;
                        mIsRShift = false;

                        this.saveData(textBox);

                        textBox.Enabled = false;
                        textBox.Enabled = true;
                        break;

                    // CTRL
                    case (int)Keys.LControlKey:
                        mIsCtrl = true;
                        textBox.Text = this.getSubKeyString();
                        break;

                    // LShift
                    case (int)Keys.LShiftKey:
                        mIsLShift = true;
                        textBox.Text = this.getSubKeyString();
                        break;

                    // RShift
                    case (int)Keys.RShiftKey:
                        mIsRShift = true;
                        textBox.Text = this.getSubKeyString();
                        break;

                    // LAlt
                    case (int)Keys.LMenu:
                        mIsAlt = true;
                        textBox.Text = this.getSubKeyString();
                        break;
                                            
                    case (int)Keys.F1:
                    case (int)Keys.F2:
                    case (int)Keys.F3:
                    case (int)Keys.F4:
                    case (int)Keys.F5:
                    case (int)Keys.F6:
                    case (int)Keys.F7:
                    case (int)Keys.F8:
                    case (int)Keys.F9:
                    case (int)Keys.F10:
                    case (int)Keys.F11:
                    case (int)Keys.F12:
                    case (int)Keys.Oemtilde:            // `~
                    case (int)Keys.OemMinus:            // -_
                    case (int)Keys.Oemplus:             // =+
                    case (int)Keys.OemOpenBrackets:     // [{
                    case (int)Keys.OemCloseBrackets:    // ]}
                    case (int)Keys.Oem5:                // \|
                    case (int)Keys.Oem1:                // ;:
                    case (int)Keys.Oem7:                // '"
                    case (int)Keys.Oemcomma:            // ,<
                    case (int)Keys.OemPeriod:           // .>
                    case (int)Keys.OemQuestion:         // /?
                    case (int)Keys.Divide:              // /
                    case (int)Keys.Multiply:            // *
                    case (int)Keys.Subtract:            // -
                    case (int)Keys.Add:                 // +
                    case (int)Keys.Decimal:             // ·
                    case (int)Keys.Tab:
                    case (int)Keys.CapsLock:
                    case (int)Keys.NumLock:
                    case (int)Keys.Space:
                    case (int)Keys.Enter:
                    case (int)Keys.Insert:
                    case (int)Keys.Delete:
                    case (int)Keys.Home:
                    case (int)Keys.End:
                    case (int)Keys.PageUp:
                    case (int)Keys.PageDown:
                    case (int)Keys.Left:
                    case (int)Keys.Right:
                    case (int)Keys.Up:
                    case (int)Keys.Down:
                    case (int)Keys.NumPad0:
                    case (int)Keys.NumPad1:
                    case (int)Keys.NumPad2:
                    case (int)Keys.NumPad3:
                    case (int)Keys.NumPad4:
                    case (int)Keys.NumPad5:
                    case (int)Keys.NumPad6:
                    case (int)Keys.NumPad7:
                    case (int)Keys.NumPad8:
                    case (int)Keys.NumPad9:
                    case (int)Keys.D1:
                    case (int)Keys.D2:
                    case (int)Keys.D3:
                    case (int)Keys.D4:
                    case (int)Keys.D5:
                    case (int)Keys.D6:
                    case (int)Keys.D7:
                    case (int)Keys.D8:
                    case (int)Keys.D9:
                    case (int)Keys.D0:
                    case (int)Keys.A:
                    case (int)Keys.B:
                    case (int)Keys.C:
                    case (int)Keys.D:
                    case (int)Keys.E:
                    case (int)Keys.F:
                    case (int)Keys.G:
                    case (int)Keys.H:
                    case (int)Keys.I:
                    case (int)Keys.J:
                    case (int)Keys.K:
                    case (int)Keys.L:
                    case (int)Keys.M:
                    case (int)Keys.N:
                    case (int)Keys.O:
                    case (int)Keys.P:
                    case (int)Keys.Q:
                    case (int)Keys.R:
                    case (int)Keys.S:
                    case (int)Keys.T:
                    case (int)Keys.U:
                    case (int)Keys.V:
                    case (int)Keys.W:
                    case (int)Keys.X:
                    case (int)Keys.Y:
                    case (int)Keys.Z:
                        {
                            mKey = vkCode;
                            var kc = new KeysConverter();
                            string keyString = this.getSubKeyString();
                            keyString = keyString + this.getKeyString((Keys)mKey);
                            textBox.Text = keyString;

                            this.saveData(textBox);

                            textBox.Enabled = false;
                            textBox.Enabled = true;
                            //Console.WriteLine(keyString);
                        }
                        break;

                    default:
                        return 1;
                }
                //Console.WriteLine("keydown : " + vkCode + ", " + keyString);
            }

            else if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
            {
                TextBox textBox = null;
                if (mEnableFanControlTextBox.Focused == true)
                    textBox = mEnableFanControlTextBox;
                else if (mModeNormalTextBox.Focused == true)
                    textBox = mModeNormalTextBox;
                else if (mModeSilenceTextBox.Focused == true)
                    textBox = mModeSilenceTextBox;
                else if (mModePerformanceTextBox.Focused == true)
                    textBox = mModePerformanceTextBox;
                else if (mModeGameTextBox.Focused == true)
                    textBox = mModeGameTextBox;
                else if (mEnableOSDTextBox.Focused == true)
                    textBox = mEnableOSDTextBox;

                if (code < 0)
                    return 0;

                int vkCode = Marshal.ReadInt32(lParam);

                switch (vkCode)
                {
                    // CTRL
                    case (int)Keys.LControlKey:
                        mIsCtrl = false;
                        if (textBox != null)
                            textBox.Text = this.getSubKeyString();
                        break;

                    // LShift
                    case (int)Keys.LShiftKey:
                        mIsLShift = false;
                        if (textBox != null)
                            textBox.Text = this.getSubKeyString();
                        break;

                    // RShift
                    case (int)Keys.RShiftKey:
                        mIsRShift = false;
                        if (textBox != null)
                            textBox.Text = this.getSubKeyString();
                        break;

                    // Alt
                    case (int)Keys.LMenu:
                        mIsAlt = false;
                        if (textBox != null)
                            textBox.Text = this.getSubKeyString();
                        break;

                    default:
                        break;
                }
            }

            return 0;
        }

        private void onTextBoxGotFocus(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Text = "";
        }

        private void onTextBoxLeave(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            if (mKey == (int)Keys.None)
            {
                textBox.Text = "NONE";
            }

            mIsCtrl = false;
            mIsAlt = false;
            mIsLShift = false;
            mIsRShift = false;
            mKey = (int)Keys.None;
        }

        private void saveData(TextBox textBox)
        {
            if (textBox == mEnableFanControlTextBox)
            {
                HotkeyManager.getInstance().mEnableFanControlData.mIsCtrl = mIsCtrl;
                HotkeyManager.getInstance().mEnableFanControlData.mIsAlt = mIsAlt;
                HotkeyManager.getInstance().mEnableFanControlData.mIsLShift = mIsLShift;
                HotkeyManager.getInstance().mEnableFanControlData.mIsRShift = mIsRShift;
                HotkeyManager.getInstance().mEnableFanControlData.mKey = mKey;
                HotkeyManager.getInstance().write();
            }

            else if (textBox == mModeNormalTextBox)
            {
                HotkeyManager.getInstance().mModeNormalData.mIsCtrl = mIsCtrl;
                HotkeyManager.getInstance().mModeNormalData.mIsAlt = mIsAlt;
                HotkeyManager.getInstance().mModeNormalData.mIsLShift = mIsLShift;
                HotkeyManager.getInstance().mModeNormalData.mIsRShift = mIsRShift;
                HotkeyManager.getInstance().mModeNormalData.mKey = mKey;
                HotkeyManager.getInstance().write();
            }

            else if (textBox == mModeSilenceTextBox)
            {
                HotkeyManager.getInstance().mModeSilenceData.mIsCtrl = mIsCtrl;
                HotkeyManager.getInstance().mModeSilenceData.mIsAlt = mIsAlt;
                HotkeyManager.getInstance().mModeSilenceData.mIsLShift = mIsLShift;
                HotkeyManager.getInstance().mModeSilenceData.mIsRShift = mIsRShift;
                HotkeyManager.getInstance().mModeSilenceData.mKey = mKey;
                HotkeyManager.getInstance().write();
            }

            else if (textBox == mModePerformanceTextBox)
            {
                HotkeyManager.getInstance().mModePerformanceData.mIsCtrl = mIsCtrl;
                HotkeyManager.getInstance().mModePerformanceData.mIsAlt = mIsAlt;
                HotkeyManager.getInstance().mModePerformanceData.mIsLShift = mIsLShift;
                HotkeyManager.getInstance().mModePerformanceData.mIsRShift = mIsRShift;
                HotkeyManager.getInstance().mModePerformanceData.mKey = mKey;
                HotkeyManager.getInstance().write();
            }

            else if (textBox == mModeGameTextBox)
            {
                HotkeyManager.getInstance().mModeGameData.mIsCtrl = mIsCtrl;
                HotkeyManager.getInstance().mModeGameData.mIsAlt = mIsAlt;
                HotkeyManager.getInstance().mModeGameData.mIsLShift = mIsLShift;
                HotkeyManager.getInstance().mModeGameData.mIsRShift = mIsRShift;
                HotkeyManager.getInstance().mModeGameData.mKey = mKey;
                HotkeyManager.getInstance().write();
            }

            else if (textBox == mEnableOSDTextBox)
            {
                HotkeyManager.getInstance().mEnableOSDData.mIsCtrl = mIsCtrl;
                HotkeyManager.getInstance().mEnableOSDData.mIsAlt = mIsAlt;
                HotkeyManager.getInstance().mEnableOSDData.mIsLShift = mIsLShift;
                HotkeyManager.getInstance().mEnableOSDData.mIsRShift = mIsRShift;
                HotkeyManager.getInstance().mEnableOSDData.mKey = mKey;
                HotkeyManager.getInstance().write();
            }
        }
    }
}
