using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PromptStructTool.Models;
using PromptStructTool.Services;
using PromptStructTool.Utils;

namespace PromptStructTool
{
    /// <summary>
    /// Main application form with improved architecture and usability
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly PromptService promptService = new();
        private readonly SensitiveDetectionService detectionService = new();
        private readonly ReplacementService replacementService = new();
        private readonly FileService fileService = new();
        private readonly CommandHistory history = new();

        private PromptData currentData = new();
        private List<Models.SensitiveItem> currentDetectedItems = new();

        public MainForm()
        {
            InitializeComponent();
            InitializeApplicationUI();
            AddMenuBar();
        }

        private void InitializeApplicationUI()
        {
            panelTip.Visible = false;
            rtbDraft.ReadOnly = true;
            ApplyModernStyling();
        }

        private void ApplyModernStyling()
        {
            this.Font = new Font("Segoe UI", 9);
            this.BackColor = Color.FromArgb(240, 240, 245);
            foreach (Control control in GetAllControls(this))
            {
                if (control is RichTextBox rtb)
                {
                    rtb.BackColor = Color.White;
                    rtb.Font = new Font("Segoe UI", 10);
                }
                else if (control is TextBox tb)
                {
                    tb.BackColor = Color.White;
                }
            }
        }

        private IEnumerable<Control> GetAllControls(Control container)
        {
            foreach (Control control in container.Controls)
            {
                yield return control;
                foreach (var child in GetAllControls(control))
                    yield return child;
            }
        }

