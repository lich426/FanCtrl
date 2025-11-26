using DarkUI.Controls;
using System.Windows.Forms;

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
            this.mIntervalGroupBox = new DarkUI.Controls.DarkGroupBox();
            this.label1 = new DarkUI.Controls.DarkLabel();
            this.mIntervalTextBox = new DarkUI.Controls.DarkTextBox();
            this.mMinimizeCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mStartupCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mOKButton = new DarkUI.Controls.DarkButton();
            this.mToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mLibraryGroupBox = new DarkUI.Controls.DarkGroupBox();
            this.mPluginCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.label2 = new DarkUI.Controls.DarkLabel();
            this.mLiquidctlCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.label10 = new DarkUI.Controls.DarkLabel();
            this.mLHMMemoryCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mHWInfoCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.label9 = new DarkUI.Controls.DarkLabel();
            this.label8 = new DarkUI.Controls.DarkLabel();
            this.label7 = new DarkUI.Controls.DarkLabel();
            this.label3 = new DarkUI.Controls.DarkLabel();
            this.label4 = new DarkUI.Controls.DarkLabel();
            this.mLHMStorageCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mLHMControllerCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mLHMGPUCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mLHMMBCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mLHMCPUCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mLHMCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mRGBnFCButton = new DarkUI.Controls.DarkButton();
            this.mRGBnFCCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mCLCButton = new DarkUI.Controls.DarkButton();
            this.mKrakenButton = new DarkUI.Controls.DarkButton();
            this.mCLCCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mNvApiCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mKrakenCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mAnimationCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mFahrenheitCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mStartupDelayLabel = new DarkUI.Controls.DarkLabel();
            this.mStartupDelayTextBox = new DarkUI.Controls.DarkTextBox();
            this.mResetButton = new DarkUI.Controls.DarkButton();
            this.mLanguageLabel = new DarkUI.Controls.DarkLabel();
            this.mLanguageComboBox = new DarkUI.Controls.DarkComboBox();
            this.mThemeComboBox = new DarkUI.Controls.DarkComboBox();
            this.mThemeLabel = new DarkUI.Controls.DarkLabel();
            this.mIntervalGroupBox.SuspendLayout();
            this.mLibraryGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mIntervalGroupBox
            // 
            this.mIntervalGroupBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
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
            this.mMinimizeCheckBox.Location = new System.Drawing.Point(18, 645);
            this.mMinimizeCheckBox.Name = "mMinimizeCheckBox";
            this.mMinimizeCheckBox.Size = new System.Drawing.Size(112, 16);
            this.mMinimizeCheckBox.TabIndex = 33;
            this.mMinimizeCheckBox.Text = "Start minimized";
            // 
            // mStartupCheckBox
            // 
            this.mStartupCheckBox.AutoSize = true;
            this.mStartupCheckBox.Location = new System.Drawing.Point(18, 670);
            this.mStartupCheckBox.Name = "mStartupCheckBox";
            this.mStartupCheckBox.Size = new System.Drawing.Size(131, 16);
            this.mStartupCheckBox.TabIndex = 34;
            this.mStartupCheckBox.Text = "Start with Windows";
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(83, 721);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Padding = new System.Windows.Forms.Padding(1);
            this.mOKButton.Size = new System.Drawing.Size(116, 38);
            this.mOKButton.TabIndex = 37;
            this.mOKButton.Text = "OK";
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // mToolTip
            // 
            this.mToolTip.IsBalloon = true;
            // 
            // mLibraryGroupBox
            // 
            this.mLibraryGroupBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.mLibraryGroupBox.Controls.Add(this.mPluginCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.label2);
            this.mLibraryGroupBox.Controls.Add(this.mLiquidctlCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.label10);
            this.mLibraryGroupBox.Controls.Add(this.mLHMMemoryCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mHWInfoCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.label9);
            this.mLibraryGroupBox.Controls.Add(this.label8);
            this.mLibraryGroupBox.Controls.Add(this.label7);
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
            this.mLibraryGroupBox.Controls.Add(this.mCLCButton);
            this.mLibraryGroupBox.Controls.Add(this.mKrakenButton);
            this.mLibraryGroupBox.Controls.Add(this.mCLCCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mNvApiCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mKrakenCheckBox);
            this.mLibraryGroupBox.Location = new System.Drawing.Point(12, 77);
            this.mLibraryGroupBox.Name = "mLibraryGroupBox";
            this.mLibraryGroupBox.Size = new System.Drawing.Size(188, 445);
            this.mLibraryGroupBox.TabIndex = 5;
            this.mLibraryGroupBox.TabStop = false;
            this.mLibraryGroupBox.Text = "Library";
            // 
            // mPluginCheckBox
            // 
            this.mPluginCheckBox.AutoSize = true;
            this.mPluginCheckBox.Location = new System.Drawing.Point(6, 416);
            this.mPluginCheckBox.Name = "mPluginCheckBox";
            this.mPluginCheckBox.Size = new System.Drawing.Size(59, 16);
            this.mPluginCheckBox.TabIndex = 18;
            this.mPluginCheckBox.Text = "Plugin";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(2, 403);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 2);
            this.label2.TabIndex = 36;
            // 
            // mLiquidctlCheckBox
            // 
            this.mLiquidctlCheckBox.AutoSize = true;
            this.mLiquidctlCheckBox.Location = new System.Drawing.Point(6, 375);
            this.mLiquidctlCheckBox.Name = "mLiquidctlCheckBox";
            this.mLiquidctlCheckBox.Size = new System.Drawing.Size(67, 16);
            this.mLiquidctlCheckBox.TabIndex = 17;
            this.mLiquidctlCheckBox.Text = "liquidctl";
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Location = new System.Drawing.Point(2, 361);
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
            // 
            // mHWInfoCheckBox
            // 
            this.mHWInfoCheckBox.AutoSize = true;
            this.mHWInfoCheckBox.Location = new System.Drawing.Point(6, 336);
            this.mHWInfoCheckBox.Name = "mHWInfoCheckBox";
            this.mHWInfoCheckBox.Size = new System.Drawing.Size(70, 16);
            this.mHWInfoCheckBox.TabIndex = 16;
            this.mHWInfoCheckBox.Text = "HWiNFO";
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Location = new System.Drawing.Point(2, 324);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(180, 2);
            this.label9.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Location = new System.Drawing.Point(4, 281);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(180, 2);
            this.label8.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(4, 243);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(180, 2);
            this.label7.TabIndex = 30;
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
            // 
            // mLHMControllerCheckBox
            // 
            this.mLHMControllerCheckBox.AutoSize = true;
            this.mLHMControllerCheckBox.Location = new System.Drawing.Point(17, 108);
            this.mLHMControllerCheckBox.Name = "mLHMControllerCheckBox";
            this.mLHMControllerCheckBox.Size = new System.Drawing.Size(78, 16);
            this.mLHMControllerCheckBox.TabIndex = 5;
            this.mLHMControllerCheckBox.Text = "Controller";
            // 
            // mLHMGPUCheckBox
            // 
            this.mLHMGPUCheckBox.AutoSize = true;
            this.mLHMGPUCheckBox.Location = new System.Drawing.Point(17, 86);
            this.mLHMGPUCheckBox.Name = "mLHMGPUCheckBox";
            this.mLHMGPUCheckBox.Size = new System.Drawing.Size(49, 16);
            this.mLHMGPUCheckBox.TabIndex = 4;
            this.mLHMGPUCheckBox.Text = "GPU";
            // 
            // mLHMMBCheckBox
            // 
            this.mLHMMBCheckBox.AutoSize = true;
            this.mLHMMBCheckBox.Location = new System.Drawing.Point(17, 64);
            this.mLHMMBCheckBox.Name = "mLHMMBCheckBox";
            this.mLHMMBCheckBox.Size = new System.Drawing.Size(95, 16);
            this.mLHMMBCheckBox.TabIndex = 3;
            this.mLHMMBCheckBox.Text = "Motherboard";
            // 
            // mLHMCPUCheckBox
            // 
            this.mLHMCPUCheckBox.AutoSize = true;
            this.mLHMCPUCheckBox.Location = new System.Drawing.Point(17, 42);
            this.mLHMCPUCheckBox.Name = "mLHMCPUCheckBox";
            this.mLHMCPUCheckBox.Size = new System.Drawing.Size(49, 16);
            this.mLHMCPUCheckBox.TabIndex = 2;
            this.mLHMCPUCheckBox.Text = "CPU";
            // 
            // mLHMCheckBox
            // 
            this.mLHMCheckBox.AutoSize = true;
            this.mLHMCheckBox.Location = new System.Drawing.Point(6, 20);
            this.mLHMCheckBox.Name = "mLHMCheckBox";
            this.mLHMCheckBox.Size = new System.Drawing.Size(148, 16);
            this.mLHMCheckBox.TabIndex = 1;
            this.mLHMCheckBox.Text = "LibreHardwareMonitor";
            // 
            // mRGBnFCButton
            // 
            this.mRGBnFCButton.Location = new System.Drawing.Point(114, 292);
            this.mRGBnFCButton.Name = "mRGBnFCButton";
            this.mRGBnFCButton.Padding = new System.Windows.Forms.Padding(1);
            this.mRGBnFCButton.Size = new System.Drawing.Size(64, 23);
            this.mRGBnFCButton.TabIndex = 15;
            this.mRGBnFCButton.Text = "Lighting";
            this.mRGBnFCButton.Click += new System.EventHandler(this.onRGBnFCButtonClick);
            // 
            // mRGBnFCCheckBox
            // 
            this.mRGBnFCCheckBox.Location = new System.Drawing.Point(6, 289);
            this.mRGBnFCCheckBox.Name = "mRGBnFCCheckBox";
            this.mRGBnFCCheckBox.Size = new System.Drawing.Size(104, 30);
            this.mRGBnFCCheckBox.TabIndex = 14;
            this.mRGBnFCCheckBox.Text = "NZXT RGB＆Fan Controller";
            // 
            // mCLCButton
            // 
            this.mCLCButton.Location = new System.Drawing.Point(114, 252);
            this.mCLCButton.Name = "mCLCButton";
            this.mCLCButton.Padding = new System.Windows.Forms.Padding(1);
            this.mCLCButton.Size = new System.Drawing.Size(64, 23);
            this.mCLCButton.TabIndex = 13;
            this.mCLCButton.Text = "Lighting";
            this.mCLCButton.Click += new System.EventHandler(this.onCLCButtonClick);
            // 
            // mKrakenButton
            // 
            this.mKrakenButton.Location = new System.Drawing.Point(114, 214);
            this.mKrakenButton.Name = "mKrakenButton";
            this.mKrakenButton.Padding = new System.Windows.Forms.Padding(1);
            this.mKrakenButton.Size = new System.Drawing.Size(64, 23);
            this.mKrakenButton.TabIndex = 11;
            this.mKrakenButton.Text = "Lighting";
            this.mKrakenButton.Click += new System.EventHandler(this.onKrakenButtonClick);
            // 
            // mCLCCheckBox
            // 
            this.mCLCCheckBox.AutoSize = true;
            this.mCLCCheckBox.Location = new System.Drawing.Point(6, 256);
            this.mCLCCheckBox.Name = "mCLCCheckBox";
            this.mCLCCheckBox.Size = new System.Drawing.Size(86, 16);
            this.mCLCCheckBox.TabIndex = 12;
            this.mCLCCheckBox.Text = "EVGA CLC";
            // 
            // mNvApiCheckBox
            // 
            this.mNvApiCheckBox.AutoSize = true;
            this.mNvApiCheckBox.Location = new System.Drawing.Point(6, 182);
            this.mNvApiCheckBox.Name = "mNvApiCheckBox";
            this.mNvApiCheckBox.Size = new System.Drawing.Size(104, 16);
            this.mNvApiCheckBox.TabIndex = 8;
            this.mNvApiCheckBox.Text = "NvAPIWrapper";
            // 
            // mKrakenCheckBox
            // 
            this.mKrakenCheckBox.AutoSize = true;
            this.mKrakenCheckBox.Location = new System.Drawing.Point(6, 219);
            this.mKrakenCheckBox.Name = "mKrakenCheckBox";
            this.mKrakenCheckBox.Size = new System.Drawing.Size(100, 16);
            this.mKrakenCheckBox.TabIndex = 10;
            this.mKrakenCheckBox.Text = "NZXT Kraken";
            // 
            // mAnimationCheckBox
            // 
            this.mAnimationCheckBox.AutoSize = true;
            this.mAnimationCheckBox.Location = new System.Drawing.Point(18, 595);
            this.mAnimationCheckBox.Name = "mAnimationCheckBox";
            this.mAnimationCheckBox.Size = new System.Drawing.Size(137, 16);
            this.mAnimationCheckBox.TabIndex = 31;
            this.mAnimationCheckBox.Text = "Tray Icon animation";
            // 
            // mFahrenheitCheckBox
            // 
            this.mFahrenheitCheckBox.AutoSize = true;
            this.mFahrenheitCheckBox.Location = new System.Drawing.Point(18, 620);
            this.mFahrenheitCheckBox.Name = "mFahrenheitCheckBox";
            this.mFahrenheitCheckBox.Size = new System.Drawing.Size(108, 16);
            this.mFahrenheitCheckBox.TabIndex = 32;
            this.mFahrenheitCheckBox.Text = "Fahrenheit (°F)";
            // 
            // mStartupDelayLabel
            // 
            this.mStartupDelayLabel.AutoSize = true;
            this.mStartupDelayLabel.Location = new System.Drawing.Point(35, 694);
            this.mStartupDelayLabel.Name = "mStartupDelayLabel";
            this.mStartupDelayLabel.Size = new System.Drawing.Size(80, 12);
            this.mStartupDelayLabel.TabIndex = 15;
            this.mStartupDelayLabel.Text = "Delay(sec) : ";
            // 
            // mStartupDelayTextBox
            // 
            this.mStartupDelayTextBox.Location = new System.Drawing.Point(122, 689);
            this.mStartupDelayTextBox.MaxLength = 2;
            this.mStartupDelayTextBox.Name = "mStartupDelayTextBox";
            this.mStartupDelayTextBox.Size = new System.Drawing.Size(73, 21);
            this.mStartupDelayTextBox.TabIndex = 35;
            this.mStartupDelayTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mResetButton
            // 
            this.mResetButton.Location = new System.Drawing.Point(11, 721);
            this.mResetButton.Name = "mResetButton";
            this.mResetButton.Padding = new System.Windows.Forms.Padding(1);
            this.mResetButton.Size = new System.Drawing.Size(66, 38);
            this.mResetButton.TabIndex = 36;
            this.mResetButton.Text = "Reset";
            this.mResetButton.Click += new System.EventHandler(this.onResetButtonClick);
            // 
            // mLanguageLabel
            // 
            this.mLanguageLabel.AutoSize = true;
            this.mLanguageLabel.Location = new System.Drawing.Point(15, 535);
            this.mLanguageLabel.Name = "mLanguageLabel";
            this.mLanguageLabel.Size = new System.Drawing.Size(69, 12);
            this.mLanguageLabel.TabIndex = 25;
            this.mLanguageLabel.Text = "Language :";
            // 
            // mLanguageComboBox
            // 
            this.mLanguageComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.mLanguageComboBox.FormattingEnabled = true;
            this.mLanguageComboBox.Location = new System.Drawing.Point(92, 531);
            this.mLanguageComboBox.Name = "mLanguageComboBox";
            this.mLanguageComboBox.Size = new System.Drawing.Size(104, 22);
            this.mLanguageComboBox.TabIndex = 30;
            // 
            // mThemeComboBox
            // 
            this.mThemeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.mThemeComboBox.FormattingEnabled = true;
            this.mThemeComboBox.Location = new System.Drawing.Point(92, 561);
            this.mThemeComboBox.Name = "mThemeComboBox";
            this.mThemeComboBox.Size = new System.Drawing.Size(104, 22);
            this.mThemeComboBox.TabIndex = 39;
            // 
            // mThemeLabel
            // 
            this.mThemeLabel.AutoSize = true;
            this.mThemeLabel.Location = new System.Drawing.Point(15, 565);
            this.mThemeLabel.Name = "mThemeLabel";
            this.mThemeLabel.Size = new System.Drawing.Size(53, 12);
            this.mThemeLabel.TabIndex = 38;
            this.mThemeLabel.Text = "Theme :";
            // 
            // OptionForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(209, 769);
            this.Controls.Add(this.mThemeComboBox);
            this.Controls.Add(this.mThemeLabel);
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
            this.Location = new System.Drawing.Point(0, 0);
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

        private DarkGroupBox mIntervalGroupBox;
        private DarkLabel label1;
        private DarkTextBox mIntervalTextBox;
        private DarkCheckBox mMinimizeCheckBox;
        private DarkCheckBox mStartupCheckBox;
        private DarkButton mOKButton;
        private System.Windows.Forms.ToolTip mToolTip;
        private DarkGroupBox mLibraryGroupBox;
        private DarkCheckBox mNvApiCheckBox;
        private DarkCheckBox mKrakenCheckBox;
        private DarkButton mCLCButton;
        private DarkButton mKrakenButton;
        private DarkCheckBox mCLCCheckBox;
        private DarkCheckBox mAnimationCheckBox;
        private DarkCheckBox mFahrenheitCheckBox;
        private DarkCheckBox mRGBnFCCheckBox;
        private DarkButton mRGBnFCButton;
        private DarkLabel mStartupDelayLabel;
        private DarkTextBox mStartupDelayTextBox;
        private DarkCheckBox mLHMControllerCheckBox;
        private DarkCheckBox mLHMGPUCheckBox;
        private DarkCheckBox mLHMMBCheckBox;
        private DarkCheckBox mLHMCPUCheckBox;
        private DarkCheckBox mLHMCheckBox;
        private DarkCheckBox mLHMStorageCheckBox;
        private DarkLabel label8;
        private DarkLabel label7;
        private DarkLabel label3;
        private DarkLabel label4;
        private DarkButton mResetButton;
        private DarkCheckBox mHWInfoCheckBox;
        private DarkLabel label9;
        private DarkCheckBox mLHMMemoryCheckBox;
        private DarkCheckBox mLiquidctlCheckBox;
        private DarkLabel label10;
        private DarkLabel mLanguageLabel;
        private DarkComboBox mLanguageComboBox;
        private DarkCheckBox mPluginCheckBox;
        private DarkLabel label2;
        private DarkComboBox mThemeComboBox;
        private DarkLabel mThemeLabel;
    }
}