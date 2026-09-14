using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PromptStructTool
{
    // Small Find & Replace dialog that operates on a target RichTextBox
    public partial class FindReplaceForm : Form
    {
        private readonly RichTextBox target;
        private int lastFindPos = 0;

        public FindReplaceForm(RichTextBox targetBox)
        {
            InitializeComponent();
            target = targetBox ?? throw new ArgumentNullException(nameof(targetBox));
        }

        private void btnFindNext_Click(object? sender, EventArgs e)
        {
            string find = txtFind.Text;
            if (string.IsNullOrEmpty(find)) return;

            // Use simple find that respects case if requested
            StringComparison comp = chkCase.Checked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int pos = target.Text.IndexOf(find, lastFindPos, comp);
            if (pos == -1 && lastFindPos > 0)
            {
                // wrap search
                pos = target.Text.IndexOf(find, 0, comp);
            }
            if (pos >= 0)
            {
                target.Select(pos, find.Length);
                target.ScrollToCaret();
                lastFindPos = pos + find.Length;
            }
            else
            {
                MessageBox.Show("Text not found.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lastFindPos = 0;
            }
        }

        private void btnFindAll_Click(object? sender, EventArgs e)
        {
            ClearHighlights();
            string find = txtFind.Text;
            if (string.IsNullOrEmpty(find)) return;
            var comp = chkCase.Checked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int start = 0;
            while (start < target.TextLength)
            {
                int idx = target.Text.IndexOf(find, start, comp);
                if (idx == -1) break;
                target.Select(idx, find.Length);
                target.SelectionBackColor = Color.LightGreen;
                start = idx + find.Length;
            }
            target.SelectionStart = target.TextLength;
            target.SelectionLength = 0;
            target.SelectionBackColor = Color.White;
        }

        private void btnReplace_Click(object? sender, EventArgs e)
        {
            if (target.SelectionLength == 0) btnFindNext_Click(sender, e);
            if (target.SelectionLength > 0)
            {
                target.SelectedText = txtReplace.Text;
            }
        }

        private void btnReplaceAll_Click(object? sender, EventArgs e)
        {
            string find = txtFind.Text;
            string replacement = txtReplace.Text;
            if (string.IsNullOrEmpty(find)) return;
            var regex = new Regex(Regex.Escape(find), chkCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase);
            target.Text = regex.Replace(target.Text, replacement);
        }

        private void ClearHighlights()
        {
            int s = target.SelectionStart; int l = target.SelectionLength;
            target.SelectAll();
            target.SelectionBackColor = Color.White;
            target.DeselectAll();
            target.SelectionStart = s; target.SelectionLength = l;
        }
    }
}
