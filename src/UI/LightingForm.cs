using DarkUI.Controls;
using FanCtrl.Resources;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FanCtrl
{
    public partial class LightingForm : ThemeForm
    {
        private USBDevice mUSBDevice = null;

        private bool mIsCancel = false;

        private List<TextBox> mHexTextBoxList = new List<TextBox>();
        private List<Button> mRemoveButtonList = new List<Button>();

        public LightingForm(USBDevice device, int num)
        {
            mUSBDevice = device;
            InitializeComponent();
            this.localizeComponent(num);            

            var customList = mUSBDevice.getCustomDataList();
            if (customList.Count == 0)
            {
                this.onAddButtonClick(null, EventArgs.Empty);
            }
            else
            {
                for(int i = 0; i < customList.Count; i++)
                {
                    this.onAddButtonClick(null, EventArgs.Empty);
                    mHexTextBoxList[i].Text = customList[i];
                }
            }
        }
        
        private void localizeComponent(int num)
        {
            if (mUSBDevice.DeviceType == USBDeviceType.Kraken)
            {
                this.Text = StringLib.Kraken_Lighting + string.Format(" ({0})", num);
            }
            else if (mUSBDevice.DeviceType == USBDeviceType.CLC)
            {
                this.Text = StringLib.CLC_Lighting + string.Format(" ({0})", num);
            }
            else if (mUSBDevice.DeviceType == USBDeviceType.RGBnFC)
            {
                this.Text = StringLib.RGBnFC_Lighting + string.Format(" ({0})", num);
            }

            mAddButton.Text = StringLib.Add;
            mApplyButton.Text = StringLib.Apply;
            mOKButton.Text = StringLib.OK;
        }

        private void onTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control || e.Shift || e.Alt || e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete) {}
            else if (Util.isHex((char)e.KeyCode) == false)
            {
                mIsCancel = true;
            }
        }

        private void onTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if(mIsCancel == true)
            {
                e.Handled = true;
                mIsCancel = false;
            }
        }

        private void onAddButtonClick(object sender, EventArgs e)
        {
            var textBox = new DarkTextBox();
            textBox.Parent = mHexDataGroupBox;
            textBox.Name = "mHexTextBox" + mHexTextBoxList.Count;
            textBox.Size = new System.Drawing.Size(310, 21);
            textBox.TabIndex = 0;
            textBox.KeyDown += onTextBoxKeyDown;
            textBox.KeyPress += onTextBoxKeyPress;

            var removeButton = new DarkButton();
            removeButton.Parent = mHexDataGroupBox;
            removeButton.Name = "mRemoveButton" + mRemoveButtonList.Count;
            removeButton.Text = StringLib.Remove;
            removeButton.Size = new System.Drawing.Size(63, 21);
            removeButton.TabIndex = 1;
            removeButton.Click += onRemoveButtonClick;

            int plusHeight = 0;
            if (mHexTextBoxList.Count == 0)
            {
                textBox.Location = new System.Drawing.Point(6, 29);
                removeButton.Location = new System.Drawing.Point(textBox.Location.X + textBox.Width + 3, 29);
            }
            else
            {
                plusHeight = 30;
                textBox.Location = new System.Drawing.Point(6, 29 + (plusHeight * mHexTextBoxList.Count));
                removeButton.Location = new System.Drawing.Point(textBox.Location.X + textBox.Width + 3, 29 + (plusHeight * mRemoveButtonList.Count));
            }

            mHexDataGroupBox.Height = mHexDataGroupBox.Height + plusHeight;
            mApplyButton.Top = mApplyButton.Top + plusHeight;
            mOKButton.Top = mOKButton.Top + plusHeight;
            this.Height = this.Height + plusHeight;

            mHexTextBoxList.Add(textBox);
            mRemoveButtonList.Add(removeButton);
        }

        private void onRemoveButtonClick(object sender, EventArgs e)
        {
            if (mRemoveButtonList.Count == 1)
            {
                if(mHexTextBoxList.Count > 0)
                {
                    mHexTextBoxList[0].Text = "";
                }                
                return;
            }

            for(int i = 0; i < mRemoveButtonList.Count; i++)
            {
                if(sender == mRemoveButtonList[i])
                {
                    int minusHeight = 30;
                    for(int j = i + 1; j < mRemoveButtonList.Count; j++)
                    {
                        mHexTextBoxList[j].Top = mHexTextBoxList[j].Top - minusHeight;
                        mRemoveButtonList[j].Top = mRemoveButtonList[j].Top - minusHeight;
                    }

                    mHexDataGroupBox.Height = mHexDataGroupBox.Height - minusHeight;
                    mApplyButton.Top = mApplyButton.Top - minusHeight;
                    mOKButton.Top = mOKButton.Top - minusHeight;
                    this.Height = this.Height - minusHeight;

                    mHexTextBoxList[i].Dispose();
                    mHexTextBoxList.RemoveAt(i);

                    mRemoveButtonList[i].Dispose();
                    mRemoveButtonList.RemoveAt(i);

                    break;
                }
            }
        }

        private void onApplyButtonClick(object sender, EventArgs e)
        {
            var hexStringList = new List<string>();
            for(int i = 0; i < mHexTextBoxList.Count; i++)
            {
                if (mHexTextBoxList[i].Text.Length == 0)
                    continue;

                var text = mHexTextBoxList[i].Text;
                text = text.Replace(" ", "");
                if (text.Length % 2 != 0)
                {
                    text = text + "0";
                }
                hexStringList.Add(text);
            }

            mUSBDevice.setCustomDataList(hexStringList);
            mUSBDevice.writeFile();           
        }

        private void onOKButtonClick(object sender, EventArgs e)
        {
            this.onApplyButtonClick(sender, e);
            this.Close();
        }

        
    }
}
