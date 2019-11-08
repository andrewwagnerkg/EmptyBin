using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace EmtyBin
{
    public partial class frmMain : Form
    {
        RecycleWorker worker;
        Microsoft.Win32.RegistryKey myKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

        public frmMain(Shell32.Folder recycle)
        {
            InitializeComponent();
            worker = new RecycleWorker(recycle);
            worker.Empty += Worker_Empty;
            worker.NotEmpty += Worker_NotEmpty;
        }

        private void Worker_NotEmpty()
        {
            Invoke(new MethodInvoker(() => { notify.Icon = Properties.Resources.full; }));
            Invoke(new MethodInvoker(() => toolStripMenuItem1.Enabled = true));
        }

        private void Worker_Empty()
        {
            Invoke(new MethodInvoker(() => { notify.Icon = Properties.Resources.full1; }));
            Invoke(new MethodInvoker(() => toolStripMenuItem1.Enabled = false));
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            worker.Dispose();
            Application.Exit();
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа для очистки корзины. ", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (myKey.GetValue("EmptyBin").ToString() != Application.ExecutablePath)
                    myKey.SetValue("EmptyBin", Application.ExecutablePath);
            }
            catch (NullReferenceException ex)
            {
                myKey.SetValue("EmptyBin", Application.ExecutablePath);
            }
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            worker.ClearRecycle();
        }

        private void DelFromAutostartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (myKey.GetValue("EmptyBin").ToString() == Application.ExecutablePath)
                    myKey.DeleteValue("EmptyBin");
            }
            catch (NullReferenceException ex)
            {
                myKey.DeleteValue("EmptyBin");
            }
        }
    }
}
