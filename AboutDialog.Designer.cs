namespace LiveSplit.Spelunky
{
    partial class AboutDialog
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
            this.TitleLabel = new System.Windows.Forms.Label();
            this.DevelopedBySashavol1 = new System.Windows.Forms.Label();
            this.AllCharContribByKonatoK = new System.Windows.Forms.Label();
            this.DevelopedBySashavol2 = new System.Windows.Forms.LinkLabel();
            this.OKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Location = new System.Drawing.Point(13, 13);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(51, 13);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "#TITLE#";
            // 
            // DevelopedBySashavol1
            // 
            this.DevelopedBySashavol1.AutoSize = true;
            this.DevelopedBySashavol1.Location = new System.Drawing.Point(13, 32);
            this.DevelopedBySashavol1.Name = "DevelopedBySashavol1";
            this.DevelopedBySashavol1.Size = new System.Drawing.Size(73, 13);
            this.DevelopedBySashavol1.TabIndex = 1;
            this.DevelopedBySashavol1.Text = "Developed by";
            // 
            // AllCharContribByKonatoK
            // 
            this.AllCharContribByKonatoK.AutoSize = true;
            this.AllCharContribByKonatoK.Location = new System.Drawing.Point(13, 51);
            this.AllCharContribByKonatoK.Name = "AllCharContribByKonatoK";
            this.AllCharContribByKonatoK.Size = new System.Drawing.Size(194, 13);
            this.AllCharContribByKonatoK.TabIndex = 2;
            this.AllCharContribByKonatoK.Text = "All Characters% contributed by KonatoK";
            // 
            // DevelopedBySashavol2
            // 
            this.DevelopedBySashavol2.AutoSize = true;
            this.DevelopedBySashavol2.Location = new System.Drawing.Point(83, 32);
            this.DevelopedBySashavol2.Name = "DevelopedBySashavol2";
            this.DevelopedBySashavol2.Size = new System.Drawing.Size(49, 13);
            this.DevelopedBySashavol2.TabIndex = 3;
            this.DevelopedBySashavol2.TabStop = true;
            this.DevelopedBySashavol2.Text = "sashavol";
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(168, 77);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // AboutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 109);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.DevelopedBySashavol2);
            this.Controls.Add(this.AllCharContribByKonatoK);
            this.Controls.Add(this.DevelopedBySashavol1);
            this.Controls.Add(this.TitleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.Text = "About SpelunkySplitter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label DevelopedBySashavol1;
        private System.Windows.Forms.Label AllCharContribByKonatoK;
        private System.Windows.Forms.LinkLabel DevelopedBySashavol2;
        private System.Windows.Forms.Button OKButton;
    }
}