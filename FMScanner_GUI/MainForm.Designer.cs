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
            this.InputFilesListBox = new System.Windows.Forms.ListBox();
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
            this.OutputDirLabel = new System.Windows.Forms.Label();
            this.OutputDirTextBox = new System.Windows.Forms.TextBox();
            this.OutputDirBrowseButton = new System.Windows.Forms.Button();
            this.ClearInputFilesButton = new System.Windows.Forms.Button();
            this.AddFMsButton = new System.Windows.Forms.Button();
            this.ScanProgressBar = new System.Windows.Forms.ProgressBar();
            this.IgnoreFMSelBakCheckBox = new System.Windows.Forms.CheckBox();
            this.ScanInfoLabel = new System.Windows.Forms.Label();
            this.CancelScanButton = new System.Windows.Forms.Button();
            this.OutputFileNoteLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InputFilesListBox
            // 
            this.InputFilesListBox.AllowDrop = true;
            this.InputFilesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputFilesListBox.FormattingEnabled = true;
            this.InputFilesListBox.Location = new System.Drawing.Point(8, 8);
            this.InputFilesListBox.Name = "InputFilesListBox";
            this.InputFilesListBox.Size = new System.Drawing.Size(904, 316);
            this.InputFilesListBox.TabIndex = 0;
            this.InputFilesListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.InputFilesListBox_DragDrop);
            this.InputFilesListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.InputFilesListBox_DragEnter);
            // 
            // TitleCheckBox
            // 
            this.TitleCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TitleCheckBox.AutoSize = true;
            this.TitleCheckBox.Checked = true;
            this.TitleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TitleCheckBox.Location = new System.Drawing.Point(16, 360);
            this.TitleCheckBox.Name = "TitleCheckBox";
            this.TitleCheckBox.Size = new System.Drawing.Size(46, 17);
            this.TitleCheckBox.TabIndex = 1;
            this.TitleCheckBox.Text = "Title";
            this.TitleCheckBox.UseVisualStyleBackColor = true;
            // 
            // AuthorCheckBox
            // 
            this.AuthorCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AuthorCheckBox.AutoSize = true;
            this.AuthorCheckBox.Checked = true;
            this.AuthorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AuthorCheckBox.Location = new System.Drawing.Point(16, 376);
            this.AuthorCheckBox.Name = "AuthorCheckBox";
            this.AuthorCheckBox.Size = new System.Drawing.Size(57, 17);
            this.AuthorCheckBox.TabIndex = 1;
            this.AuthorCheckBox.Text = "Author";
            this.AuthorCheckBox.UseVisualStyleBackColor = true;
            // 
            // GameCheckBox
            // 
            this.GameCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GameCheckBox.AutoSize = true;
            this.GameCheckBox.Checked = true;
            this.GameCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GameCheckBox.Location = new System.Drawing.Point(16, 392);
            this.GameCheckBox.Name = "GameCheckBox";
            this.GameCheckBox.Size = new System.Drawing.Size(54, 17);
            this.GameCheckBox.TabIndex = 1;
            this.GameCheckBox.Text = "Game";
            this.GameCheckBox.UseVisualStyleBackColor = true;
            // 
            // CustomResourcesCheckBox
            // 
            this.CustomResourcesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CustomResourcesCheckBox.AutoSize = true;
            this.CustomResourcesCheckBox.Checked = true;
            this.CustomResourcesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CustomResourcesCheckBox.Location = new System.Drawing.Point(16, 408);
            this.CustomResourcesCheckBox.Name = "CustomResourcesCheckBox";
            this.CustomResourcesCheckBox.Size = new System.Drawing.Size(110, 17);
            this.CustomResourcesCheckBox.TabIndex = 1;
            this.CustomResourcesCheckBox.Text = "Custom resources";
            this.CustomResourcesCheckBox.UseVisualStyleBackColor = true;
            // 
            // SizeCheckBox
            // 
            this.SizeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SizeCheckBox.AutoSize = true;
            this.SizeCheckBox.Checked = true;
            this.SizeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SizeCheckBox.Location = new System.Drawing.Point(16, 424);
            this.SizeCheckBox.Name = "SizeCheckBox";
            this.SizeCheckBox.Size = new System.Drawing.Size(46, 17);
            this.SizeCheckBox.TabIndex = 1;
            this.SizeCheckBox.Text = "Size";
            this.SizeCheckBox.UseVisualStyleBackColor = true;
            // 
            // LastUpdatedDateCheckBox
            // 
            this.LastUpdatedDateCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LastUpdatedDateCheckBox.AutoSize = true;
            this.LastUpdatedDateCheckBox.Checked = true;
            this.LastUpdatedDateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LastUpdatedDateCheckBox.Location = new System.Drawing.Point(16, 440);
            this.LastUpdatedDateCheckBox.Name = "LastUpdatedDateCheckBox";
            this.LastUpdatedDateCheckBox.Size = new System.Drawing.Size(112, 17);
            this.LastUpdatedDateCheckBox.TabIndex = 1;
            this.LastUpdatedDateCheckBox.Text = "Last updated date";
            this.LastUpdatedDateCheckBox.UseVisualStyleBackColor = true;
            // 
            // TagsCheckBox
            // 
            this.TagsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TagsCheckBox.AutoSize = true;
            this.TagsCheckBox.Checked = true;
            this.TagsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TagsCheckBox.Location = new System.Drawing.Point(16, 456);
            this.TagsCheckBox.Name = "TagsCheckBox";
            this.TagsCheckBox.Size = new System.Drawing.Size(50, 17);
            this.TagsCheckBox.TabIndex = 1;
            this.TagsCheckBox.Text = "Tags";
            this.TagsCheckBox.UseVisualStyleBackColor = true;
            // 
            // DescriptionCheckBox
            // 
            this.DescriptionCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DescriptionCheckBox.AutoSize = true;
            this.DescriptionCheckBox.Checked = true;
            this.DescriptionCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DescriptionCheckBox.Location = new System.Drawing.Point(200, 456);
            this.DescriptionCheckBox.Name = "DescriptionCheckBox";
            this.DescriptionCheckBox.Size = new System.Drawing.Size(79, 17);
            this.DescriptionCheckBox.TabIndex = 1;
            this.DescriptionCheckBox.Text = "Description";
            this.DescriptionCheckBox.UseVisualStyleBackColor = true;
            // 
            // NewDarkMinVerCheckBox
            // 
            this.NewDarkMinVerCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewDarkMinVerCheckBox.AutoSize = true;
            this.NewDarkMinVerCheckBox.Checked = true;
            this.NewDarkMinVerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NewDarkMinVerCheckBox.Location = new System.Drawing.Point(200, 440);
            this.NewDarkMinVerCheckBox.Name = "NewDarkMinVerCheckBox";
            this.NewDarkMinVerCheckBox.Size = new System.Drawing.Size(151, 17);
            this.NewDarkMinVerCheckBox.TabIndex = 1;
            this.NewDarkMinVerCheckBox.Text = "NewDark minimum version";
            this.NewDarkMinVerCheckBox.UseVisualStyleBackColor = true;
            // 
            // NewDarkRequiredCheckBox
            // 
            this.NewDarkRequiredCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewDarkRequiredCheckBox.AutoSize = true;
            this.NewDarkRequiredCheckBox.Checked = true;
            this.NewDarkRequiredCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NewDarkRequiredCheckBox.Location = new System.Drawing.Point(200, 424);
            this.NewDarkRequiredCheckBox.Name = "NewDarkRequiredCheckBox";
            this.NewDarkRequiredCheckBox.Size = new System.Drawing.Size(112, 17);
            this.NewDarkRequiredCheckBox.TabIndex = 1;
            this.NewDarkRequiredCheckBox.Text = "NewDark required";
            this.NewDarkRequiredCheckBox.UseVisualStyleBackColor = true;
            // 
            // MissionCountCheckBox
            // 
            this.MissionCountCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MissionCountCheckBox.AutoSize = true;
            this.MissionCountCheckBox.Checked = true;
            this.MissionCountCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MissionCountCheckBox.Location = new System.Drawing.Point(200, 360);
            this.MissionCountCheckBox.Name = "MissionCountCheckBox";
            this.MissionCountCheckBox.Size = new System.Drawing.Size(91, 17);
            this.MissionCountCheckBox.TabIndex = 1;
            this.MissionCountCheckBox.Text = "Mission count";
            this.MissionCountCheckBox.UseVisualStyleBackColor = true;
            // 
            // MissionNamesCheckBox
            // 
            this.MissionNamesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MissionNamesCheckBox.AutoSize = true;
            this.MissionNamesCheckBox.Checked = true;
            this.MissionNamesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MissionNamesCheckBox.Location = new System.Drawing.Point(200, 376);
            this.MissionNamesCheckBox.Name = "MissionNamesCheckBox";
            this.MissionNamesCheckBox.Size = new System.Drawing.Size(144, 17);
            this.MissionNamesCheckBox.TabIndex = 1;
            this.MissionNamesCheckBox.Text = "Campaign mission names";
            this.MissionNamesCheckBox.UseVisualStyleBackColor = true;
            // 
            // VersionCheckBox
            // 
            this.VersionCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VersionCheckBox.AutoSize = true;
            this.VersionCheckBox.Checked = true;
            this.VersionCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VersionCheckBox.Location = new System.Drawing.Point(200, 392);
            this.VersionCheckBox.Name = "VersionCheckBox";
            this.VersionCheckBox.Size = new System.Drawing.Size(61, 17);
            this.VersionCheckBox.TabIndex = 1;
            this.VersionCheckBox.Text = "Version";
            this.VersionCheckBox.UseVisualStyleBackColor = true;
            // 
            // LanguagesCheckBox
            // 
            this.LanguagesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LanguagesCheckBox.AutoSize = true;
            this.LanguagesCheckBox.Checked = true;
            this.LanguagesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LanguagesCheckBox.Location = new System.Drawing.Point(200, 408);
            this.LanguagesCheckBox.Name = "LanguagesCheckBox";
            this.LanguagesCheckBox.Size = new System.Drawing.Size(79, 17);
            this.LanguagesCheckBox.TabIndex = 1;
            this.LanguagesCheckBox.Text = "Languages";
            this.LanguagesCheckBox.UseVisualStyleBackColor = true;
            // 
            // ScanButton
            // 
            this.ScanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ScanButton.Location = new System.Drawing.Point(560, 484);
            this.ScanButton.Name = "ScanButton";
            this.ScanButton.Size = new System.Drawing.Size(96, 32);
            this.ScanButton.TabIndex = 2;
            this.ScanButton.Text = "Scan";
            this.ScanButton.UseVisualStyleBackColor = true;
            this.ScanButton.Click += new System.EventHandler(this.ScanButton_Click);
            // 
            // OutputDirLabel
            // 
            this.OutputDirLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OutputDirLabel.AutoSize = true;
            this.OutputDirLabel.Location = new System.Drawing.Point(416, 364);
            this.OutputDirLabel.Name = "OutputDirLabel";
            this.OutputDirLabel.Size = new System.Drawing.Size(56, 13);
            this.OutputDirLabel.TabIndex = 3;
            this.OutputDirLabel.Text = "Output dir:";
            // 
            // OutputDirTextBox
            // 
            this.OutputDirTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OutputDirTextBox.Location = new System.Drawing.Point(416, 380);
            this.OutputDirTextBox.Name = "OutputDirTextBox";
            this.OutputDirTextBox.Size = new System.Drawing.Size(336, 20);
            this.OutputDirTextBox.TabIndex = 4;
            // 
            // OutputDirBrowseButton
            // 
            this.OutputDirBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OutputDirBrowseButton.Location = new System.Drawing.Point(752, 379);
            this.OutputDirBrowseButton.Name = "OutputDirBrowseButton";
            this.OutputDirBrowseButton.Size = new System.Drawing.Size(75, 22);
            this.OutputDirBrowseButton.TabIndex = 5;
            this.OutputDirBrowseButton.Text = "Browse...";
            this.OutputDirBrowseButton.UseVisualStyleBackColor = true;
            this.OutputDirBrowseButton.Click += new System.EventHandler(this.OutputFileBrowseButton_Click);
            // 
            // ClearInputFilesButton
            // 
            this.ClearInputFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearInputFilesButton.Location = new System.Drawing.Point(838, 324);
            this.ClearInputFilesButton.Name = "ClearInputFilesButton";
            this.ClearInputFilesButton.Size = new System.Drawing.Size(75, 23);
            this.ClearInputFilesButton.TabIndex = 6;
            this.ClearInputFilesButton.Text = "Clear";
            this.ClearInputFilesButton.UseVisualStyleBackColor = true;
            this.ClearInputFilesButton.Click += new System.EventHandler(this.ClearInputFilesButton_Click);
            // 
            // AddFMsButton
            // 
            this.AddFMsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddFMsButton.Location = new System.Drawing.Point(758, 324);
            this.AddFMsButton.Name = "AddFMsButton";
            this.AddFMsButton.Size = new System.Drawing.Size(80, 23);
            this.AddFMsButton.TabIndex = 6;
            this.AddFMsButton.Text = "Add FMs...";
            this.AddFMsButton.UseVisualStyleBackColor = true;
            this.AddFMsButton.Click += new System.EventHandler(this.AddFMsButton_Click);
            // 
            // ScanProgressBar
            // 
            this.ScanProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ScanProgressBar.Location = new System.Drawing.Point(424, 460);
            this.ScanProgressBar.Name = "ScanProgressBar";
            this.ScanProgressBar.Size = new System.Drawing.Size(320, 16);
            this.ScanProgressBar.TabIndex = 7;
            // 
            // IgnoreFMSelBakCheckBox
            // 
            this.IgnoreFMSelBakCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.IgnoreFMSelBakCheckBox.AutoSize = true;
            this.IgnoreFMSelBakCheckBox.Checked = true;
            this.IgnoreFMSelBakCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IgnoreFMSelBakCheckBox.Location = new System.Drawing.Point(560, 520);
            this.IgnoreFMSelBakCheckBox.Name = "IgnoreFMSelBakCheckBox";
            this.IgnoreFMSelBakCheckBox.Size = new System.Drawing.Size(204, 17);
            this.IgnoreFMSelBakCheckBox.TabIndex = 8;
            this.IgnoreFMSelBakCheckBox.Text = "Ignore files ending in \".FMSelBak.zip\"";
            this.IgnoreFMSelBakCheckBox.UseVisualStyleBackColor = true;
            // 
            // ScanInfoLabel
            // 
            this.ScanInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ScanInfoLabel.AutoSize = true;
            this.ScanInfoLabel.Location = new System.Drawing.Point(424, 428);
            this.ScanInfoLabel.Name = "ScanInfoLabel";
            this.ScanInfoLabel.Size = new System.Drawing.Size(56, 13);
            this.ScanInfoLabel.TabIndex = 9;
            this.ScanInfoLabel.Text = "[scan info]";
            // 
            // CancelScanButton
            // 
            this.CancelScanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelScanButton.Location = new System.Drawing.Point(656, 488);
            this.CancelScanButton.Name = "CancelScanButton";
            this.CancelScanButton.Size = new System.Drawing.Size(84, 24);
            this.CancelScanButton.TabIndex = 10;
            this.CancelScanButton.Text = "Cancel scan";
            this.CancelScanButton.UseVisualStyleBackColor = true;
            this.CancelScanButton.Click += new System.EventHandler(this.CancelScanButton_Click);
            // 
            // OutputFileNoteLabel
            // 
            this.OutputFileNoteLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OutputFileNoteLabel.AutoSize = true;
            this.OutputFileNoteLabel.Location = new System.Drawing.Point(416, 404);
            this.OutputFileNoteLabel.Name = "OutputFileNoteLabel";
            this.OutputFileNoteLabel.Size = new System.Drawing.Size(83, 13);
            this.OutputFileNoteLabel.TabIndex = 3;
            this.OutputFileNoteLabel.Text = "[output file note]";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 555);
            this.Controls.Add(this.CancelScanButton);
            this.Controls.Add(this.ScanInfoLabel);
            this.Controls.Add(this.IgnoreFMSelBakCheckBox);
            this.Controls.Add(this.ScanProgressBar);
            this.Controls.Add(this.AddFMsButton);
            this.Controls.Add(this.ClearInputFilesButton);
            this.Controls.Add(this.OutputDirBrowseButton);
            this.Controls.Add(this.OutputDirTextBox);
            this.Controls.Add(this.OutputFileNoteLabel);
            this.Controls.Add(this.OutputDirLabel);
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
            this.Controls.Add(this.InputFilesListBox);
            this.Name = "MainForm";
            this.Text = "FMScanner GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox InputFilesListBox;
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
        private System.Windows.Forms.Label OutputDirLabel;
        private System.Windows.Forms.TextBox OutputDirTextBox;
        private System.Windows.Forms.Button OutputDirBrowseButton;
        private System.Windows.Forms.Button ClearInputFilesButton;
        private System.Windows.Forms.Button AddFMsButton;
        private System.Windows.Forms.ProgressBar ScanProgressBar;
        private System.Windows.Forms.CheckBox IgnoreFMSelBakCheckBox;
        private System.Windows.Forms.Label ScanInfoLabel;
        private System.Windows.Forms.Button CancelScanButton;
        private System.Windows.Forms.Label OutputFileNoteLabel;
    }
}