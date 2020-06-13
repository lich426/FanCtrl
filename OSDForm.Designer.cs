namespace FanCtrl
{
    partial class OSDForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OSDForm));
            this.mEnableCheckBox = new System.Windows.Forms.CheckBox();
            this.mGroupGroupBox = new System.Windows.Forms.GroupBox();
            this.mGroupRemoveButton = new System.Windows.Forms.Button();
            this.mGroupColorButton = new System.Windows.Forms.Button();
            this.mGroupAddButton = new System.Windows.Forms.Button();
            this.mGroupDownButton = new System.Windows.Forms.Button();
            this.mGroupUpButton = new System.Windows.Forms.Button();
            this.mGroupListView = new System.Windows.Forms.ListView();
            this.mGroupAddTextBox = new System.Windows.Forms.TextBox();
            this.mItemGroupBox = new System.Windows.Forms.GroupBox();
            this.mItemComboBox = new System.Windows.Forms.ComboBox();
            this.mItemRemoveButton = new System.Windows.Forms.Button();
            this.mItemColorButton = new System.Windows.Forms.Button();
            this.mItemAddButton = new System.Windows.Forms.Button();
            this.mItemDownButton = new System.Windows.Forms.Button();
            this.mItemUpButton = new System.Windows.Forms.Button();
            this.mItemListView = new System.Windows.Forms.ListView();
            this.mApplyButton = new System.Windows.Forms.Button();
            this.mOKButton = new System.Windows.Forms.Button();
            this.mGroupEditTextBox = new System.Windows.Forms.TextBox();
            this.mGroupGroupBox.SuspendLayout();
            this.mItemGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mEnableCheckBox
            // 
            this.mEnableCheckBox.AutoSize = true;
            this.mEnableCheckBox.Location = new System.Drawing.Point(19, 17);
            this.mEnableCheckBox.Name = "mEnableCheckBox";
            this.mEnableCheckBox.Size = new System.Drawing.Size(221, 16);
            this.mEnableCheckBox.TabIndex = 0;
            this.mEnableCheckBox.Text = "Enable On-Screen Display (RTSS)";
            this.mEnableCheckBox.UseVisualStyleBackColor = true;
            // 
            // mGroupGroupBox
            // 
            this.mGroupGroupBox.Controls.Add(this.mGroupEditTextBox);
            this.mGroupGroupBox.Controls.Add(this.mGroupRemoveButton);
            this.mGroupGroupBox.Controls.Add(this.mGroupColorButton);
            this.mGroupGroupBox.Controls.Add(this.mGroupAddButton);
            this.mGroupGroupBox.Controls.Add(this.mGroupDownButton);
            this.mGroupGroupBox.Controls.Add(this.mGroupUpButton);
            this.mGroupGroupBox.Controls.Add(this.mGroupListView);
            this.mGroupGroupBox.Controls.Add(this.mGroupAddTextBox);
            this.mGroupGroupBox.Location = new System.Drawing.Point(13, 45);
            this.mGroupGroupBox.Name = "mGroupGroupBox";
            this.mGroupGroupBox.Size = new System.Drawing.Size(270, 195);
            this.mGroupGroupBox.TabIndex = 1;
            this.mGroupGroupBox.TabStop = false;
            this.mGroupGroupBox.Text = "Groups";
            // 
            // mGroupRemoveButton
            // 
            this.mGroupRemoveButton.Location = new System.Drawing.Point(203, 153);
            this.mGroupRemoveButton.Name = "mGroupRemoveButton";
            this.mGroupRemoveButton.Size = new System.Drawing.Size(61, 29);
            this.mGroupRemoveButton.TabIndex = 6;
            this.mGroupRemoveButton.Text = "Remove";
            this.mGroupRemoveButton.UseVisualStyleBackColor = true;
            this.mGroupRemoveButton.Click += new System.EventHandler(this.onGroupRemoveButtonClick);
            // 
            // mGroupColorButton
            // 
            this.mGroupColorButton.Location = new System.Drawing.Point(203, 118);
            this.mGroupColorButton.Name = "mGroupColorButton";
            this.mGroupColorButton.Size = new System.Drawing.Size(61, 29);
            this.mGroupColorButton.TabIndex = 5;
            this.mGroupColorButton.Text = "Color";
            this.mGroupColorButton.UseVisualStyleBackColor = true;
            this.mGroupColorButton.Click += new System.EventHandler(this.onGroupColorButtonClick);
            // 
            // mGroupAddButton
            // 
            this.mGroupAddButton.Location = new System.Drawing.Point(203, 15);
            this.mGroupAddButton.Name = "mGroupAddButton";
            this.mGroupAddButton.Size = new System.Drawing.Size(61, 29);
            this.mGroupAddButton.TabIndex = 1;
            this.mGroupAddButton.Text = "Add";
            this.mGroupAddButton.UseVisualStyleBackColor = true;
            this.mGroupAddButton.Click += new System.EventHandler(this.onGroupAddButtonClick);
            // 
            // mGroupDownButton
            // 
            this.mGroupDownButton.Location = new System.Drawing.Point(203, 83);
            this.mGroupDownButton.Name = "mGroupDownButton";
            this.mGroupDownButton.Size = new System.Drawing.Size(61, 29);
            this.mGroupDownButton.TabIndex = 4;
            this.mGroupDownButton.Text = "▼";
            this.mGroupDownButton.UseVisualStyleBackColor = true;
            this.mGroupDownButton.Click += new System.EventHandler(this.onGroupDownButtonClick);
            // 
            // mGroupUpButton
            // 
            this.mGroupUpButton.Location = new System.Drawing.Point(203, 50);
            this.mGroupUpButton.Name = "mGroupUpButton";
            this.mGroupUpButton.Size = new System.Drawing.Size(61, 29);
            this.mGroupUpButton.TabIndex = 3;
            this.mGroupUpButton.Text = "▲";
            this.mGroupUpButton.UseVisualStyleBackColor = true;
            this.mGroupUpButton.Click += new System.EventHandler(this.onGroupUpButtonClick);
            // 
            // mGroupListView
            // 
            this.mGroupListView.FullRowSelect = true;
            this.mGroupListView.HideSelection = false;
            this.mGroupListView.Location = new System.Drawing.Point(6, 47);
            this.mGroupListView.MultiSelect = false;
            this.mGroupListView.Name = "mGroupListView";
            this.mGroupListView.Size = new System.Drawing.Size(191, 134);
            this.mGroupListView.TabIndex = 2;
            this.mGroupListView.UseCompatibleStateImageBehavior = false;
            this.mGroupListView.View = System.Windows.Forms.View.Details;
            // 
            // mGroupAddTextBox
            // 
            this.mGroupAddTextBox.Location = new System.Drawing.Point(6, 19);
            this.mGroupAddTextBox.Name = "mGroupAddTextBox";
            this.mGroupAddTextBox.Size = new System.Drawing.Size(191, 21);
            this.mGroupAddTextBox.TabIndex = 0;
            // 
            // mItemGroupBox
            // 
            this.mItemGroupBox.Controls.Add(this.mItemComboBox);
            this.mItemGroupBox.Controls.Add(this.mItemRemoveButton);
            this.mItemGroupBox.Controls.Add(this.mItemColorButton);
            this.mItemGroupBox.Controls.Add(this.mItemAddButton);
            this.mItemGroupBox.Controls.Add(this.mItemDownButton);
            this.mItemGroupBox.Controls.Add(this.mItemUpButton);
            this.mItemGroupBox.Controls.Add(this.mItemListView);
            this.mItemGroupBox.Location = new System.Drawing.Point(289, 45);
            this.mItemGroupBox.Name = "mItemGroupBox";
            this.mItemGroupBox.Size = new System.Drawing.Size(335, 195);
            this.mItemGroupBox.TabIndex = 7;
            this.mItemGroupBox.TabStop = false;
            this.mItemGroupBox.Text = "Items";
            // 
            // mItemComboBox
            // 
            this.mItemComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mItemComboBox.FormattingEnabled = true;
            this.mItemComboBox.Location = new System.Drawing.Point(6, 19);
            this.mItemComboBox.Name = "mItemComboBox";
            this.mItemComboBox.Size = new System.Drawing.Size(256, 20);
            this.mItemComboBox.TabIndex = 1;
            // 
            // mItemRemoveButton
            // 
            this.mItemRemoveButton.Location = new System.Drawing.Point(268, 153);
            this.mItemRemoveButton.Name = "mItemRemoveButton";
            this.mItemRemoveButton.Size = new System.Drawing.Size(61, 29);
            this.mItemRemoveButton.TabIndex = 6;
            this.mItemRemoveButton.Text = "Remove";
            this.mItemRemoveButton.UseVisualStyleBackColor = true;
            this.mItemRemoveButton.Click += new System.EventHandler(this.onItemRemoveButtonClick);
            // 
            // mItemColorButton
            // 
            this.mItemColorButton.Location = new System.Drawing.Point(268, 118);
            this.mItemColorButton.Name = "mItemColorButton";
            this.mItemColorButton.Size = new System.Drawing.Size(61, 29);
            this.mItemColorButton.TabIndex = 5;
            this.mItemColorButton.Text = "Color";
            this.mItemColorButton.UseVisualStyleBackColor = true;
            this.mItemColorButton.Click += new System.EventHandler(this.onItemColorButtonClick);
            // 
            // mItemAddButton
            // 
            this.mItemAddButton.Location = new System.Drawing.Point(268, 15);
            this.mItemAddButton.Name = "mItemAddButton";
            this.mItemAddButton.Size = new System.Drawing.Size(61, 29);
            this.mItemAddButton.TabIndex = 1;
            this.mItemAddButton.Text = "Add";
            this.mItemAddButton.UseVisualStyleBackColor = true;
            this.mItemAddButton.Click += new System.EventHandler(this.onItemAddButtonClick);
            // 
            // mItemDownButton
            // 
            this.mItemDownButton.Location = new System.Drawing.Point(268, 83);
            this.mItemDownButton.Name = "mItemDownButton";
            this.mItemDownButton.Size = new System.Drawing.Size(61, 29);
            this.mItemDownButton.TabIndex = 4;
            this.mItemDownButton.Text = "▼";
            this.mItemDownButton.UseVisualStyleBackColor = true;
            this.mItemDownButton.Click += new System.EventHandler(this.onItemDownButtonClick);
            // 
            // mItemUpButton
            // 
            this.mItemUpButton.Location = new System.Drawing.Point(268, 50);
            this.mItemUpButton.Name = "mItemUpButton";
            this.mItemUpButton.Size = new System.Drawing.Size(61, 29);
            this.mItemUpButton.TabIndex = 3;
            this.mItemUpButton.Text = "▲";
            this.mItemUpButton.UseVisualStyleBackColor = true;
            this.mItemUpButton.Click += new System.EventHandler(this.onItemUpButtonClick);
            // 
            // mItemListView
            // 
            this.mItemListView.FullRowSelect = true;
            this.mItemListView.HideSelection = false;
            this.mItemListView.Location = new System.Drawing.Point(6, 47);
            this.mItemListView.MultiSelect = false;
            this.mItemListView.Name = "mItemListView";
            this.mItemListView.Size = new System.Drawing.Size(256, 134);
            this.mItemListView.TabIndex = 2;
            this.mItemListView.UseCompatibleStateImageBehavior = false;
            this.mItemListView.View = System.Windows.Forms.View.Details;
            // 
            // mApplyButton
            // 
            this.mApplyButton.Location = new System.Drawing.Point(396, 246);
            this.mApplyButton.Name = "mApplyButton";
            this.mApplyButton.Size = new System.Drawing.Size(111, 35);
            this.mApplyButton.TabIndex = 8;
            this.mApplyButton.Text = "Apply";
            this.mApplyButton.UseVisualStyleBackColor = true;
            this.mApplyButton.Click += new System.EventHandler(this.onApplyButtonClick);
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(513, 246);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(111, 35);
            this.mOKButton.TabIndex = 9;
            this.mOKButton.Text = "OK";
            this.mOKButton.UseVisualStyleBackColor = true;
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // mGroupEditTextBox
            // 
            this.mGroupEditTextBox.Location = new System.Drawing.Point(51, 168);
            this.mGroupEditTextBox.Name = "mGroupEditTextBox";
            this.mGroupEditTextBox.Size = new System.Drawing.Size(100, 21);
            this.mGroupEditTextBox.TabIndex = 7;
            // 
            // OSDForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(636, 289);
            this.Controls.Add(this.mOKButton);
            this.Controls.Add(this.mApplyButton);
            this.Controls.Add(this.mItemGroupBox);
            this.Controls.Add(this.mGroupGroupBox);
            this.Controls.Add(this.mEnableCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OSDForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "On-Screen Display (RTSS)";
            this.mGroupGroupBox.ResumeLayout(false);
            this.mGroupGroupBox.PerformLayout();
            this.mItemGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox mEnableCheckBox;
        private System.Windows.Forms.GroupBox mGroupGroupBox;
        private System.Windows.Forms.ListView mGroupListView;
        private System.Windows.Forms.Button mGroupAddButton;
        private System.Windows.Forms.TextBox mGroupAddTextBox;
        private System.Windows.Forms.Button mGroupRemoveButton;
        private System.Windows.Forms.Button mGroupColorButton;
        private System.Windows.Forms.Button mGroupDownButton;
        private System.Windows.Forms.Button mGroupUpButton;
        private System.Windows.Forms.GroupBox mItemGroupBox;
        private System.Windows.Forms.ComboBox mItemComboBox;
        private System.Windows.Forms.Button mItemRemoveButton;
        private System.Windows.Forms.Button mItemColorButton;
        private System.Windows.Forms.Button mItemAddButton;
        private System.Windows.Forms.Button mItemDownButton;
        private System.Windows.Forms.Button mItemUpButton;
        private System.Windows.Forms.ListView mItemListView;
        private System.Windows.Forms.Button mApplyButton;
        private System.Windows.Forms.Button mOKButton;
        private System.Windows.Forms.TextBox mGroupEditTextBox;
    }
}