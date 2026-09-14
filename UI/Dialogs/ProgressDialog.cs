using System;
using System.Drawing;
using System.Windows.Forms;

namespace PromptStructTool.UI.Dialogs
{
    /// <summary>
    /// Progress dialog for long-running operations
    /// </summary>
    public partial class ProgressDialog : Form
    {
        public ProgressDialog(string title, string message)
        {
            InitializeComponent();
            this.Text = title;
            labelMessage.Text = message;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            ApplyModernStyling();
        }

        private void ApplyModernStyling()
        {
            this.Font = new Font("Segoe UI", 9);
            this.BackColor = Color.FromArgb(240, 240, 245);
            progressBar1.Style = ProgressBarStyle.Marquee;
        }

        public void SetMessage(string message)
        {
            labelMessage.Text = message;
            Application.DoEvents();
        }
    }
}
