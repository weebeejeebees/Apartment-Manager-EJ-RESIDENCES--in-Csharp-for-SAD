using System;
using System.Windows.Forms;

namespace apartmentManager
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Form1.Instance); // Use the singleton instance
        }
    }
}
