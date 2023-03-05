using DarkUI.Controls;

namespace FanCtrl
{
    partial class PluginAddForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginAddForm));
            this.mKeyTextBox = new DarkTextBox();
            this.label1 = new DarkLabel();
            this.mOKButton = new DarkButton();
            this.mNameTextBox = new DarkTextBox();
            this.label2 = new DarkLabel();
            this.SuspendLayout();
            // 
            // mKeyTextBox
            // 
            this.mKeyTextBox.Location = new System.Drawing.Point(68, 12);
            this.mKeyTextBox.Name = "mKeyTextBox";
            this.mKeyTextBox.Size = new System.Drawing.Size(150, 21);
            this.mKeyTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Key :";
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(17, 72);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(201, 44);
            this.mOKButton.TabIndex = 1;
            this.mOKButton.Text = "OK";
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // mNameTextBox
            // 
            this.mNameTextBox.Location = new System.Drawing.Point(68, 39);
            this.mNameTextBox.Name = "mNameTextBox";
            this.mNameTextBox.Size = new System.Drawing.Size(150, 21);
            this.mNameTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name :";
            // 
            // PluginAddForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(229, 128);
            this.Controls.Add(this.mNameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mKeyTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mOKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginAddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PluginAddForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkTextBox mKeyTextBox;
        private DarkLabel label1;
        private DarkButton mOKButton;
        private DarkTextBox mNameTextBox;
        private DarkLabel label2;
    }
}