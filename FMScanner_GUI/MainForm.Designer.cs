namespace FMScanner_GUI
{
    sealed partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.TitleCheckBox = new System.Windows.Forms.CheckBox();
            this.AuthorCheckBox = new System.Windows.Forms.CheckBox();
            this.GameCheckBox = new System.Windows.Forms.CheckBox();
            this.CustomResourcesCheckBox = new System.Windows.Forms.CheckBox();
            this.SizeCheckBox = new System.Windows.Forms.CheckBox();
            this.LastUpdatedDateCheckBox = new System.Windows.Forms.CheckBox();
            this.TagsCheckBox = new System.Windows.Forms.CheckBox();
            this.DescriptionCheckBox = new System.Windows.Forms.CheckBox();
            this.NewDarkMinVerCheckBox = new System.Windows.Forms.CheckBox();
            this.NewDarkRequiredCheckBox = new System.Windows.Forms.CheckBox();
            this.MissionCountCheckBox = new System.Windows.Forms.CheckBox();
            this.MissionNamesCheckBox = new System.Windows.Forms.CheckBox();
            this.VersionCheckBox = new System.Windows.Forms.CheckBox();
            this.LanguagesCheckBox = new System.Windows.Forms.CheckBox();
            this.ScanButton = new System.Windows.Forms.Button();
            this.OutputFileLabel = new System.Windows.Forms.Label();
            this.OutputFileTextBox = new System.Windows.Forms.TextBox();
            this.OutputFileBrowseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(8, 8);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(904, 342);
            this.listBox1.TabIndex = 0;
            // 
            // TitleCheckBox
            // 
            this.TitleCheckBox.AutoSize = true;
            this.TitleCheckBox.Checked = true;
            this.TitleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TitleCheckBox.Location = new System.Drawing.Point(16, 376);
            this.TitleCheckBox.Name = "TitleCheckBox";
            this.TitleCheckBox.Size = new System.Drawing.Size(46, 17);
            this.TitleCheckBox.TabIndex = 1;
            this.TitleCheckBox.Text = "Title";
            this.TitleCheckBox.UseVisualStyleBackColor = true;
            // 
            // AuthorCheckBox
            // 
            this.AuthorCheckBox.AutoSize = true;
            this.AuthorCheckBox.Checked = true;
            this.AuthorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AuthorCheckBox.Location = new System.Drawing.Point(16, 392);
            this.AuthorCheckBox.Name = "AuthorCheckBox";
            this.AuthorCheckBox.Size = new System.Drawing.Size(57, 17);
            this.AuthorCheckBox.TabIndex = 1;
            this.AuthorCheckBox.Text = "Author";
            this.AuthorCheckBox.UseVisualStyleBackColor = true;
            // 
            // GameCheckBox
            // 
            this.GameCheckBox.AutoSize = true;
            this.GameCheckBox.Checked = true;
            this.GameCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GameCheckBox.Location = new System.Drawing.Point(16, 408);
            this.GameCheckBox.Name = "GameCheckBox";
            this.GameCheckBox.Size = new System.Drawing.Size(54, 17);
            this.GameCheckBox.TabIndex = 1;
            this.GameCheckBox.Text = "Game";
            this.GameCheckBox.UseVisualStyleBackColor = true;
            // 
            // CustomResourcesCheckBox
            // 
            this.CustomResourcesCheckBox.AutoSize = true;
            this.CustomResourcesCheckBox.Checked = true;
            this.CustomResourcesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CustomResourcesCheckBox.Location = new System.Drawing.Point(16, 424);
            this.CustomResourcesCheckBox.Name = "CustomResourcesCheckBox";
            this.CustomResourcesCheckBox.Size = new System.Drawing.Size(110, 17);
            this.CustomResourcesCheckBox.TabIndex = 1;
            this.CustomResourcesCheckBox.Text = "Custom resources";
            this.CustomResourcesCheckBox.UseVisualStyleBackColor = true;
            // 
            // SizeCheckBox
            // 
            this.SizeCheckBox.AutoSize = true;
            this.SizeCheckBox.Checked = true;
            this.SizeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SizeCheckBox.Location = new System.Drawing.Point(16, 440);
            this.SizeCheckBox.Name = "SizeCheckBox";
            this.SizeCheckBox.Size = new System.Drawing.Size(46, 17);
            this.SizeCheckBox.TabIndex = 1;
            this.SizeCheckBox.Text = "Size";
            this.SizeCheckBox.UseVisualStyleBackColor = true;
            // 
            // LastUpdatedDateCheckBox
            // 
            this.LastUpdatedDateCheckBox.AutoSize = true;
            this.LastUpdatedDateCheckBox.Checked = true;
            this.LastUpdatedDateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LastUpdatedDateCheckBox.Location = new System.Drawing.Point(16, 456);
            this.LastUpdatedDateCheckBox.Name = "LastUpdatedDateCheckBox";
            this.LastUpdatedDateCheckBox.Size = new System.Drawing.Size(112, 17);
            this.LastUpdatedDateCheckBox.TabIndex = 1;
            this.LastUpdatedDateCheckBox.Text = "Last updated date";
            this.LastUpdatedDateCheckBox.UseVisualStyleBackColor = true;
            // 
            // TagsCheckBox
            // 
            this.TagsCheckBox.AutoSize = true;
            this.TagsCheckBox.Checked = true;
            this.TagsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TagsCheckBox.Location = new System.Drawing.Point(16, 472);
            this.TagsCheckBox.Name = "TagsCheckBox";
            this.TagsCheckBox.Size = new System.Drawing.Size(50, 17);
            this.TagsCheckBox.TabIndex = 1;
            this.TagsCheckBox.Text = "Tags";
            this.TagsCheckBox.UseVisualStyleBackColor = true;
            // 
            // DescriptionCheckBox
            // 
            this.DescriptionCheckBox.AutoSize = true;
            this.DescriptionCheckBox.Checked = true;
            this.DescriptionCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DescriptionCheckBox.Location = new System.Drawing.Point(200, 472);
            this.DescriptionCheckBox.Name = "DescriptionCheckBox";
            this.DescriptionCheckBox.Size = new System.Drawing.Size(79, 17);
            this.DescriptionCheckBox.TabIndex = 1;
            this.DescriptionCheckBox.Text = "Description";
            this.DescriptionCheckBox.UseVisualStyleBackColor = true;
            // 
            // NewDarkMinVerCheckBox
            // 
            this.NewDarkMinVerCheckBox.AutoSize = true;
            this.NewDarkMinVerCheckBox.Checked = true;
            this.NewDarkMinVerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NewDarkMinVerCheckBox.Location = new System.Drawing.Point(200, 456);
            this.NewDarkMinVerCheckBox.Name = "NewDarkMinVerCheckBox";
            this.NewDarkMinVerCheckBox.Size = new System.Drawing.Size(151, 17);
            this.NewDarkMinVerCheckBox.TabIndex = 1;
            this.NewDarkMinVerCheckBox.Text = "NewDark minimum version";
            this.NewDarkMinVerCheckBox.UseVisualStyleBackColor = true;
            // 
            // NewDarkRequiredCheckBox
            // 
            this.NewDarkRequiredCheckBox.AutoSize = true;
            this.NewDarkRequiredCheckBox.Checked = true;
            this.NewDarkRequiredCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NewDarkRequiredCheckBox.Location = new System.Drawing.Point(200, 440);
            this.NewDarkRequiredCheckBox.Name = "NewDarkRequiredCheckBox";
            this.NewDarkRequiredCheckBox.Size = new System.Drawing.Size(112, 17);
            this.NewDarkRequiredCheckBox.TabIndex = 1;
            this.NewDarkRequiredCheckBox.Text = "NewDark required";
            this.NewDarkRequiredCheckBox.UseVisualStyleBackColor = true;
            // 
            // MissionCountCheckBox
            // 
            this.MissionCountCheckBox.AutoSize = true;
            this.MissionCountCheckBox.Checked = true;
            this.MissionCountCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MissionCountCheckBox.Location = new System.Drawing.Point(200, 376);
            this.MissionCountCheckBox.Name = "MissionCountCheckBox";
            this.MissionCountCheckBox.Size = new System.Drawing.Size(91, 17);
            this.MissionCountCheckBox.TabIndex = 1;
            this.MissionCountCheckBox.Text = "Mission count";
            this.MissionCountCheckBox.UseVisualStyleBackColor = true;
            // 
            // MissionNamesCheckBox
            // 
            this.MissionNamesCheckBox.AutoSize = true;
            this.MissionNamesCheckBox.Checked = true;
            this.MissionNamesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MissionNamesCheckBox.Location = new System.Drawing.Point(200, 392);
            this.MissionNamesCheckBox.Name = "MissionNamesCheckBox";
            this.MissionNamesCheckBox.Size = new System.Drawing.Size(144, 17);
            this.MissionNamesCheckBox.TabIndex = 1;
            this.MissionNamesCheckBox.Text = "Campaign mission names";
            this.MissionNamesCheckBox.UseVisualStyleBackColor = true;
            // 
            // VersionCheckBox
            // 
            this.VersionCheckBox.AutoSize = true;
            this.VersionCheckBox.Checked = true;
            this.VersionCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VersionCheckBox.Location = new System.Drawing.Point(200, 408);
            this.VersionCheckBox.Name = "VersionCheckBox";
            this.VersionCheckBox.Size = new System.Drawing.Size(61, 17);
            this.VersionCheckBox.TabIndex = 1;
            this.VersionCheckBox.Text = "Version";
            this.VersionCheckBox.UseVisualStyleBackColor = true;
            // 
            // LanguagesCheckBox
            // 
            this.LanguagesCheckBox.AutoSize = true;
            this.LanguagesCheckBox.Checked = true;
            this.LanguagesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LanguagesCheckBox.Location = new System.Drawing.Point(200, 424);
            this.LanguagesCheckBox.Name = "LanguagesCheckBox";
            this.LanguagesCheckBox.Size = new System.Drawing.Size(79, 17);
            this.LanguagesCheckBox.TabIndex = 1;
            this.LanguagesCheckBox.Text = "Languages";
            this.LanguagesCheckBox.UseVisualStyleBackColor = true;
            // 
            // ScanButton
            // 
            this.ScanButton.Location = new System.Drawing.Point(480, 472);
            this.ScanButton.Name = "ScanButton";
            this.ScanButton.Size = new System.Drawing.Size(104, 23);
            this.ScanButton.TabIndex = 2;
            this.ScanButton.Text = "Scan";
            this.ScanButton.UseVisualStyleBackColor = true;
            // 
            // OutputFileLabel
            // 
            this.OutputFileLabel.AutoSize = true;
            this.OutputFileLabel.Location = new System.Drawing.Point(416, 376);
            this.OutputFileLabel.Name = "OutputFileLabel";
            this.OutputFileLabel.Size = new System.Drawing.Size(58, 13);
            this.OutputFileLabel.TabIndex = 3;
            this.OutputFileLabel.Text = "Output file:";
            // 
            // OutputFileTextBox
            // 
            this.OutputFileTextBox.Location = new System.Drawing.Point(480, 373);
            this.OutputFileTextBox.Name = "OutputFileTextBox";
            this.OutputFileTextBox.Size = new System.Drawing.Size(336, 20);
            this.OutputFileTextBox.TabIndex = 4;
            // 
            // OutputFileBrowseButton
            // 
            this.OutputFileBrowseButton.Location = new System.Drawing.Point(816, 372);
            this.OutputFileBrowseButton.Name = "OutputFileBrowseButton";
            this.OutputFileBrowseButton.Size = new System.Drawing.Size(75, 22);
            this.OutputFileBrowseButton.TabIndex = 5;
            this.OutputFileBrowseButton.Text = "Browse...";
            this.OutputFileBrowseButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 563);
            this.Controls.Add(this.OutputFileBrowseButton);
            this.Controls.Add(this.OutputFileTextBox);
            this.Controls.Add(this.OutputFileLabel);
            this.Controls.Add(this.ScanButton);
            this.Controls.Add(this.LanguagesCheckBox);
            this.Controls.Add(this.VersionCheckBox);
            this.Controls.Add(this.MissionNamesCheckBox);
            this.Controls.Add(this.MissionCountCheckBox);
            this.Controls.Add(this.NewDarkRequiredCheckBox);
            this.Controls.Add(this.NewDarkMinVerCheckBox);
            this.Controls.Add(this.DescriptionCheckBox);
            this.Controls.Add(this.TagsCheckBox);
            this.Controls.Add(this.LastUpdatedDateCheckBox);
            this.Controls.Add(this.SizeCheckBox);
            this.Controls.Add(this.CustomResourcesCheckBox);
            this.Controls.Add(this.GameCheckBox);
            this.Controls.Add(this.AuthorCheckBox);
            this.Controls.Add(this.TitleCheckBox);
            this.Controls.Add(this.listBox1);
            this.Name = "MainForm";
            this.Text = "FMScanner GUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox TitleCheckBox;
        private System.Windows.Forms.CheckBox AuthorCheckBox;
        private System.Windows.Forms.CheckBox GameCheckBox;
        private System.Windows.Forms.CheckBox CustomResourcesCheckBox;
        private System.Windows.Forms.CheckBox SizeCheckBox;
        private System.Windows.Forms.CheckBox LastUpdatedDateCheckBox;
        private System.Windows.Forms.CheckBox TagsCheckBox;
        private System.Windows.Forms.CheckBox DescriptionCheckBox;
        private System.Windows.Forms.CheckBox NewDarkMinVerCheckBox;
        private System.Windows.Forms.CheckBox NewDarkRequiredCheckBox;
        private System.Windows.Forms.CheckBox MissionCountCheckBox;
        private System.Windows.Forms.CheckBox MissionNamesCheckBox;
        private System.Windows.Forms.CheckBox VersionCheckBox;
        private System.Windows.Forms.CheckBox LanguagesCheckBox;
        private System.Windows.Forms.Button ScanButton;
        private System.Windows.Forms.Label OutputFileLabel;
        private System.Windows.Forms.TextBox OutputFileTextBox;
        private System.Windows.Forms.Button OutputFileBrowseButton;
    }
}