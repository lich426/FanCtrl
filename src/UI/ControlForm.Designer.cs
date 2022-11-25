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
            this.mEnableCheckBox = new System.Windows.Forms.CheckBox();
            this.mTempGroupBox = new System.Windows.Forms.GroupBox();
            this.mTempComboBox = new System.Windows.Forms.ComboBox();
            this.mFanGroupBox = new System.Windows.Forms.GroupBox();
            this.mRemoveButton = new System.Windows.Forms.Button();
            this.mAddButton = new System.Windows.Forms.Button();
            this.mFanListView = new System.Windows.Forms.ListView();
            this.mFanComboBox = new System.Windows.Forms.ComboBox();
            this.mGraphGroupBox = new System.Windows.Forms.GroupBox();
            this.mPresetLabel = new System.Windows.Forms.Label();
            this.mUnitLabel = new System.Windows.Forms.Label();
            this.mHysLabel = new System.Windows.Forms.Label();
            this.mStepCheckBox = new System.Windows.Forms.CheckBox();
            this.mGraph = new ZedGraph.ZedGraphControl();
            this.mOKButton = new System.Windows.Forms.Button();
            this.mApplyButton = new System.Windows.Forms.Button();
            this.mHysNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mModeGroupBox = new System.Windows.Forms.GroupBox();
            this.mGameRadioButton = new System.Windows.Forms.RadioButton();
            this.mPerformanceRadioButton = new System.Windows.Forms.RadioButton();
            this.mSilenceRadioButton = new System.Windows.Forms.RadioButton();
            this.mNormalRadioButton = new System.Windows.Forms.RadioButton();
            this.mUnitComboBox = new System.Windows.Forms.ComboBox();
            this.mPresetLoadButton = new System.Windows.Forms.Button();
            this.mPresetSaveButton = new System.Windows.Forms.Button();
            this.mAutoNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mAutoLabel = new System.Windows.Forms.Label();
            this.mDelayLabel = new System.Windows.Forms.Label();
            this.mDelayNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mDelayLabel2 = new System.Windows.Forms.Label();
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
            this.mEnableCheckBox.UseVisualStyleBackColor = true;
            // 
            // mTempGroupBox
            // 
            this.mTempGroupBox.Controls.Add(this.mTempComboBox);
            this.mTempGroupBox.Location = new System.Drawing.Point(12, 61);
            this.mTempGroupBox.Name = "mTempGroupBox";
            this.mTempGroupBox.Size = new System.Drawing.Size(208, 51);
            this.mTempGroupBox.TabIndex = 1;
            this.mTempGroupBox.TabStop = false;
            this.mTempGroupBox.Text = "Temperature Sensor";
            // 
            // mTempComboBox
            // 
            this.mTempComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mTempComboBox.FormattingEnabled = true;
            this.mTempComboBox.Location = new System.Drawing.Point(7, 21);
            this.mTempComboBox.Name = "mTempComboBox";
            this.mTempComboBox.Size = new System.Drawing.Size(195, 20);
            this.mTempComboBox.TabIndex = 1;
            // 
            // mFanGroupBox
            // 
            this.mFanGroupBox.Controls.Add(this.mRemoveButton);
            this.mFanGroupBox.Controls.Add(this.mAddButton);
            this.mFanGroupBox.Controls.Add(this.mFanListView);
            this.mFanGroupBox.Controls.Add(this.mFanComboBox);
            this.mFanGroupBox.Location = new System.Drawing.Point(12, 118);
            this.mFanGroupBox.Name = "mFanGroupBox";
            this.mFanGroupBox.Size = new System.Drawing.Size(208, 401);
            this.mFanGroupBox.TabIndex = 2;
            this.mFanGroupBox.TabStop = false;
            this.mFanGroupBox.Text = "Fan";
            // 
            // mRemoveButton
            // 
            this.mRemoveButton.Location = new System.Drawing.Point(7, 337);
            this.mRemoveButton.Name = "mRemoveButton";
            this.mRemoveButton.Size = new System.Drawing.Size(195, 58);
            this.mRemoveButton.TabIndex = 4;
            this.mRemoveButton.Text = "Remove";
            this.mRemoveButton.UseVisualStyleBackColor = true;
            this.mRemoveButton.Click += new System.EventHandler(this.onRemoveButtonClick);
            // 
            // mAddButton
            // 
            this.mAddButton.Location = new System.Drawing.Point(135, 15);
            this.mAddButton.Name = "mAddButton";
            this.mAddButton.Size = new System.Drawing.Size(67, 28);
            this.mAddButton.TabIndex = 3;
            this.mAddButton.Text = "Add";
            this.mAddButton.UseVisualStyleBackColor = true;
            this.mAddButton.Click += new System.EventHandler(this.onAddButtonClick);
            // 
            // mFanListView
            // 
            this.mFanListView.FullRowSelect = true;
            this.mFanListView.HideSelection = false;
            this.mFanListView.Location = new System.Drawing.Point(7, 49);
            this.mFanListView.MultiSelect = false;
            this.mFanListView.Name = "mFanListView";
            this.mFanListView.Size = new System.Drawing.Size(195, 282);
            this.mFanListView.TabIndex = 2;
            this.mFanListView.UseCompatibleStateImageBehavior = false;
            this.mFanListView.View = System.Windows.Forms.View.List;
            // 
            // mFanComboBox
            // 
            this.mFanComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mFanComboBox.FormattingEnabled = true;
            this.mFanComboBox.Location = new System.Drawing.Point(7, 20);
            this.mFanComboBox.Name = "mFanComboBox";
            this.mFanComboBox.Size = new System.Drawing.Size(125, 20);
            this.mFanComboBox.TabIndex = 2;
            // 
            // mGraphGroupBox
            // 
            this.mGraphGroupBox.Controls.Add(this.mPresetLabel);
            this.mGraphGroupBox.Controls.Add(this.mUnitLabel);
            this.mGraphGroupBox.Controls.Add(this.mHysLabel);
            this.mGraphGroupBox.Controls.Add(this.mStepCheckBox);
            this.mGraphGroupBox.Controls.Add(this.mGraph);
            this.mGraphGroupBox.Location = new System.Drawing.Point(227, 61);
            this.mGraphGroupBox.Name = "mGraphGroupBox";
            this.mGraphGroupBox.Size = new System.Drawing.Size(870, 458);
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
            this.mStepCheckBox.UseVisualStyleBackColor = true;
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
            this.mGraph.Size = new System.Drawing.Size(857, 429);
            this.mGraph.TabIndex = 4;
            this.mGraph.ZoomButtons = System.Windows.Forms.MouseButtons.None;
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(916, 525);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(181, 47);
            this.mOKButton.TabIndex = 17;
            this.mOKButton.Text = "OK";
            this.mOKButton.UseVisualStyleBackColor = true;
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // mApplyButton
            // 
            this.mApplyButton.Location = new System.Drawing.Point(729, 525);
            this.mApplyButton.Name = "mApplyButton";
            this.mApplyButton.Size = new System.Drawing.Size(181, 47);
            this.mApplyButton.TabIndex = 16;
            this.mApplyButton.Text = "Apply";
            this.mApplyButton.UseVisualStyleBackColor = true;
            this.mApplyButton.Click += new System.EventHandler(this.onApplyButtonClick);
            // 
            // mHysNumericUpDown
            // 
            this.mHysNumericUpDown.Location = new System.Drawing.Point(736, 59);
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
            this.mModeGroupBox.Location = new System.Drawing.Point(227, 9);
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
            this.mGameRadioButton.UseVisualStyleBackColor = true;
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
            this.mPerformanceRadioButton.UseVisualStyleBackColor = true;
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
            this.mSilenceRadioButton.UseVisualStyleBackColor = true;
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
            this.mNormalRadioButton.UseVisualStyleBackColor = true;
            // 
            // mUnitComboBox
            // 
            this.mUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mUnitComboBox.FormattingEnabled = true;
            this.mUnitComboBox.Location = new System.Drawing.Point(593, 58);
            this.mUnitComboBox.Name = "mUnitComboBox";
            this.mUnitComboBox.Size = new System.Drawing.Size(44, 20);
            this.mUnitComboBox.TabIndex = 11;
            // 
            // mPresetLoadButton
            // 
            this.mPresetLoadButton.Location = new System.Drawing.Point(424, 56);
            this.mPresetLoadButton.Name = "mPresetLoadButton";
            this.mPresetLoadButton.Size = new System.Drawing.Size(57, 23);
            this.mPresetLoadButton.TabIndex = 9;
            this.mPresetLoadButton.Text = "Load";
            this.mPresetLoadButton.UseVisualStyleBackColor = true;
            this.mPresetLoadButton.Click += new System.EventHandler(this.onPresetLoadButtonClick);
            // 
            // mPresetSaveButton
            // 
            this.mPresetSaveButton.Location = new System.Drawing.Point(483, 56);
            this.mPresetSaveButton.Name = "mPresetSaveButton";
            this.mPresetSaveButton.Size = new System.Drawing.Size(57, 23);
            this.mPresetSaveButton.TabIndex = 10;
            this.mPresetSaveButton.Text = "Save";
            this.mPresetSaveButton.UseVisualStyleBackColor = true;
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
            this.mAutoNumericUpDown.Location = new System.Drawing.Point(902, 59);
            this.mAutoNumericUpDown.Name = "mAutoNumericUpDown";
            this.mAutoNumericUpDown.ReadOnly = true;
            this.mAutoNumericUpDown.Size = new System.Drawing.Size(41, 21);
            this.mAutoNumericUpDown.TabIndex = 14;
            this.mAutoNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mAutoLabel
            // 
            this.mAutoLabel.AutoSize = true;
            this.mAutoLabel.Location = new System.Drawing.Point(861, 62);
            this.mAutoLabel.Name = "mAutoLabel";
            this.mAutoLabel.Size = new System.Drawing.Size(38, 12);
            this.mAutoLabel.TabIndex = 6;
            this.mAutoLabel.Text = "Auto :";
            this.mAutoLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mDelayLabel
            // 
            this.mDelayLabel.AutoSize = true;
            this.mDelayLabel.Location = new System.Drawing.Point(965, 62);
            this.mDelayLabel.Name = "mDelayLabel";
            this.mDelayLabel.Size = new System.Drawing.Size(45, 12);
            this.mDelayLabel.TabIndex = 8;
            this.mDelayLabel.Text = "Delay :";
            this.mDelayLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mDelayNumericUpDown
            // 
            this.mDelayNumericUpDown.Location = new System.Drawing.Point(1013, 58);
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
            this.mDelayLabel2.Location = new System.Drawing.Point(1065, 61);
            this.mDelayLabel2.Name = "mDelayLabel2";
            this.mDelayLabel2.Size = new System.Drawing.Size(23, 12);
            this.mDelayLabel2.TabIndex = 18;
            this.mDelayLabel2.Text = "ms";
            this.mDelayLabel2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ControlForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1108, 584);
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
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1124, 623);
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

        private System.Windows.Forms.CheckBox mEnableCheckBox;
        private System.Windows.Forms.GroupBox mTempGroupBox;
        private System.Windows.Forms.ComboBox mTempComboBox;
        private System.Windows.Forms.GroupBox mFanGroupBox;
        private System.Windows.Forms.ListView mFanListView;
        private System.Windows.Forms.ComboBox mFanComboBox;
        private System.Windows.Forms.Button mRemoveButton;
        private System.Windows.Forms.Button mAddButton;
        private System.Windows.Forms.GroupBox mGraphGroupBox;
        private ZedGraph.ZedGraphControl mGraph;
        private System.Windows.Forms.CheckBox mStepCheckBox;
        private System.Windows.Forms.Button mOKButton;
        private System.Windows.Forms.Button mApplyButton;
        private System.Windows.Forms.NumericUpDown mHysNumericUpDown;
        private System.Windows.Forms.Label mHysLabel;
        private System.Windows.Forms.GroupBox mModeGroupBox;
        private System.Windows.Forms.RadioButton mGameRadioButton;
        private System.Windows.Forms.RadioButton mPerformanceRadioButton;
        private System.Windows.Forms.RadioButton mSilenceRadioButton;
        private System.Windows.Forms.RadioButton mNormalRadioButton;
        private System.Windows.Forms.Label mUnitLabel;
        private System.Windows.Forms.ComboBox mUnitComboBox;
        private System.Windows.Forms.Label mPresetLabel;
        private System.Windows.Forms.Button mPresetLoadButton;
        private System.Windows.Forms.Button mPresetSaveButton;
        private System.Windows.Forms.NumericUpDown mAutoNumericUpDown;
        private System.Windows.Forms.Label mAutoLabel;
        private System.Windows.Forms.Label mDelayLabel;
        private System.Windows.Forms.NumericUpDown mDelayNumericUpDown;
        private System.Windows.Forms.Label mDelayLabel2;
    }
}