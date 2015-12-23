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
            this.AlternativeSaveFileLinkLabel = new System.Windows.Forms.LinkLabel();
            this.ForceAlternativeSaveFileCheckBox = new System.Windows.Forms.CheckBox();
            this.SaveFileBrowseButton = new System.Windows.Forms.Button();
            this.SaveFileTextBox = new System.Windows.Forms.TextBox();
            this.SaveFilePrefixLabel = new System.Windows.Forms.Label();
            this.AutoLoadSaveCheckBox = new System.Windows.Forms.CheckBox();
            this.DownloadReferenceSplitsLabel = new System.Windows.Forms.LinkLabel();
            this.AutoSplittingEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.RunCategorySelectionLabel = new System.Windows.Forms.Label();
            this.RunCategoryNameComboBox = new System.Windows.Forms.ComboBox();
            this.MainGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGroup
            // 
            this.MainGroup.Controls.Add(this.AlternativeSaveFileLinkLabel);
            this.MainGroup.Controls.Add(this.ForceAlternativeSaveFileCheckBox);
            this.MainGroup.Controls.Add(this.SaveFileBrowseButton);
            this.MainGroup.Controls.Add(this.SaveFileTextBox);
            this.MainGroup.Controls.Add(this.SaveFilePrefixLabel);
            this.MainGroup.Controls.Add(this.AutoLoadSaveCheckBox);
            this.MainGroup.Controls.Add(this.DownloadReferenceSplitsLabel);
            this.MainGroup.Controls.Add(this.AutoSplittingEnabledCheckBox);
            this.MainGroup.Controls.Add(this.RunCategorySelectionLabel);
            this.MainGroup.Controls.Add(this.RunCategoryNameComboBox);
            this.MainGroup.Location = new System.Drawing.Point(3, 3);
            this.MainGroup.Name = "MainGroup";
            this.MainGroup.Size = new System.Drawing.Size(287, 151);
            this.MainGroup.TabIndex = 1;
            this.MainGroup.TabStop = false;
            this.MainGroup.Text = "SpelunkySplitter (sashavol)";
            // 
            // AlternativeSaveFileLinkLabel
            // 
            this.AlternativeSaveFileLinkLabel.AutoSize = true;
            this.AlternativeSaveFileLinkLabel.Location = new System.Drawing.Point(211, 41);
            this.AlternativeSaveFileLinkLabel.Name = "AlternativeSaveFileLinkLabel";
            this.AlternativeSaveFileLinkLabel.Size = new System.Drawing.Size(13, 13);
            this.AlternativeSaveFileLinkLabel.TabIndex = 12;
            this.AlternativeSaveFileLinkLabel.TabStop = true;
            this.AlternativeSaveFileLinkLabel.Text = "?";
            // 
            // ForceAlternativeSaveFileCheckBox
            // 
            this.ForceAlternativeSaveFileCheckBox.AutoSize = true;
            this.ForceAlternativeSaveFileCheckBox.Location = new System.Drawing.Point(9, 40);
            this.ForceAlternativeSaveFileCheckBox.Name = "ForceAlternativeSaveFileCheckBox";
            this.ForceAlternativeSaveFileCheckBox.Size = new System.Drawing.Size(208, 17);
            this.ForceAlternativeSaveFileCheckBox.TabIndex = 11;
            this.ForceAlternativeSaveFileCheckBox.Text = "Force game to use alternative save file";
            this.ForceAlternativeSaveFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveFileBrowseButton
            // 
            this.SaveFileBrowseButton.Location = new System.Drawing.Point(206, 105);
            this.SaveFileBrowseButton.Name = "SaveFileBrowseButton";
            this.SaveFileBrowseButton.Size = new System.Drawing.Size(76, 22);
            this.SaveFileBrowseButton.TabIndex = 10;
            this.SaveFileBrowseButton.Text = "Browse";
            this.SaveFileBrowseButton.UseVisualStyleBackColor = true;
            // 
            // SaveFileTextBox
            // 
            this.SaveFileTextBox.Location = new System.Drawing.Point(59, 106);
            this.SaveFileTextBox.Name = "SaveFileTextBox";
            this.SaveFileTextBox.ReadOnly = true;
            this.SaveFileTextBox.Size = new System.Drawing.Size(145, 20);
            this.SaveFileTextBox.TabIndex = 9;
            // 
            // SaveFilePrefixLabel
            // 
            this.SaveFilePrefixLabel.AutoSize = true;
            this.SaveFilePrefixLabel.Location = new System.Drawing.Point(5, 109);
            this.SaveFilePrefixLabel.Name = "SaveFilePrefixLabel";
            this.SaveFilePrefixLabel.Size = new System.Drawing.Size(54, 13);
            this.SaveFilePrefixLabel.TabIndex = 8;
            this.SaveFilePrefixLabel.Text = "Save File:";
            // 
            // AutoLoadSaveCheckBox
            // 
            this.AutoLoadSaveCheckBox.AutoSize = true;
            this.AutoLoadSaveCheckBox.Location = new System.Drawing.Point(9, 60);
            this.AutoLoadSaveCheckBox.Name = "AutoLoadSaveCheckBox";
            this.AutoLoadSaveCheckBox.Size = new System.Drawing.Size(261, 17);
            this.AutoLoadSaveCheckBox.TabIndex = 7;
            this.AutoLoadSaveCheckBox.Text = "Auto-load save file before run (backup suggested)";
            this.AutoLoadSaveCheckBox.UseVisualStyleBackColor = true;
            // 
            // DownloadReferenceSplitsLabel
            // 
            this.DownloadReferenceSplitsLabel.AutoSize = true;
            this.DownloadReferenceSplitsLabel.Location = new System.Drawing.Point(5, 131);
            this.DownloadReferenceSplitsLabel.Name = "DownloadReferenceSplitsLabel";
            this.DownloadReferenceSplitsLabel.Size = new System.Drawing.Size(136, 13);
            this.DownloadReferenceSplitsLabel.TabIndex = 4;
            this.DownloadReferenceSplitsLabel.TabStop = true;
            this.DownloadReferenceSplitsLabel.Text = "Download Reference Splits";
            // 
            // AutoSplittingEnabledCheckBox
            // 
            this.AutoSplittingEnabledCheckBox.AutoSize = true;
            this.AutoSplittingEnabledCheckBox.Checked = true;
            this.AutoSplittingEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoSplittingEnabledCheckBox.Location = new System.Drawing.Point(9, 19);
            this.AutoSplittingEnabledCheckBox.Name = "AutoSplittingEnabledCheckBox";
            this.AutoSplittingEnabledCheckBox.Size = new System.Drawing.Size(127, 17);
            this.AutoSplittingEnabledCheckBox.TabIndex = 2;
            this.AutoSplittingEnabledCheckBox.Text = "Auto splitting enabled";
            this.AutoSplittingEnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // RunCategorySelectionLabel
            // 
            this.RunCategorySelectionLabel.AutoSize = true;
            this.RunCategorySelectionLabel.Location = new System.Drawing.Point(6, 86);
            this.RunCategorySelectionLabel.Name = "RunCategorySelectionLabel";
            this.RunCategorySelectionLabel.Size = new System.Drawing.Size(52, 13);
            this.RunCategorySelectionLabel.TabIndex = 1;
            this.RunCategorySelectionLabel.Text = "Category:";
            // 
            // RunCategoryNameComboBox
            // 
            this.RunCategoryNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RunCategoryNameComboBox.FormattingEnabled = true;
            this.RunCategoryNameComboBox.Location = new System.Drawing.Point(59, 82);
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
            this.Size = new System.Drawing.Size(293, 157);
            this.MainGroup.ResumeLayout(false);
            this.MainGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox MainGroup;
        private System.Windows.Forms.Label RunCategorySelectionLabel;
        private System.Windows.Forms.CheckBox AutoSplittingEnabledCheckBox;
        private System.Windows.Forms.ComboBox RunCategoryNameComboBox;
        private System.Windows.Forms.LinkLabel DownloadReferenceSplitsLabel;
        private System.Windows.Forms.Label SaveFilePrefixLabel;
        private System.Windows.Forms.CheckBox AutoLoadSaveCheckBox;
        private System.Windows.Forms.Button SaveFileBrowseButton;
        private System.Windows.Forms.TextBox SaveFileTextBox;
        private System.Windows.Forms.LinkLabel AlternativeSaveFileLinkLabel;
        private System.Windows.Forms.CheckBox ForceAlternativeSaveFileCheckBox;
    }
}
