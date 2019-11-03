using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace EmtyBin
{
    public partial class Form1 : Form
    {
        RecycleWorker worker;

        public Form1(Shell32.Folder re)
        {
            InitializeComponent();
            worker = new RecycleWorker(re);
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

        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Notify_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void ContextMenu_Click(object sender, EventArgs e)
        {

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
            Microsoft.Win32.RegistryKey myKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
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

        private void УдалитьИзАвтозагрузкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey myKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
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

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            worker.ClearRecycle();
        }
    }
}
