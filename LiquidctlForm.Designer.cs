namespace FanCtrl
{
    partial class LiquidctlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiquidctlForm));
            this.mLocationGroupBox = new System.Windows.Forms.GroupBox();
            this.mLocationButton = new System.Windows.Forms.Button();
            this.mLocationTextBox = new System.Windows.Forms.TextBox();
            this.mControlGroupBox = new System.Windows.Forms.GroupBox();
            this.mControlAddButton = new System.Windows.Forms.Button();
            this.mControlRemoveButton = new System.Windows.Forms.Button();
            this.mControlListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mCommandGroupBox = new System.Windows.Forms.GroupBox();
            this.mCommandAddButton = new System.Windows.Forms.Button();
            this.mCommandRemoveButton = new System.Windows.Forms.Button();
            this.mCommandListView = new System.Windows.Forms.ListView();
            this.mOKButton = new System.Windows.Forms.Button();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mLocationGroupBox.SuspendLayout();
            this.mControlGroupBox.SuspendLayout();
            this.mCommandGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mLocationGroupBox
            // 
            this.mLocationGroupBox.Controls.Add(this.mLocationButton);
            this.mLocationGroupBox.Controls.Add(this.mLocationTextBox);
            this.mLocationGroupBox.Location = new System.Drawing.Point(12, 12);
            this.mLocationGroupBox.Name = "mLocationGroupBox";
            this.mLocationGroupBox.Size = new System.Drawing.Size(509, 71);
            this.mLocationGroupBox.TabIndex = 0;
            this.mLocationGroupBox.TabStop = false;
            this.mLocationGroupBox.Text = "Location";
            // 
            // mLocationButton
            // 
            this.mLocationButton.Location = new System.Drawing.Point(413, 20);
            this.mLocationButton.Name = "mLocationButton";
            this.mLocationButton.Size = new System.Drawing.Size(90, 32);
            this.mLocationButton.TabIndex = 1;
            this.mLocationButton.Text = "Open";
            this.mLocationButton.UseVisualStyleBackColor = true;
            this.mLocationButton.Click += new System.EventHandler(this.onLocationButtonClick);
            // 
            // mLocationTextBox
            // 
            this.mLocationTextBox.Location = new System.Drawing.Point(6, 27);
            this.mLocationTextBox.Name = "mLocationTextBox";
            this.mLocationTextBox.Size = new System.Drawing.Size(401, 21);
            this.mLocationTextBox.TabIndex = 0;
            // 
            // mControlGroupBox
            // 
            this.mControlGroupBox.Controls.Add(this.mControlAddButton);
            this.mControlGroupBox.Controls.Add(this.mControlRemoveButton);
            this.mControlGroupBox.Controls.Add(this.mControlListView);
            this.mControlGroupBox.Location = new System.Drawing.Point(12, 89);
            this.mControlGroupBox.Name = "mControlGroupBox";
            this.mControlGroupBox.Size = new System.Drawing.Size(509, 201);
            this.mControlGroupBox.TabIndex = 1;
            this.mControlGroupBox.TabStop = false;
            this.mControlGroupBox.Text = "Fan Control";
            // 
            // mControlAddButton
            // 
            this.mControlAddButton.Location = new System.Drawing.Point(7, 159);
            this.mControlAddButton.Name = "mControlAddButton";
            this.mControlAddButton.Size = new System.Drawing.Size(90, 36);
            this.mControlAddButton.TabIndex = 3;
            this.mControlAddButton.Text = "button3";
            this.mControlAddButton.UseVisualStyleBackColor = true;
            this.mControlAddButton.Click += new System.EventHandler(this.onControlAddButtonClick);
            // 
            // mControlRemoveButton
            // 
            this.mControlRemoveButton.Location = new System.Drawing.Point(103, 159);
            this.mControlRemoveButton.Name = "mControlRemoveButton";
            this.mControlRemoveButton.Size = new System.Drawing.Size(90, 36);
            this.mControlRemoveButton.TabIndex = 2;
            this.mControlRemoveButton.Text = "button2";
            this.mControlRemoveButton.UseVisualStyleBackColor = true;
            this.mControlRemoveButton.Click += new System.EventHandler(this.onControlRemoveButtonClick);
            // 
            // mControlListView
            // 
            this.mControlListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.mControlListView.FullRowSelect = true;
            this.mControlListView.GridLines = true;
            this.mControlListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.mControlListView.HideSelection = false;
            this.mControlListView.Location = new System.Drawing.Point(7, 21);
            this.mControlListView.Name = "mControlListView";
            this.mControlListView.Size = new System.Drawing.Size(496, 132);
            this.mControlListView.TabIndex = 0;
            this.mControlListView.UseCompatibleStateImageBehavior = false;
            this.mControlListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Device name";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Channel name";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Address";
            this.columnHeader3.Width = 215;
            // 
            // mCommandGroupBox
            // 
            this.mCommandGroupBox.Controls.Add(this.mCommandAddButton);
            this.mCommandGroupBox.Controls.Add(this.mCommandRemoveButton);
            this.mCommandGroupBox.Controls.Add(this.mCommandListView);
            this.mCommandGroupBox.Location = new System.Drawing.Point(12, 296);
            this.mCommandGroupBox.Name = "mCommandGroupBox";
            this.mCommandGroupBox.Size = new System.Drawing.Size(509, 159);
            this.mCommandGroupBox.TabIndex = 2;
            this.mCommandGroupBox.TabStop = false;
            this.mCommandGroupBox.Text = "Command";
            // 
            // mCommandAddButton
            // 
            this.mCommandAddButton.Location = new System.Drawing.Point(7, 117);
            this.mCommandAddButton.Name = "mCommandAddButton";
            this.mCommandAddButton.Size = new System.Drawing.Size(90, 36);
            this.mCommandAddButton.TabIndex = 3;
            this.mCommandAddButton.Text = "button6";
            this.mCommandAddButton.UseVisualStyleBackColor = true;
            this.mCommandAddButton.Click += new System.EventHandler(this.onCommandAddButtonClick);
            // 
            // mCommandRemoveButton
            // 
            this.mCommandRemoveButton.Location = new System.Drawing.Point(103, 117);
            this.mCommandRemoveButton.Name = "mCommandRemoveButton";
            this.mCommandRemoveButton.Size = new System.Drawing.Size(90, 36);
            this.mCommandRemoveButton.TabIndex = 2;
            this.mCommandRemoveButton.Text = "button5";
            this.mCommandRemoveButton.UseVisualStyleBackColor = true;
            this.mCommandRemoveButton.Click += new System.EventHandler(this.onCommandRemoveButtonClick);
            // 
            // mCommandListView
            // 
            this.mCommandListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            this.mCommandListView.FullRowSelect = true;
            this.mCommandListView.GridLines = true;
            this.mCommandListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.mCommandListView.HideSelection = false;
            this.mCommandListView.Location = new System.Drawing.Point(7, 21);
            this.mCommandListView.Name = "mCommandListView";
            this.mCommandListView.Size = new System.Drawing.Size(496, 90);
            this.mCommandListView.TabIndex = 0;
            this.mCommandListView.UseCompatibleStateImageBehavior = false;
            this.mCommandListView.View = System.Windows.Forms.View.Details;
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(329, 462);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(192, 45);
            this.mOKButton.TabIndex = 3;
            this.mOKButton.Text = "button7";
            this.mOKButton.UseVisualStyleBackColor = true;
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Command";
            this.columnHeader4.Width = 470;
            // 
            // LiquidctlForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(533, 515);
            this.Controls.Add(this.mOKButton);
            this.Controls.Add(this.mCommandGroupBox);
            this.Controls.Add(this.mControlGroupBox);
            this.Controls.Add(this.mLocationGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LiquidctlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "liquidctl Setting";
            this.mLocationGroupBox.ResumeLayout(false);
            this.mLocationGroupBox.PerformLayout();
            this.mControlGroupBox.ResumeLayout(false);
            this.mCommandGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox mLocationGroupBox;
        private System.Windows.Forms.GroupBox mControlGroupBox;
        private System.Windows.Forms.Button mLocationButton;
        private System.Windows.Forms.TextBox mLocationTextBox;
        private System.Windows.Forms.GroupBox mCommandGroupBox;
        private System.Windows.Forms.Button mControlAddButton;
        private System.Windows.Forms.Button mControlRemoveButton;
        private System.Windows.Forms.ListView mControlListView;
        private System.Windows.Forms.Button mCommandAddButton;
        private System.Windows.Forms.Button mCommandRemoveButton;
        private System.Windows.Forms.ListView mCommandListView;
        private System.Windows.Forms.Button mOKButton;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}