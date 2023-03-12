using DarkUI.Controls;
using System.Windows.Forms;

namespace FanCtrl
{
    partial class ControlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlForm));
            this.mEnableCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mTempGroupBox = new DarkUI.Controls.DarkGroupBox();
            this.mAddTempListView = new FanCtrl.ThemeListView();
            this.mFanGroupBox = new DarkUI.Controls.DarkGroupBox();
            this.mFanListView = new FanCtrl.ThemeListView();
            this.mAddFanListView = new FanCtrl.ThemeListView();
            this.mRemoveButton = new DarkUI.Controls.DarkButton();
            this.mAddButton = new DarkUI.Controls.DarkButton();
            this.mGraphGroupBox = new DarkUI.Controls.DarkGroupBox();
            this.mPresetLabel = new DarkUI.Controls.DarkLabel();
            this.mUnitLabel = new DarkUI.Controls.DarkLabel();
            this.mHysLabel = new DarkUI.Controls.DarkLabel();
            this.mStepCheckBox = new DarkUI.Controls.DarkCheckBox();
            this.mGraph = new ZedGraph.ZedGraphControl();
            this.mOKButton = new DarkUI.Controls.DarkButton();
            this.mApplyButton = new DarkUI.Controls.DarkButton();
            this.mHysNumericUpDown = new DarkUI.Controls.DarkNumericUpDown();
            this.mModeGroupBox = new DarkUI.Controls.DarkGroupBox();
            this.mGameRadioButton = new DarkUI.Controls.DarkRadioButton();
            this.mPerformanceRadioButton = new DarkUI.Controls.DarkRadioButton();
            this.mSilenceRadioButton = new DarkUI.Controls.DarkRadioButton();
            this.mNormalRadioButton = new DarkUI.Controls.DarkRadioButton();
            this.mUnitComboBox = new DarkUI.Controls.DarkComboBox();
            this.mPresetLoadButton = new DarkUI.Controls.DarkButton();
            this.mPresetSaveButton = new DarkUI.Controls.DarkButton();
            this.mAutoNumericUpDown = new DarkUI.Controls.DarkNumericUpDown();
            this.mAutoLabel = new DarkUI.Controls.DarkLabel();
            this.mDelayLabel = new DarkUI.Controls.DarkLabel();
            this.mDelayNumericUpDown = new DarkUI.Controls.DarkNumericUpDown();
            this.mDelayLabel2 = new DarkUI.Controls.DarkLabel();
            this.mTempGroupBox.SuspendLayout();
            this.mFanGroupBox.SuspendLayout();
            this.mGraphGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mHysNumericUpDown)).BeginInit();
            this.mModeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mAutoNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mDelayNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // mEnableCheckBox
            // 
            this.mEnableCheckBox.AutoSize = true;
            this.mEnableCheckBox.Location = new System.Drawing.Point(19, 27);
            this.mEnableCheckBox.Name = "mEnableCheckBox";
            this.mEnableCheckBox.Size = new System.Drawing.Size(185, 16);
            this.mEnableCheckBox.TabIndex = 0;
            this.mEnableCheckBox.Text = "Enable automatic fan control";
            // 
            // mTempGroupBox
            // 
            this.mTempGroupBox.Controls.Add(this.mAddTempListView);
            this.mTempGroupBox.Location = new System.Drawing.Point(12, 61);
            this.mTempGroupBox.Name = "mTempGroupBox";
            this.mTempGroupBox.Size = new System.Drawing.Size(305, 211);
            this.mTempGroupBox.TabIndex = 1;
            this.mTempGroupBox.TabStop = false;
            this.mTempGroupBox.Text = "Temperature Sensor";
            // 
            // mAddTempListView
            // 
            this.mAddTempListView.FullRowSelect = true;
            this.mAddTempListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.mAddTempListView.HideSelection = false;
            this.mAddTempListView.Location = new System.Drawing.Point(6, 20);
            this.mAddTempListView.MultiSelect = false;
            this.mAddTempListView.Name = "mAddTempListView";
            this.mAddTempListView.Size = new System.Drawing.Size(293, 185);
            this.mAddTempListView.TabIndex = 5;
            this.mAddTempListView.UseCompatibleStateImageBehavior = false;
            this.mAddTempListView.View = System.Windows.Forms.View.Details;
            // 
            // mFanGroupBox
            // 
            this.mFanGroupBox.Controls.Add(this.mFanListView);
            this.mFanGroupBox.Controls.Add(this.mAddFanListView);
            this.mFanGroupBox.Controls.Add(this.mRemoveButton);
            this.mFanGroupBox.Controls.Add(this.mAddButton);
            this.mFanGroupBox.Location = new System.Drawing.Point(12, 278);
            this.mFanGroupBox.Name = "mFanGroupBox";
            this.mFanGroupBox.Size = new System.Drawing.Size(305, 416);
            this.mFanGroupBox.TabIndex = 2;
            this.mFanGroupBox.TabStop = false;
            this.mFanGroupBox.Text = "Fan";
            // 
            // mFanListView
            // 
            this.mFanListView.FullRowSelect = true;
            this.mFanListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.mFanListView.HideSelection = false;
            this.mFanListView.Location = new System.Drawing.Point(5, 181);
            this.mFanListView.MultiSelect = false;
            this.mFanListView.Name = "mFanListView";
            this.mFanListView.Size = new System.Drawing.Size(292, 192);
            this.mFanListView.TabIndex = 7;
            this.mFanListView.UseCompatibleStateImageBehavior = false;
            this.mFanListView.View = System.Windows.Forms.View.Details;
            // 
            // mAddFanListView
            // 
            this.mAddFanListView.FullRowSelect = true;
            this.mAddFanListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.mAddFanListView.HideSelection = false;
            this.mAddFanListView.Location = new System.Drawing.Point(5, 20);
            this.mAddFanListView.MultiSelect = false;
            this.mAddFanListView.Name = "mAddFanListView";
            this.mAddFanListView.Size = new System.Drawing.Size(231, 155);
            this.mAddFanListView.TabIndex = 6;
            this.mAddFanListView.UseCompatibleStateImageBehavior = false;
            this.mAddFanListView.View = System.Windows.Forms.View.Details;
            // 
            // mRemoveButton
            // 
            this.mRemoveButton.Location = new System.Drawing.Point(6, 379);
            this.mRemoveButton.Name = "mRemoveButton";
            this.mRemoveButton.Padding = new System.Windows.Forms.Padding(1);
            this.mRemoveButton.Size = new System.Drawing.Size(292, 31);
            this.mRemoveButton.TabIndex = 4;
            this.mRemoveButton.Text = "Remove";
            this.mRemoveButton.Click += new System.EventHandler(this.onRemoveButtonClick);
            // 
            // mAddButton
            // 
            this.mAddButton.Location = new System.Drawing.Point(242, 20);
            this.mAddButton.Name = "mAddButton";
            this.mAddButton.Padding = new System.Windows.Forms.Padding(1);
            this.mAddButton.Size = new System.Drawing.Size(55, 155);
            this.mAddButton.TabIndex = 3;
            this.mAddButton.Text = "Add";
            this.mAddButton.Click += new System.EventHandler(this.onAddButtonClick);
            // 
            // mGraphGroupBox
            // 
            this.mGraphGroupBox.Controls.Add(this.mPresetLabel);
            this.mGraphGroupBox.Controls.Add(this.mUnitLabel);
            this.mGraphGroupBox.Controls.Add(this.mHysLabel);
            this.mGraphGroupBox.Controls.Add(this.mStepCheckBox);
            this.mGraphGroupBox.Controls.Add(this.mGraph);
            this.mGraphGroupBox.Location = new System.Drawing.Point(323, 61);
            this.mGraphGroupBox.Name = "mGraphGroupBox";
            this.mGraphGroupBox.Size = new System.Drawing.Size(870, 633);
            this.mGraphGroupBox.TabIndex = 4;
            this.mGraphGroupBox.TabStop = false;
            this.mGraphGroupBox.Text = "Graph";
            // 
            // mPresetLabel
            // 
            this.mPresetLabel.AutoSize = true;
            this.mPresetLabel.Location = new System.Drawing.Point(144, 1);
            this.mPresetLabel.Name = "mPresetLabel";
            this.mPresetLabel.Size = new System.Drawing.Size(49, 12);
            this.mPresetLabel.TabIndex = 5;
            this.mPresetLabel.Text = "Preset :";
            this.mPresetLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mUnitLabel
            // 
            this.mUnitLabel.AutoSize = true;
            this.mUnitLabel.Location = new System.Drawing.Point(329, 1);
            this.mUnitLabel.Name = "mUnitLabel";
            this.mUnitLabel.Size = new System.Drawing.Size(34, 12);
            this.mUnitLabel.TabIndex = 4;
            this.mUnitLabel.Text = "Unit :";
            this.mUnitLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mHysLabel
            // 
            this.mHysLabel.AutoSize = true;
            this.mHysLabel.Location = new System.Drawing.Point(434, 1);
            this.mHysLabel.Name = "mHysLabel";
            this.mHysLabel.Size = new System.Drawing.Size(73, 12);
            this.mHysLabel.TabIndex = 4;
            this.mHysLabel.Text = "Hysteresis :";
            this.mHysLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mStepCheckBox
            // 
            this.mStepCheckBox.AutoSize = true;
            this.mStepCheckBox.Location = new System.Drawing.Point(573, 0);
            this.mStepCheckBox.Name = "mStepCheckBox";
            this.mStepCheckBox.Size = new System.Drawing.Size(49, 16);
            this.mStepCheckBox.TabIndex = 13;
            this.mStepCheckBox.Text = "Step";
            this.mStepCheckBox.CheckedChanged += new System.EventHandler(this.onStepCheckBoxCheckedChanged);
            // 
            // mGraph
            // 
            this.mGraph.Location = new System.Drawing.Point(6, 23);
            this.mGraph.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mGraph.Name = "mGraph";
            this.mGraph.ScrollGrace = 0D;
            this.mGraph.ScrollMaxX = 0D;
            this.mGraph.ScrollMaxY = 0D;
            this.mGraph.ScrollMaxY2 = 0D;
            this.mGraph.ScrollMinX = 0D;
            this.mGraph.ScrollMinY = 0D;
            this.mGraph.ScrollMinY2 = 0D;
            this.mGraph.Size = new System.Drawing.Size(857, 604);
            this.mGraph.TabIndex = 4;
            this.mGraph.ZoomButtons = System.Windows.Forms.MouseButtons.None;
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(1012, 700);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Padding = new System.Windows.Forms.Padding(1);
            this.mOKButton.Size = new System.Drawing.Size(181, 47);
            this.mOKButton.TabIndex = 17;
            this.mOKButton.Text = "OK";
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // mApplyButton
            // 
            this.mApplyButton.Location = new System.Drawing.Point(825, 700);
            this.mApplyButton.Name = "mApplyButton";
            this.mApplyButton.Padding = new System.Windows.Forms.Padding(1);
            this.mApplyButton.Size = new System.Drawing.Size(181, 47);
            this.mApplyButton.TabIndex = 16;
            this.mApplyButton.Text = "Apply";
            this.mApplyButton.Click += new System.EventHandler(this.onApplyButtonClick);
            // 
            // mHysNumericUpDown
            // 
            this.mHysNumericUpDown.Location = new System.Drawing.Point(832, 59);
            this.mHysNumericUpDown.Name = "mHysNumericUpDown";
            this.mHysNumericUpDown.ReadOnly = true;
            this.mHysNumericUpDown.Size = new System.Drawing.Size(38, 21);
            this.mHysNumericUpDown.TabIndex = 12;
            this.mHysNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mModeGroupBox
            // 
            this.mModeGroupBox.Controls.Add(this.mGameRadioButton);
            this.mModeGroupBox.Controls.Add(this.mPerformanceRadioButton);
            this.mModeGroupBox.Controls.Add(this.mSilenceRadioButton);
            this.mModeGroupBox.Controls.Add(this.mNormalRadioButton);
            this.mModeGroupBox.Location = new System.Drawing.Point(323, 9);
            this.mModeGroupBox.Name = "mModeGroupBox";
            this.mModeGroupBox.Size = new System.Drawing.Size(870, 43);
            this.mModeGroupBox.TabIndex = 3;
            this.mModeGroupBox.TabStop = false;
            this.mModeGroupBox.Text = "Mode";
            // 
            // mGameRadioButton
            // 
            this.mGameRadioButton.AutoSize = true;
            this.mGameRadioButton.Location = new System.Drawing.Point(384, 18);
            this.mGameRadioButton.Name = "mGameRadioButton";
            this.mGameRadioButton.Size = new System.Drawing.Size(57, 16);
            this.mGameRadioButton.TabIndex = 8;
            this.mGameRadioButton.TabStop = true;
            this.mGameRadioButton.Text = "Game";
            // 
            // mPerformanceRadioButton
            // 
            this.mPerformanceRadioButton.AutoSize = true;
            this.mPerformanceRadioButton.Location = new System.Drawing.Point(255, 18);
            this.mPerformanceRadioButton.Name = "mPerformanceRadioButton";
            this.mPerformanceRadioButton.Size = new System.Drawing.Size(95, 16);
            this.mPerformanceRadioButton.TabIndex = 7;
            this.mPerformanceRadioButton.TabStop = true;
            this.mPerformanceRadioButton.Text = "Performance";
            // 
            // mSilenceRadioButton
            // 
            this.mSilenceRadioButton.AutoSize = true;
            this.mSilenceRadioButton.Location = new System.Drawing.Point(146, 18);
            this.mSilenceRadioButton.Name = "mSilenceRadioButton";
            this.mSilenceRadioButton.Size = new System.Drawing.Size(65, 16);
            this.mSilenceRadioButton.TabIndex = 6;
            this.mSilenceRadioButton.TabStop = true;
            this.mSilenceRadioButton.Text = "Silence";
            // 
            // mNormalRadioButton
            // 
            this.mNormalRadioButton.AutoSize = true;
            this.mNormalRadioButton.Location = new System.Drawing.Point(41, 18);
            this.mNormalRadioButton.Name = "mNormalRadioButton";
            this.mNormalRadioButton.Size = new System.Drawing.Size(64, 16);
            this.mNormalRadioButton.TabIndex = 5;
            this.mNormalRadioButton.TabStop = true;
            this.mNormalRadioButton.Text = "Normal";
            // 
            // mUnitComboBox
            // 
            this.mUnitComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.mUnitComboBox.FormattingEnabled = true;
            this.mUnitComboBox.Location = new System.Drawing.Point(689, 58);
            this.mUnitComboBox.Name = "mUnitComboBox";
            this.mUnitComboBox.Size = new System.Drawing.Size(44, 22);
            this.mUnitComboBox.TabIndex = 11;
            // 
            // mPresetLoadButton
            // 
            this.mPresetLoadButton.Location = new System.Drawing.Point(520, 56);
            this.mPresetLoadButton.Name = "mPresetLoadButton";
            this.mPresetLoadButton.Padding = new System.Windows.Forms.Padding(1);
            this.mPresetLoadButton.Size = new System.Drawing.Size(57, 23);
            this.mPresetLoadButton.TabIndex = 9;
            this.mPresetLoadButton.Text = "Load";
            this.mPresetLoadButton.Click += new System.EventHandler(this.onPresetLoadButtonClick);
            // 
            // mPresetSaveButton
            // 
            this.mPresetSaveButton.Location = new System.Drawing.Point(579, 56);
            this.mPresetSaveButton.Name = "mPresetSaveButton";
            this.mPresetSaveButton.Padding = new System.Windows.Forms.Padding(1);
            this.mPresetSaveButton.Size = new System.Drawing.Size(57, 23);
            this.mPresetSaveButton.TabIndex = 10;
            this.mPresetSaveButton.Text = "Save";
            this.mPresetSaveButton.Click += new System.EventHandler(this.onPresetSaveButtonClick);
            // 
            // mAutoNumericUpDown
            // 
            this.mAutoNumericUpDown.AutoSize = true;
            this.mAutoNumericUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.mAutoNumericUpDown.Location = new System.Drawing.Point(998, 59);
            this.mAutoNumericUpDown.Name = "mAutoNumericUpDown";
            this.mAutoNumericUpDown.ReadOnly = true;
            this.mAutoNumericUpDown.Size = new System.Drawing.Size(41, 21);
            this.mAutoNumericUpDown.TabIndex = 14;
            this.mAutoNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mAutoLabel
            // 
            this.mAutoLabel.AutoSize = true;
            this.mAutoLabel.Location = new System.Drawing.Point(957, 62);
            this.mAutoLabel.Name = "mAutoLabel";
            this.mAutoLabel.Size = new System.Drawing.Size(38, 12);
            this.mAutoLabel.TabIndex = 6;
            this.mAutoLabel.Text = "Auto :";
            this.mAutoLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mDelayLabel
            // 
            this.mDelayLabel.AutoSize = true;
            this.mDelayLabel.Location = new System.Drawing.Point(1061, 62);
            this.mDelayLabel.Name = "mDelayLabel";
            this.mDelayLabel.Size = new System.Drawing.Size(45, 12);
            this.mDelayLabel.TabIndex = 8;
            this.mDelayLabel.Text = "Delay :";
            this.mDelayLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mDelayNumericUpDown
            // 
            this.mDelayNumericUpDown.Location = new System.Drawing.Point(1109, 58);
            this.mDelayNumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.mDelayNumericUpDown.Name = "mDelayNumericUpDown";
            this.mDelayNumericUpDown.Size = new System.Drawing.Size(50, 21);
            this.mDelayNumericUpDown.TabIndex = 15;
            this.mDelayNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mDelayLabel2
            // 
            this.mDelayLabel2.AutoSize = true;
            this.mDelayLabel2.Location = new System.Drawing.Point(1161, 61);
            this.mDelayLabel2.Name = "mDelayLabel2";
            this.mDelayLabel2.Size = new System.Drawing.Size(23, 12);
            this.mDelayLabel2.TabIndex = 18;
            this.mDelayLabel2.Text = "ms";
            this.mDelayLabel2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ControlForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1203, 757);
            this.Controls.Add(this.mDelayLabel2);
            this.Controls.Add(this.mDelayLabel);
            this.Controls.Add(this.mDelayNumericUpDown);
            this.Controls.Add(this.mAutoLabel);
            this.Controls.Add(this.mAutoNumericUpDown);
            this.Controls.Add(this.mPresetSaveButton);
            this.Controls.Add(this.mPresetLoadButton);
            this.Controls.Add(this.mUnitComboBox);
            this.Controls.Add(this.mModeGroupBox);
            this.Controls.Add(this.mHysNumericUpDown);
            this.Controls.Add(this.mApplyButton);
            this.Controls.Add(this.mOKButton);
            this.Controls.Add(this.mGraphGroupBox);
            this.Controls.Add(this.mFanGroupBox);
            this.Controls.Add(this.mTempGroupBox);
            this.Controls.Add(this.mEnableCheckBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1219, 796);
            this.Name = "ControlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FanCtrl";
            this.mTempGroupBox.ResumeLayout(false);
            this.mFanGroupBox.ResumeLayout(false);
            this.mGraphGroupBox.ResumeLayout(false);
            this.mGraphGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mHysNumericUpDown)).EndInit();
            this.mModeGroupBox.ResumeLayout(false);
            this.mModeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mAutoNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mDelayNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkCheckBox mEnableCheckBox;
        private DarkGroupBox mTempGroupBox;
        private DarkGroupBox mFanGroupBox;
        private DarkButton mRemoveButton;
        private DarkButton mAddButton;
        private DarkGroupBox mGraphGroupBox;
        private ZedGraph.ZedGraphControl mGraph;
        private DarkCheckBox mStepCheckBox;
        private DarkButton mOKButton;
        private DarkButton mApplyButton;
        private DarkNumericUpDown mHysNumericUpDown;
        private DarkLabel mHysLabel;
        private DarkGroupBox mModeGroupBox;
        private DarkRadioButton mGameRadioButton;
        private DarkRadioButton mPerformanceRadioButton;
        private DarkRadioButton mSilenceRadioButton;
        private DarkRadioButton mNormalRadioButton;
        private DarkLabel mUnitLabel;
        private DarkComboBox mUnitComboBox;
        private DarkLabel mPresetLabel;
        private DarkButton mPresetLoadButton;
        private DarkButton mPresetSaveButton;
        private DarkNumericUpDown mAutoNumericUpDown;
        private DarkLabel mAutoLabel;
        private DarkLabel mDelayLabel;
        private DarkNumericUpDown mDelayNumericUpDown;
        private DarkLabel mDelayLabel2;
        private ThemeListView mAddTempListView;
        private ThemeListView mAddFanListView;
        private ThemeListView mFanListView;
    }
}