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
            this.mFanGroupBox = new System.Windows.Forms.GroupBox();
            this.mControlGroupBox = new System.Windows.Forms.GroupBox();
            this.mFanControlButton = new System.Windows.Forms.Button();
            this.mToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mOptionButton = new System.Windows.Forms.Button();
            this.mTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.mTrayMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mEnableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mNormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSilenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mPerformanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mShowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mMadeLabel1 = new System.Windows.Forms.Label();
            this.mMadeLabel2 = new System.Windows.Forms.Label();
            this.mDonateQRPictureBox = new System.Windows.Forms.PictureBox();
            this.mDonatePictureBox = new System.Windows.Forms.PictureBox();
            this.mTrayMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mDonateQRPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mDonatePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mTempGroupBox
            // 
            this.mTempGroupBox.Location = new System.Drawing.Point(12, 12);
            this.mTempGroupBox.Name = "mTempGroupBox";
            this.mTempGroupBox.Size = new System.Drawing.Size(252, 53);
            this.mTempGroupBox.TabIndex = 0;
            this.mTempGroupBox.TabStop = false;
            this.mTempGroupBox.Text = "Temperature";
            // 
            // mFanGroupBox
            // 
            this.mFanGroupBox.Location = new System.Drawing.Point(270, 12);
            this.mFanGroupBox.Name = "mFanGroupBox";
            this.mFanGroupBox.Size = new System.Drawing.Size(217, 53);
            this.mFanGroupBox.TabIndex = 1;
            this.mFanGroupBox.TabStop = false;
            this.mFanGroupBox.Text = "Fan speed";
            // 
            // mControlGroupBox
            // 
            this.mControlGroupBox.Location = new System.Drawing.Point(493, 12);
            this.mControlGroupBox.Name = "mControlGroupBox";
            this.mControlGroupBox.Size = new System.Drawing.Size(217, 53);
            this.mControlGroupBox.TabIndex = 2;
            this.mControlGroupBox.TabStop = false;
            this.mControlGroupBox.Text = "Fan control";
            // 
            // mFanControlButton
            // 
            this.mFanControlButton.Location = new System.Drawing.Point(574, 75);
            this.mFanControlButton.Name = "mFanControlButton";
            this.mFanControlButton.Size = new System.Drawing.Size(136, 38);
            this.mFanControlButton.TabIndex = 5;
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
            this.mOptionButton.Location = new System.Drawing.Point(432, 75);
            this.mOptionButton.Name = "mOptionButton";
            this.mOptionButton.Size = new System.Drawing.Size(136, 38);
            this.mOptionButton.TabIndex = 6;
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
            this.mNormalToolStripMenuItem,
            this.mSilenceToolStripMenuItem,
            this.mPerformanceToolStripMenuItem,
            this.mGameToolStripMenuItem,
            this.toolStripSeparator2,
            this.mShowToolStripMenuItem,
            this.mExitToolStripMenuItem});
            this.mTrayMenuStrip.Name = "mTrayMenuStrip";
            this.mTrayMenuStrip.Size = new System.Drawing.Size(231, 170);
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
            // mDonateQRPictureBox
            // 
            this.mDonateQRPictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mDonateQRPictureBox.BackgroundImage")));
            this.mDonateQRPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.mDonateQRPictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("mDonateQRPictureBox.InitialImage")));
            this.mDonateQRPictureBox.Location = new System.Drawing.Point(151, 68);
            this.mDonateQRPictureBox.Name = "mDonateQRPictureBox";
            this.mDonateQRPictureBox.Size = new System.Drawing.Size(50, 50);
            this.mDonateQRPictureBox.TabIndex = 11;
            this.mDonateQRPictureBox.TabStop = false;
            // 
            // mDonatePictureBox
            // 
            this.mDonatePictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mDonatePictureBox.BackgroundImage")));
            this.mDonatePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.mDonatePictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("mDonatePictureBox.InitialImage")));
            this.mDonatePictureBox.Location = new System.Drawing.Point(208, 82);
            this.mDonatePictureBox.Name = "mDonatePictureBox";
            this.mDonatePictureBox.Size = new System.Drawing.Size(92, 26);
            this.mDonatePictureBox.TabIndex = 12;
            this.mDonatePictureBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(719, 122);
            this.Controls.Add(this.mDonatePictureBox);
            this.Controls.Add(this.mDonateQRPictureBox);
            this.Controls.Add(this.mMadeLabel2);
            this.Controls.Add(this.mMadeLabel1);
            this.Controls.Add(this.mOptionButton);
            this.Controls.Add(this.mFanControlButton);
            this.Controls.Add(this.mControlGroupBox);
            this.Controls.Add(this.mFanGroupBox);
            this.Controls.Add(this.mTempGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FanControl";
            this.mTrayMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mDonateQRPictureBox)).EndInit();
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
        private System.Windows.Forms.PictureBox mDonateQRPictureBox;
        private System.Windows.Forms.PictureBox mDonatePictureBox;
    }
}

