namespace LiveSplit.Spelunky
{
    partial class SpelunkySettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainGroup = new System.Windows.Forms.GroupBox();
            this.AutoSplittingEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.RunCategorySelectionLabel = new System.Windows.Forms.Label();
            this.RunCategoryNameComboBox = new System.Windows.Forms.ComboBox();
            this.MainGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGroup
            // 
            this.MainGroup.Controls.Add(this.AutoSplittingEnabledCheckBox);
            this.MainGroup.Controls.Add(this.RunCategorySelectionLabel);
            this.MainGroup.Controls.Add(this.RunCategoryNameComboBox);
            this.MainGroup.Location = new System.Drawing.Point(3, 3);
            this.MainGroup.Name = "MainGroup";
            this.MainGroup.Size = new System.Drawing.Size(287, 74);
            this.MainGroup.TabIndex = 1;
            this.MainGroup.TabStop = false;
            this.MainGroup.Text = "SpelunkySplitter (sashavol)";
            // 
            // AutoSplittingEnabledCheckBox
            // 
            this.AutoSplittingEnabledCheckBox.AutoSize = true;
            this.AutoSplittingEnabledCheckBox.Checked = true;
            this.AutoSplittingEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoSplittingEnabledCheckBox.Location = new System.Drawing.Point(9, 19);
            this.AutoSplittingEnabledCheckBox.Name = "AutoSplittingEnabledCheckBox";
            this.AutoSplittingEnabledCheckBox.Size = new System.Drawing.Size(130, 17);
            this.AutoSplittingEnabledCheckBox.TabIndex = 2;
            this.AutoSplittingEnabledCheckBox.Text = "Auto Splitting Enabled";
            this.AutoSplittingEnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // RunCategorySelectionLabel
            // 
            this.RunCategorySelectionLabel.AutoSize = true;
            this.RunCategorySelectionLabel.Location = new System.Drawing.Point(6, 45);
            this.RunCategorySelectionLabel.Name = "RunCategorySelectionLabel";
            this.RunCategorySelectionLabel.Size = new System.Drawing.Size(52, 13);
            this.RunCategorySelectionLabel.TabIndex = 1;
            this.RunCategorySelectionLabel.Text = "Category:";
            // 
            // RunCategoryNameComboBox
            // 
            this.RunCategoryNameComboBox.FormattingEnabled = true;
            this.RunCategoryNameComboBox.Location = new System.Drawing.Point(59, 42);
            this.RunCategoryNameComboBox.Name = "RunCategoryNameComboBox";
            this.RunCategoryNameComboBox.Size = new System.Drawing.Size(222, 21);
            this.RunCategoryNameComboBox.TabIndex = 0;
            // 
            // SpelunkySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainGroup);
            this.Name = "SpelunkySettings";
            this.Size = new System.Drawing.Size(293, 80);
            this.MainGroup.ResumeLayout(false);
            this.MainGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox MainGroup;
        private System.Windows.Forms.Label RunCategorySelectionLabel;
        private System.Windows.Forms.CheckBox AutoSplittingEnabledCheckBox;
        private System.Windows.Forms.ComboBox RunCategoryNameComboBox;
    }
}
