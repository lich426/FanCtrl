using NZXTSharp.KrakenX;
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
    public partial class KrakenForm : Form
    {
        private KrakenX mKrakenX = null;
        private bool mIsCancel = false;

        private List<TextBox> mHexTextBoxList = new List<TextBox>();
        private List<Button> mRemoveButtonList = new List<Button>();

        public event EventHandler onCloseCallback;

        public KrakenForm(KrakenX krakenX)
        {
            InitializeComponent();
            this.localizeComponent();

            this.FormClosed += (sender, e) =>
            {
                onCloseCallback(sender, EventArgs.Empty);
            };

            mKrakenX = krakenX;

            var customList = mKrakenX.getCustomDataList();
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

        private void localizeComponent()
        {
            this.Text = StringLib.Kraken_Setting;
            mAddButton.Text = StringLib.Add;
            mApplyButton.Text = StringLib.Apply;
            mOKButton.Text = StringLib.OK;
        }

        private void onTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control || e.Shift || e.Alt || e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete) {}
            else if (this.isHex((char)e.KeyCode) == false)
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

        private bool isHex(char value)
        {
            if ((value >= 48 && value <= 57) ||
                (value >= 65 && value <= 70) ||
                (value >= 97 && value <= 102))
            {
                return true;
            }
            return false;
        }

        private void onAddButtonClick(object sender, EventArgs e)
        {
            var textBox = new TextBox();
            textBox.Parent = mHexDataGroupBox;
            textBox.Name = "mHexTextBox" + mHexTextBoxList.Count;
            textBox.Size = new System.Drawing.Size(310, 21);
            textBox.TabIndex = 0;
            textBox.KeyDown += onTextBoxKeyDown;
            textBox.KeyPress += onTextBoxKeyPress;

            var removeButton = new Button();
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
                return;

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
                if(text.Length % 2 != 0)
                {
                    text = text + "0";
                }
                hexStringList.Add(text);
            }

            if(hexStringList.Count > 0)
            {
                mKrakenX.setCustomDataList(hexStringList);
                mKrakenX.write();
            }            
        }

        private void onOKButtonClick(object sender, EventArgs e)
        {
            this.onApplyButtonClick(sender, e);
            this.Close();
        }

        
    }
}
