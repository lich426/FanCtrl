namespace FanCtrl
{
    partial class PluginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginForm));
            this.mServerButton = new System.Windows.Forms.Button();
            this.mServerGroupBox = new System.Windows.Forms.GroupBox();
            this.mPortNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mPortLabel = new System.Windows.Forms.Label();
            this.mTempGroupBox = new System.Windows.Forms.GroupBox();
            this.mTempAddButton = new System.Windows.Forms.Button();
            this.mTempRemoveButton = new System.Windows.Forms.Button();
            this.mTempListView = new System.Windows.Forms.ListView();
            this.mFanSpeedGroupBox = new System.Windows.Forms.GroupBox();
            this.mFanSpeedAddButton = new System.Windows.Forms.Button();
            this.mFanSpeedListView = new System.Windows.Forms.ListView();
            this.mFanSpeedRemoveButton = new System.Windows.Forms.Button();
            this.mFanControlGroupBox = new System.Windows.Forms.GroupBox();
            this.mFanControlAddButton = new System.Windows.Forms.Button();
            this.mFanControlListView = new System.Windows.Forms.ListView();
            this.mFanControlRemoveButton = new System.Windows.Forms.Button();
            this.mOKButton = new System.Windows.Forms.Button();
            this.mClientGroupBox = new System.Windows.Forms.GroupBox();
            this.mClientListView = new System.Windows.Forms.ListView();
            this.mServerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mPortNumericUpDown)).BeginInit();
            this.mTempGroupBox.SuspendLayout();
            this.mFanSpeedGroupBox.SuspendLayout();
            this.mFanControlGroupBox.SuspendLayout();
            this.mClientGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mServerButton
            // 
            this.mServerButton.Location = new System.Drawing.Point(7, 67);
            this.mServerButton.Name = "mServerButton";
            this.mServerButton.Size = new System.Drawing.Size(194, 51);
            this.mServerButton.TabIndex = 2;
            this.mServerButton.Text = "Start";
            this.mServerButton.UseVisualStyleBackColor = true;
            this.mServerButton.Click += new System.EventHandler(this.onServerButtonClick);
            // 
            // mServerGroupBox
            // 
            this.mServerGroupBox.Controls.Add(this.mPortNumericUpDown);
            this.mServerGroupBox.Controls.Add(this.mPortLabel);
            this.mServerGroupBox.Controls.Add(this.mServerButton);
            this.mServerGroupBox.Location = new System.Drawing.Point(12, 12);
            this.mServerGroupBox.Name = "mServerGroupBox";
            this.mServerGroupBox.Size = new System.Drawing.Size(207, 137);
            this.mServerGroupBox.TabIndex = 3;
            this.mServerGroupBox.TabStop = false;
            this.mServerGroupBox.Text = "Server (Stopped)";
            // 
            // mPortNumericUpDown
            // 
            this.mPortNumericUpDown.Location = new System.Drawing.Point(78, 32);
            this.mPortNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.mPortNumericUpDown.Name = "mPortNumericUpDown";
            this.mPortNumericUpDown.Size = new System.Drawing.Size(123, 21);
            this.mPortNumericUpDown.TabIndex = 1;
            this.mPortNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mPortNumericUpDown.Value = new decimal(new int[] {
            9989,
            0,
            0,
            0});
            // 
            // mPortLabel
            // 
            this.mPortLabel.AutoSize = true;
            this.mPortLabel.Location = new System.Drawing.Point(10, 34);
            this.mPortLabel.Name = "mPortLabel";
            this.mPortLabel.Size = new System.Drawing.Size(35, 12);
            this.mPortLabel.TabIndex = 3;
            this.mPortLabel.Text = "Port :";
            // 
            // mTempGroupBox
            // 
            this.mTempGroupBox.Controls.Add(this.mTempAddButton);
            this.mTempGroupBox.Controls.Add(this.mTempRemoveButton);
            this.mTempGroupBox.Controls.Add(this.mTempListView);
            this.mTempGroupBox.Location = new System.Drawing.Point(12, 155);
            this.mTempGroupBox.Name = "mTempGroupBox";
            this.mTempGroupBox.Size = new System.Drawing.Size(233, 410);
            this.mTempGroupBox.TabIndex = 4;
            this.mTempGroupBox.TabStop = false;
            this.mTempGroupBox.Text = "Temperature";
            // 
            // mTempAddButton
            // 
            this.mTempAddButton.Location = new System.Drawing.Point(120, 365);
            this.mTempAddButton.Name = "mTempAddButton";
            this.mTempAddButton.Size = new System.Drawing.Size(107, 38);
            this.mTempAddButton.TabIndex = 4;
            this.mTempAddButton.Text = "Add";
            this.mTempAddButton.UseVisualStyleBackColor = true;
            this.mTempAddButton.Click += new System.EventHandler(this.onTempAddButtonClick);
            // 
            // mTempRemoveButton
            // 
            this.mTempRemoveButton.Location = new System.Drawing.Point(7, 365);
            this.mTempRemoveButton.Name = "mTempRemoveButton";
            this.mTempRemoveButton.Size = new System.Drawing.Size(107, 38);
            this.mTempRemoveButton.TabIndex = 3;
            this.mTempRemoveButton.Text = "Remove";
            this.mTempRemoveButton.UseVisualStyleBackColor = true;
            this.mTempRemoveButton.Click += new System.EventHandler(this.onTempRemoveButtonClick);
            // 
            // mTempListView
            // 
            this.mTempListView.FullRowSelect = true;
            this.mTempListView.GridLines = true;
            this.mTempListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.mTempListView.HideSelection = false;
            this.mTempListView.Location = new System.Drawing.Point(7, 21);
            this.mTempListView.Name = "mTempListView";
            this.mTempListView.Size = new System.Drawing.Size(220, 339);
            this.mTempListView.TabIndex = 0;
            this.mTempListView.UseCompatibleStateImageBehavior = false;
            this.mTempListView.View = System.Windows.Forms.View.Details;
            // 
            // mFanSpeedGroupBox
            // 
            this.mFanSpeedGroupBox.Controls.Add(this.mFanSpeedAddButton);
            this.mFanSpeedGroupBox.Controls.Add(this.mFanSpeedListView);
            this.mFanSpeedGroupBox.Controls.Add(this.mFanSpeedRemoveButton);
            this.mFanSpeedGroupBox.Location = new System.Drawing.Point(251, 155);
            this.mFanSpeedGroupBox.Name = "mFanSpeedGroupBox";
            this.mFanSpeedGroupBox.Size = new System.Drawing.Size(233, 410);
            this.mFanSpeedGroupBox.TabIndex = 5;
            this.mFanSpeedGroupBox.TabStop = false;
            this.mFanSpeedGroupBox.Text = "Fan speed";
            // 
            // mFanSpeedAddButton
            // 
            this.mFanSpeedAddButton.Location = new System.Drawing.Point(120, 366);
            this.mFanSpeedAddButton.Name = "mFanSpeedAddButton";
            this.mFanSpeedAddButton.Size = new System.Drawing.Size(107, 38);
            this.mFanSpeedAddButton.TabIndex = 6;
            this.mFanSpeedAddButton.Text = "Add";
            this.mFanSpeedAddButton.UseVisualStyleBackColor = true;
            this.mFanSpeedAddButton.Click += new System.EventHandler(this.onFanSpeedAddButtonClick);
            // 
            // mFanSpeedListView
            // 
            this.mFanSpeedListView.FullRowSelect = true;
            this.mFanSpeedListView.GridLines = true;
            this.mFanSpeedListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.mFanSpeedListView.HideSelection = false;
            this.mFanSpeedListView.Location = new System.Drawing.Point(6, 20);
            this.mFanSpeedListView.Name = "mFanSpeedListView";
            this.mFanSpeedListView.Size = new System.Drawing.Size(221, 339);
            this.mFanSpeedListView.TabIndex = 0;
            this.mFanSpeedListView.UseCompatibleStateImageBehavior = false;
            this.mFanSpeedListView.View = System.Windows.Forms.View.Details;
            // 
            // mFanSpeedRemoveButton
            // 
            this.mFanSpeedRemoveButton.Location = new System.Drawing.Point(6, 365);
            this.mFanSpeedRemoveButton.Name = "mFanSpeedRemoveButton";
            this.mFanSpeedRemoveButton.Size = new System.Drawing.Size(107, 38);
            this.mFanSpeedRemoveButton.TabIndex = 5;
            this.mFanSpeedRemoveButton.Text = "Remove";
            this.mFanSpeedRemoveButton.UseVisualStyleBackColor = true;
            this.mFanSpeedRemoveButton.Click += new System.EventHandler(this.onFanSpeedRemoveButtonClick);
            // 
            // mFanControlGroupBox
            // 
            this.mFanControlGroupBox.Controls.Add(this.mFanControlAddButton);
            this.mFanControlGroupBox.Controls.Add(this.mFanControlListView);
            this.mFanControlGroupBox.Controls.Add(this.mFanControlRemoveButton);
            this.mFanControlGroupBox.Location = new System.Drawing.Point(490, 155);
            this.mFanControlGroupBox.Name = "mFanControlGroupBox";
            this.mFanControlGroupBox.Size = new System.Drawing.Size(233, 410);
            this.mFanControlGroupBox.TabIndex = 6;
            this.mFanControlGroupBox.TabStop = false;
            this.mFanControlGroupBox.Text = "Fan Control";
            // 
            // mFanControlAddButton
            // 
            this.mFanControlAddButton.Location = new System.Drawing.Point(120, 365);
            this.mFanControlAddButton.Name = "mFanControlAddButton";
            this.mFanControlAddButton.Size = new System.Drawing.Size(107, 38);
            this.mFanControlAddButton.TabIndex = 8;
            this.mFanControlAddButton.Text = "Add";
            this.mFanControlAddButton.UseVisualStyleBackColor = true;
            this.mFanControlAddButton.Click += new System.EventHandler(this.onFanControlAddButtonClick);
            // 
            // mFanControlListView
            // 
            this.mFanControlListView.FullRowSelect = true;
            this.mFanControlListView.GridLines = true;
            this.mFanControlListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.mFanControlListView.HideSelection = false;
            this.mFanControlListView.Location = new System.Drawing.Point(7, 20);
            this.mFanControlListView.Name = "mFanControlListView";
            this.mFanControlListView.Size = new System.Drawing.Size(220, 339);
            this.mFanControlListView.TabIndex = 0;
            this.mFanControlListView.UseCompatibleStateImageBehavior = false;
            this.mFanControlListView.View = System.Windows.Forms.View.Details;
            // 
            // mFanControlRemoveButton
            // 
            this.mFanControlRemoveButton.Location = new System.Drawing.Point(7, 365);
            this.mFanControlRemoveButton.Name = "mFanControlRemoveButton";
            this.mFanControlRemoveButton.Size = new System.Drawing.Size(107, 38);
            this.mFanControlRemoveButton.TabIndex = 7;
            this.mFanControlRemoveButton.Text = "Remove";
            this.mFanControlRemoveButton.UseVisualStyleBackColor = true;
            this.mFanControlRemoveButton.Click += new System.EventHandler(this.onFanControlRemoveButtonClick);
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(490, 571);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(233, 51);
            this.mOKButton.TabIndex = 9;
            this.mOKButton.Text = "OK";
            this.mOKButton.UseVisualStyleBackColor = true;
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // mClientGroupBox
            // 
            this.mClientGroupBox.Controls.Add(this.mClientListView);
            this.mClientGroupBox.Location = new System.Drawing.Point(225, 13);
            this.mClientGroupBox.Name = "mClientGroupBox";
            this.mClientGroupBox.Size = new System.Drawing.Size(498, 136);
            this.mClientGroupBox.TabIndex = 10;
            this.mClientGroupBox.TabStop = false;
            this.mClientGroupBox.Text = "Clients";
            // 
            // mClientListView
            // 
            this.mClientListView.FullRowSelect = true;
            this.mClientListView.GridLines = true;
            this.mClientListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.mClientListView.HideSelection = false;
            this.mClientListView.Location = new System.Drawing.Point(6, 20);
            this.mClientListView.Name = "mClientListView";
            this.mClientListView.Size = new System.Drawing.Size(486, 110);
            this.mClientListView.TabIndex = 9;
            this.mClientListView.UseCompatibleStateImageBehavior = false;
            this.mClientListView.View = System.Windows.Forms.View.Details;
            // 
            // PluginForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(733, 633);
            this.Controls.Add(this.mClientGroupBox);
            this.Controls.Add(this.mOKButton);
            this.Controls.Add(this.mFanControlGroupBox);
            this.Controls.Add(this.mFanSpeedGroupBox);
            this.Controls.Add(this.mTempGroupBox);
            this.Controls.Add(this.mServerGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PluginForm";
            this.mServerGroupBox.ResumeLayout(false);
            this.mServerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mPortNumericUpDown)).EndInit();
            this.mTempGroupBox.ResumeLayout(false);
            this.mFanSpeedGroupBox.ResumeLayout(false);
            this.mFanControlGroupBox.ResumeLayout(false);
            this.mClientGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button mServerButton;
        private System.Windows.Forms.GroupBox mServerGroupBox;
        private System.Windows.Forms.NumericUpDown mPortNumericUpDown;
        private System.Windows.Forms.Label mPortLabel;
        private System.Windows.Forms.GroupBox mTempGroupBox;
        private System.Windows.Forms.Button mTempAddButton;
        private System.Windows.Forms.Button mTempRemoveButton;
        private System.Windows.Forms.ListView mTempListView;
        private System.Windows.Forms.GroupBox mFanSpeedGroupBox;
        private System.Windows.Forms.Button mFanSpeedAddButton;
        private System.Windows.Forms.Button mFanSpeedRemoveButton;
        private System.Windows.Forms.ListView mFanSpeedListView;
        private System.Windows.Forms.GroupBox mFanControlGroupBox;
        private System.Windows.Forms.Button mFanControlAddButton;
        private System.Windows.Forms.Button mFanControlRemoveButton;
        private System.Windows.Forms.ListView mFanControlListView;
        private System.Windows.Forms.Button mOKButton;
        private System.Windows.Forms.GroupBox mClientGroupBox;
        private System.Windows.Forms.ListView mClientListView;
    }
}