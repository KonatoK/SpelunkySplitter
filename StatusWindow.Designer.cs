namespace LiveSplit.Spelunky
{
    partial class StatusWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusWindow));
            this.CurrentRunPrefixLabel = new System.Windows.Forms.Label();
            this.StatusPrefixLabel = new System.Windows.Forms.Label();
            this.CurrentRunLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CurrentRunPrefixLabel
            // 
            this.CurrentRunPrefixLabel.AutoSize = true;
            this.CurrentRunPrefixLabel.Location = new System.Drawing.Point(4, 9);
            this.CurrentRunPrefixLabel.Name = "CurrentRunPrefixLabel";
            this.CurrentRunPrefixLabel.Size = new System.Drawing.Size(67, 13);
            this.CurrentRunPrefixLabel.TabIndex = 0;
            this.CurrentRunPrefixLabel.Text = "Current Run:";
            // 
            // StatusPrefixLabel
            // 
            this.StatusPrefixLabel.AutoSize = true;
            this.StatusPrefixLabel.Location = new System.Drawing.Point(4, 32);
            this.StatusPrefixLabel.Name = "StatusPrefixLabel";
            this.StatusPrefixLabel.Size = new System.Drawing.Size(40, 13);
            this.StatusPrefixLabel.TabIndex = 1;
            this.StatusPrefixLabel.Text = "Status:";
            // 
            // CurrentRunLabel
            // 
            this.CurrentRunLabel.AutoSize = true;
            this.CurrentRunLabel.Location = new System.Drawing.Point(70, 9);
            this.CurrentRunLabel.Name = "CurrentRunLabel";
            this.CurrentRunLabel.Size = new System.Drawing.Size(98, 13);
            this.CurrentRunLabel.TabIndex = 2;
            this.CurrentRunLabel.Text = "#CURRENTRUN#";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(44, 32);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(64, 13);
            this.StatusLabel.TabIndex = 3;
            this.StatusLabel.Text = "#STATUS#";
            // 
            // StatusWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 57);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.CurrentRunLabel);
            this.Controls.Add(this.StatusPrefixLabel);
            this.Controls.Add(this.CurrentRunPrefixLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StatusWindow";
            this.Text = "SpelunkySplitter Activity";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurrentRunPrefixLabel;
        private System.Windows.Forms.Label StatusPrefixLabel;
        private System.Windows.Forms.Label CurrentRunLabel;
        private System.Windows.Forms.Label StatusLabel;
    }
}