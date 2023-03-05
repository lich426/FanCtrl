using DarkUI.Controls;
using System.Windows.Forms;

namespace FanCtrl
{
    partial class LiquidctlFanControlForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiquidctlFanControlForm));
            this.mDeviceGroupBox = new DarkGroupBox();
            this.mAddressTextBox = new DarkTextBox();
            this.label1 = new DarkLabel();
            this.mDeviceComboBox = new DarkComboBox();
            this.groupBox1 = new DarkGroupBox();
            this.mChannelTextBox = new DarkTextBox();
            this.mOKButton = new DarkButton();
            this.mDeviceGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mDeviceGroupBox
            // 
            this.mDeviceGroupBox.Controls.Add(this.mAddressTextBox);
            this.mDeviceGroupBox.Controls.Add(this.label1);
            this.mDeviceGroupBox.Controls.Add(this.mDeviceComboBox);
            this.mDeviceGroupBox.Location = new System.Drawing.Point(13, 13);
            this.mDeviceGroupBox.Name = "mDeviceGroupBox";
            this.mDeviceGroupBox.Size = new System.Drawing.Size(441, 98);
            this.mDeviceGroupBox.TabIndex = 0;
            this.mDeviceGroupBox.TabStop = false;
            this.mDeviceGroupBox.Text = "Device select";
            // 
            // mAddressTextBox
            // 
            this.mAddressTextBox.Location = new System.Drawing.Point(74, 58);
            this.mAddressTextBox.Name = "mAddressTextBox";
            this.mAddressTextBox.ReadOnly = true;
            this.mAddressTextBox.Size = new System.Drawing.Size(361, 21);
            this.mAddressTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Address :";
            // 
            // mDeviceComboBox
            // 
            this.mDeviceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.mDeviceComboBox.FormattingEnabled = true;
            this.mDeviceComboBox.Location = new System.Drawing.Point(7, 21);
            this.mDeviceComboBox.Name = "mDeviceComboBox";
            this.mDeviceComboBox.Size = new System.Drawing.Size(428, 20);
            this.mDeviceComboBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mChannelTextBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(441, 65);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channel name";
            // 
            // mChannelTextBox
            // 
            this.mChannelTextBox.Location = new System.Drawing.Point(7, 21);
            this.mChannelTextBox.Name = "mChannelTextBox";
            this.mChannelTextBox.Size = new System.Drawing.Size(428, 21);
            this.mChannelTextBox.TabIndex = 0;
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(303, 190);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(151, 44);
            this.mOKButton.TabIndex = 2;
            this.mOKButton.Text = "button1";
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // LiquidctlFanControlForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(466, 244);
            this.Controls.Add(this.mOKButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mDeviceGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LiquidctlFanControlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "liquidctl Fan Control";
            this.mDeviceGroupBox.ResumeLayout(false);
            this.mDeviceGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkGroupBox mDeviceGroupBox;
        private DarkTextBox mAddressTextBox;
        private DarkLabel label1;
        private DarkComboBox mDeviceComboBox;
        private DarkGroupBox groupBox1;
        private DarkTextBox mChannelTextBox;
        private DarkButton mOKButton;
    }
}