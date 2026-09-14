namespace PromptStructTool
{
    partial class MainForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPrompt = new System.Windows.Forms.TabPage();
            this.btnFindReplace = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.chkEditable = new System.Windows.Forms.CheckBox();
            this.rtbDraft = new System.Windows.Forms.RichTextBox();
            this.btnAssembleDraft = new System.Windows.Forms.Button();
            this.panelTip = new System.Windows.Forms.Panel();
            this.labelTip = new System.Windows.Forms.Label();
            this.linkWritingTip = new System.Windows.Forms.LinkLabel();
            this.btnConstraintsExample = new System.Windows.Forms.Button();
            this.btnOutputExample = new System.Windows.Forms.Button();
            this.btnContextExample = new System.Windows.Forms.Button();
            this.btnTaskExample = new System.Windows.Forms.Button();
            this.txtConstraints = new System.Windows.Forms.TextBox();
            this.txtOutputFormat = new System.Windows.Forms.TextBox();
            this.txtContext = new System.Windows.Forms.TextBox();
            this.txtTask = new System.Windows.Forms.TextBox();
            this.lblConstraints = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this.lblContext = new System.Windows.Forms.Label();
            this.lblTask = new System.Windows.Forms.Label();
            this.tabSensitive = new System.Windows.Forms.TabPage();
            this.btnCopyFinal = new System.Windows.Forms.Button();
            this.btnManualReview = new System.Windows.Forms.Button();
            this.btnAutoReplace = new System.Windows.Forms.Button();
            this.btnMarkSensitive = new System.Windows.Forms.Button();
            this.lstDetectedItems = new System.Windows.Forms.ListBox();
            this.rtbSensitiveCheck = new System.Windows.Forms.RichTextBox();
            this.btnDetect = new System.Windows.Forms.Button();
            this.btnOpenFindReplace2 = new System.Windows.Forms.Button();
            this.labelDetected = new System.Windows.Forms.Label();
            this.labelSensitive = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPrompt.SuspendLayout();
            this.panelTip.SuspendLayout();
            this.tabSensitive.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPrompt);
            this.tabControl1.Controls.Add(this.tabSensitive);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(960, 637);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPrompt
            // 
            this.tabPrompt.Controls.Add(this.btnFindReplace);
            this.tabPrompt.Controls.Add(this.btnNext);
            this.tabPrompt.Controls.Add(this.btnCopy);
            this.tabPrompt.Controls.Add(this.chkEditable);
            this.tabPrompt.Controls.Add(this.rtbDraft);
            this.tabPrompt.Controls.Add(this.btnAssembleDraft);
            this.tabPrompt.Controls.Add(this.panelTip);
            this.tabPrompt.Controls.Add(this.btnConstraintsExample);
            this.tabPrompt.Controls.Add(this.btnOutputExample);
            this.tabPrompt.Controls.Add(this.btnContextExample);
            this.tabPrompt.Controls.Add(this.btnTaskExample);
            this.tabPrompt.Controls.Add(this.txtConstraints);
            this.tabPrompt.Controls.Add(this.txtOutputFormat);
            this.tabPrompt.Controls.Add(this.txtContext);
            this.tabPrompt.Controls.Add(this.txtTask);
            this.tabPrompt.Controls.Add(this.lblConstraints);
            this.tabPrompt.Controls.Add(this.lblOutput);
            this.tabPrompt.Controls.Add(this.lblContext);
            this.tabPrompt.Controls.Add(this.lblTask);
            this.tabPrompt.Location = new System.Drawing.Point(4, 24);
            this.tabPrompt.Name = "tabPrompt";
            this.tabPrompt.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrompt.Size = new System.Drawing.Size(952, 609);
            this.tabPrompt.TabIndex = 0;
            this.tabPrompt.Text = "Prompt Structuring";
            this.tabPrompt.UseVisualStyleBackColor = true;
            // 
            // btnFindReplace
            // 
            this.btnFindReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindReplace.Location = new System.Drawing.Point(839, 10);
            this.btnFindReplace.Name = "btnFindReplace";
            this.btnFindReplace.Size = new System.Drawing.Size(100, 26);
            this.btnFindReplace.TabIndex = 18;
            this.btnFindReplace.Text = "Find/Replace";
            this.btnFindReplace.UseVisualStyleBackColor = true;
            this.btnFindReplace.Click += new System.EventHandler(this.btnFindReplace_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(839, 570);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(100, 28);
            this.btnNext.TabIndex = 17;
            this.btnNext.Text = "Next: Check for Sensitive Info";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCopy.Location = new System.Drawing.Point(12, 570);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(100, 28);
            this.btnCopy.TabIndex = 16;
            this.btnCopy.Text = "Copy to Clipboard";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // chkEditable
            // 
            this.chkEditable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkEditable.AutoSize = true;
            this.chkEditable.Location = new System.Drawing.Point(130, 575);
            this.chkEditable.Name = "chkEditable";
            this.chkEditable.Size = new System.Drawing.Size(90, 19);
            this.chkEditable.TabIndex = 15;
            this.chkEditable.Text = "Editable";
            this.chkEditable.UseVisualStyleBackColor = true;
            this.chkEditable.CheckedChanged += new System.EventHandler(this.chkEditable_CheckedChanged);
            // 
            // rtbDraft
            // 
            this.rtbDraft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbDraft.Location = new System.Drawing.Point(12, 280);
            this.rtbDraft.Name = "rtbDraft";
            this.rtbDraft.Size = new System.Drawing.Size(927, 280);
            this.rtbDraft.TabIndex = 14;
            this.rtbDraft.Text = "";
            // 
            // btnAssembleDraft
            // 
            this.btnAssembleDraft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAssembleDraft.Location = new System.Drawing.Point(747, 10);
            this.btnAssembleDraft.Name = "btnAssembleDraft";
            this.btnAssembleDraft.Size = new System.Drawing.Size(86, 26);
            this.btnAssembleDraft.TabIndex = 13;
            this.btnAssembleDraft.Text = "Assemble Draft";
            this.btnAssembleDraft.UseVisualStyleBackColor = true;
            this.btnAssembleDraft.Click += new System.EventHandler(this.btnAssembleDraft_Click);
            // 
            // panelTip
            // 
            this.panelTip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTip.Controls.Add(this.labelTip);
            this.panelTip.Controls.Add(this.linkWritingTip);
            this.panelTip.Location = new System.Drawing.Point(12, 210);
            this.panelTip.Name = "panelTip";
            this.panelTip.Size = new System.Drawing.Size(927, 64);
            this.panelTip.TabIndex = 12;
            // 
            // labelTip
            // 
            this.labelTip.AutoSize = true;
            this.labelTip.Location = new System.Drawing.Point(10, 8);
            this.labelTip.Name = "labelTip";
            this.labelTip.Size = new System.Drawing.Size(652, 30);
            this.labelTip.TabIndex = 1;
            this.labelTip.Text = "Too vague: 'Write a function to handle user data.'\r\nToo verbose: long rambling sentence with hedges.\r\nJust right: single, clear sentence stating inputs, checks and expected output.";
            // 
            // linkWritingTip
            // 
            this.linkWritingTip.AutoSize = true;
            this.linkWritingTip.Location = new System.Drawing.Point(10, 40);
            this.linkWritingTip.Name = "linkWritingTip";
            this.linkWritingTip.Size = new System.Drawing.Size(105, 15);
            this.linkWritingTip.TabIndex = 0;
            this.linkWritingTip.TabStop = true;
            this.linkWritingTip.Text = "Writing tip (toggle)";
            this.linkWritingTip.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWritingTip_LinkClicked);
            // 
            // btnConstraintsExample
            // 
            this.btnConstraintsExample.Location = new System.Drawing.Point(12, 180);
            this.btnConstraintsExample.Name = "btnConstraintsExample";
            this.btnConstraintsExample.Size = new System.Drawing.Size(140, 24);
            this.btnConstraintsExample.TabIndex = 11;
            this.btnConstraintsExample.Text = "Insert Constraints \u25BC";
            this.btnConstraintsExample.UseVisualStyleBackColor = true;
            this.btnConstraintsExample.Click += new System.EventHandler(this.btnConstraintsExample_Click);
            // 
            // btnOutputExample
            // 
            this.btnOutputExample.Location = new System.Drawing.Point(12, 130);
            this.btnOutputExample.Name = "btnOutputExample";
            this.btnOutputExample.Size = new System.Drawing.Size(140, 24);
            this.btnOutputExample.TabIndex = 10;
            this.btnOutputExample.Text = "Insert Output Format \u25BC";
            this.btnOutputExample.UseVisualStyleBackColor = true;
            this.btnOutputExample.Click += new System.EventHandler(this.btnOutputExample_Click);
            // 
            // btnContextExample
            // 
            this.btnContextExample.Location = new System.Drawing.Point(12, 80);
            this.btnContextExample.Name = "btnContextExample";
            this.btnContextExample.Size = new System.Drawing.Size(140, 24);
            this.btnContextExample.TabIndex = 9;
            this.btnContextExample.Text = "Insert Context \u25BC";
            this.btnContextExample.UseVisualStyleBackColor = true;
            this.btnContextExample.Click += new System.EventHandler(this.btnContextExample_Click);
            // 
            // btnTaskExample
            // 
            this.btnTaskExample.Location = new System.Drawing.Point(12, 30);
            this.btnTaskExample.Name = "btnTaskExample";
            this.btnTaskExample.Size = new System.Drawing.Size(140, 24);
            this.btnTaskExample.TabIndex = 8;
            this.btnTaskExample.Text = "Insert Task \u25BC";
            this.btnTaskExample.UseVisualStyleBackColor = true;
            this.btnTaskExample.Click += new System.EventHandler(this.btnTaskExample_Click);
            // 
            // txtConstraints
            // 
            this.txtConstraints.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConstraints.Location = new System.Drawing.Point(170, 180);
            this.txtConstraints.Multiline = true;
            this.txtConstraints.Name = "txtConstraints";
            this.txtConstraints.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConstraints.Size = new System.Drawing.Size(769, 24);
            this.txtConstraints.TabIndex = 7;
            // 
            // txtOutputFormat
            // 
            this.txtOutputFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFormat.Location = new System.Drawing.Point(170, 130);
            this.txtOutputFormat.Multiline = true;
            this.txtOutputFormat.Name = "txtOutputFormat";
            this.txtOutputFormat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutputFormat.Size = new System.Drawing.Size(769, 24);
            this.txtOutputFormat.TabIndex = 6;
            // 
            // txtContext
            // 
            this.txtContext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContext.Location = new System.Drawing.Point(170, 80);
            this.txtContext.Multiline = true;
            this.txtContext.Name = "txtContext";
            this.txtContext.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContext.Size = new System.Drawing.Size(769, 24);
            this.txtContext.TabIndex = 5;
            // 
            // txtTask
            // 
            this.txtTask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTask.Location = new System.Drawing.Point(170, 30);
            this.txtTask.Multiline = true;
            this.txtTask.Name = "txtTask";
            this.txtTask.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTask.Size = new System.Drawing.Size(569, 24);
            this.txtTask.TabIndex = 4;
            // 
            // lblConstraints
            // 
            this.lblConstraints.AutoSize = true;
            this.lblConstraints.Location = new System.Drawing.Point(170, 162);
            this.lblConstraints.Name = "lblConstraints";
            this.lblConstraints.Size = new System.Drawing.Size(63, 15);
            this.lblConstraints.TabIndex = 3;
            this.lblConstraints.Text = "Constraints";
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(170, 112);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(78, 15);
            this.lblOutput.TabIndex = 2;
            this.lblOutput.Text = "Output Format";
            // 
            // lblContext
            // 
            this.lblContext.AutoSize = true;
            this.lblContext.Location = new System.Drawing.Point(170, 62);
            this.lblContext.Name = "lblContext";
            this.lblContext.Size = new System.Drawing.Size(47, 15);
            this.lblContext.TabIndex = 1;
            this.lblContext.Text = "Context";
            // 
            // lblTask
            // 
            this.lblTask.AutoSize = true;
            this.lblTask.Location = new System.Drawing.Point(170, 12);
            this.lblTask.Name = "lblTask";
            this.lblTask.Size = new System.Drawing.Size(30, 15);
            this.lblTask.TabIndex = 0;
            this.lblTask.Text = "Task";
            // 
            // tabSensitive
            // 
            this.tabSensitive.Controls.Add(this.btnCopyFinal);
            this.tabSensitive.Controls.Add(this.btnManualReview);
            this.tabSensitive.Controls.Add(this.btnAutoReplace);
            this.tabSensitive.Controls.Add(this.btnMarkSensitive);
            this.tabSensitive.Controls.Add(this.lstDetectedItems);
            this.tabSensitive.Controls.Add(this.rtbSensitiveCheck);
            this.tabSensitive.Controls.Add(this.btnDetect);
            this.tabSensitive.Controls.Add(this.btnOpenFindReplace2);
            this.tabSensitive.Controls.Add(this.labelDetected);
            this.tabSensitive.Controls.Add(this.labelSensitive);
            this.tabSensitive.Location = new System.Drawing.Point(4, 24);
            this.tabSensitive.Name = "tabSensitive";
            this.tabSensitive.Padding = new System.Windows.Forms.Padding(3);
            this.tabSensitive.Size = new System.Drawing.Size(952, 609);
            this.tabSensitive.TabIndex = 1;
            this.tabSensitive.Text = "Sensitive Check & Sanitizing";
            this.tabSensitive.UseVisualStyleBackColor = true;
            // 
            // btnCopyFinal
            // 
            this.btnCopyFinal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCopyFinal.Location = new System.Drawing.Point(12, 567);
            this.btnCopyFinal.Name = "btnCopyFinal";
            this.btnCopyFinal.Size = new System.Drawing.Size(100, 28);
            this.btnCopyFinal.TabIndex = 9;
            this.btnCopyFinal.Text = "Copy Final";
            this.btnCopyFinal.UseVisualStyleBackColor = true;
            this.btnCopyFinal.Click += new System.EventHandler(this.btnCopyFinal_Click);
            // 
            // btnManualReview
            // 
            this.btnManualReview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManualReview.Location = new System.Drawing.Point(839, 567);
            this.btnManualReview.Name = "btnManualReview";
            this.btnManualReview.Size = new System.Drawing.Size(100, 28);
            this.btnManualReview.TabIndex = 8;
            this.btnManualReview.Text = "Manual Review";
            this.btnManualReview.UseVisualStyleBackColor = true;
            this.btnManualReview.Click += new System.EventHandler(this.btnManualReview_Click);
            // 
            // btnAutoReplace
            // 
            this.btnAutoReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoReplace.Location = new System.Drawing.Point(733, 567);
            this.btnAutoReplace.Name = "btnAutoReplace";
            this.btnAutoReplace.Size = new System.Drawing.Size(100, 28);
            this.btnAutoReplace.TabIndex = 7;
            this.btnAutoReplace.Text = "Auto Replace";
            this.btnAutoReplace.UseVisualStyleBackColor = true;
            this.btnAutoReplace.Click += new System.EventHandler(this.btnAutoReplace_Click);
            // 
            // btnMarkSensitive
            // 
            this.btnMarkSensitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMarkSensitive.Location = new System.Drawing.Point(130, 567);
            this.btnMarkSensitive.Name = "btnMarkSensitive";
            this.btnMarkSensitive.Size = new System.Drawing.Size(110, 28);
            this.btnMarkSensitive.TabIndex = 6;
            this.btnMarkSensitive.Text = "Mark Selection";
            this.btnMarkSensitive.UseVisualStyleBackColor = true;
            this.btnMarkSensitive.Click += new System.EventHandler(this.btnMarkSensitive_Click);
            // 
            // lstDetectedItems
            // 
            this.lstDetectedItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDetectedItems.FormattingEnabled = true;
            this.lstDetectedItems.ItemHeight = 15;
            this.lstDetectedItems.Location = new System.Drawing.Point(652, 30);
            this.lstDetectedItems.Name = "lstDetectedItems";
            this.lstDetectedItems.Size = new System.Drawing.Size(287, 514);
            this.lstDetectedItems.TabIndex = 5;
            this.lstDetectedItems.DoubleClick += new System.EventHandler(this.lstDetectedItems_DoubleClick);
            // 
            // rtbSensitiveCheck
            // 
            this.rtbSensitiveCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbSensitiveCheck.Location = new System.Drawing.Point(12, 30);
            this.rtbSensitiveCheck.Name = "rtbSensitiveCheck";
            this.rtbSensitiveCheck.Size = new System.Drawing.Size(634, 514);
            this.rtbSensitiveCheck.TabIndex = 4;
            this.rtbSensitiveCheck.Text = "";
            // 
            // btnDetect
            // 
            this.btnDetect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDetect.Location = new System.Drawing.Point(652, 6);
            this.btnDetect.Name = "btnDetect";
            this.btnDetect.Size = new System.Drawing.Size(140, 24);
            this.btnDetect.TabIndex = 3;
            this.btnDetect.Text = "Detect Sensitive Info";
            this.btnDetect.UseVisualStyleBackColor = true;
            this.btnDetect.Click += new System.EventHandler(this.btnDetect_Click);
            // 
            // btnOpenFindReplace2
            // 
            this.btnOpenFindReplace2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenFindReplace2.Location = new System.Drawing.Point(12, 6);
            this.btnOpenFindReplace2.Name = "btnOpenFindReplace2";
            this.btnOpenFindReplace2.Size = new System.Drawing.Size(120, 24);
            this.btnOpenFindReplace2.TabIndex = 2;
            this.btnOpenFindReplace2.Text = "Find/Replace";
            this.btnOpenFindReplace2.UseVisualStyleBackColor = true;
            this.btnOpenFindReplace2.Click += new System.EventHandler(this.btnFindReplace_Click);
            // 
            // labelDetected
            // 
            this.labelDetected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDetected.AutoSize = true;
            this.labelDetected.Location = new System.Drawing.Point(652, 12);
            this.labelDetected.Name = "labelDetected";
            this.labelDetected.Size = new System.Drawing.Size(92, 15);
            this.labelDetected.TabIndex = 1;
            this.labelDetected.Text = "Detected Items";
            // 
            // labelSensitive
            // 
            this.labelSensitive.AutoSize = true;
            this.labelSensitive.Location = new System.Drawing.Point(12, 12);
            this.labelSensitive.Name = "labelSensitive";
            this.labelSensitive.Size = new System.Drawing.Size(95, 15);
            this.labelSensitive.TabIndex = 0;
            this.labelSensitive.Text = "Editable Text Area";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Prompt Structuring & Sanitizing Tool";
            this.tabControl1.ResumeLayout(false);
            this.tabPrompt.ResumeLayout(false);
            this.tabPrompt.PerformLayout();
            this.panelTip.ResumeLayout(false);
            this.panelTip.PerformLayout();
            this.tabSensitive.ResumeLayout(false);
            this.tabSensitive.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPrompt;
        private System.Windows.Forms.TabPage tabSensitive;
        private System.Windows.Forms.Label lblTask;
        private System.Windows.Forms.Label lblContext;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Label lblConstraints;
        private System.Windows.Forms.TextBox txtTask;
        private System.Windows.Forms.TextBox txtContext;
        private System.Windows.Forms.TextBox txtOutputFormat;
        private System.Windows.Forms.TextBox txtConstraints;
        private System.Windows.Forms.Button btnTaskExample;
        private System.Windows.Forms.Button btnContextExample;
        private System.Windows.Forms.Button btnOutputExample;
        private System.Windows.Forms.Button btnConstraintsExample;
        private System.Windows.Forms.Panel panelTip;
        private System.Windows.Forms.Label labelTip;
        private System.Windows.Forms.LinkLabel linkWritingTip;
        private System.Windows.Forms.Button btnAssembleDraft;
        private System.Windows.Forms.RichTextBox rtbDraft;
        private System.Windows.Forms.CheckBox chkEditable;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFindReplace;
        private System.Windows.Forms.Button btnDetect;
        private System.Windows.Forms.RichTextBox rtbSensitiveCheck;
        private System.Windows.Forms.ListBox lstDetectedItems;
        private System.Windows.Forms.Button btnMarkSensitive;
        private System.Windows.Forms.Button btnAutoReplace;
        private System.Windows.Forms.Button btnManualReview;
        private System.Windows.Forms.Button btnCopyFinal;
        private System.Windows.Forms.Button btnOpenFindReplace2;
        private System.Windows.Forms.Label labelDetected;
        private System.Windows.Forms.Label labelSensitive;
    }
}
