﻿namespace Capgemini.Xrm.DataMigration.XrmToolBoxPlugin.UserControls
{
    partial class importWizard
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
            this.stepWizardControl1 = new AeroWizard.WizardPageContainer();
            this.wizardPage3 = new AeroWizard.WizardPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbIgnoreStatuses = new Capgemini.Xrm.DataMigration.XrmToolBoxPlugin.UserControls.ToggleCheckBox();
            this.cbIgnoreSystemFields = new Capgemini.Xrm.DataMigration.XrmToolBoxPlugin.UserControls.ToggleCheckBox();
            this.nudSavePageSize = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.nudMaxThreads = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ConnectionString = new System.Windows.Forms.Label();
            this.buttonTargetConnectionString = new System.Windows.Forms.Button();
            this.labelTargetConnectionString = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.wizardPage4 = new AeroWizard.WizardPage();
            this.label10 = new System.Windows.Forms.Label();
            this.tbLogger = new System.Windows.Forms.TextBox();
            this.wizardPage1 = new AeroWizard.WizardPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbImportSchema = new System.Windows.Forms.TextBox();
            this.btnImportSchema = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.wizardPage2 = new AeroWizard.WizardPage();
            this.tbSourceDataLocation = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.wizardPage5 = new AeroWizard.WizardPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btLoadImportConfigFile = new System.Windows.Forms.Button();
            this.tbImportConfigFile = new System.Windows.Forms.TextBox();
            this.wizardButtons1 = new Capgemini.Xrm.DataMigration.XrmToolBoxPlugin.UserControls.WizardButtons();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.stepWizardControl1)).BeginInit();
            this.stepWizardControl1.SuspendLayout();
            this.wizardPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSavePageSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxThreads)).BeginInit();
            this.wizardPage4.SuspendLayout();
            this.wizardPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.wizardPage2.SuspendLayout();
            this.wizardPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // stepWizardControl1
            // 
            this.stepWizardControl1.BackButton = null;
            this.stepWizardControl1.BackButtonText = "";
            this.stepWizardControl1.CancelButton = null;
            this.stepWizardControl1.CancelButtonText = "";
            this.stepWizardControl1.Controls.Add(this.wizardPage1);
            this.stepWizardControl1.Controls.Add(this.wizardPage2);
            this.stepWizardControl1.Controls.Add(this.wizardPage4);
            this.stepWizardControl1.Controls.Add(this.wizardPage3);
            this.stepWizardControl1.Controls.Add(this.wizardPage5);
            this.stepWizardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stepWizardControl1.Location = new System.Drawing.Point(0, 0);
            this.stepWizardControl1.Name = "stepWizardControl1";
            this.stepWizardControl1.NextButton = null;
            this.stepWizardControl1.Pages.Add(this.wizardPage1);
            this.stepWizardControl1.Pages.Add(this.wizardPage2);
            this.stepWizardControl1.Pages.Add(this.wizardPage5);
            this.stepWizardControl1.Pages.Add(this.wizardPage3);
            this.stepWizardControl1.Pages.Add(this.wizardPage4);
            this.stepWizardControl1.ShowProgressInTaskbarIcon = true;
            this.stepWizardControl1.Size = new System.Drawing.Size(1164, 569);
            this.stepWizardControl1.TabIndex = 0;
            // 
            // wizardPage3
            // 
            this.wizardPage3.AllowCancel = false;
            this.wizardPage3.AllowNext = false;
            this.wizardPage3.Controls.Add(this.label9);
            this.wizardPage3.Controls.Add(this.label8);
            this.wizardPage3.Controls.Add(this.cbIgnoreStatuses);
            this.wizardPage3.Controls.Add(this.cbIgnoreSystemFields);
            this.wizardPage3.Controls.Add(this.nudSavePageSize);
            this.wizardPage3.Controls.Add(this.numericUpDown1);
            this.wizardPage3.Controls.Add(this.label11);
            this.wizardPage3.Controls.Add(this.nudMaxThreads);
            this.wizardPage3.Controls.Add(this.label6);
            this.wizardPage3.Controls.Add(this.label5);
            this.wizardPage3.Controls.Add(this.ConnectionString);
            this.wizardPage3.Controls.Add(this.buttonTargetConnectionString);
            this.wizardPage3.Controls.Add(this.labelTargetConnectionString);
            this.wizardPage3.Controls.Add(this.label3);
            this.wizardPage3.Name = "wizardPage3";
            this.wizardPage3.NextPage = this.wizardPage4;
            this.wizardPage3.ShowCancel = false;
            this.wizardPage3.Size = new System.Drawing.Size(1164, 569);
            this.wizardPage3.TabIndex = 4;
            this.wizardPage3.Text = "Import Settings";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(53, 266);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(225, 31);
            this.label9.TabIndex = 57;
            this.label9.Text = "Ignore Record Status";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(53, 228);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(223, 31);
            this.label8.TabIndex = 56;
            this.label8.Text = "Ignore System Fields";
            // 
            // cbIgnoreStatuses
            // 
            this.cbIgnoreStatuses.Location = new System.Drawing.Point(284, 263);
            this.cbIgnoreStatuses.Name = "cbIgnoreStatuses";
            this.cbIgnoreStatuses.Padding = new System.Windows.Forms.Padding(4);
            this.cbIgnoreStatuses.Size = new System.Drawing.Size(49, 24);
            this.cbIgnoreStatuses.TabIndex = 55;
            this.cbIgnoreStatuses.Text = "toggleCheckBox2";
            this.cbIgnoreStatuses.UseVisualStyleBackColor = true;
            // 
            // cbIgnoreSystemFields
            // 
            this.cbIgnoreSystemFields.Location = new System.Drawing.Point(285, 228);
            this.cbIgnoreSystemFields.Name = "cbIgnoreSystemFields";
            this.cbIgnoreSystemFields.Padding = new System.Windows.Forms.Padding(4);
            this.cbIgnoreSystemFields.Size = new System.Drawing.Size(49, 24);
            this.cbIgnoreSystemFields.TabIndex = 54;
            this.cbIgnoreSystemFields.Text = "toggleCheckBox1";
            this.cbIgnoreSystemFields.UseVisualStyleBackColor = true;
            // 
            // nudSavePageSize
            // 
            this.nudSavePageSize.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSavePageSize.Location = new System.Drawing.Point(285, 136);
            this.nudSavePageSize.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudSavePageSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSavePageSize.Name = "nudSavePageSize";
            this.nudSavePageSize.Size = new System.Drawing.Size(120, 37);
            this.nudSavePageSize.TabIndex = 53;
            this.nudSavePageSize.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown1.Location = new System.Drawing.Point(284, 179);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 37);
            this.numericUpDown1.TabIndex = 52;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(53, 179);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(145, 31);
            this.label11.TabIndex = 49;
            this.label11.Text = "Max Threads";
            // 
            // nudMaxThreads
            // 
            this.nudMaxThreads.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMaxThreads.Location = new System.Drawing.Point(285, 179);
            this.nudMaxThreads.Name = "nudMaxThreads";
            this.nudMaxThreads.Size = new System.Drawing.Size(120, 37);
            this.nudMaxThreads.TabIndex = 52;
            this.nudMaxThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(53, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(145, 31);
            this.label6.TabIndex = 49;
            this.label6.Text = "Max Threads";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(53, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 31);
            this.label5.TabIndex = 47;
            this.label5.Text = "Save Page Size";
            // 
            // ConnectionString
            // 
            this.ConnectionString.AutoSize = true;
            this.ConnectionString.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectionString.Location = new System.Drawing.Point(52, 93);
            this.ConnectionString.Name = "ConnectionString";
            this.ConnectionString.Size = new System.Drawing.Size(265, 31);
            this.ConnectionString.TabIndex = 39;
            this.ConnectionString.Text = "Target Connection String";
            // 
            // buttonTargetConnectionString
            // 
            this.buttonTargetConnectionString.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTargetConnectionString.Location = new System.Drawing.Point(692, 93);
            this.buttonTargetConnectionString.Name = "buttonTargetConnectionString";
            this.buttonTargetConnectionString.Size = new System.Drawing.Size(49, 41);
            this.buttonTargetConnectionString.TabIndex = 40;
            this.buttonTargetConnectionString.Text = "...";
            this.buttonTargetConnectionString.UseVisualStyleBackColor = true;
            this.buttonTargetConnectionString.Click += new System.EventHandler(this.buttonTargetConnectionString_Click);
            // 
            // labelTargetConnectionString
            // 
            this.labelTargetConnectionString.AutoSize = true;
            this.labelTargetConnectionString.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTargetConnectionString.ForeColor = System.Drawing.Color.Green;
            this.labelTargetConnectionString.Location = new System.Drawing.Point(361, 93);
            this.labelTargetConnectionString.MinimumSize = new System.Drawing.Size(170, 0);
            this.labelTargetConnectionString.Name = "labelTargetConnectionString";
            this.labelTargetConnectionString.Size = new System.Drawing.Size(170, 31);
            this.labelTargetConnectionString.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(52, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(424, 40);
            this.label3.TabIndex = 0;
            this.label3.Text = "Please set the setting for import";
            // 
            // wizardPage4
            // 
            this.wizardPage4.AllowCancel = false;
            this.wizardPage4.AllowNext = false;
            this.wizardPage4.Controls.Add(this.label10);
            this.wizardPage4.Controls.Add(this.tbLogger);
            this.wizardPage4.IsFinishPage = true;
            this.wizardPage4.Name = "wizardPage4";
            this.wizardPage4.ShowCancel = false;
            this.wizardPage4.Size = new System.Drawing.Size(1164, 569);
            this.wizardPage4.TabIndex = 5;
            this.wizardPage4.Text = "Perform Import";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(34, 1);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 31);
            this.label10.TabIndex = 13;
            this.label10.Text = "Logs";
            // 
            // tbLogger
            // 
            this.tbLogger.BackColor = System.Drawing.SystemColors.Control;
            this.tbLogger.Location = new System.Drawing.Point(40, 35);
            this.tbLogger.Multiline = true;
            this.tbLogger.Name = "tbLogger";
            this.tbLogger.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLogger.Size = new System.Drawing.Size(1082, 526);
            this.tbLogger.TabIndex = 14;
            // 
            // wizardPage1
            // 
            this.wizardPage1.AllowCancel = false;
            this.wizardPage1.AllowNext = false;
            this.wizardPage1.Controls.Add(this.groupBox1);
            this.wizardPage1.Controls.Add(this.label1);
            this.wizardPage1.Controls.Add(this.radioButton2);
            this.wizardPage1.Controls.Add(this.radioButton1);
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.NextPage = this.wizardPage2;
            this.wizardPage1.ShowCancel = false;
            this.wizardPage1.Size = new System.Drawing.Size(1164, 569);
            this.wizardPage1.TabIndex = 2;
            this.wizardPage1.Text = "Select Data Format";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbImportSchema);
            this.groupBox1.Controls.Add(this.btnImportSchema);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.groupBox1.Location = new System.Drawing.Point(59, 177);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(9);
            this.groupBox1.Size = new System.Drawing.Size(460, 88);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Schema file (CSV only)";
            this.groupBox1.Visible = false;
            // 
            // tbImportSchema
            // 
            this.tbImportSchema.Location = new System.Drawing.Point(6, 38);
            this.tbImportSchema.Name = "tbImportSchema";
            this.tbImportSchema.Size = new System.Drawing.Size(376, 37);
            this.tbImportSchema.TabIndex = 4;
            this.tbImportSchema.TextChanged += new System.EventHandler(this.tbimportSchema_textChanged);
            // 
            // btnImportSchema
            // 
            this.btnImportSchema.Location = new System.Drawing.Point(388, 38);
            this.btnImportSchema.Name = "btnImportSchema";
            this.btnImportSchema.Size = new System.Drawing.Size(45, 34);
            this.btnImportSchema.TabIndex = 3;
            this.btnImportSchema.Text = "...";
            this.btnImportSchema.UseVisualStyleBackColor = true;
            this.btnImportSchema.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(52, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(707, 40);
            this.label1.TabIndex = 2;
            this.label1.Text = "Which format would you like to import the data from?";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(59, 105);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(93, 35);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "JSON";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(59, 144);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(79, 35);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "CSV";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // wizardPage2
            // 
            this.wizardPage2.AllowCancel = false;
            this.wizardPage2.AllowNext = false;
            this.wizardPage2.Controls.Add(this.tbSourceDataLocation);
            this.wizardPage2.Controls.Add(this.button1);
            this.wizardPage2.Controls.Add(this.label2);
            this.wizardPage2.Name = "wizardPage2";
            this.wizardPage2.NextPage = this.wizardPage5;
            this.wizardPage2.ShowCancel = false;
            this.wizardPage2.Size = new System.Drawing.Size(1164, 569);
            this.wizardPage2.TabIndex = 3;
            this.wizardPage2.Text = "Select Source Directory";
            // 
            // tbSourceDataLocation
            // 
            this.tbSourceDataLocation.Location = new System.Drawing.Point(59, 102);
            this.tbSourceDataLocation.Name = "tbSourceDataLocation";
            this.tbSourceDataLocation.Size = new System.Drawing.Size(345, 26);
            this.tbSourceDataLocation.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(410, 93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 38);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(52, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(538, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "What directory contains the source data?";
            // 
            // wizardPage5
            // 
            this.wizardPage5.AllowCancel = false;
            this.wizardPage5.Controls.Add(this.label7);
            this.wizardPage5.Controls.Add(this.label4);
            this.wizardPage5.Controls.Add(this.btLoadImportConfigFile);
            this.wizardPage5.Controls.Add(this.tbImportConfigFile);
            this.wizardPage5.Name = "wizardPage5";
            this.wizardPage5.NextPage = this.wizardPage3;
            this.wizardPage5.ShowCancel = false;
            this.wizardPage5.Size = new System.Drawing.Size(1164, 569);
            this.wizardPage5.TabIndex = 6;
            this.wizardPage5.Text = "Import Config";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(52, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(807, 40);
            this.label7.TabIndex = 49;
            this.label7.Text = "Do you have an existing Import Config file? Please supply if so";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(53, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(306, 31);
            this.label4.TabIndex = 46;
            this.label4.Text = "Import Config File (Optional)";
            // 
            // btLoadImportConfigFile
            // 
            this.btLoadImportConfigFile.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLoadImportConfigFile.Location = new System.Drawing.Point(418, 128);
            this.btLoadImportConfigFile.Margin = new System.Windows.Forms.Padding(2);
            this.btLoadImportConfigFile.Name = "btLoadImportConfigFile";
            this.btLoadImportConfigFile.Size = new System.Drawing.Size(49, 41);
            this.btLoadImportConfigFile.TabIndex = 48;
            this.btLoadImportConfigFile.Text = "...";
            this.btLoadImportConfigFile.UseVisualStyleBackColor = true;
            this.btLoadImportConfigFile.Click += new System.EventHandler(this.btLoadImportConfigFile_Click);
            // 
            // tbImportConfigFile
            // 
            this.tbImportConfigFile.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbImportConfigFile.Location = new System.Drawing.Point(57, 132);
            this.tbImportConfigFile.Name = "tbImportConfigFile";
            this.tbImportConfigFile.Size = new System.Drawing.Size(356, 37);
            this.tbImportConfigFile.TabIndex = 47;
            // 
            // wizardButtons1
            // 
            this.wizardButtons1.Container = this.stepWizardControl1;
            this.wizardButtons1.Dock = System.Windows.Forms.DockStyle.Right;
            this.wizardButtons1.Location = new System.Drawing.Point(719, 0);
            this.wizardButtons1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.wizardButtons1.Name = "wizardButtons1";
            this.wizardButtons1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.wizardButtons1.ShowExecuteButton = false;
            this.wizardButtons1.Size = new System.Drawing.Size(445, 229);
            this.wizardButtons1.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.stepWizardControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.wizardButtons1);
            this.splitContainer1.Size = new System.Drawing.Size(1164, 802);
            this.splitContainer1.SplitterDistance = 569;
            this.splitContainer1.TabIndex = 1;
            // 
            // importWizard
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "importWizard";
            this.Size = new System.Drawing.Size(1164, 802);
            ((System.ComponentModel.ISupportInitialize)(this.stepWizardControl1)).EndInit();
            this.stepWizardControl1.ResumeLayout(false);
            this.wizardPage3.ResumeLayout(false);
            this.wizardPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSavePageSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxThreads)).EndInit();
            this.wizardPage4.ResumeLayout(false);
            this.wizardPage4.PerformLayout();
            this.wizardPage1.ResumeLayout(false);
            this.wizardPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.wizardPage2.ResumeLayout(false);
            this.wizardPage2.PerformLayout();
            this.wizardPage5.ResumeLayout(false);
            this.wizardPage5.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardPageContainer stepWizardControl1;
        private AeroWizard.WizardPage wizardPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private AeroWizard.WizardPage wizardPage2;
        private System.Windows.Forms.TextBox tbSourceDataLocation;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private AeroWizard.WizardPage wizardPage3;
        private System.Windows.Forms.Label label3;
        private AeroWizard.WizardPage wizardPage4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label ConnectionString;
        private System.Windows.Forms.Button buttonTargetConnectionString;
        private System.Windows.Forms.Label labelTargetConnectionString;
        private AeroWizard.WizardPage wizardPage5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btLoadImportConfigFile;
        private System.Windows.Forms.TextBox tbImportConfigFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.NumericUpDown nudSavePageSize;
        private System.Windows.Forms.NumericUpDown nudMaxThreads;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbImportSchema;
        private System.Windows.Forms.Button btnImportSchema;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Xrm.DataMigration.XrmToolBoxPlugin.UserControls.WizardButtons wizardButtons1;
        private Xrm.DataMigration.XrmToolBoxPlugin.UserControls.ToggleCheckBox cbIgnoreStatuses;
        private Xrm.DataMigration.XrmToolBoxPlugin.UserControls.ToggleCheckBox cbIgnoreSystemFields;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbLogger;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
    }
}
