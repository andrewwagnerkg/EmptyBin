using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace EmtyBin
{
    class RecycleWorker : IDisposable
    {
        public event Action Empty;
        public event Action NotEmpty;
        Thread currentthread;
        bool isRun = true;

        enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000002,
            SHERB_NOSOUND = 0x00000004
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        private readonly Shell32.Folder recycle;
        public RecycleWorker(Shell32.Folder recycle)
        {
            this.recycle = recycle;
            currentthread = new Thread(Run);
            currentthread.Start();
        }

        [STAThread]
        private void Run()
        {
            while (isRun)
            {
                try
                {
                    int count = recycle.Items().Count;
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
            isRun = false;
        }

        internal void ClearRecycle()
        {
            uint result = SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOCONFIRMATION | RecycleFlags.SHERB_NOPROGRESSUI | RecycleFlags.SHERB_NOSOUND);
        }


    }
}
