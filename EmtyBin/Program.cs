using System;
using System.Windows.Forms;

namespace EmtyBin
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder recycle = shell.NameSpace(10);
            Application.Run(new frmMain(recycle));
        }
    }
}
