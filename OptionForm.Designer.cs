namespace FanControl
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
            this.mLibraryRadioButton1 = new System.Windows.Forms.RadioButton();
            this.mLibraryRadioButton2 = new System.Windows.Forms.RadioButton();
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
            this.mIntervalGroupBox.Size = new System.Drawing.Size(163, 58);
            this.mIntervalGroupBox.TabIndex = 0;
            this.mIntervalGroupBox.TabStop = false;
            this.mIntervalGroupBox.Text = "Interval";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(133, 30);
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
            this.mIntervalTextBox.Size = new System.Drawing.Size(114, 21);
            this.mIntervalTextBox.TabIndex = 0;
            // 
            // mMinimizeCheckBox
            // 
            this.mMinimizeCheckBox.AutoSize = true;
            this.mMinimizeCheckBox.Location = new System.Drawing.Point(18, 159);
            this.mMinimizeCheckBox.Name = "mMinimizeCheckBox";
            this.mMinimizeCheckBox.Size = new System.Drawing.Size(112, 16);
            this.mMinimizeCheckBox.TabIndex = 1;
            this.mMinimizeCheckBox.Text = "Start minimized";
            this.mMinimizeCheckBox.UseVisualStyleBackColor = true;
            // 
            // mStartupCheckBox
            // 
            this.mStartupCheckBox.AutoSize = true;
            this.mStartupCheckBox.Location = new System.Drawing.Point(18, 185);
            this.mStartupCheckBox.Name = "mStartupCheckBox";
            this.mStartupCheckBox.Size = new System.Drawing.Size(131, 16);
            this.mStartupCheckBox.TabIndex = 3;
            this.mStartupCheckBox.Text = "Start with Windows";
            this.mStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(12, 211);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(163, 38);
            this.mOKButton.TabIndex = 4;
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
            this.mLibraryGroupBox.Controls.Add(this.mLibraryRadioButton2);
            this.mLibraryGroupBox.Controls.Add(this.mLibraryRadioButton1);
            this.mLibraryGroupBox.Location = new System.Drawing.Point(12, 77);
            this.mLibraryGroupBox.Name = "mLibraryGroupBox";
            this.mLibraryGroupBox.Size = new System.Drawing.Size(163, 71);
            this.mLibraryGroupBox.TabIndex = 5;
            this.mLibraryGroupBox.TabStop = false;
            this.mLibraryGroupBox.Text = "Library";
            // 
            // mLibraryRadioButton1
            // 
            this.mLibraryRadioButton1.AutoSize = true;
            this.mLibraryRadioButton1.Location = new System.Drawing.Point(8, 21);
            this.mLibraryRadioButton1.Name = "mLibraryRadioButton1";
            this.mLibraryRadioButton1.Size = new System.Drawing.Size(147, 16);
            this.mLibraryRadioButton1.TabIndex = 0;
            this.mLibraryRadioButton1.TabStop = true;
            this.mLibraryRadioButton1.Text = "LibreHardwareMonitor";
            this.mLibraryRadioButton1.UseVisualStyleBackColor = true;
            // 
            // mLibraryRadioButton2
            // 
            this.mLibraryRadioButton2.AutoSize = true;
            this.mLibraryRadioButton2.Location = new System.Drawing.Point(8, 43);
            this.mLibraryRadioButton2.Name = "mLibraryRadioButton2";
            this.mLibraryRadioButton2.Size = new System.Drawing.Size(149, 16);
            this.mLibraryRadioButton2.TabIndex = 1;
            this.mLibraryRadioButton2.TabStop = true;
            this.mLibraryRadioButton2.Text = "OpenHardwareMonitor";
            this.mLibraryRadioButton2.UseVisualStyleBackColor = true;
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(185, 257);
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
        private System.Windows.Forms.RadioButton mLibraryRadioButton2;
        private System.Windows.Forms.RadioButton mLibraryRadioButton1;
    }
}