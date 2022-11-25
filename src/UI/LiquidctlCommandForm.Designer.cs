namespace FanCtrl
{
    partial class LiquidctlCommandForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiquidctlCommandForm));
            this.mCommandGroupBox = new System.Windows.Forms.GroupBox();
            this.mCommandTextBox = new System.Windows.Forms.TextBox();
            this.mOKButton = new System.Windows.Forms.Button();
            this.mCommandGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mCommandGroupBox
            // 
            this.mCommandGroupBox.Controls.Add(this.mCommandTextBox);
            this.mCommandGroupBox.Location = new System.Drawing.Point(13, 13);
            this.mCommandGroupBox.Name = "mCommandGroupBox";
            this.mCommandGroupBox.Size = new System.Drawing.Size(450, 58);
            this.mCommandGroupBox.TabIndex = 0;
            this.mCommandGroupBox.TabStop = false;
            this.mCommandGroupBox.Text = "Command";
            // 
            // mCommandTextBox
            // 
            this.mCommandTextBox.Location = new System.Drawing.Point(7, 21);
            this.mCommandTextBox.Name = "mCommandTextBox";
            this.mCommandTextBox.Size = new System.Drawing.Size(437, 21);
            this.mCommandTextBox.TabIndex = 0;
            // 
            // mOKButton
            // 
            this.mOKButton.Location = new System.Drawing.Point(321, 78);
            this.mOKButton.Name = "mOKButton";
            this.mOKButton.Size = new System.Drawing.Size(142, 34);
            this.mOKButton.TabIndex = 1;
            this.mOKButton.Text = "button1";
            this.mOKButton.UseVisualStyleBackColor = true;
            this.mOKButton.Click += new System.EventHandler(this.onOKButtonClick);
            // 
            // LiquidctlCommandForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(473, 124);
            this.Controls.Add(this.mOKButton);
            this.Controls.Add(this.mCommandGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LiquidctlCommandForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "liquidctl Command";
            this.mCommandGroupBox.ResumeLayout(false);
            this.mCommandGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox mCommandGroupBox;
        private System.Windows.Forms.TextBox mCommandTextBox;
        private System.Windows.Forms.Button mOKButton;
    }
}