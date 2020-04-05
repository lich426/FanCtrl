namespace FanControl
{
    partial class FanControlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FanControlForm));
            this.mEnableCheckBox = new System.Windows.Forms.CheckBox();
            this.mSensorGroupBox = new System.Windows.Forms.GroupBox();
            this.mSensorComboBox = new System.Windows.Forms.ComboBox();
            this.mFanGroupBox = new System.Windows.Forms.GroupBox();
            this.mRemoveButton = new System.Windows.Forms.Button();
            this.mAddButton = new System.Windows.Forms.Button();
            this.mFanListView = new System.Windows.Forms.ListView();
            this.mFanComboBox = new System.Windows.Forms.ComboBox();
            this.mGraphGroupBox = new System.Windows.Forms.GroupBox();
            this.mStepCheckBox = new System.Windows.Forms.CheckBox();
            this.mGraph = new ZedGraph.ZedGraphControl();
            this.mOKButton = new System.Windows.Forms.Button();
            this.mApplyButton = new System.Windows.Forms.Button();
            this.mSensorGroupBox.SuspendLayout();
            this.mFanGroupBox.SuspendLayout();
            this.mGraphGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mEnableCheckBox
            // 
            this.mEnableCheckBox.AutoSize = true;
            this.mEnableCheckBox.Location = new System.Drawing.Point(15, 17);
            this.mEnableCheckBox.Name = "mEnableCheckBox";
            this.mEnableCheckBox.Size = new System.Drawing.Size(185, 16);
            this.mEnableCheckBox.TabIndex = 0;
            this.mEnableCheckBox.Text = "Enable automatic fan control";
            this.mEnableCheckBox.UseVisualStyleBackColor = true;
            // 
            // mSensorGroupBox
            // 
            this.mSensorGroupBox.Controls.Add(this.mSensorComboBox);
            this.mSensorGroupBox.Location = new System.Drawing.Point(12, 49);
            this.mSensorGroupBox.Name = "mSensorGroupBox";
            this.mSensorGroupBox.Size = new System.Drawing.Size(208, 51);
            this.mSensorGroupBox.TabIndex = 1;
            this.mSensorGroupBox.TabStop = false;
            this.mSensorGroupBox.Text = "Sensor";
            // 
            // mSensorComboBox
            // 
            this.mSensorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mSensorComboBox.FormattingEnabled = true;
            this.mSensorComboBox.Location = new System.Drawing.Point(7, 21);
            this.mSensorComboBox.Name = "mSensorComboBox";
            this.mSensorComboBox.Size = new System.Drawing.Size(195, 20);
            this.mSensorComboBox.TabIndex = 0;
            // 
            // mFanGroupBox
            // 
            this.mFanGroupBox.Controls.Add(this.mRemoveButton);
            this.mFanGroupBox.Controls.Add(this.mAddButton);
            this.mFanGroupBox.Controls.Add(this.mFanListView);
            this.mFanGroupBox.Controls.Add(this.mFanComboBox);
            this.mFanGroupBox.Location = new System.Drawing.Point(12, 106);
            this.mFanGroupBox.Name = "mFanGroupBox";
            this.mFanGroupBox.Size = new System.Drawing.Size(208, 210);
            this.mFanGroupBox.TabIndex = 2;
            this.mFanGroupBox.TabStop = false;
            this.mFanGroupBox.Text = "Fan";
            // 
            // mRemoveButton
            // 
            this.mRemoveButton.Location = new System.Drawing.Point(7, 171);
            this.mRemoveButton.Name = "mRemoveButton";
            this.mRemoveButton.Size = new System.Drawing.Size(195, 33);
            this.mRemoveButton.TabIndex = 3;
            this.mRemoveButton.Text = "Remove";
            this.mRemoveButton.UseVisualStyleBackColor = true;
            this.mRemoveButton.Click += new System.EventHandler(this.onRemoveButtonClick);
            // 
            // mAddButton
            // 
            this.mAddButton.Location = new System.Drawing.Point(135, 15);
            this.mAddButton.Name = "mAddButton";
            this.mAddButton.Size = new System.Drawing.Size(67, 28);
            this.mAddButton.TabIndex = 2;
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
            this.mFanListView.Size = new System.Drawing.Size(195, 116);
            this.mFanListView.TabIndex = 1;
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
            this.mFanComboBox.TabIndex = 0;
            // 
            // mGraphGroupBox
            // 
            this.mGraphGroupBox.Controls.Add(this.mStepCheckBox);
            this.mGraphGroupBox.Controls.Add(this.mGraph);
            this.mGraphGroupBox.Location = new System.Drawing.Point(227, 49);
            this.mGraphGroupBox.Name = "mGraphGroupBox";
            this.mGraphGroupBox.Size = new System.Drawing.Size(493, 267);
            this.mGraphGroupBox.TabIndex = 3;
            this.mGraphGroupBox.TabStop = false;
            this.mGraphGroupBox.Text = "Graph";
            // 
            // mStepCheckBox
            // 
            this.mStepCheckBox.AutoSize = true;
            this.mStepCheckBox.Location = new System.Drawing.Point(414, 25);
            this.mStepCheckBox.Name = "mStepCheckBox";
            this.mStepCheckBox.Size = new System.Drawing.Size(49, 16);
            this.mStepCheckBox.TabIndex = 4;
            this.mStepCheckBox.Text = "Step";
            this.mStepCheckBox.UseVisualStyleBackColor = true;
            this.mStepCheckBox.CheckedChanged += new System.EventHandler(this.onStepCheckBoxCheckedChanged);
            // 
            // mGraph
            // 
            this.mGraph.Location = new System.Drawing.Point(6, 21);
            this.mGraph.Name = "mGraph";
            this.mGraph.ScrollGrace = 0D;
            this.mGraph.ScrollMaxX = 0D;
            this.mGraph.ScrollMaxY = 0D;
            this.mGraph.ScrollMaxY2 = 0D;
            this.mGraph.ScrollMinX = 0D;
            this.mGraph.ScrollMinY = 0D;
            this.mGraph.ScrollMinY2 = 0D;
            this.mGraph.Size = new System.Drawing.Size(481, 240);
            this.mGraph.TabIndex = 0;
            this.mGraph.ZoomButtons = System.Windows.Forms.MouseButtons.None;
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(585, 322);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(135, 33);
            this.mOKButton.TabIndex = 4;
            this.mOKButton.Text = "OK";
            this.mOKButton.UseVisualStyleBackColor = true;
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // mApplyButton
            // 
            this.mApplyButton.Location = new System.Drawing.Point(444, 322);
            this.mApplyButton.Name = "mApplyButton";
            this.mApplyButton.Size = new System.Drawing.Size(135, 33);
            this.mApplyButton.TabIndex = 5;
            this.mApplyButton.Text = "Apply";
            this.mApplyButton.UseVisualStyleBackColor = true;
            this.mApplyButton.Click += new System.EventHandler(this.onApplyButtonClick);
            // 
            // FanControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(732, 364);
            this.Controls.Add(this.mApplyButton);
            this.Controls.Add(this.mOKButton);
            this.Controls.Add(this.mGraphGroupBox);
            this.Controls.Add(this.mFanGroupBox);
            this.Controls.Add(this.mSensorGroupBox);
            this.Controls.Add(this.mEnableCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FanControlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FanControl";
            this.mSensorGroupBox.ResumeLayout(false);
            this.mFanGroupBox.ResumeLayout(false);
            this.mGraphGroupBox.ResumeLayout(false);
            this.mGraphGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox mEnableCheckBox;
        private System.Windows.Forms.GroupBox mSensorGroupBox;
        private System.Windows.Forms.ComboBox mSensorComboBox;
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
    }
}