using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FanCtrl
{
    public partial class LiquidctlFanControlForm : Form
    {
        public string DeviceName { get; set; }

        public string Address { get; set; }

        public string ChannelName { get; set; }

        public LiquidctlFanControlForm(string address, string channel)
        {
            InitializeComponent();
            this.localizeComponent();

            mDeviceComboBox.SelectedIndexChanged += onComboBoxChange;

            int count = LiquidctlManager.getInstance().getLiquidctlDataCount();
            for (int i = 0; i < count; i++)
            {
                var data = LiquidctlManager.getInstance().getLiquidctlData(i);
                mDeviceComboBox.Items.Add(data.Description);
            }

            if (address.Length == 0 && count > 0)
            {
                mDeviceComboBox.SelectedIndex = 0;
            }
            else
            {
                bool isSelect = false;
                for (int i = 0; i < count; i++)
                {
                    var data = LiquidctlManager.getInstance().getLiquidctlData(i);
                    if (data.Address.CompareTo(address) == 0)
                    {
                        isSelect = true;
                        mDeviceComboBox.SelectedIndex = i;
                        break;
                    }
                }
                if (isSelect == false && count > 0)
                {
                    mDeviceComboBox.SelectedIndex = 0;
                }
            }
            if (channel.Length > 0)
            {
                mChannelTextBox.Text = channel;
                ChannelName = channel;
            }            
        }

        private void localizeComponent()
        {
            mOKButton.Text = StringLib.OK;
        }

        private void onComboBoxChange(object sender, EventArgs e)
        {
            if (mDeviceComboBox.SelectedIndex >= 0)
            {
                int count = LiquidctlManager.getInstance().getLiquidctlDataCount();
                if (mDeviceComboBox.SelectedIndex < count)
                {
                    var data = LiquidctlManager.getInstance().getLiquidctlData(mDeviceComboBox.SelectedIndex);
                    DeviceName = data.Description;
                    Address = data.Address;
                    mAddressTextBox.Text = data.Address;
                }
            }
        }

        private void onOKButtonClick(object sender, EventArgs e)
        {
            if (mDeviceComboBox.Items.Count == 0)
            {
                this.Close();
                return;
            }

            ChannelName = mChannelTextBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
