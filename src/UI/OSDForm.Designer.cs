using DarkUI.Controls;
using System.Windows.Forms;

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
            this.mEnableCheckBox = new DarkCheckBox();
            this.mGroupGroupBox = new DarkGroupBox();
            this.mGroupEditTextBox = new DarkTextBox();
            this.mGroupRemoveButton = new DarkButton();
            this.mGroupColorButton = new DarkButton();
            this.mGroupAddButton = new DarkButton();
            this.mGroupDownButton = new DarkButton();
            this.mGroupUpButton = new DarkButton();
            this.mGroupListView = new ThemeListView();
            this.mGroupAddTextBox = new DarkTextBox();
            this.mItemGroupBox = new DarkGroupBox();
            this.mDigitLabel = new DarkLabel();
            this.mItemComboBox = new DarkComboBox();
            this.mItemRemoveButton = new DarkButton();
            this.mItemColorButton = new DarkButton();
            this.mItemAddButton = new DarkButton();
            this.mItemDownButton = new DarkButton();
            this.mItemUpButton = new DarkButton();
            this.mItemListView = new ThemeListView();
            this.mApplyButton = new DarkButton();
            this.mOKButton = new DarkButton();
            this.mSystemTimeCheckBox = new DarkCheckBox();
            this.mDigitNumericUpDown = new DarkNumericUpDown();
            this.mGroupGroupBox.SuspendLayout();
            this.mItemGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mDigitNumericUpDown)).BeginInit();
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
            this.mGroupGroupBox.Location = new System.Drawing.Point(13, 49);
            this.mGroupGroupBox.Name = "mGroupGroupBox";
            this.mGroupGroupBox.Size = new System.Drawing.Size(270, 279);
            this.mGroupGroupBox.TabIndex = 1;
            this.mGroupGroupBox.TabStop = false;
            this.mGroupGroupBox.Text = "Groups";
            // 
            // mGroupEditTextBox
            // 
            this.mGroupEditTextBox.Location = new System.Drawing.Point(51, 168);
            this.mGroupEditTextBox.Name = "mGroupEditTextBox";
            this.mGroupEditTextBox.Size = new System.Drawing.Size(100, 21);
            this.mGroupEditTextBox.TabIndex = 7;
            // 
            // mGroupRemoveButton
            // 
            this.mGroupRemoveButton.Location = new System.Drawing.Point(203, 156);
            this.mGroupRemoveButton.Name = "mGroupRemoveButton";
            this.mGroupRemoveButton.Size = new System.Drawing.Size(61, 29);
            this.mGroupRemoveButton.TabIndex = 6;
            this.mGroupRemoveButton.Text = "Remove";
            this.mGroupRemoveButton.Click += new System.EventHandler(this.onGroupRemoveButtonClick);
            // 
            // mGroupColorButton
            // 
            this.mGroupColorButton.Location = new System.Drawing.Point(203, 124);
            this.mGroupColorButton.Name = "mGroupColorButton";
            this.mGroupColorButton.Size = new System.Drawing.Size(61, 29);
            this.mGroupColorButton.TabIndex = 5;
            this.mGroupColorButton.Text = "Color";
            this.mGroupColorButton.Click += new System.EventHandler(this.onGroupColorButtonClick);
            // 
            // mGroupAddButton
            // 
            this.mGroupAddButton.Location = new System.Drawing.Point(203, 23);
            this.mGroupAddButton.Name = "mGroupAddButton";
            this.mGroupAddButton.Size = new System.Drawing.Size(61, 29);
            this.mGroupAddButton.TabIndex = 1;
            this.mGroupAddButton.Text = "Add";
            this.mGroupAddButton.Click += new System.EventHandler(this.onGroupAddButtonClick);
            // 
            // mGroupDownButton
            // 
            this.mGroupDownButton.Location = new System.Drawing.Point(203, 91);
            this.mGroupDownButton.Name = "mGroupDownButton";
            this.mGroupDownButton.Size = new System.Drawing.Size(61, 29);
            this.mGroupDownButton.TabIndex = 4;
            this.mGroupDownButton.Text = "▼";
            this.mGroupDownButton.Click += new System.EventHandler(this.onGroupDownButtonClick);
            // 
            // mGroupUpButton
            // 
            this.mGroupUpButton.Location = new System.Drawing.Point(203, 58);
            this.mGroupUpButton.Name = "mGroupUpButton";
            this.mGroupUpButton.Size = new System.Drawing.Size(61, 29);
            this.mGroupUpButton.TabIndex = 3;
            this.mGroupUpButton.Text = "▲";
            this.mGroupUpButton.Click += new System.EventHandler(this.onGroupUpButtonClick);
            // 
            // mGroupListView
            // 
            this.mGroupListView.FullRowSelect = true;
            this.mGroupListView.HideSelection = false;
            this.mGroupListView.Location = new System.Drawing.Point(6, 57);
            this.mGroupListView.MultiSelect = false;
            this.mGroupListView.Name = "mGroupListView";
            this.mGroupListView.Size = new System.Drawing.Size(191, 213);
            this.mGroupListView.TabIndex = 2;
            this.mGroupListView.UseCompatibleStateImageBehavior = false;
            this.mGroupListView.View = System.Windows.Forms.View.Details;
            // 
            // mGroupAddTextBox
            // 
            this.mGroupAddTextBox.Location = new System.Drawing.Point(6, 27);
            this.mGroupAddTextBox.Name = "mGroupAddTextBox";
            this.mGroupAddTextBox.Size = new System.Drawing.Size(191, 21);
            this.mGroupAddTextBox.TabIndex = 0;
            // 
            // mItemGroupBox
            // 
            this.mItemGroupBox.Controls.Add(this.mDigitLabel);
            this.mItemGroupBox.Controls.Add(this.mItemComboBox);
            this.mItemGroupBox.Controls.Add(this.mItemRemoveButton);
            this.mItemGroupBox.Controls.Add(this.mItemColorButton);
            this.mItemGroupBox.Controls.Add(this.mItemAddButton);
            this.mItemGroupBox.Controls.Add(this.mItemDownButton);
            this.mItemGroupBox.Controls.Add(this.mItemUpButton);
            this.mItemGroupBox.Controls.Add(this.mItemListView);
            this.mItemGroupBox.Location = new System.Drawing.Point(289, 49);
            this.mItemGroupBox.Name = "mItemGroupBox";
            this.mItemGroupBox.Size = new System.Drawing.Size(418, 279);
            this.mItemGroupBox.TabIndex = 7;
            this.mItemGroupBox.TabStop = false;
            this.mItemGroupBox.Text = "Items";
            // 
            // mDigitLabel
            // 
            this.mDigitLabel.AutoSize = true;
            this.mDigitLabel.Location = new System.Drawing.Point(270, 1);
            this.mDigitLabel.Name = "mDigitLabel";
            this.mDigitLabel.Size = new System.Drawing.Size(75, 12);
            this.mDigitLabel.TabIndex = 7;
            this.mDigitLabel.Text = "Align digits :";
            this.mDigitLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mItemComboBox
            // 
            this.mItemComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.mItemComboBox.FormattingEnabled = true;
            this.mItemComboBox.Location = new System.Drawing.Point(6, 28);
            this.mItemComboBox.Name = "mItemComboBox";
            this.mItemComboBox.Size = new System.Drawing.Size(339, 20);
            this.mItemComboBox.TabIndex = 1;
            // 
            // mItemRemoveButton
            // 
            this.mItemRemoveButton.Location = new System.Drawing.Point(351, 157);
            this.mItemRemoveButton.Name = "mItemRemoveButton";
            this.mItemRemoveButton.Size = new System.Drawing.Size(61, 29);
            this.mItemRemoveButton.TabIndex = 6;
            this.mItemRemoveButton.Text = "Remove";
            this.mItemRemoveButton.Click += new System.EventHandler(this.onItemRemoveButtonClick);
            // 
            // mItemColorButton
            // 
            this.mItemColorButton.Location = new System.Drawing.Point(351, 124);
            this.mItemColorButton.Name = "mItemColorButton";
            this.mItemColorButton.Size = new System.Drawing.Size(61, 29);
            this.mItemColorButton.TabIndex = 5;
            this.mItemColorButton.Text = "Color";
            this.mItemColorButton.Click += new System.EventHandler(this.onItemColorButtonClick);
            // 
            // mItemAddButton
            // 
            this.mItemAddButton.Location = new System.Drawing.Point(351, 23);
            this.mItemAddButton.Name = "mItemAddButton";
            this.mItemAddButton.Size = new System.Drawing.Size(61, 29);
            this.mItemAddButton.TabIndex = 1;
            this.mItemAddButton.Text = "Add";
            this.mItemAddButton.Click += new System.EventHandler(this.onItemAddButtonClick);
            // 
            // mItemDownButton
            // 
            this.mItemDownButton.Location = new System.Drawing.Point(351, 91);
            this.mItemDownButton.Name = "mItemDownButton";
            this.mItemDownButton.Size = new System.Drawing.Size(61, 29);
            this.mItemDownButton.TabIndex = 4;
            this.mItemDownButton.Text = "▼";
            this.mItemDownButton.Click += new System.EventHandler(this.onItemDownButtonClick);
            // 
            // mItemUpButton
            // 
            this.mItemUpButton.Location = new System.Drawing.Point(351, 57);
            this.mItemUpButton.Name = "mItemUpButton";
            this.mItemUpButton.Size = new System.Drawing.Size(61, 29);
            this.mItemUpButton.TabIndex = 3;
            this.mItemUpButton.Text = "▲";
            this.mItemUpButton.Click += new System.EventHandler(this.onItemUpButtonClick);
            // 
            // mItemListView
            // 
            this.mItemListView.FullRowSelect = true;
            this.mItemListView.HideSelection = false;
            this.mItemListView.Location = new System.Drawing.Point(6, 57);
            this.mItemListView.MultiSelect = false;
            this.mItemListView.Name = "mItemListView";
            this.mItemListView.Size = new System.Drawing.Size(339, 213);
            this.mItemListView.TabIndex = 2;
            this.mItemListView.UseCompatibleStateImageBehavior = false;
            this.mItemListView.View = System.Windows.Forms.View.Details;
            // 
            // mApplyButton
            // 
            this.mApplyButton.Location = new System.Drawing.Point(479, 334);
            this.mApplyButton.Name = "mApplyButton";
            this.mApplyButton.Size = new System.Drawing.Size(111, 35);
            this.mApplyButton.TabIndex = 8;
            this.mApplyButton.Text = "Apply";
            this.mApplyButton.Click += new System.EventHandler(this.onApplyButtonClick);
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(596, 334);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(111, 35);
            this.mOKButton.TabIndex = 9;
            this.mOKButton.Text = "OK";
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // mSystemTimeCheckBox
            // 
            this.mSystemTimeCheckBox.AutoSize = true;
            this.mSystemTimeCheckBox.Location = new System.Drawing.Point(295, 17);
            this.mSystemTimeCheckBox.Name = "mSystemTimeCheckBox";
            this.mSystemTimeCheckBox.Size = new System.Drawing.Size(130, 16);
            this.mSystemTimeCheckBox.TabIndex = 0;
            this.mSystemTimeCheckBox.Text = "Show system time";
            // 
            // mDigitNumericUpDown
            // 
            this.mDigitNumericUpDown.Location = new System.Drawing.Point(640, 46);
            this.mDigitNumericUpDown.Name = "mDigitNumericUpDown";
            this.mDigitNumericUpDown.Size = new System.Drawing.Size(61, 21);
            this.mDigitNumericUpDown.TabIndex = 7;
            this.mDigitNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // OSDForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(719, 377);
            this.Controls.Add(this.mDigitNumericUpDown);
            this.Controls.Add(this.mSystemTimeCheckBox);
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
            this.mItemGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mDigitNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkCheckBox mEnableCheckBox;
        private DarkGroupBox mGroupGroupBox;
        private ThemeListView mGroupListView;
        private DarkButton mGroupAddButton;
        private DarkTextBox mGroupAddTextBox;
        private DarkButton mGroupRemoveButton;
        private DarkButton mGroupColorButton;
        private DarkButton mGroupDownButton;
        private DarkButton mGroupUpButton;
        private DarkGroupBox mItemGroupBox;
        private DarkComboBox mItemComboBox;
        private DarkButton mItemRemoveButton;
        private DarkButton mItemColorButton;
        private DarkButton mItemAddButton;
        private DarkButton mItemDownButton;
        private DarkButton mItemUpButton;
        private ThemeListView mItemListView;
        private DarkButton mApplyButton;
        private DarkButton mOKButton;
        private DarkTextBox mGroupEditTextBox;
        private DarkCheckBox mSystemTimeCheckBox;
        private DarkNumericUpDown mDigitNumericUpDown;
        private DarkLabel mDigitLabel;
    }
}