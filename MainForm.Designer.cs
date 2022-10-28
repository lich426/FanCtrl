namespace FanCtrl
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mTempGroupBox = new System.Windows.Forms.GroupBox();
            this.mTempPanel = new System.Windows.Forms.Panel();
            this.mFanGroupBox = new System.Windows.Forms.GroupBox();
            this.mFanPanel = new System.Windows.Forms.Panel();
            this.mControlGroupBox = new System.Windows.Forms.GroupBox();
            this.mControlPanel = new System.Windows.Forms.Panel();
            this.mFanControlButton = new System.Windows.Forms.Button();
            this.mToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mOptionButton = new System.Windows.Forms.Button();
            this.mTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.mTrayMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mEnableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mEnableOSDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mNormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSilenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mPerformanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mShowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mMadeLabel1 = new System.Windows.Forms.Label();
            this.mMadeLabel2 = new System.Windows.Forms.Label();
            this.mOSDButton = new System.Windows.Forms.Button();
            this.mLoadingPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.mReloadButton = new System.Windows.Forms.Button();
            this.mHotKeyButton = new System.Windows.Forms.Button();
            this.mDonatePictureBox = new System.Windows.Forms.PictureBox();
            this.mLiquidctlButton = new System.Windows.Forms.Button();
            this.mTempGroupBox.SuspendLayout();
            this.mFanGroupBox.SuspendLayout();
            this.mControlGroupBox.SuspendLayout();
            this.mTrayMenuStrip.SuspendLayout();
            this.mLoadingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mDonatePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mTempGroupBox
            // 
            this.mTempGroupBox.Controls.Add(this.mTempPanel);
            this.mTempGroupBox.Location = new System.Drawing.Point(12, 12);
            this.mTempGroupBox.Name = "mTempGroupBox";
            this.mTempGroupBox.Size = new System.Drawing.Size(272, 53);
            this.mTempGroupBox.TabIndex = 0;
            this.mTempGroupBox.TabStop = false;
            this.mTempGroupBox.Text = "Temperature";
            // 
            // mTempPanel
            // 
            this.mTempPanel.AutoScroll = true;
            this.mTempPanel.Location = new System.Drawing.Point(5, 12);
            this.mTempPanel.Name = "mTempPanel";
            this.mTempPanel.Size = new System.Drawing.Size(261, 35);
            this.mTempPanel.TabIndex = 0;
            // 
            // mFanGroupBox
            // 
            this.mFanGroupBox.Controls.Add(this.mFanPanel);
            this.mFanGroupBox.Location = new System.Drawing.Point(290, 12);
            this.mFanGroupBox.Name = "mFanGroupBox";
            this.mFanGroupBox.Size = new System.Drawing.Size(272, 53);
            this.mFanGroupBox.TabIndex = 1;
            this.mFanGroupBox.TabStop = false;
            this.mFanGroupBox.Text = "Fan speed";
            // 
            // mFanPanel
            // 
            this.mFanPanel.AutoScroll = true;
            this.mFanPanel.Location = new System.Drawing.Point(6, 12);
            this.mFanPanel.Name = "mFanPanel";
            this.mFanPanel.Size = new System.Drawing.Size(260, 35);
            this.mFanPanel.TabIndex = 1;
            // 
            // mControlGroupBox
            // 
            this.mControlGroupBox.Controls.Add(this.mControlPanel);
            this.mControlGroupBox.Location = new System.Drawing.Point(568, 12);
            this.mControlGroupBox.Name = "mControlGroupBox";
            this.mControlGroupBox.Size = new System.Drawing.Size(306, 53);
            this.mControlGroupBox.TabIndex = 2;
            this.mControlGroupBox.TabStop = false;
            this.mControlGroupBox.Text = "Fan control";
            // 
            // mControlPanel
            // 
            this.mControlPanel.AutoScroll = true;
            this.mControlPanel.Location = new System.Drawing.Point(6, 12);
            this.mControlPanel.Name = "mControlPanel";
            this.mControlPanel.Size = new System.Drawing.Size(294, 35);
            this.mControlPanel.TabIndex = 2;
            // 
            // mFanControlButton
            // 
            this.mFanControlButton.Location = new System.Drawing.Point(779, 71);
            this.mFanControlButton.Name = "mFanControlButton";
            this.mFanControlButton.Size = new System.Drawing.Size(95, 38);
            this.mFanControlButton.TabIndex = 8;
            this.mFanControlButton.Text = "Auto Fan Control";
            this.mFanControlButton.UseVisualStyleBackColor = true;
            this.mFanControlButton.Click += new System.EventHandler(this.onFanControlButtonClick);
            // 
            // mToolTip
            // 
            this.mToolTip.IsBalloon = true;
            // 
            // mOptionButton
            // 
            this.mOptionButton.Location = new System.Drawing.Point(678, 71);
            this.mOptionButton.Name = "mOptionButton";
            this.mOptionButton.Size = new System.Drawing.Size(95, 38);
            this.mOptionButton.TabIndex = 7;
            this.mOptionButton.Text = "Option";
            this.mOptionButton.UseVisualStyleBackColor = true;
            this.mOptionButton.Click += new System.EventHandler(this.onOptionButtonClick);
            // 
            // mTrayIcon
            // 
            this.mTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("mTrayIcon.Icon")));
            this.mTrayIcon.Text = "notifyIcon1";
            this.mTrayIcon.Visible = true;
            // 
            // mTrayMenuStrip
            // 
            this.mTrayMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mEnableToolStripMenuItem,
            this.toolStripSeparator1,
            this.mEnableOSDToolStripMenuItem,
            this.toolStripSeparator3,
            this.mNormalToolStripMenuItem,
            this.mSilenceToolStripMenuItem,
            this.mPerformanceToolStripMenuItem,
            this.mGameToolStripMenuItem,
            this.toolStripSeparator2,
            this.mShowToolStripMenuItem,
            this.mExitToolStripMenuItem});
            this.mTrayMenuStrip.Name = "mTrayMenuStrip";
            this.mTrayMenuStrip.Size = new System.Drawing.Size(231, 198);
            // 
            // mEnableToolStripMenuItem
            // 
            this.mEnableToolStripMenuItem.Name = "mEnableToolStripMenuItem";
            this.mEnableToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.mEnableToolStripMenuItem.Text = "Enable automatic fan control";
            this.mEnableToolStripMenuItem.Click += new System.EventHandler(this.onTrayMenuEnableClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(227, 6);
            // 
            // mEnableOSDToolStripMenuItem
            // 
            this.mEnableOSDToolStripMenuItem.Name = "mEnableOSDToolStripMenuItem";
            this.mEnableOSDToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.mEnableOSDToolStripMenuItem.Text = "Enable OSD (RTSS)";
            this.mEnableOSDToolStripMenuItem.Click += new System.EventHandler(this.onTrayManuEnableOSDClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(227, 6);
            // 
            // mNormalToolStripMenuItem
            // 
            this.mNormalToolStripMenuItem.Name = "mNormalToolStripMenuItem";
            this.mNormalToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.mNormalToolStripMenuItem.Text = "Normal";
            this.mNormalToolStripMenuItem.Click += new System.EventHandler(this.onTrayMenuNormalClick);
            // 
            // mSilenceToolStripMenuItem
            // 
            this.mSilenceToolStripMenuItem.Name = "mSilenceToolStripMenuItem";
            this.mSilenceToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.mSilenceToolStripMenuItem.Text = "Silence";
            this.mSilenceToolStripMenuItem.Click += new System.EventHandler(this.onTrayMenuSilenceClick);
            // 
            // mPerformanceToolStripMenuItem
            // 
            this.mPerformanceToolStripMenuItem.Name = "mPerformanceToolStripMenuItem";
            this.mPerformanceToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.mPerformanceToolStripMenuItem.Text = "Performance";
            this.mPerformanceToolStripMenuItem.Click += new System.EventHandler(this.onTrayMenuPerformanceClick);
            // 
            // mGameToolStripMenuItem
            // 
            this.mGameToolStripMenuItem.Name = "mGameToolStripMenuItem";
            this.mGameToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.mGameToolStripMenuItem.Text = "Game";
            this.mGameToolStripMenuItem.Click += new System.EventHandler(this.onTrayMenuGameClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(227, 6);
            // 
            // mShowToolStripMenuItem
            // 
            this.mShowToolStripMenuItem.Name = "mShowToolStripMenuItem";
            this.mShowToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.mShowToolStripMenuItem.Text = "Show";
            this.mShowToolStripMenuItem.Click += new System.EventHandler(this.onTrayMenuShow);
            // 
            // mExitToolStripMenuItem
            // 
            this.mExitToolStripMenuItem.Name = "mExitToolStripMenuItem";
            this.mExitToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.mExitToolStripMenuItem.Text = "Exit";
            this.mExitToolStripMenuItem.Click += new System.EventHandler(this.onTrayMenuExit);
            // 
            // mMadeLabel1
            // 
            this.mMadeLabel1.AutoSize = true;
            this.mMadeLabel1.Location = new System.Drawing.Point(17, 78);
            this.mMadeLabel1.Name = "mMadeLabel1";
            this.mMadeLabel1.Size = new System.Drawing.Size(83, 12);
            this.mMadeLabel1.TabIndex = 7;
            this.mMadeLabel1.Text = "Made by Lich";
            // 
            // mMadeLabel2
            // 
            this.mMadeLabel2.AutoSize = true;
            this.mMadeLabel2.Location = new System.Drawing.Point(15, 95);
            this.mMadeLabel2.Name = "mMadeLabel2";
            this.mMadeLabel2.Size = new System.Drawing.Size(125, 12);
            this.mMadeLabel2.TabIndex = 8;
            this.mMadeLabel2.Text = "(lich426@gmail.com)";
            // 
            // mOSDButton
            // 
            this.mOSDButton.Location = new System.Drawing.Point(577, 71);
            this.mOSDButton.Name = "mOSDButton";
            this.mOSDButton.Size = new System.Drawing.Size(95, 38);
            this.mOSDButton.TabIndex = 6;
            this.mOSDButton.Text = "OSD (RTSS)";
            this.mOSDButton.UseVisualStyleBackColor = true;
            this.mOSDButton.Click += new System.EventHandler(this.onOSDButtonClick);
            // 
            // mLoadingPanel
            // 
            this.mLoadingPanel.Controls.Add(this.label1);
            this.mLoadingPanel.Location = new System.Drawing.Point(5, 8);
            this.mLoadingPanel.Name = "mLoadingPanel";
            this.mLoadingPanel.Size = new System.Drawing.Size(874, 111);
            this.mLoadingPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(403, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Loading...";
            // 
            // mReloadButton
            // 
            this.mReloadButton.Location = new System.Drawing.Point(375, 71);
            this.mReloadButton.Name = "mReloadButton";
            this.mReloadButton.Size = new System.Drawing.Size(95, 38);
            this.mReloadButton.TabIndex = 4;
            this.mReloadButton.Text = "Reload";
            this.mReloadButton.UseVisualStyleBackColor = true;
            this.mReloadButton.Click += new System.EventHandler(this.onReloadButtonClick);
            // 
            // mHotKeyButton
            // 
            this.mHotKeyButton.Location = new System.Drawing.Point(476, 71);
            this.mHotKeyButton.Name = "mHotKeyButton";
            this.mHotKeyButton.Size = new System.Drawing.Size(95, 38);
            this.mHotKeyButton.TabIndex = 5;
            this.mHotKeyButton.Text = "Hotkey";
            this.mHotKeyButton.UseVisualStyleBackColor = true;
            this.mHotKeyButton.Click += new System.EventHandler(this.onHotKeyButtonClick);
            // 
            // mDonatePictureBox
            // 
            this.mDonatePictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mDonatePictureBox.BackgroundImage")));
            this.mDonatePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.mDonatePictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("mDonatePictureBox.InitialImage")));
            this.mDonatePictureBox.Location = new System.Drawing.Point(146, 78);
            this.mDonatePictureBox.Name = "mDonatePictureBox";
            this.mDonatePictureBox.Size = new System.Drawing.Size(92, 26);
            this.mDonatePictureBox.TabIndex = 12;
            this.mDonatePictureBox.TabStop = false;
            // 
            // mLiquidctlButton
            // 
            this.mLiquidctlButton.Location = new System.Drawing.Point(274, 71);
            this.mLiquidctlButton.Name = "mLiquidctlButton";
            this.mLiquidctlButton.Size = new System.Drawing.Size(95, 38);
            this.mLiquidctlButton.TabIndex = 3;
            this.mLiquidctlButton.Text = "liquidctl Setting";
            this.mLiquidctlButton.UseVisualStyleBackColor = true;
            this.mLiquidctlButton.Click += new System.EventHandler(this.onLiquidctlButtonClick);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(883, 127);
            this.Controls.Add(this.mLoadingPanel);
            this.Controls.Add(this.mLiquidctlButton);
            this.Controls.Add(this.mReloadButton);
            this.Controls.Add(this.mOSDButton);
            this.Controls.Add(this.mOptionButton);
            this.Controls.Add(this.mDonatePictureBox);
            this.Controls.Add(this.mMadeLabel2);
            this.Controls.Add(this.mMadeLabel1);
            this.Controls.Add(this.mFanControlButton);
            this.Controls.Add(this.mControlGroupBox);
            this.Controls.Add(this.mFanGroupBox);
            this.Controls.Add(this.mTempGroupBox);
            this.Controls.Add(this.mHotKeyButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FanCtrl";
            this.mTempGroupBox.ResumeLayout(false);
            this.mFanGroupBox.ResumeLayout(false);
            this.mControlGroupBox.ResumeLayout(false);
            this.mTrayMenuStrip.ResumeLayout(false);
            this.mLoadingPanel.ResumeLayout(false);
            this.mLoadingPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mDonatePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox mTempGroupBox;
        private System.Windows.Forms.GroupBox mFanGroupBox;
        private System.Windows.Forms.GroupBox mControlGroupBox;
        private System.Windows.Forms.Button mFanControlButton;
        private System.Windows.Forms.ToolTip mToolTip;
        private System.Windows.Forms.Button mOptionButton;
        private System.Windows.Forms.NotifyIcon mTrayIcon;
        private System.Windows.Forms.ContextMenuStrip mTrayMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mShowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mExitToolStripMenuItem;
        private System.Windows.Forms.Label mMadeLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mEnableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mNormalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSilenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mPerformanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Label mMadeLabel2;
        private System.Windows.Forms.Button mOSDButton;
        private System.Windows.Forms.ToolStripMenuItem mEnableOSDToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Panel mLoadingPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel mTempPanel;
        private System.Windows.Forms.Panel mFanPanel;
        private System.Windows.Forms.Panel mControlPanel;
        private System.Windows.Forms.Button mReloadButton;
        private System.Windows.Forms.Button mHotKeyButton;
        private System.Windows.Forms.PictureBox mDonatePictureBox;
        private System.Windows.Forms.Button mLiquidctlButton;
    }
}

