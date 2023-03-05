
using DarkUI.Controls;

namespace FanCtrl
{
    partial class HotkeyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HotkeyForm));
            this.mFanControlGroupBox = new DarkGroupBox();
            this.mModeGameTextBox = new DarkTextBox();
            this.mModeGameLabel = new DarkTitle();
            this.mModePerformanceTextBox = new DarkTextBox();
            this.mModePerformanceLabel = new DarkTitle();
            this.mModeSilenceTextBox = new DarkTextBox();
            this.mModeSilenceLabel = new DarkTitle();
            this.mModeNormalTextBox = new DarkTextBox();
            this.mModeNormalLabel = new DarkTitle();
            this.mEnableFanControlTextBox = new DarkTextBox();
            this.mEnableFanControlLabel = new DarkTitle();
            this.mOSDGroupBox = new DarkGroupBox();
            this.mEnableOSDTextBox = new DarkTextBox();
            this.mEnableOSDLabel = new DarkTitle();
            this.mFanControlGroupBox.SuspendLayout();
            this.mOSDGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mFanControlGroupBox
            // 
            this.mFanControlGroupBox.Controls.Add(this.mModeGameTextBox);
            this.mFanControlGroupBox.Controls.Add(this.mModeGameLabel);
            this.mFanControlGroupBox.Controls.Add(this.mModePerformanceTextBox);
            this.mFanControlGroupBox.Controls.Add(this.mModePerformanceLabel);
            this.mFanControlGroupBox.Controls.Add(this.mModeSilenceTextBox);
            this.mFanControlGroupBox.Controls.Add(this.mModeSilenceLabel);
            this.mFanControlGroupBox.Controls.Add(this.mModeNormalTextBox);
            this.mFanControlGroupBox.Controls.Add(this.mModeNormalLabel);
            this.mFanControlGroupBox.Controls.Add(this.mEnableFanControlTextBox);
            this.mFanControlGroupBox.Controls.Add(this.mEnableFanControlLabel);
            this.mFanControlGroupBox.Location = new System.Drawing.Point(13, 13);
            this.mFanControlGroupBox.Name = "mFanControlGroupBox";
            this.mFanControlGroupBox.Size = new System.Drawing.Size(514, 170);
            this.mFanControlGroupBox.TabIndex = 0;
            this.mFanControlGroupBox.TabStop = false;
            this.mFanControlGroupBox.Text = "Fan Control";
            // 
            // mModeGameTextBox
            // 
            this.mModeGameTextBox.Location = new System.Drawing.Point(190, 138);
            this.mModeGameTextBox.Name = "mModeGameTextBox";
            this.mModeGameTextBox.ReadOnly = true;
            this.mModeGameTextBox.Size = new System.Drawing.Size(318, 21);
            this.mModeGameTextBox.TabIndex = 5;
            this.mModeGameTextBox.TabStop = false;
            // 
            // mModeGameLabel
            // 
            this.mModeGameLabel.AutoSize = true;
            this.mModeGameLabel.Location = new System.Drawing.Point(9, 144);
            this.mModeGameLabel.Name = "mModeGameLabel";
            this.mModeGameLabel.Size = new System.Drawing.Size(93, 12);
            this.mModeGameLabel.TabIndex = 8;
            this.mModeGameLabel.Text = "Mode - Game :";
            // 
            // mModePerformanceTextBox
            // 
            this.mModePerformanceTextBox.Location = new System.Drawing.Point(190, 107);
            this.mModePerformanceTextBox.Name = "mModePerformanceTextBox";
            this.mModePerformanceTextBox.ReadOnly = true;
            this.mModePerformanceTextBox.Size = new System.Drawing.Size(318, 21);
            this.mModePerformanceTextBox.TabIndex = 4;
            this.mModePerformanceTextBox.TabStop = false;
            // 
            // mModePerformanceLabel
            // 
            this.mModePerformanceLabel.AutoSize = true;
            this.mModePerformanceLabel.Location = new System.Drawing.Point(9, 113);
            this.mModePerformanceLabel.Name = "mModePerformanceLabel";
            this.mModePerformanceLabel.Size = new System.Drawing.Size(131, 12);
            this.mModePerformanceLabel.TabIndex = 6;
            this.mModePerformanceLabel.Text = "Mode - Performance :";
            // 
            // mModeSilenceTextBox
            // 
            this.mModeSilenceTextBox.Location = new System.Drawing.Point(190, 76);
            this.mModeSilenceTextBox.Name = "mModeSilenceTextBox";
            this.mModeSilenceTextBox.ReadOnly = true;
            this.mModeSilenceTextBox.Size = new System.Drawing.Size(318, 21);
            this.mModeSilenceTextBox.TabIndex = 3;
            this.mModeSilenceTextBox.TabStop = false;
            // 
            // mModeSilenceLabel
            // 
            this.mModeSilenceLabel.AutoSize = true;
            this.mModeSilenceLabel.Location = new System.Drawing.Point(9, 82);
            this.mModeSilenceLabel.Name = "mModeSilenceLabel";
            this.mModeSilenceLabel.Size = new System.Drawing.Size(101, 12);
            this.mModeSilenceLabel.TabIndex = 4;
            this.mModeSilenceLabel.Text = "Mode - Silence :";
            // 
            // mModeNormalTextBox
            // 
            this.mModeNormalTextBox.Location = new System.Drawing.Point(190, 47);
            this.mModeNormalTextBox.Name = "mModeNormalTextBox";
            this.mModeNormalTextBox.ReadOnly = true;
            this.mModeNormalTextBox.Size = new System.Drawing.Size(318, 21);
            this.mModeNormalTextBox.TabIndex = 2;
            this.mModeNormalTextBox.TabStop = false;
            // 
            // mModeNormalLabel
            // 
            this.mModeNormalLabel.AutoSize = true;
            this.mModeNormalLabel.Location = new System.Drawing.Point(9, 53);
            this.mModeNormalLabel.Name = "mModeNormalLabel";
            this.mModeNormalLabel.Size = new System.Drawing.Size(100, 12);
            this.mModeNormalLabel.TabIndex = 2;
            this.mModeNormalLabel.Text = "Mode - Normal :";
            // 
            // mEnableFanControlTextBox
            // 
            this.mEnableFanControlTextBox.Location = new System.Drawing.Point(190, 18);
            this.mEnableFanControlTextBox.Name = "mEnableFanControlTextBox";
            this.mEnableFanControlTextBox.ReadOnly = true;
            this.mEnableFanControlTextBox.Size = new System.Drawing.Size(318, 21);
            this.mEnableFanControlTextBox.TabIndex = 1;
            this.mEnableFanControlTextBox.TabStop = false;
            // 
            // mEnableFanControlLabel
            // 
            this.mEnableFanControlLabel.AutoSize = true;
            this.mEnableFanControlLabel.Location = new System.Drawing.Point(9, 23);
            this.mEnableFanControlLabel.Name = "mEnableFanControlLabel";
            this.mEnableFanControlLabel.Size = new System.Drawing.Size(174, 12);
            this.mEnableFanControlLabel.TabIndex = 0;
            this.mEnableFanControlLabel.Text = "Enable automatic fan control :";
            // 
            // mOSDGroupBox
            // 
            this.mOSDGroupBox.Controls.Add(this.mEnableOSDTextBox);
            this.mOSDGroupBox.Controls.Add(this.mEnableOSDLabel);
            this.mOSDGroupBox.Location = new System.Drawing.Point(13, 189);
            this.mOSDGroupBox.Name = "mOSDGroupBox";
            this.mOSDGroupBox.Size = new System.Drawing.Size(514, 50);
            this.mOSDGroupBox.TabIndex = 10;
            this.mOSDGroupBox.TabStop = false;
            this.mOSDGroupBox.Text = "OSD (RTSS)";
            // 
            // mEnableOSDTextBox
            // 
            this.mEnableOSDTextBox.Location = new System.Drawing.Point(190, 18);
            this.mEnableOSDTextBox.Name = "mEnableOSDTextBox";
            this.mEnableOSDTextBox.ReadOnly = true;
            this.mEnableOSDTextBox.Size = new System.Drawing.Size(318, 21);
            this.mEnableOSDTextBox.TabIndex = 6;
            this.mEnableOSDTextBox.TabStop = false;
            // 
            // mEnableOSDLabel
            // 
            this.mEnableOSDLabel.AutoSize = true;
            this.mEnableOSDLabel.Location = new System.Drawing.Point(9, 23);
            this.mEnableOSDLabel.Name = "mEnableOSDLabel";
            this.mEnableOSDLabel.Size = new System.Drawing.Size(127, 12);
            this.mEnableOSDLabel.TabIndex = 0;
            this.mEnableOSDLabel.Text = "Enable OSD (RTSS) :";
            // 
            // HotkeyForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(539, 251);
            this.Controls.Add(this.mOSDGroupBox);
            this.Controls.Add(this.mFanControlGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HotkeyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hotkey";
            this.mFanControlGroupBox.ResumeLayout(false);
            this.mFanControlGroupBox.PerformLayout();
            this.mOSDGroupBox.ResumeLayout(false);
            this.mOSDGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkGroupBox mFanControlGroupBox;
        private DarkTitle mEnableFanControlLabel;
        private DarkTextBox mModeNormalTextBox;
        private DarkTitle mModeNormalLabel;
        private DarkTextBox mEnableFanControlTextBox;
        private DarkTextBox mModeGameTextBox;
        private DarkTitle mModeGameLabel;
        private DarkTextBox mModePerformanceTextBox;
        private DarkTitle mModePerformanceLabel;
        private DarkTextBox mModeSilenceTextBox;
        private DarkTitle mModeSilenceLabel;
        private DarkGroupBox mOSDGroupBox;
        private DarkTextBox mEnableOSDTextBox;
        private DarkTitle mEnableOSDLabel;
    }
}