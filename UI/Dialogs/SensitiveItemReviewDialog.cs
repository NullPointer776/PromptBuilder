using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PromptStructTool.Models;

namespace PromptStructTool.UI.Dialogs
{
    /// <summary>
    /// Dialog for reviewing and customizing sensitive item replacements
    /// </summary>
    public partial class SensitiveItemReviewDialog : Form
    {
        public Dictionary<string, string> ReplacementMap { get; set; } = new();

        public SensitiveItemReviewDialog(List<SensitiveItem> items)
        {
            InitializeComponent();
            LoadItems(items);
            ApplyModernStyling();
        }

        private void LoadItems(List<SensitiveItem> items)
        {
            foreach (var item in items)
            {
                var row = dataGridView1.Rows.Add(
                    item.Category,
                    item.Value,
                    item.MatchCount,
                    "[Suggestion]"
                );
                dataGridView1.Rows[row].Tag = item;
            }
        }

        private void ApplyModernStyling()
        {
            this.Font = new Font("Segoe UI", 9);
            this.BackColor = Color.FromArgb(240, 240, 245);
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 252);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 235, 245);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Collect replacements from grid
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    string original = row.Cells[1].Value?.ToString() ?? "";
                    string replacement = row.Cells[3].Value?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(original) && !string.IsNullOrEmpty(replacement))
                    {
                        ReplacementMap[original] = replacement;
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
