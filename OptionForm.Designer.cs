namespace FanControl
{
    partial class OptionForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionForm));
            this.mIntervalGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mIntervalTextBox = new System.Windows.Forms.TextBox();
            this.mMinimizeCheckBox = new System.Windows.Forms.CheckBox();
            this.mStartupCheckBox = new System.Windows.Forms.CheckBox();
            this.mOKButton = new System.Windows.Forms.Button();
            this.mToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mLibraryGroupBox = new System.Windows.Forms.GroupBox();
            this.mDimmCheckBox = new System.Windows.Forms.CheckBox();
            this.mCLCButton = new System.Windows.Forms.Button();
            this.mKrakenButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mGigabyteCheckBox = new System.Windows.Forms.CheckBox();
            this.mLibraryRadioButton1 = new System.Windows.Forms.RadioButton();
            this.mLibraryRadioButton2 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.mCLCCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mNvApiCheckBox = new System.Windows.Forms.CheckBox();
            this.mKrakenCheckBox = new System.Windows.Forms.CheckBox();
            this.mAnimationCheckBox = new System.Windows.Forms.CheckBox();
            this.mFahrenheitCheckBox = new System.Windows.Forms.CheckBox();
            this.mIntervalGroupBox.SuspendLayout();
            this.mLibraryGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mIntervalGroupBox
            // 
            this.mIntervalGroupBox.Controls.Add(this.label1);
            this.mIntervalGroupBox.Controls.Add(this.mIntervalTextBox);
            this.mIntervalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.mIntervalGroupBox.Name = "mIntervalGroupBox";
            this.mIntervalGroupBox.Size = new System.Drawing.Size(188, 58);
            this.mIntervalGroupBox.TabIndex = 0;
            this.mIntervalGroupBox.TabStop = false;
            this.mIntervalGroupBox.Text = "Interval";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(159, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "ms";
            // 
            // mIntervalTextBox
            // 
            this.mIntervalTextBox.Location = new System.Drawing.Point(13, 21);
            this.mIntervalTextBox.MaxLength = 4;
            this.mIntervalTextBox.Name = "mIntervalTextBox";
            this.mIntervalTextBox.Size = new System.Drawing.Size(142, 21);
            this.mIntervalTextBox.TabIndex = 0;
            // 
            // mMinimizeCheckBox
            // 
            this.mMinimizeCheckBox.AutoSize = true;
            this.mMinimizeCheckBox.Location = new System.Drawing.Point(19, 360);
            this.mMinimizeCheckBox.Name = "mMinimizeCheckBox";
            this.mMinimizeCheckBox.Size = new System.Drawing.Size(112, 16);
            this.mMinimizeCheckBox.TabIndex = 12;
            this.mMinimizeCheckBox.Text = "Start minimized";
            this.mMinimizeCheckBox.UseVisualStyleBackColor = true;
            // 
            // mStartupCheckBox
            // 
            this.mStartupCheckBox.AutoSize = true;
            this.mStartupCheckBox.Location = new System.Drawing.Point(19, 385);
            this.mStartupCheckBox.Name = "mStartupCheckBox";
            this.mStartupCheckBox.Size = new System.Drawing.Size(131, 16);
            this.mStartupCheckBox.TabIndex = 13;
            this.mStartupCheckBox.Text = "Start with Windows";
            this.mStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(11, 411);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(188, 38);
            this.mOKButton.TabIndex = 14;
            this.mOKButton.Text = "OK";
            this.mOKButton.UseVisualStyleBackColor = true;
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // mToolTip
            // 
            this.mToolTip.IsBalloon = true;
            // 
            // mLibraryGroupBox
            // 
            this.mLibraryGroupBox.Controls.Add(this.mDimmCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mCLCButton);
            this.mLibraryGroupBox.Controls.Add(this.mKrakenButton);
            this.mLibraryGroupBox.Controls.Add(this.panel1);
            this.mLibraryGroupBox.Controls.Add(this.label3);
            this.mLibraryGroupBox.Controls.Add(this.mCLCCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.label2);
            this.mLibraryGroupBox.Controls.Add(this.mNvApiCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mKrakenCheckBox);
            this.mLibraryGroupBox.Location = new System.Drawing.Point(12, 77);
            this.mLibraryGroupBox.Name = "mLibraryGroupBox";
            this.mLibraryGroupBox.Size = new System.Drawing.Size(188, 221);
            this.mLibraryGroupBox.TabIndex = 5;
            this.mLibraryGroupBox.TabStop = false;
            this.mLibraryGroupBox.Text = "Library";
            // 
            // mDimmCheckBox
            // 
            this.mDimmCheckBox.AutoSize = true;
            this.mDimmCheckBox.Location = new System.Drawing.Point(8, 129);
            this.mDimmCheckBox.Name = "mDimmCheckBox";
            this.mDimmCheckBox.Size = new System.Drawing.Size(100, 16);
            this.mDimmCheckBox.TabIndex = 5;
            this.mDimmCheckBox.Text = "DIMM sensor";
            this.mDimmCheckBox.UseVisualStyleBackColor = true;
            // 
            // mCLCButton
            // 
            this.mCLCButton.Location = new System.Drawing.Point(116, 188);
            this.mCLCButton.Name = "mCLCButton";
            this.mCLCButton.Size = new System.Drawing.Size(64, 23);
            this.mCLCButton.TabIndex = 9;
            this.mCLCButton.Text = "Lighting";
            this.mCLCButton.UseVisualStyleBackColor = true;
            this.mCLCButton.Click += new System.EventHandler(this.onCLCButtonClick);
            // 
            // mKrakenButton
            // 
            this.mKrakenButton.Location = new System.Drawing.Point(116, 162);
            this.mKrakenButton.Name = "mKrakenButton";
            this.mKrakenButton.Size = new System.Drawing.Size(64, 23);
            this.mKrakenButton.TabIndex = 7;
            this.mKrakenButton.Text = "Lighting";
            this.mKrakenButton.UseVisualStyleBackColor = true;
            this.mKrakenButton.Click += new System.EventHandler(this.onKrakenButtonClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mGigabyteCheckBox);
            this.panel1.Controls.Add(this.mLibraryRadioButton1);
            this.panel1.Controls.Add(this.mLibraryRadioButton2);
            this.panel1.Location = new System.Drawing.Point(5, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(177, 66);
            this.panel1.TabIndex = 6;
            // 
            // mGigabyteCheckBox
            // 
            this.mGigabyteCheckBox.AutoSize = true;
            this.mGigabyteCheckBox.Location = new System.Drawing.Point(3, 3);
            this.mGigabyteCheckBox.Name = "mGigabyteCheckBox";
            this.mGigabyteCheckBox.Size = new System.Drawing.Size(174, 16);
            this.mGigabyteCheckBox.TabIndex = 1;
            this.mGigabyteCheckBox.Text = "Gigabyte (with AppCenter)";
            this.mGigabyteCheckBox.UseVisualStyleBackColor = true;
            // 
            // mLibraryRadioButton1
            // 
            this.mLibraryRadioButton1.AutoSize = true;
            this.mLibraryRadioButton1.Location = new System.Drawing.Point(3, 26);
            this.mLibraryRadioButton1.Name = "mLibraryRadioButton1";
            this.mLibraryRadioButton1.Size = new System.Drawing.Size(147, 16);
            this.mLibraryRadioButton1.TabIndex = 2;
            this.mLibraryRadioButton1.TabStop = true;
            this.mLibraryRadioButton1.Text = "LibreHardwareMonitor";
            this.mLibraryRadioButton1.UseVisualStyleBackColor = true;
            // 
            // mLibraryRadioButton2
            // 
            this.mLibraryRadioButton2.AutoSize = true;
            this.mLibraryRadioButton2.Location = new System.Drawing.Point(3, 48);
            this.mLibraryRadioButton2.Name = "mLibraryRadioButton2";
            this.mLibraryRadioButton2.Size = new System.Drawing.Size(149, 16);
            this.mLibraryRadioButton2.TabIndex = 3;
            this.mLibraryRadioButton2.TabStop = true;
            this.mLibraryRadioButton2.Text = "OpenHardwareMonitor";
            this.mLibraryRadioButton2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "-----------------------------";
            // 
            // mCLCCheckBox
            // 
            this.mCLCCheckBox.AutoSize = true;
            this.mCLCCheckBox.Location = new System.Drawing.Point(8, 193);
            this.mCLCCheckBox.Name = "mCLCCheckBox";
            this.mCLCCheckBox.Size = new System.Drawing.Size(86, 16);
            this.mCLCCheckBox.TabIndex = 8;
            this.mCLCCheckBox.Text = "EVGA CLC";
            this.mCLCCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "-----------------------------";
            // 
            // mNvApiCheckBox
            // 
            this.mNvApiCheckBox.AutoSize = true;
            this.mNvApiCheckBox.Location = new System.Drawing.Point(8, 105);
            this.mNvApiCheckBox.Name = "mNvApiCheckBox";
            this.mNvApiCheckBox.Size = new System.Drawing.Size(104, 16);
            this.mNvApiCheckBox.TabIndex = 4;
            this.mNvApiCheckBox.Text = "NvAPIWrapper";
            this.mNvApiCheckBox.UseVisualStyleBackColor = true;
            // 
            // mKrakenCheckBox
            // 
            this.mKrakenCheckBox.AutoSize = true;
            this.mKrakenCheckBox.Location = new System.Drawing.Point(8, 167);
            this.mKrakenCheckBox.Name = "mKrakenCheckBox";
            this.mKrakenCheckBox.Size = new System.Drawing.Size(100, 16);
            this.mKrakenCheckBox.TabIndex = 6;
            this.mKrakenCheckBox.Text = "NZXT Kraken";
            this.mKrakenCheckBox.UseVisualStyleBackColor = true;
            // 
            // mAnimationCheckBox
            // 
            this.mAnimationCheckBox.AutoSize = true;
            this.mAnimationCheckBox.Location = new System.Drawing.Point(19, 310);
            this.mAnimationCheckBox.Name = "mAnimationCheckBox";
            this.mAnimationCheckBox.Size = new System.Drawing.Size(137, 16);
            this.mAnimationCheckBox.TabIndex = 10;
            this.mAnimationCheckBox.Text = "Tray Icon animation";
            this.mAnimationCheckBox.UseVisualStyleBackColor = true;
            // 
            // mFahrenheitCheckBox
            // 
            this.mFahrenheitCheckBox.AutoSize = true;
            this.mFahrenheitCheckBox.Location = new System.Drawing.Point(19, 335);
            this.mFahrenheitCheckBox.Name = "mFahrenheitCheckBox";
            this.mFahrenheitCheckBox.Size = new System.Drawing.Size(108, 16);
            this.mFahrenheitCheckBox.TabIndex = 11;
            this.mFahrenheitCheckBox.Text = "Fahrenheit (°F)";
            this.mFahrenheitCheckBox.UseVisualStyleBackColor = true;
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(212, 459);
            this.Controls.Add(this.mFahrenheitCheckBox);
            this.Controls.Add(this.mAnimationCheckBox);
            this.Controls.Add(this.mLibraryGroupBox);
            this.Controls.Add(this.mOKButton);
            this.Controls.Add(this.mStartupCheckBox);
            this.Controls.Add(this.mMinimizeCheckBox);
            this.Controls.Add(this.mIntervalGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Option";
            this.mIntervalGroupBox.ResumeLayout(false);
            this.mIntervalGroupBox.PerformLayout();
            this.mLibraryGroupBox.ResumeLayout(false);
            this.mLibraryGroupBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox mIntervalGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox mIntervalTextBox;
        private System.Windows.Forms.CheckBox mMinimizeCheckBox;
        private System.Windows.Forms.CheckBox mStartupCheckBox;
        private System.Windows.Forms.Button mOKButton;
        private System.Windows.Forms.ToolTip mToolTip;
        private System.Windows.Forms.GroupBox mLibraryGroupBox;
        private System.Windows.Forms.RadioButton mLibraryRadioButton2;
        private System.Windows.Forms.RadioButton mLibraryRadioButton1;
        private System.Windows.Forms.CheckBox mNvApiCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox mKrakenCheckBox;
        private System.Windows.Forms.Button mCLCButton;
        private System.Windows.Forms.Button mKrakenButton;
        private System.Windows.Forms.CheckBox mCLCCheckBox;
        private System.Windows.Forms.CheckBox mGigabyteCheckBox;
        private System.Windows.Forms.CheckBox mAnimationCheckBox;
        private System.Windows.Forms.CheckBox mDimmCheckBox;
        private System.Windows.Forms.CheckBox mFahrenheitCheckBox;
    }
}