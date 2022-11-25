namespace FanCtrl
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
            this.mLiquidctlCheckBox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.mLHMMemoryCheckBox = new System.Windows.Forms.CheckBox();
            this.mHWInfoCheckBox = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mLHMStorageCheckBox = new System.Windows.Forms.CheckBox();
            this.mLHMControllerCheckBox = new System.Windows.Forms.CheckBox();
            this.mLHMGPUCheckBox = new System.Windows.Forms.CheckBox();
            this.mLHMMBCheckBox = new System.Windows.Forms.CheckBox();
            this.mLHMCPUCheckBox = new System.Windows.Forms.CheckBox();
            this.mLHMCheckBox = new System.Windows.Forms.CheckBox();
            this.mRGBnFCButton = new System.Windows.Forms.Button();
            this.mRGBnFCCheckBox = new System.Windows.Forms.CheckBox();
            this.mDimmCheckBox = new System.Windows.Forms.CheckBox();
            this.mCLCButton = new System.Windows.Forms.Button();
            this.mKrakenButton = new System.Windows.Forms.Button();
            this.mCLCCheckBox = new System.Windows.Forms.CheckBox();
            this.mNvApiCheckBox = new System.Windows.Forms.CheckBox();
            this.mKrakenCheckBox = new System.Windows.Forms.CheckBox();
            this.mAnimationCheckBox = new System.Windows.Forms.CheckBox();
            this.mFahrenheitCheckBox = new System.Windows.Forms.CheckBox();
            this.mStartupDelayLabel = new System.Windows.Forms.Label();
            this.mStartupDelayTextBox = new System.Windows.Forms.TextBox();
            this.mResetButton = new System.Windows.Forms.Button();
            this.mLanguageLabel = new System.Windows.Forms.Label();
            this.mLanguageComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mPluginCheckBox = new System.Windows.Forms.CheckBox();
            this.mIntervalGroupBox.SuspendLayout();
            this.mLibraryGroupBox.SuspendLayout();
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
            this.label1.Location = new System.Drawing.Point(162, 30);
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
            this.mIntervalTextBox.Size = new System.Drawing.Size(143, 21);
            this.mIntervalTextBox.TabIndex = 0;
            // 
            // mMinimizeCheckBox
            // 
            this.mMinimizeCheckBox.AutoSize = true;
            this.mMinimizeCheckBox.Location = new System.Drawing.Point(19, 645);
            this.mMinimizeCheckBox.Name = "mMinimizeCheckBox";
            this.mMinimizeCheckBox.Size = new System.Drawing.Size(112, 16);
            this.mMinimizeCheckBox.TabIndex = 33;
            this.mMinimizeCheckBox.Text = "Start minimized";
            this.mMinimizeCheckBox.UseVisualStyleBackColor = true;
            // 
            // mStartupCheckBox
            // 
            this.mStartupCheckBox.AutoSize = true;
            this.mStartupCheckBox.Location = new System.Drawing.Point(19, 670);
            this.mStartupCheckBox.Name = "mStartupCheckBox";
            this.mStartupCheckBox.Size = new System.Drawing.Size(131, 16);
            this.mStartupCheckBox.TabIndex = 34;
            this.mStartupCheckBox.Text = "Start with Windows";
            this.mStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(84, 721);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(116, 38);
            this.mOKButton.TabIndex = 37;
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
            this.mLibraryGroupBox.Controls.Add(this.mPluginCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.label2);
            this.mLibraryGroupBox.Controls.Add(this.mLiquidctlCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.label10);
            this.mLibraryGroupBox.Controls.Add(this.mLHMMemoryCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mHWInfoCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.label9);
            this.mLibraryGroupBox.Controls.Add(this.label8);
            this.mLibraryGroupBox.Controls.Add(this.label7);
            this.mLibraryGroupBox.Controls.Add(this.label6);
            this.mLibraryGroupBox.Controls.Add(this.label3);
            this.mLibraryGroupBox.Controls.Add(this.label4);
            this.mLibraryGroupBox.Controls.Add(this.mLHMStorageCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mLHMControllerCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mLHMGPUCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mLHMMBCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mLHMCPUCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mLHMCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mRGBnFCButton);
            this.mLibraryGroupBox.Controls.Add(this.mRGBnFCCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mDimmCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mCLCButton);
            this.mLibraryGroupBox.Controls.Add(this.mKrakenButton);
            this.mLibraryGroupBox.Controls.Add(this.mCLCCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mNvApiCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mKrakenCheckBox);
            this.mLibraryGroupBox.Location = new System.Drawing.Point(12, 77);
            this.mLibraryGroupBox.Name = "mLibraryGroupBox";
            this.mLibraryGroupBox.Size = new System.Drawing.Size(188, 479);
            this.mLibraryGroupBox.TabIndex = 5;
            this.mLibraryGroupBox.TabStop = false;
            this.mLibraryGroupBox.Text = "Library";
            // 
            // mLiquidctlCheckBox
            // 
            this.mLiquidctlCheckBox.AutoSize = true;
            this.mLiquidctlCheckBox.Location = new System.Drawing.Point(6, 407);
            this.mLiquidctlCheckBox.Name = "mLiquidctlCheckBox";
            this.mLiquidctlCheckBox.Size = new System.Drawing.Size(67, 16);
            this.mLiquidctlCheckBox.TabIndex = 17;
            this.mLiquidctlCheckBox.Text = "liquidctl";
            this.mLiquidctlCheckBox.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Location = new System.Drawing.Point(2, 393);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(180, 2);
            this.label10.TabIndex = 35;
            // 
            // mLHMMemoryCheckBox
            // 
            this.mLHMMemoryCheckBox.AutoSize = true;
            this.mLHMMemoryCheckBox.Location = new System.Drawing.Point(17, 152);
            this.mLHMMemoryCheckBox.Name = "mLHMMemoryCheckBox";
            this.mLHMMemoryCheckBox.Size = new System.Drawing.Size(71, 16);
            this.mLHMMemoryCheckBox.TabIndex = 7;
            this.mLHMMemoryCheckBox.Text = "Memory";
            this.mLHMMemoryCheckBox.UseVisualStyleBackColor = true;
            // 
            // mHWInfoCheckBox
            // 
            this.mHWInfoCheckBox.AutoSize = true;
            this.mHWInfoCheckBox.Location = new System.Drawing.Point(6, 368);
            this.mHWInfoCheckBox.Name = "mHWInfoCheckBox";
            this.mHWInfoCheckBox.Size = new System.Drawing.Size(70, 16);
            this.mHWInfoCheckBox.TabIndex = 16;
            this.mHWInfoCheckBox.Text = "HWiNFO";
            this.mHWInfoCheckBox.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Location = new System.Drawing.Point(2, 356);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(180, 2);
            this.label9.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Location = new System.Drawing.Point(4, 313);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(180, 2);
            this.label8.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(4, 275);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(180, 2);
            this.label7.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(4, 239);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(180, 2);
            this.label6.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(4, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(180, 2);
            this.label3.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(5, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(180, 2);
            this.label4.TabIndex = 20;
            // 
            // mLHMStorageCheckBox
            // 
            this.mLHMStorageCheckBox.AutoSize = true;
            this.mLHMStorageCheckBox.Location = new System.Drawing.Point(17, 130);
            this.mLHMStorageCheckBox.Name = "mLHMStorageCheckBox";
            this.mLHMStorageCheckBox.Size = new System.Drawing.Size(67, 16);
            this.mLHMStorageCheckBox.TabIndex = 6;
            this.mLHMStorageCheckBox.Text = "Storage";
            this.mLHMStorageCheckBox.UseVisualStyleBackColor = true;
            // 
            // mLHMControllerCheckBox
            // 
            this.mLHMControllerCheckBox.AutoSize = true;
            this.mLHMControllerCheckBox.Location = new System.Drawing.Point(17, 108);
            this.mLHMControllerCheckBox.Name = "mLHMControllerCheckBox";
            this.mLHMControllerCheckBox.Size = new System.Drawing.Size(78, 16);
            this.mLHMControllerCheckBox.TabIndex = 5;
            this.mLHMControllerCheckBox.Text = "Controller";
            this.mLHMControllerCheckBox.UseVisualStyleBackColor = true;
            // 
            // mLHMGPUCheckBox
            // 
            this.mLHMGPUCheckBox.AutoSize = true;
            this.mLHMGPUCheckBox.Location = new System.Drawing.Point(17, 86);
            this.mLHMGPUCheckBox.Name = "mLHMGPUCheckBox";
            this.mLHMGPUCheckBox.Size = new System.Drawing.Size(49, 16);
            this.mLHMGPUCheckBox.TabIndex = 4;
            this.mLHMGPUCheckBox.Text = "GPU";
            this.mLHMGPUCheckBox.UseVisualStyleBackColor = true;
            // 
            // mLHMMBCheckBox
            // 
            this.mLHMMBCheckBox.AutoSize = true;
            this.mLHMMBCheckBox.Location = new System.Drawing.Point(17, 64);
            this.mLHMMBCheckBox.Name = "mLHMMBCheckBox";
            this.mLHMMBCheckBox.Size = new System.Drawing.Size(95, 16);
            this.mLHMMBCheckBox.TabIndex = 3;
            this.mLHMMBCheckBox.Text = "Motherboard";
            this.mLHMMBCheckBox.UseVisualStyleBackColor = true;
            // 
            // mLHMCPUCheckBox
            // 
            this.mLHMCPUCheckBox.AutoSize = true;
            this.mLHMCPUCheckBox.Location = new System.Drawing.Point(17, 42);
            this.mLHMCPUCheckBox.Name = "mLHMCPUCheckBox";
            this.mLHMCPUCheckBox.Size = new System.Drawing.Size(49, 16);
            this.mLHMCPUCheckBox.TabIndex = 2;
            this.mLHMCPUCheckBox.Text = "CPU";
            this.mLHMCPUCheckBox.UseVisualStyleBackColor = true;
            // 
            // mLHMCheckBox
            // 
            this.mLHMCheckBox.AutoSize = true;
            this.mLHMCheckBox.Location = new System.Drawing.Point(6, 20);
            this.mLHMCheckBox.Name = "mLHMCheckBox";
            this.mLHMCheckBox.Size = new System.Drawing.Size(148, 16);
            this.mLHMCheckBox.TabIndex = 1;
            this.mLHMCheckBox.Text = "LibreHardwareMonitor";
            this.mLHMCheckBox.UseVisualStyleBackColor = true;
            // 
            // mRGBnFCButton
            // 
            this.mRGBnFCButton.Location = new System.Drawing.Point(114, 324);
            this.mRGBnFCButton.Name = "mRGBnFCButton";
            this.mRGBnFCButton.Size = new System.Drawing.Size(64, 23);
            this.mRGBnFCButton.TabIndex = 15;
            this.mRGBnFCButton.Text = "Lighting";
            this.mRGBnFCButton.UseVisualStyleBackColor = true;
            this.mRGBnFCButton.Click += new System.EventHandler(this.onRGBnFCButtonClick);
            // 
            // mRGBnFCCheckBox
            // 
            this.mRGBnFCCheckBox.Location = new System.Drawing.Point(6, 321);
            this.mRGBnFCCheckBox.Name = "mRGBnFCCheckBox";
            this.mRGBnFCCheckBox.Size = new System.Drawing.Size(104, 30);
            this.mRGBnFCCheckBox.TabIndex = 14;
            this.mRGBnFCCheckBox.Text = "NZXT RGB＆Fan Controller";
            this.mRGBnFCCheckBox.UseVisualStyleBackColor = true;
            // 
            // mDimmCheckBox
            // 
            this.mDimmCheckBox.AutoSize = true;
            this.mDimmCheckBox.Location = new System.Drawing.Point(6, 217);
            this.mDimmCheckBox.Name = "mDimmCheckBox";
            this.mDimmCheckBox.Size = new System.Drawing.Size(100, 16);
            this.mDimmCheckBox.TabIndex = 9;
            this.mDimmCheckBox.Text = "DIMM sensor";
            this.mDimmCheckBox.UseVisualStyleBackColor = true;
            // 
            // mCLCButton
            // 
            this.mCLCButton.Location = new System.Drawing.Point(114, 284);
            this.mCLCButton.Name = "mCLCButton";
            this.mCLCButton.Size = new System.Drawing.Size(64, 23);
            this.mCLCButton.TabIndex = 13;
            this.mCLCButton.Text = "Lighting";
            this.mCLCButton.UseVisualStyleBackColor = true;
            this.mCLCButton.Click += new System.EventHandler(this.onCLCButtonClick);
            // 
            // mKrakenButton
            // 
            this.mKrakenButton.Location = new System.Drawing.Point(114, 246);
            this.mKrakenButton.Name = "mKrakenButton";
            this.mKrakenButton.Size = new System.Drawing.Size(64, 23);
            this.mKrakenButton.TabIndex = 11;
            this.mKrakenButton.Text = "Lighting";
            this.mKrakenButton.UseVisualStyleBackColor = true;
            this.mKrakenButton.Click += new System.EventHandler(this.onKrakenButtonClick);
            // 
            // mCLCCheckBox
            // 
            this.mCLCCheckBox.AutoSize = true;
            this.mCLCCheckBox.Location = new System.Drawing.Point(6, 288);
            this.mCLCCheckBox.Name = "mCLCCheckBox";
            this.mCLCCheckBox.Size = new System.Drawing.Size(86, 16);
            this.mCLCCheckBox.TabIndex = 12;
            this.mCLCCheckBox.Text = "EVGA CLC";
            this.mCLCCheckBox.UseVisualStyleBackColor = true;
            // 
            // mNvApiCheckBox
            // 
            this.mNvApiCheckBox.AutoSize = true;
            this.mNvApiCheckBox.Location = new System.Drawing.Point(6, 182);
            this.mNvApiCheckBox.Name = "mNvApiCheckBox";
            this.mNvApiCheckBox.Size = new System.Drawing.Size(104, 16);
            this.mNvApiCheckBox.TabIndex = 8;
            this.mNvApiCheckBox.Text = "NvAPIWrapper";
            this.mNvApiCheckBox.UseVisualStyleBackColor = true;
            // 
            // mKrakenCheckBox
            // 
            this.mKrakenCheckBox.AutoSize = true;
            this.mKrakenCheckBox.Location = new System.Drawing.Point(6, 251);
            this.mKrakenCheckBox.Name = "mKrakenCheckBox";
            this.mKrakenCheckBox.Size = new System.Drawing.Size(100, 16);
            this.mKrakenCheckBox.TabIndex = 10;
            this.mKrakenCheckBox.Text = "NZXT Kraken";
            this.mKrakenCheckBox.UseVisualStyleBackColor = true;
            // 
            // mAnimationCheckBox
            // 
            this.mAnimationCheckBox.AutoSize = true;
            this.mAnimationCheckBox.Location = new System.Drawing.Point(19, 595);
            this.mAnimationCheckBox.Name = "mAnimationCheckBox";
            this.mAnimationCheckBox.Size = new System.Drawing.Size(137, 16);
            this.mAnimationCheckBox.TabIndex = 31;
            this.mAnimationCheckBox.Text = "Tray Icon animation";
            this.mAnimationCheckBox.UseVisualStyleBackColor = true;
            // 
            // mFahrenheitCheckBox
            // 
            this.mFahrenheitCheckBox.AutoSize = true;
            this.mFahrenheitCheckBox.Location = new System.Drawing.Point(19, 620);
            this.mFahrenheitCheckBox.Name = "mFahrenheitCheckBox";
            this.mFahrenheitCheckBox.Size = new System.Drawing.Size(108, 16);
            this.mFahrenheitCheckBox.TabIndex = 32;
            this.mFahrenheitCheckBox.Text = "Fahrenheit (°F)";
            this.mFahrenheitCheckBox.UseVisualStyleBackColor = true;
            // 
            // mStartupDelayLabel
            // 
            this.mStartupDelayLabel.AutoSize = true;
            this.mStartupDelayLabel.Location = new System.Drawing.Point(36, 694);
            this.mStartupDelayLabel.Name = "mStartupDelayLabel";
            this.mStartupDelayLabel.Size = new System.Drawing.Size(80, 12);
            this.mStartupDelayLabel.TabIndex = 15;
            this.mStartupDelayLabel.Text = "Delay(sec) : ";
            // 
            // mStartupDelayTextBox
            // 
            this.mStartupDelayTextBox.Location = new System.Drawing.Point(123, 689);
            this.mStartupDelayTextBox.MaxLength = 2;
            this.mStartupDelayTextBox.Name = "mStartupDelayTextBox";
            this.mStartupDelayTextBox.Size = new System.Drawing.Size(73, 21);
            this.mStartupDelayTextBox.TabIndex = 35;
            this.mStartupDelayTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mResetButton
            // 
            this.mResetButton.Location = new System.Drawing.Point(12, 721);
            this.mResetButton.Name = "mResetButton";
            this.mResetButton.Size = new System.Drawing.Size(66, 38);
            this.mResetButton.TabIndex = 36;
            this.mResetButton.Text = "Reset";
            this.mResetButton.UseVisualStyleBackColor = true;
            this.mResetButton.Click += new System.EventHandler(this.onResetButtonClick);
            // 
            // mLanguageLabel
            // 
            this.mLanguageLabel.AutoSize = true;
            this.mLanguageLabel.Location = new System.Drawing.Point(15, 570);
            this.mLanguageLabel.Name = "mLanguageLabel";
            this.mLanguageLabel.Size = new System.Drawing.Size(69, 12);
            this.mLanguageLabel.TabIndex = 25;
            this.mLanguageLabel.Text = "Language :";
            // 
            // mLanguageComboBox
            // 
            this.mLanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mLanguageComboBox.FormattingEnabled = true;
            this.mLanguageComboBox.Location = new System.Drawing.Point(92, 567);
            this.mLanguageComboBox.Name = "mLanguageComboBox";
            this.mLanguageComboBox.Size = new System.Drawing.Size(104, 20);
            this.mLanguageComboBox.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(2, 435);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 2);
            this.label2.TabIndex = 36;
            // 
            // mPluginCheckBox
            // 
            this.mPluginCheckBox.AutoSize = true;
            this.mPluginCheckBox.Location = new System.Drawing.Point(6, 448);
            this.mPluginCheckBox.Name = "mPluginCheckBox";
            this.mPluginCheckBox.Size = new System.Drawing.Size(59, 16);
            this.mPluginCheckBox.TabIndex = 18;
            this.mPluginCheckBox.Text = "Plugin";
            this.mPluginCheckBox.UseVisualStyleBackColor = true;
            // 
            // OptionForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(209, 768);
            this.Controls.Add(this.mLanguageComboBox);
            this.Controls.Add(this.mLanguageLabel);
            this.Controls.Add(this.mResetButton);
            this.Controls.Add(this.mStartupDelayTextBox);
            this.Controls.Add(this.mStartupDelayLabel);
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
        private System.Windows.Forms.CheckBox mNvApiCheckBox;
        private System.Windows.Forms.CheckBox mKrakenCheckBox;
        private System.Windows.Forms.Button mCLCButton;
        private System.Windows.Forms.Button mKrakenButton;
        private System.Windows.Forms.CheckBox mCLCCheckBox;
        private System.Windows.Forms.CheckBox mAnimationCheckBox;
        private System.Windows.Forms.CheckBox mDimmCheckBox;
        private System.Windows.Forms.CheckBox mFahrenheitCheckBox;
        private System.Windows.Forms.CheckBox mRGBnFCCheckBox;
        private System.Windows.Forms.Button mRGBnFCButton;
        private System.Windows.Forms.Label mStartupDelayLabel;
        private System.Windows.Forms.TextBox mStartupDelayTextBox;
        private System.Windows.Forms.CheckBox mLHMControllerCheckBox;
        private System.Windows.Forms.CheckBox mLHMGPUCheckBox;
        private System.Windows.Forms.CheckBox mLHMMBCheckBox;
        private System.Windows.Forms.CheckBox mLHMCPUCheckBox;
        private System.Windows.Forms.CheckBox mLHMCheckBox;
        private System.Windows.Forms.CheckBox mLHMStorageCheckBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button mResetButton;
        private System.Windows.Forms.CheckBox mHWInfoCheckBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox mLHMMemoryCheckBox;
        private System.Windows.Forms.CheckBox mLiquidctlCheckBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label mLanguageLabel;
        private System.Windows.Forms.ComboBox mLanguageComboBox;
        private System.Windows.Forms.CheckBox mPluginCheckBox;
        private System.Windows.Forms.Label label2;
    }
}