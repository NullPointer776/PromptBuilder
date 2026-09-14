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
            string example = promptService.GetExample("Task");
            ShowExampleDialog("Task Example", example, txtTask);
        }

        private void btnContextExample_Click(object? sender, EventArgs e)
        {
            string example = promptService.GetExample("Context");
            ShowExampleDialog("Context Example", example, txtContext);
        }

        private void btnOutputExample_Click(object? sender, EventArgs e)
        {
            string example = promptService.GetExample("OutputFormat");
            ShowExampleDialog("Output Format Example", example, txtOutputFormat);
        }

        private void btnConstraintsExample_Click(object? sender, EventArgs e)
        {
            string example = promptService.GetExample("Constraints");
            ShowExampleDialog("Constraints Example", example, txtConstraints);
        }

        private void ShowExampleDialog(string title, string example, TextBox targetField)
        {
            if (string.IsNullOrEmpty(example)) return;

            var dlg = new Form
            {
                Text = title,
                Size = new Size(600, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Font = new Font("Segoe UI", 10)
            };

            var txtExample = new RichTextBox
            {
                Text = example,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.White,
                Margin = new Padding(5)
            };

            var btnCopy = new Button
            {
                Text = "Copy",
                Dock = DockStyle.Bottom,
                Height = 35,
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White
            };

            btnCopy.Click += (s, e) =>
            {
                Clipboard.SetText(example);
                MessageBox.Show("Copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            var btnInsert = new Button
            {
                Text = "Insert",
                Dock = DockStyle.Bottom,
                Height = 35,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White
            };

            btnInsert.Click += (s, e) =>
            {
                targetField.Text = example;
                dlg.Close();
            };

            var panel = new Panel { Dock = DockStyle.Bottom, Height = 40 };
            panel.Controls.Add(btnInsert);
            panel.Controls.Add(btnCopy);

            dlg.Controls.Add(panel);
            dlg.Controls.Add(txtExample);

            dlg.ShowDialog(this);
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
                string category = cb.SelectedItem.ToString() ?? "Other";
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
            lstDetectedItems.Items.Clear();
            ClearAllHighlights(rtbSensitiveCheck);

            string text = rtbSensitiveCheck.Text;
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("No text to scan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Email
            foreach (Match m in Regex.Matches(text, @"\b[\w.-]+@[\w.-]+\.[A-Za-z]{2,6}\b"))
            {
                AddDetected("Email", m.Value);
            }

            // IPv4
            foreach (Match m in Regex.Matches(text, @"\b(?:\d{1,3}\.){3}\d{1,3}\b"))
            {
                AddDetected("IP Address", m.Value);
            }

            // Phone (common international-ish patterns)
            foreach (Match m in Regex.Matches(text, @"\b(?:\+?\d{1,3}[ -]?)?(?:\(\d+\)[ -]?)?\d{2,4}[ -]?\d{3,4}[ -]?\d{3,4}\b"))
            {
                // crude filter to avoid matching years
                if (m.Value.Length >= 7)
                    AddDetected("Phone", m.Value);
            }

            // API keys / tokens (>=20 alnum with optional prefix sk- key-)
            foreach (Match m in Regex.Matches(text, @"\b(?:sk-|key-)?[A-Za-z0-9\-_=]{20,}\b"))
            {
                if (m.Value.Length >= 20)
                    AddDetected("API Key", m.Value);
            }

            // Bank accounts / card numbers (12-19 digits in groups)
            foreach (Match m in Regex.Matches(text, @"\b(?:\d[ -]?){12,19}\b"))
            {
                string digitsOnly = Regex.Replace(m.Value, "[^0-9]", "");
                if (digitsOnly.Length >= 12 && digitsOnly.Length <= 19)
                    AddDetected("Bank Account", m.Value);
            }

            // Passwords labeled explicitly
            foreach (Match m in Regex.Matches(text, @"(?i)(?:password|pwd|pass)\s*[:=]\s*(\S+)") )
            {
                string val = m.Groups[1].Value;
                if (!string.IsNullOrEmpty(val))
                    AddDetected("Password", val);
            }

            // Simple personal name heuristic: look for phrases like "my name is X" or nearby 'name'
            foreach (Match m in Regex.Matches(text, @"(?i)(?:my name is|i am|name is|name:)\s*([A-Z][a-z]{1,24})"))
            {
                AddDetected("Name", m.Groups[1].Value);
            }

            // Also check for common names anywhere (best-effort)
            string[] common = new[] { "John", "Mary", "Alice", "Bob", "Charlie", "Dana" };
            foreach (string n in common)
            {
                foreach (Match m in Regex.Matches(text, $"\\b{Regex.Escape(n)}\\b"))
                {
                    AddDetected("Name", m.Value);
                }
            }

            // Deduplicate list
            var unique = new Dictionary<string, string>();
            foreach (DetectedItem it in detectedItems)
            {
                if (!unique.ContainsKey(it.Value)) unique[it.Value] = it.Category;
            }
            detectedItems.Clear();
            foreach (var kv in unique) detectedItems.Add(new DetectedItem { Category = kv.Value, Value = kv.Key });

            // Highlight in RichTextBox and populate list
            foreach (var item in detectedItems)
            {
                HighlightAllOccurrences(rtbSensitiveCheck, item.Value, Color.Yellow);
                lstDetectedItems.Items.Add($"{item.Category}: {item.Value}");
            }

            if (detectedItems.Count == 0)
            {
                MessageBox.Show("No likely sensitive items were detected. You can still mark text manually.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Detected {detectedItems.Count} distinct items. Review them in the list on the right.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Helper to add to internal detected list
        private void AddDetected(string category, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            detectedItems.Add(new DetectedItem { Category = category, Value = value });
        }

        // Highlight all occurrences of a value in a RichTextBox
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

        // Remove all highlights (resets background)
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

        // Allow user to mark current selection as sensitive
        private void btnMarkSensitive_Click(object? sender, EventArgs e)
        {
            if (rtbSensitiveCheck.SelectionLength <= 0)
            {
                MessageBox.Show("Select some text first to mark as sensitive.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string val = rtbSensitiveCheck.SelectedText;
            // Ask category
            using var dlg = new Form();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.Size = new Size(350, 160);
            dlg.Text = "Mark as Sensitive";
            var cb = new ComboBox { Left = 10, Top = 10, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cb.Items.AddRange(new object[] { "Name", "Email", "Phone", "IP Address", "Bank Account", "Password", "API Key", "Address", "Other" });
            cb.SelectedIndex = 0;
            var btnOk = new Button { Text = "OK", Left = 60, Top = 60, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancel", Left = 160, Top = 60, DialogResult = DialogResult.Cancel };
            dlg.Controls.Add(cb); dlg.Controls.Add(btnOk); dlg.Controls.Add(btnCancel);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string cat = cb.SelectedItem.ToString() ?? "Other";
                AddDetected(cat, val);
                lstDetectedItems.Items.Add($"{cat}: {val}");
                HighlightAllOccurrences(rtbSensitiveCheck, val, Color.Yellow);
            }
        }

        // Auto-replace all detected items using the mapping rules
        private void btnAutoReplace_Click(object? sender, EventArgs e)
        {
            if (detectedItems.Count == 0)
            {
                MessageBox.Show("No detected items to replace. Run detection first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Build mapping for each unique detected value
            foreach (var it in detectedItems)
            {
                if (replacementMap.ContainsKey(it.Value)) continue;
                string replacement = SuggestReplacement(it.Category, it.Value);
                replacementMap[it.Value] = replacement;
            }

            // Perform replacements consistently using Regex.Escape to match literals
            string text = rtbSensitiveCheck.Text;
            foreach (var kv in replacementMap)
            {
                text = Regex.Replace(text, Regex.Escape(kv.Key), kv.Value);
            }
            rtbSensitiveCheck.Text = text;

            // Re-run highlight briefly to show replaced text
            ClearAllHighlights(rtbSensitiveCheck);
            MessageBox.Show("Auto-replace completed. Review the edited text.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Suggest replacement values based on category
        private string SuggestReplacement(string category, string original)
        {
            return category switch
            {
                "Name" => GetOrAllocateName(original),
                "Email" => GetOrAllocateEmail(original),
                "Phone" => "021 123 4567",
                "Bank Account" => "12-3456-7890123-00",
                "IP Address" => GetNextFakeIp(),
                "Password" => "********",
                "API Key" => "sk-test-XXXXXXXXXXXXXXXXXXXX",
                "Address" => "123 Queen Street, Auckland 1010, New Zealand",
                _ => "[REDACTED]",
            };
        }

        private string GetOrAllocateName(string original)
        {
            if (replacementMap.TryGetValue(original, out var r)) return r;
            string name = namePool[namePoolIndex % namePool.Count];
            namePoolIndex++;
            replacementMap[original] = name;
            return name;
        }

        private string GetOrAllocateEmail(string original)
        {
            if (replacementMap.TryGetValue(original, out var r)) return r;
            string em = $"test{emailCounter}@test.com";
            emailCounter++;
            replacementMap[original] = em;
            return em;
        }

        private string GetNextFakeIp()
        {
            ipPoolLastOctet++;
            return $"123.123.123.{ipPoolLastOctet}";
        }

        // Manual review: when user selects an item in list, show suggestion and allow edit/accept/skip
        private void btnManualReview_Click(object? sender, EventArgs e)
        {
            if (lstDetectedItems.SelectedIndex == -1)
            {
                MessageBox.Show("Select an item from the detected list first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selected = lstDetectedItems.SelectedItem!.ToString() ?? string.Empty;
            int idx = selected.IndexOf(": ");
            if (idx == -1) return;
            string category = selected.Substring(0, idx);
            string value = selected.Substring(idx + 2);

            string suggestion = SuggestReplacement(category, value);

            using var dlg = new Form();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.Size = new Size(500, 220);
            dlg.Text = "Manual review";

            var lblOrig = new Label { Left = 10, Top = 10, Width = 460, Text = $"Original ({category}): {value}" };
            var lblSug = new Label { Left = 10, Top = 40, Width = 100, Text = "Suggestion:" };
            var txtSug = new TextBox { Left = 10, Top = 60, Width = 460, Text = suggestion };
            var btnAccept = new Button { Text = "Accept & Replace", Left = 80, Top = 100, DialogResult = DialogResult.OK };
            var btnSkip = new Button { Text = "Skip", Left = 220, Top = 100, DialogResult = DialogResult.Cancel };

            dlg.Controls.Add(lblOrig); dlg.Controls.Add(lblSug); dlg.Controls.Add(txtSug); dlg.Controls.Add(btnAccept); dlg.Controls.Add(btnSkip);

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                // update mapping and replace all occurrences of value
                replacementMap[value] = txtSug.Text;
                rtbSensitiveCheck.Text = Regex.Replace(rtbSensitiveCheck.Text, Regex.Escape(value), txtSug.Text);
                MessageBox.Show("Replaced all occurrences.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // When user clicks an item in the list, locate it in the text
        private void lstDetectedItems_DoubleClick(object? sender, EventArgs e)
        {
            if (lstDetectedItems.SelectedIndex == -1) return;
            string selected = lstDetectedItems.SelectedItem!.ToString() ?? string.Empty;
            int idx = selected.IndexOf(": ");
            if (idx == -1) return;
            string value = selected.Substring(idx + 2);
            int pos = rtbSensitiveCheck.Find(value);
            if (pos >= 0)
            {
                rtbSensitiveCheck.Select(pos, value.Length);
                rtbSensitiveCheck.ScrollToCaret();
            }
        }

        // Open find & replace dialog for active RichTextBox
        private void btnFindReplace_Click(object? sender, EventArgs e)
        {
            RichTextBox target = tabControl1.SelectedTab == tabPrompt ? rtbDraft : rtbSensitiveCheck;
            var dlg = new FindReplaceForm(target);
            dlg.Show(this);
        }

        // Copy final sanitized text
        private void btnCopyFinal_Click(object? sender, EventArgs e)
        {
            Clipboard.SetText(rtbSensitiveCheck.Text);
            MessageBox.Show("Sanitized text copied to clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
