using System;
using System.Drawing;
using System.Windows.Forms;

namespace PromptStructTool.UI.Dialogs
{
    /// <summary>
    /// Modern dialog for loading built-in examples
    /// </summary>
    public partial class ExampleDialog : Form
    {
        public ExampleDialog(string title, string content)
        {
            InitializeComponent();
            this.Text = title;
            textBoxContent.Text = content;
            ApplyModernStyling();
        }

        private void ApplyModernStyling()
        {
            this.Font = new Font("Segoe UI", 9);
            this.BackColor = Color.FromArgb(240, 240, 245);
            textBoxContent.BackColor = Color.White;
            textBoxContent.Font = new Font("Segoe UI", 10);
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxContent.Text);
            MessageBox.Show("Example copied to clipboard", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
