using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PromptStructTool
{
    // The main application form - uses a TabControl wizard flow (TabControl chosen to keep UI simple and familiar)
    public partial class MainForm : Form
    {
        // Detected sensitive items structure
        private class DetectedItem
        {
            public string Category { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
        }

        private readonly List<DetectedItem> detectedItems = new();
        private readonly Dictionary<string, string> replacementMap = new();
        private readonly List<string> namePool = new() { "Alice", "Bob", "Charlie", "Dana" };
        private int namePoolIndex = 0;
        private int emailCounter = 1;
        private int ipPoolLastOctet = 123;

        public MainForm()
        {
            InitializeComponent();
            panelTip.Visible = false;
            rtbDraft.ReadOnly = true;
        }

        // Insert built-in examples for each field
        private void btnTaskExample_Click(object? sender, EventArgs e)
        {
            txtTask.Text = "**Summarize** the following {{document_type}} in **no more than {{max_words}} words**, focusing on {{key_topic}}.";
        }

        private void btnContextExample_Click(object? sender, EventArgs e)
        {
            txtContext.Text = "The audience is a non-technical product team. The document is a 2-page meeting notes about upcoming feature decisions. Prior attempts: a terse bullet list; needs more explanation.";
        }

        private void btnOutputExample_Click(object? sender, EventArgs e)
        {
            txtOutputFormat.Text = "Bullet list with 5 bullets, each 1-2 sentences. Include a 2-sentence summary at the top.";
        }

        private void btnConstraintsExample_Click(object? sender, EventArgs e)
        {
            txtConstraints.Text = "Use plain English, avoid jargon. Do not reference internal project names. Maximum 300 words.";
        }

        // Toggle writing tip panel
        private void linkWritingTip_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            panelTip.Visible = !panelTip.Visible;
        }

        // Assemble the draft prompt in Markdown format
        private void btnAssembleDraft_Click(object? sender, EventArgs e)
        {
            string task = txtTask.Text.Trim();
            string context = txtContext.Text.Trim();
            string output = txtOutputFormat.Text.Trim();
            string constraints = txtConstraints.Text.Trim();

            // Ensure key verbs are bolded (simple heuristic: if first word is a verb and not already bolded)
            if (!task.StartsWith("**") && task.Length > 0)
            {
                string firstWord = task.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
                task = task.Replace(firstWord, $"**{firstWord}**");
            }

            string markdown = "## Instruction\r\n" + task + "\r\n\r\n" +
                              "## Context\r\n" + context + "\r\n\r\n" +
                              "## Output Format\r\n" + output + "\r\n\r\n" +
                              "## Constraints\r\n" + constraints + "\r\n";

            rtbDraft.Text = markdown;
            rtbDraft.ReadOnly = !chkEditable.Checked;
        }

        // Toggle read-only of draft
        private void chkEditable_CheckedChanged(object? sender, EventArgs e)
        {
            rtbDraft.ReadOnly = !chkEditable.Checked;
        }

        // Copy draft to clipboard
        private void btnCopy_Click(object? sender, EventArgs e)
        {
            Clipboard.SetText(rtbDraft.Text);
            MessageBox.Show("Draft copied to clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Move to sensitive check tab and load draft
        private void btnNext_Click(object? sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabSensitive;
            rtbSensitiveCheck.Text = rtbDraft.Text;
        }

        // Run regex / heuristic detection
        private void btnDetect_Click(object? sender, EventArgs e)
        {
            detectedItems.Clear();
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
