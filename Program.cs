using System;
using System.Windows.Forms;

namespace PromptStructTool
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configure application and run MainForm (wizard)
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}