        private void AddMenuBar()
        {
            var menuBar = new MenuStrip();

            // File menu
            var fileMenu = new ToolStripMenuItem("&File");
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("&New", null, (s, e) => NewProject()));
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("&Open...", null, (s, e) => OpenProject()));
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("&Save...", null, (s, e) => SaveProject()));
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("&Export Text...", null, (s, e) => ExportAsText()));
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("E&xit", null, (s, e) => this.Close()));

            // Edit menu
            var editMenu = new ToolStripMenuItem("&Edit");
            editMenu.DropDownItems.Add(new ToolStripMenuItem("&Undo", null, (s, e) => Undo()) { ShortcutKeys = Keys.Control | Keys.Z });
            editMenu.DropDownItems.Add(new ToolStripMenuItem("&Redo", null, (s, e) => Redo()) { ShortcutKeys = Keys.Control | Keys.Y });
            editMenu.DropDownItems.Add(new ToolStripSeparator());
            editMenu.DropDownItems.Add(new ToolStripMenuItem("Clear All", null, (s, e) => ClearAll()));

            // Help menu
            var helpMenu = new ToolStripMenuItem("&Help");
            helpMenu.DropDownItems.Add(new ToolStripMenuItem("&About", null, (s, e) => ShowAbout()));

            menuBar.Items.Add(fileMenu);
            menuBar.Items.Add(editMenu);
            menuBar.Items.Add(helpMenu);

            this.MainMenuStrip = menuBar;
            this.Controls.Add(menuBar);
        }


        // ==================== Prompt Structuring Tab ====================

        private void btnTaskExample_Click(object? sender, EventArgs e)
        {
            ShowTemplateMenu(btnTaskExample, "Task", txtTask);
        }

        private void btnContextExample_Click(object? sender, EventArgs e)
        {
            ShowTemplateMenu(btnContextExample, "Context", txtContext);
        }

        private void btnOutputExample_Click(object? sender, EventArgs e)
        {
            ShowTemplateMenu(btnOutputExample, "OutputFormat", txtOutputFormat);
        }

        private void btnConstraintsExample_Click(object? sender, EventArgs e)
        {
            ShowTemplateMenu(btnConstraintsExample, "Constraints", txtConstraints);
        }

        /// <summary>
        /// Shows a dropdown of templates for the given field and inserts the
        /// selected template directly into the target text box.
        /// </summary>
        private void ShowTemplateMenu(Control anchor, string fieldName, TextBox targetField)
        {
            var templates = promptService.GetTemplates(fieldName);
            if (templates.Count == 0) return;

            var menu = new ContextMenuStrip
            {
                Font = new Font("Segoe UI", 9)
            };

            foreach (var template in templates)
            {
                var item = new ToolStripMenuItem(template.Key)
                {
                    Tag = template.Value,
                    ToolTipText = template.Value
                };
                item.Click += (s, e) =>
                {
                    if (s is ToolStripMenuItem clicked && clicked.Tag is string value)
                    {
                        InsertTemplate(targetField, value);
                    }
                };
                menu.Items.Add(item);
            }

            menu.Show(anchor, new Point(0, anchor.Height));
        }

        /// <summary>
        /// Inserts a template into the target text box, replacing any existing
        /// content (with undo support) and placing the caret at the end.
        /// </summary>
        private void InsertTemplate(TextBox targetField, string template)
        {
            if (string.IsNullOrEmpty(template)) return;

            RecordState();

            targetField.Text = template;
            targetField.SelectionStart = targetField.TextLength;
            targetField.SelectionLength = 0;
            targetField.Focus();
        }

        private void linkWritingTip_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            panelTip.Visible = !panelTip.Visible;
        }

        private void btnAssembleDraft_Click(object? sender, EventArgs e)
        {
            RecordState();

            string task = txtTask.Text.Trim();
            string context = txtContext.Text.Trim();
            string output = txtOutputFormat.Text.Trim();
            string constraints = txtConstraints.Text.Trim();

            if (string.IsNullOrEmpty(task))
            {
                MessageBox.Show("Please enter a Task.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string markdown = promptService.AssemblePrompt(task, context, output, constraints);
                rtbDraft.Text = markdown;
                rtbDraft.ReadOnly = !chkEditable.Checked;

                currentData.Task = task;
                currentData.Context = context;
                currentData.OutputFormat = output;
                currentData.Constraints = constraints;
                currentData.DraftPrompt = markdown;

                MessageBox.Show("Prompt assembled successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error assembling prompt: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkEditable_CheckedChanged(object? sender, EventArgs e)
        {
            rtbDraft.ReadOnly = !chkEditable.Checked;
        }

        private void btnCopy_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtbDraft.Text))
            {
                MessageBox.Show("No draft to copy. Assemble a prompt first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Clipboard.SetText(rtbDraft.Text);
            MessageBox.Show("Draft copied to clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnNext_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtbDraft.Text))
            {
                MessageBox.Show("Please assemble a draft first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            tabControl1.SelectedTab = tabSensitive;
            rtbSensitiveCheck.Text = rtbDraft.Text;
            currentData.SanitizedPrompt = rtbDraft.Text;
        }

        // ==================== Sensitive Check Tab ====================

        private void btnDetect_Click(object? sender, EventArgs e)
        {
            try
            {
                string text = rtbSensitiveCheck.Text;
                if (string.IsNullOrEmpty(text))
                {
                    MessageBox.Show("No text to scan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                currentDetectedItems = detectionService.Detect(text);
                UpdateDetectedItemsList(currentDetectedItems);

                ClearAllHighlights(rtbSensitiveCheck);
                foreach (var item in currentDetectedItems)
                {
                    HighlightAllOccurrences(rtbSensitiveCheck, item.Value, Color.Yellow);
                }

                MessageBox.Show(
                    $"Detection complete. Found {currentDetectedItems.Count} distinct sensitive items.",
                    "Done",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during detection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDetectedItemsList(List<Models.SensitiveItem> items)
        {
            lstDetectedItems.Items.Clear();
            foreach (var item in items)
            {
                string displayText = $"{item.Category}: {item.Value} ({item.MatchCount}x)";
                lstDetectedItems.Items.Add(displayText);
            }
        }

        private void btnAutoReplace_Click(object? sender, EventArgs e)
        {
            RecordState();

            try
            {
                if (currentDetectedItems.Count == 0)
                {
                    MessageBox.Show("No detected items. Run detection first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var replacementMap = replacementService.GenerateReplacements(currentDetectedItems);
                currentData.ReplacementMap = replacementMap;

                string sanitized = replacementService.ApplyReplacements(rtbSensitiveCheck.Text, replacementMap);
                rtbSensitiveCheck.Text = sanitized;
                currentData.SanitizedPrompt = sanitized;

                ClearAllHighlights(rtbSensitiveCheck);
                MessageBox.Show("Auto-replacement completed.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during auto-replace: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMarkSensitive_Click(object? sender, EventArgs e)
        {
            if (rtbSensitiveCheck.SelectionLength <= 0)
            {
                MessageBox.Show("Select some text first to mark as sensitive.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectedValue = rtbSensitiveCheck.SelectedText;
            var dlg = new Form
            {
                Text = "Mark as Sensitive",
                Size = new Size(350, 160),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Font = new Font("Segoe UI", 9)
            };

            var cb = new ComboBox
            {
                Left = 10,
                Top = 10,
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cb.Items.AddRange(new object[] { "Name", "Email", "Phone", "IP Address", "Bank Account", "Password", "API Key", "Address", "Other" });
            cb.SelectedIndex = 0;

            var btnOk = new Button { Text = "OK", Left = 60, Top = 60, Width = 80, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancel", Left = 160, Top = 60, Width = 80, DialogResult = DialogResult.Cancel };

            dlg.Controls.Add(cb);
            dlg.Controls.Add(btnOk);
            dlg.Controls.Add(btnCancel);

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string category = cb.SelectedItem?.ToString() ?? "Other";
                var item = new Models.SensitiveItem { Category = category, Value = selectedValue };
                currentDetectedItems.Add(item);
                UpdateDetectedItemsList(currentDetectedItems);
                HighlightAllOccurrences(rtbSensitiveCheck, selectedValue, Color.Yellow);
                MessageBox.Show("Item marked as sensitive.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnManualReview_Click(object? sender, EventArgs e)
        {
            if (lstDetectedItems.SelectedIndex == -1)
            {
                MessageBox.Show("Select an item from the detected list first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selected = lstDetectedItems.SelectedItem?.ToString() ?? string.Empty;
            var parts = selected.Split(new[] { ": " }, StringSplitOptions.None);
            if (parts.Length < 2) return;

            string category = parts[0];
            string value = parts[1].Split('(')[0].Trim();

            string suggestion = replacementService.SuggestReplacement(category, value);

            var dlg = new Form
            {
                Text = "Manual Review",
                Size = new Size(500, 220),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Font = new Font("Segoe UI", 9)
            };

            var lblOrig = new Label { Left = 10, Top = 10, Width = 460, Text = $"Original ({category}): {value}" };
            var lblSug = new Label { Left = 10, Top = 40, Width = 100, Text = "Replacement:" };
            var txtSug = new TextBox { Left = 10, Top = 60, Width = 460, Height = 30, Text = suggestion, Multiline = true };
            var btnAccept = new Button { Text = "Accept & Replace", Left = 80, Top = 100, Width = 120, DialogResult = DialogResult.OK };
            var btnSkip = new Button { Text = "Skip", Left = 220, Top = 100, Width = 80, DialogResult = DialogResult.Cancel };

            dlg.Controls.Add(lblOrig);
            dlg.Controls.Add(lblSug);
            dlg.Controls.Add(txtSug);
            dlg.Controls.Add(btnAccept);
            dlg.Controls.Add(btnSkip);

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                RecordState();
                currentData.ReplacementMap[value] = txtSug.Text;
                rtbSensitiveCheck.Text = System.Text.RegularExpressions.Regex.Replace(
                    rtbSensitiveCheck.Text,
                    System.Text.RegularExpressions.Regex.Escape(value),
                    txtSug.Text
                );
                currentData.SanitizedPrompt = rtbSensitiveCheck.Text;
                MessageBox.Show("Replaced all occurrences.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lstDetectedItems_DoubleClick(object? sender, EventArgs e)
        {
            if (lstDetectedItems.SelectedIndex == -1) return;

            string selected = lstDetectedItems.SelectedItem?.ToString() ?? string.Empty;
            var parts = selected.Split(new[] { ": " }, StringSplitOptions.None);
            if (parts.Length < 2) return;

            string value = parts[1].Split('(')[0].Trim();
            int pos = rtbSensitiveCheck.Find(value);
            if (pos >= 0)
            {
                rtbSensitiveCheck.Select(pos, value.Length);
                rtbSensitiveCheck.ScrollToCaret();
            }
        }

        private void btnFindReplace_Click(object? sender, EventArgs e)
        {
            RichTextBox target = tabControl1.SelectedTab == tabPrompt ? rtbDraft : rtbSensitiveCheck;
            var dlg = new FindReplaceForm(target);
            dlg.Show(this);
        }

        private void btnCopyFinal_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtbSensitiveCheck.Text))
            {
                MessageBox.Show("No sanitized text to copy.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Clipboard.SetText(rtbSensitiveCheck.Text);
            MessageBox.Show("Sanitized text copied to clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ==================== Helper Methods ====================

        private void HighlightAllOccurrences(RichTextBox rtb, string value, Color color)
        {
            int start = 0;
            while (start < rtb.TextLength)
            {
                int idx = rtb.Find(value, start, RichTextBoxFinds.None);
                if (idx == -1) break;
                rtb.Select(idx, value.Length);
                rtb.SelectionBackColor = color;
                start = idx + value.Length;
            }
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            rtb.SelectionBackColor = Color.White;
        }

        private void ClearAllHighlights(RichTextBox rtb)
        {
            int selStart = rtb.SelectionStart;
            int selLen = rtb.SelectionLength;
            rtb.SelectAll();
            rtb.SelectionBackColor = Color.White;
            rtb.DeselectAll();
            rtb.SelectionStart = selStart;
            rtb.SelectionLength = selLen;
        }

        // ==================== File Operations ====================

        private void NewProject()
        {
            RecordState();
            currentData = new PromptData();
            currentDetectedItems.Clear();
            txtTask.Clear();
            txtContext.Clear();
            txtOutputFormat.Clear();
            txtConstraints.Clear();
            rtbDraft.Clear();
            rtbSensitiveCheck.Clear();
            lstDetectedItems.Items.Clear();
            replacementService.Reset();
            MessageBox.Show("New project created.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OpenProject()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Prompt Projects (*.prompt)|*.prompt|All files (*.*)|*.*",
                Title = "Open Prompt Project"
            };

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var loadedData = fileService.LoadAsync(dlg.FileName).Result;
                    currentData = loadedData;
                    txtTask.Text = currentData.Task;
                    txtContext.Text = currentData.Context;
                    txtOutputFormat.Text = currentData.OutputFormat;
                    txtConstraints.Text = currentData.Constraints;
                    rtbDraft.Text = currentData.DraftPrompt;
                    rtbSensitiveCheck.Text = currentData.SanitizedPrompt;
                    UpdateDetectedItemsList(currentData.DetectedItems);
                    MessageBox.Show("Project opened successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening project: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveProject()
        {
            var dlg = new SaveFileDialog
            {
                Filter = "Prompt Projects (*.prompt)|*.prompt|All files (*.*)|*.*",
                Title = "Save Prompt Project",
                DefaultExt = "prompt"
            };

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    currentData.DetectedItems = currentDetectedItems;
                    fileService.SaveAsync(currentData, dlg.FileName).Wait();
                    MessageBox.Show("Project saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving project: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportAsText()
        {
            if (string.IsNullOrEmpty(rtbSensitiveCheck.Text))
            {
                MessageBox.Show("No sanitized text to export.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dlg = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Export Sanitized Prompt",
                DefaultExt = "txt"
            };

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    fileService.ExportAsTextAsync(rtbSensitiveCheck.Text, dlg.FileName).Wait();
                    MessageBox.Show("Prompt exported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==================== Undo/Redo ====================

        private void RecordState()
        {
            history.Record(currentData.Clone());
        }

        private void Undo()
        {
            var previousState = history.Undo();
            if (previousState != null)
            {
                LoadState(previousState);
                MessageBox.Show("Undo completed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No previous state to undo.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Redo()
        {
            var nextState = history.Redo();
            if (nextState != null)
            {
                LoadState(nextState);
                MessageBox.Show("Redo completed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No next state to redo.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadState(PromptData data)
        {
            currentData = data;
            txtTask.Text = currentData.Task;
            txtContext.Text = currentData.Context;
            txtOutputFormat.Text = currentData.OutputFormat;
            txtConstraints.Text = currentData.Constraints;
            rtbDraft.Text = currentData.DraftPrompt;
            rtbSensitiveCheck.Text = currentData.SanitizedPrompt;
            currentDetectedItems = currentData.DetectedItems;
            UpdateDetectedItemsList(currentDetectedItems);
        }

        private void ClearAll()
        {
            if (MessageBox.Show(
                "Are you sure you want to clear all data?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                NewProject();
            }
        }

        private void ShowAbout()
        {
            MessageBox.Show(
                "Prompt Builder v2.0\n\n" +
                "A tool for structuring and sanitizing AI prompts.\n\n" +
                "Features:\n" +
                "• Structured prompt building\n" +
                "• Automatic sensitive information detection\n" +
                "• Flexible replacement and sanitization\n" +
                "• Project save/load\n" +
                "• Full undo/redo support\n\n" +
                "© 2024 Prompt Builder Team",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
