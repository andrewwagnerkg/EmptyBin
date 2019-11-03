using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EmtyBin
{
    class RecycleWorker : IDisposable
    {
        public event Action Empty;
        public event Action NotEmpty;
        Thread thread;

        enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000002,
            SHERB_NOSOUND = 0x00000004
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        Shell32.Folder re1;
        public RecycleWorker(Shell32.Folder re)
        {
            re1 = re;
            thread = new Thread(Run);
            thread.Start();
        }

        [STAThread]
        private void Run()
        {
            while (true)
            {
                try
                {
                    int count = re1.Items().Count;
                    if (count != 0)
                        NotEmpty?.Invoke();
                    else
                        Empty?.Invoke();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Thread.Sleep(10);
            }
        }

        public void Dispose()
        {
            try
            {
                thread.Abort();
            }
            catch
            {

            }
        }

        internal void ClearRecycle()
        {
            uint result = SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOCONFIRMATION | RecycleFlags.SHERB_NOPROGRESSUI | RecycleFlags.SHERB_NOSOUND);
        }


    }
}
