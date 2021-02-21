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
            this.mHWInfoCheckBox = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mOHMStorageCheckBox = new System.Windows.Forms.CheckBox();
            this.mOHMControllerCheckBox = new System.Windows.Forms.CheckBox();
            this.mOHMGPUCheckBox = new System.Windows.Forms.CheckBox();
            this.mOHMMBCheckBox = new System.Windows.Forms.CheckBox();
            this.mOHMCPUCheckBox = new System.Windows.Forms.CheckBox();
            this.mOHMCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.mLHMStorageCheckBox = new System.Windows.Forms.CheckBox();
            this.mLHMControllerCheckBox = new System.Windows.Forms.CheckBox();
            this.mLHMGPUCheckBox = new System.Windows.Forms.CheckBox();
            this.mLHMMBCheckBox = new System.Windows.Forms.CheckBox();
            this.mLHMCPUCheckBox = new System.Windows.Forms.CheckBox();
            this.mLHMCheckBox = new System.Windows.Forms.CheckBox();
            this.mGigabyteGPUCheckBox = new System.Windows.Forms.CheckBox();
            this.mGigabyteCPUCheckBox = new System.Windows.Forms.CheckBox();
            this.mGigabyteCheckBox = new System.Windows.Forms.CheckBox();
            this.mRGBnFCButton = new System.Windows.Forms.Button();
            this.mRGBnFCCheckBox = new System.Windows.Forms.CheckBox();
            this.mDimmCheckBox = new System.Windows.Forms.CheckBox();
            this.mCLCButton = new System.Windows.Forms.Button();
            this.mKrakenButton = new System.Windows.Forms.Button();
            this.mCLCCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mNvApiCheckBox = new System.Windows.Forms.CheckBox();
            this.mKrakenCheckBox = new System.Windows.Forms.CheckBox();
            this.mAnimationCheckBox = new System.Windows.Forms.CheckBox();
            this.mFahrenheitCheckBox = new System.Windows.Forms.CheckBox();
            this.mStartupDelayLabel = new System.Windows.Forms.Label();
            this.mStartupDelayTextBox = new System.Windows.Forms.TextBox();
            this.mResetButton = new System.Windows.Forms.Button();
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
            this.mMinimizeCheckBox.Location = new System.Drawing.Point(19, 722);
            this.mMinimizeCheckBox.Name = "mMinimizeCheckBox";
            this.mMinimizeCheckBox.Size = new System.Drawing.Size(112, 16);
            this.mMinimizeCheckBox.TabIndex = 28;
            this.mMinimizeCheckBox.Text = "Start minimized";
            this.mMinimizeCheckBox.UseVisualStyleBackColor = true;
            // 
            // mStartupCheckBox
            // 
            this.mStartupCheckBox.AutoSize = true;
            this.mStartupCheckBox.Location = new System.Drawing.Point(19, 747);
            this.mStartupCheckBox.Name = "mStartupCheckBox";
            this.mStartupCheckBox.Size = new System.Drawing.Size(131, 16);
            this.mStartupCheckBox.TabIndex = 29;
            this.mStartupCheckBox.Text = "Start with Windows";
            this.mStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(84, 798);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(116, 38);
            this.mOKButton.TabIndex = 32;
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
            this.mLibraryGroupBox.Controls.Add(this.mHWInfoCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.label9);
            this.mLibraryGroupBox.Controls.Add(this.label8);
            this.mLibraryGroupBox.Controls.Add(this.label7);
            this.mLibraryGroupBox.Controls.Add(this.label6);
            this.mLibraryGroupBox.Controls.Add(this.label3);
            this.mLibraryGroupBox.Controls.Add(this.label5);
            this.mLibraryGroupBox.Controls.Add(this.mOHMStorageCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mOHMControllerCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mOHMGPUCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mOHMMBCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mOHMCPUCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mOHMCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.label4);
            this.mLibraryGroupBox.Controls.Add(this.mLHMStorageCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mLHMControllerCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mLHMGPUCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mLHMMBCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mLHMCPUCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mLHMCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mGigabyteGPUCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mGigabyteCPUCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mGigabyteCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mRGBnFCButton);
            this.mLibraryGroupBox.Controls.Add(this.mRGBnFCCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mDimmCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mCLCButton);
            this.mLibraryGroupBox.Controls.Add(this.mKrakenButton);
            this.mLibraryGroupBox.Controls.Add(this.mCLCCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.label2);
            this.mLibraryGroupBox.Controls.Add(this.mNvApiCheckBox);
            this.mLibraryGroupBox.Controls.Add(this.mKrakenCheckBox);
            this.mLibraryGroupBox.Location = new System.Drawing.Point(12, 77);
            this.mLibraryGroupBox.Name = "mLibraryGroupBox";
            this.mLibraryGroupBox.Size = new System.Drawing.Size(188, 589);
            this.mLibraryGroupBox.TabIndex = 5;
            this.mLibraryGroupBox.TabStop = false;
            this.mLibraryGroupBox.Text = "Library";
            // 
            // mHWInfoCheckBox
            // 
            this.mHWInfoCheckBox.AutoSize = true;
            this.mHWInfoCheckBox.Location = new System.Drawing.Point(6, 560);
            this.mHWInfoCheckBox.Name = "mHWInfoCheckBox";
            this.mHWInfoCheckBox.Size = new System.Drawing.Size(70, 16);
            this.mHWInfoCheckBox.TabIndex = 24;
            this.mHWInfoCheckBox.Text = "HWiNFO";
            this.mHWInfoCheckBox.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Location = new System.Drawing.Point(2, 548);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(180, 2);
            this.label9.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Location = new System.Drawing.Point(4, 505);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(180, 2);
            this.label8.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(4, 467);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(180, 2);
            this.label7.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(4, 431);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(180, 2);
            this.label6.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(4, 397);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(180, 2);
            this.label3.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(5, 365);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(180, 2);
            this.label5.TabIndex = 27;
            // 
            // mOHMStorageCheckBox
            // 
            this.mOHMStorageCheckBox.AutoSize = true;
            this.mOHMStorageCheckBox.Location = new System.Drawing.Point(17, 344);
            this.mOHMStorageCheckBox.Name = "mOHMStorageCheckBox";
            this.mOHMStorageCheckBox.Size = new System.Drawing.Size(67, 16);
            this.mOHMStorageCheckBox.TabIndex = 15;
            this.mOHMStorageCheckBox.Text = "Storage";
            this.mOHMStorageCheckBox.UseVisualStyleBackColor = true;
            // 
            // mOHMControllerCheckBox
            // 
            this.mOHMControllerCheckBox.AutoSize = true;
            this.mOHMControllerCheckBox.Location = new System.Drawing.Point(17, 322);
            this.mOHMControllerCheckBox.Name = "mOHMControllerCheckBox";
            this.mOHMControllerCheckBox.Size = new System.Drawing.Size(78, 16);
            this.mOHMControllerCheckBox.TabIndex = 14;
            this.mOHMControllerCheckBox.Text = "Controller";
            this.mOHMControllerCheckBox.UseVisualStyleBackColor = true;
            // 
            // mOHMGPUCheckBox
            // 
            this.mOHMGPUCheckBox.AutoSize = true;
            this.mOHMGPUCheckBox.Location = new System.Drawing.Point(17, 300);
            this.mOHMGPUCheckBox.Name = "mOHMGPUCheckBox";
            this.mOHMGPUCheckBox.Size = new System.Drawing.Size(49, 16);
            this.mOHMGPUCheckBox.TabIndex = 13;
            this.mOHMGPUCheckBox.Text = "GPU";
            this.mOHMGPUCheckBox.UseVisualStyleBackColor = true;
            // 
            // mOHMMBCheckBox
            // 
            this.mOHMMBCheckBox.AutoSize = true;
            this.mOHMMBCheckBox.Location = new System.Drawing.Point(17, 278);
            this.mOHMMBCheckBox.Name = "mOHMMBCheckBox";
            this.mOHMMBCheckBox.Size = new System.Drawing.Size(95, 16);
            this.mOHMMBCheckBox.TabIndex = 12;
            this.mOHMMBCheckBox.Text = "Motherboard";
            this.mOHMMBCheckBox.UseVisualStyleBackColor = true;
            // 
            // mOHMCPUCheckBox
            // 
            this.mOHMCPUCheckBox.AutoSize = true;
            this.mOHMCPUCheckBox.Location = new System.Drawing.Point(17, 256);
            this.mOHMCPUCheckBox.Name = "mOHMCPUCheckBox";
            this.mOHMCPUCheckBox.Size = new System.Drawing.Size(49, 16);
            this.mOHMCPUCheckBox.TabIndex = 11;
            this.mOHMCPUCheckBox.Text = "CPU";
            this.mOHMCPUCheckBox.UseVisualStyleBackColor = true;
            // 
            // mOHMCheckBox
            // 
            this.mOHMCheckBox.AutoSize = true;
            this.mOHMCheckBox.Location = new System.Drawing.Point(6, 234);
            this.mOHMCheckBox.Name = "mOHMCheckBox";
            this.mOHMCheckBox.Size = new System.Drawing.Size(150, 16);
            this.mOHMCheckBox.TabIndex = 10;
            this.mOHMCheckBox.Text = "OpenHardwareMonitor";
            this.mOHMCheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(5, 226);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(180, 2);
            this.label4.TabIndex = 20;
            // 
            // mLHMStorageCheckBox
            // 
            this.mLHMStorageCheckBox.AutoSize = true;
            this.mLHMStorageCheckBox.Location = new System.Drawing.Point(17, 206);
            this.mLHMStorageCheckBox.Name = "mLHMStorageCheckBox";
            this.mLHMStorageCheckBox.Size = new System.Drawing.Size(67, 16);
            this.mLHMStorageCheckBox.TabIndex = 9;
            this.mLHMStorageCheckBox.Text = "Storage";
            this.mLHMStorageCheckBox.UseVisualStyleBackColor = true;
            // 
            // mLHMControllerCheckBox
            // 
            this.mLHMControllerCheckBox.AutoSize = true;
            this.mLHMControllerCheckBox.Location = new System.Drawing.Point(17, 184);
            this.mLHMControllerCheckBox.Name = "mLHMControllerCheckBox";
            this.mLHMControllerCheckBox.Size = new System.Drawing.Size(78, 16);
            this.mLHMControllerCheckBox.TabIndex = 8;
            this.mLHMControllerCheckBox.Text = "Controller";
            this.mLHMControllerCheckBox.UseVisualStyleBackColor = true;
            // 
            // mLHMGPUCheckBox
            // 
            this.mLHMGPUCheckBox.AutoSize = true;
            this.mLHMGPUCheckBox.Location = new System.Drawing.Point(17, 162);
            this.mLHMGPUCheckBox.Name = "mLHMGPUCheckBox";
            this.mLHMGPUCheckBox.Size = new System.Drawing.Size(49, 16);
            this.mLHMGPUCheckBox.TabIndex = 7;
            this.mLHMGPUCheckBox.Text = "GPU";
            this.mLHMGPUCheckBox.UseVisualStyleBackColor = true;
            // 
            // mLHMMBCheckBox
            // 
            this.mLHMMBCheckBox.AutoSize = true;
            this.mLHMMBCheckBox.Location = new System.Drawing.Point(17, 140);
            this.mLHMMBCheckBox.Name = "mLHMMBCheckBox";
            this.mLHMMBCheckBox.Size = new System.Drawing.Size(95, 16);
            this.mLHMMBCheckBox.TabIndex = 6;
            this.mLHMMBCheckBox.Text = "Motherboard";
            this.mLHMMBCheckBox.UseVisualStyleBackColor = true;
            // 
            // mLHMCPUCheckBox
            // 
            this.mLHMCPUCheckBox.AutoSize = true;
            this.mLHMCPUCheckBox.Location = new System.Drawing.Point(17, 118);
            this.mLHMCPUCheckBox.Name = "mLHMCPUCheckBox";
            this.mLHMCPUCheckBox.Size = new System.Drawing.Size(49, 16);
            this.mLHMCPUCheckBox.TabIndex = 5;
            this.mLHMCPUCheckBox.Text = "CPU";
            this.mLHMCPUCheckBox.UseVisualStyleBackColor = true;
            // 
            // mLHMCheckBox
            // 
            this.mLHMCheckBox.AutoSize = true;
            this.mLHMCheckBox.Location = new System.Drawing.Point(6, 96);
            this.mLHMCheckBox.Name = "mLHMCheckBox";
            this.mLHMCheckBox.Size = new System.Drawing.Size(148, 16);
            this.mLHMCheckBox.TabIndex = 4;
            this.mLHMCheckBox.Text = "LibreHardwareMonitor";
            this.mLHMCheckBox.UseVisualStyleBackColor = true;
            // 
            // mGigabyteGPUCheckBox
            // 
            this.mGigabyteGPUCheckBox.AutoSize = true;
            this.mGigabyteGPUCheckBox.Location = new System.Drawing.Point(17, 64);
            this.mGigabyteGPUCheckBox.Name = "mGigabyteGPUCheckBox";
            this.mGigabyteGPUCheckBox.Size = new System.Drawing.Size(49, 16);
            this.mGigabyteGPUCheckBox.TabIndex = 3;
            this.mGigabyteGPUCheckBox.Text = "GPU";
            this.mGigabyteGPUCheckBox.UseVisualStyleBackColor = true;
            // 
            // mGigabyteCPUCheckBox
            // 
            this.mGigabyteCPUCheckBox.AutoSize = true;
            this.mGigabyteCPUCheckBox.Location = new System.Drawing.Point(17, 42);
            this.mGigabyteCPUCheckBox.Name = "mGigabyteCPUCheckBox";
            this.mGigabyteCPUCheckBox.Size = new System.Drawing.Size(128, 16);
            this.mGigabyteCPUCheckBox.TabIndex = 2;
            this.mGigabyteCPUCheckBox.Text = "CPU, Motherboard";
            this.mGigabyteCPUCheckBox.UseVisualStyleBackColor = true;
            // 
            // mGigabyteCheckBox
            // 
            this.mGigabyteCheckBox.AutoSize = true;
            this.mGigabyteCheckBox.Location = new System.Drawing.Point(6, 20);
            this.mGigabyteCheckBox.Name = "mGigabyteCheckBox";
            this.mGigabyteCheckBox.Size = new System.Drawing.Size(174, 16);
            this.mGigabyteCheckBox.TabIndex = 1;
            this.mGigabyteCheckBox.Text = "Gigabyte (with AppCenter)";
            this.mGigabyteCheckBox.UseVisualStyleBackColor = true;
            // 
            // mRGBnFCButton
            // 
            this.mRGBnFCButton.Location = new System.Drawing.Point(114, 516);
            this.mRGBnFCButton.Name = "mRGBnFCButton";
            this.mRGBnFCButton.Size = new System.Drawing.Size(64, 23);
            this.mRGBnFCButton.TabIndex = 23;
            this.mRGBnFCButton.Text = "Lighting";
            this.mRGBnFCButton.UseVisualStyleBackColor = true;
            this.mRGBnFCButton.Click += new System.EventHandler(this.onRGBnFCButtonClick);
            // 
            // mRGBnFCCheckBox
            // 
            this.mRGBnFCCheckBox.Location = new System.Drawing.Point(6, 513);
            this.mRGBnFCCheckBox.Name = "mRGBnFCCheckBox";
            this.mRGBnFCCheckBox.Size = new System.Drawing.Size(104, 30);
            this.mRGBnFCCheckBox.TabIndex = 22;
            this.mRGBnFCCheckBox.Text = "NZXT RGB＆Fan Controller";
            this.mRGBnFCCheckBox.UseVisualStyleBackColor = true;
            // 
            // mDimmCheckBox
            // 
            this.mDimmCheckBox.AutoSize = true;
            this.mDimmCheckBox.Location = new System.Drawing.Point(6, 409);
            this.mDimmCheckBox.Name = "mDimmCheckBox";
            this.mDimmCheckBox.Size = new System.Drawing.Size(100, 16);
            this.mDimmCheckBox.TabIndex = 17;
            this.mDimmCheckBox.Text = "DIMM sensor";
            this.mDimmCheckBox.UseVisualStyleBackColor = true;
            // 
            // mCLCButton
            // 
            this.mCLCButton.Location = new System.Drawing.Point(114, 476);
            this.mCLCButton.Name = "mCLCButton";
            this.mCLCButton.Size = new System.Drawing.Size(64, 23);
            this.mCLCButton.TabIndex = 21;
            this.mCLCButton.Text = "Lighting";
            this.mCLCButton.UseVisualStyleBackColor = true;
            this.mCLCButton.Click += new System.EventHandler(this.onCLCButtonClick);
            // 
            // mKrakenButton
            // 
            this.mKrakenButton.Location = new System.Drawing.Point(114, 438);
            this.mKrakenButton.Name = "mKrakenButton";
            this.mKrakenButton.Size = new System.Drawing.Size(64, 23);
            this.mKrakenButton.TabIndex = 19;
            this.mKrakenButton.Text = "Lighting";
            this.mKrakenButton.UseVisualStyleBackColor = true;
            this.mKrakenButton.Click += new System.EventHandler(this.onKrakenButtonClick);
            // 
            // mCLCCheckBox
            // 
            this.mCLCCheckBox.AutoSize = true;
            this.mCLCCheckBox.Location = new System.Drawing.Point(6, 480);
            this.mCLCCheckBox.Name = "mCLCCheckBox";
            this.mCLCCheckBox.Size = new System.Drawing.Size(86, 16);
            this.mCLCCheckBox.TabIndex = 20;
            this.mCLCCheckBox.Text = "EVGA CLC";
            this.mCLCCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(5, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 2);
            this.label2.TabIndex = 4;
            // 
            // mNvApiCheckBox
            // 
            this.mNvApiCheckBox.AutoSize = true;
            this.mNvApiCheckBox.Location = new System.Drawing.Point(6, 374);
            this.mNvApiCheckBox.Name = "mNvApiCheckBox";
            this.mNvApiCheckBox.Size = new System.Drawing.Size(104, 16);
            this.mNvApiCheckBox.TabIndex = 16;
            this.mNvApiCheckBox.Text = "NvAPIWrapper";
            this.mNvApiCheckBox.UseVisualStyleBackColor = true;
            // 
            // mKrakenCheckBox
            // 
            this.mKrakenCheckBox.AutoSize = true;
            this.mKrakenCheckBox.Location = new System.Drawing.Point(6, 443);
            this.mKrakenCheckBox.Name = "mKrakenCheckBox";
            this.mKrakenCheckBox.Size = new System.Drawing.Size(100, 16);
            this.mKrakenCheckBox.TabIndex = 18;
            this.mKrakenCheckBox.Text = "NZXT Kraken";
            this.mKrakenCheckBox.UseVisualStyleBackColor = true;
            // 
            // mAnimationCheckBox
            // 
            this.mAnimationCheckBox.AutoSize = true;
            this.mAnimationCheckBox.Location = new System.Drawing.Point(19, 672);
            this.mAnimationCheckBox.Name = "mAnimationCheckBox";
            this.mAnimationCheckBox.Size = new System.Drawing.Size(137, 16);
            this.mAnimationCheckBox.TabIndex = 26;
            this.mAnimationCheckBox.Text = "Tray Icon animation";
            this.mAnimationCheckBox.UseVisualStyleBackColor = true;
            // 
            // mFahrenheitCheckBox
            // 
            this.mFahrenheitCheckBox.AutoSize = true;
            this.mFahrenheitCheckBox.Location = new System.Drawing.Point(19, 697);
            this.mFahrenheitCheckBox.Name = "mFahrenheitCheckBox";
            this.mFahrenheitCheckBox.Size = new System.Drawing.Size(108, 16);
            this.mFahrenheitCheckBox.TabIndex = 27;
            this.mFahrenheitCheckBox.Text = "Fahrenheit (°F)";
            this.mFahrenheitCheckBox.UseVisualStyleBackColor = true;
            // 
            // mStartupDelayLabel
            // 
            this.mStartupDelayLabel.AutoSize = true;
            this.mStartupDelayLabel.Location = new System.Drawing.Point(36, 771);
            this.mStartupDelayLabel.Name = "mStartupDelayLabel";
            this.mStartupDelayLabel.Size = new System.Drawing.Size(80, 12);
            this.mStartupDelayLabel.TabIndex = 15;
            this.mStartupDelayLabel.Text = "Delay(sec) : ";
            // 
            // mStartupDelayTextBox
            // 
            this.mStartupDelayTextBox.Location = new System.Drawing.Point(123, 766);
            this.mStartupDelayTextBox.MaxLength = 2;
            this.mStartupDelayTextBox.Name = "mStartupDelayTextBox";
            this.mStartupDelayTextBox.Size = new System.Drawing.Size(73, 21);
            this.mStartupDelayTextBox.TabIndex = 30;
            this.mStartupDelayTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mResetButton
            // 
            this.mResetButton.Location = new System.Drawing.Point(12, 798);
            this.mResetButton.Name = "mResetButton";
            this.mResetButton.Size = new System.Drawing.Size(66, 38);
            this.mResetButton.TabIndex = 31;
            this.mResetButton.Text = "Reset";
            this.mResetButton.UseVisualStyleBackColor = true;
            this.mResetButton.Click += new System.EventHandler(this.onResetButtonClick);
            // 
            // OptionForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(209, 842);
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
        private System.Windows.Forms.CheckBox mGigabyteCheckBox;
        private System.Windows.Forms.CheckBox mAnimationCheckBox;
        private System.Windows.Forms.CheckBox mDimmCheckBox;
        private System.Windows.Forms.CheckBox mFahrenheitCheckBox;
        private System.Windows.Forms.CheckBox mRGBnFCCheckBox;
        private System.Windows.Forms.Button mRGBnFCButton;
        private System.Windows.Forms.Label mStartupDelayLabel;
        private System.Windows.Forms.TextBox mStartupDelayTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox mLHMControllerCheckBox;
        private System.Windows.Forms.CheckBox mLHMGPUCheckBox;
        private System.Windows.Forms.CheckBox mLHMMBCheckBox;
        private System.Windows.Forms.CheckBox mLHMCPUCheckBox;
        private System.Windows.Forms.CheckBox mLHMCheckBox;
        private System.Windows.Forms.CheckBox mGigabyteGPUCheckBox;
        private System.Windows.Forms.CheckBox mGigabyteCPUCheckBox;
        private System.Windows.Forms.CheckBox mLHMStorageCheckBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox mOHMStorageCheckBox;
        private System.Windows.Forms.CheckBox mOHMControllerCheckBox;
        private System.Windows.Forms.CheckBox mOHMGPUCheckBox;
        private System.Windows.Forms.CheckBox mOHMMBCheckBox;
        private System.Windows.Forms.CheckBox mOHMCPUCheckBox;
        private System.Windows.Forms.CheckBox mOHMCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button mResetButton;
        private System.Windows.Forms.CheckBox mHWInfoCheckBox;
        private System.Windows.Forms.Label label9;
    }
}