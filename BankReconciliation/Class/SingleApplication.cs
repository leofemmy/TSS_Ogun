using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BankReconciliation.Class
{
    class SingleApplication
    {
        #region Private Variables
        /// <summary>
        /// Imports 
        /// </summary>

        [DllImport("user32.Dll")]
        private static extern int EnumWindows(EnumWinCallBack callBackFunc, int lParam);

        [DllImport("User32.Dll")]
        private static extern void GetWindowText(int hWnd, StringBuilder str, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        static Mutex mutex;
        const int SW_RESTORE = 9;
        static string sTitle;
        static IntPtr windowHandle;
        delegate bool EnumWinCallBack(int hwnd, int lParam);

        #endregion

        #region private static bool EnumWindowCallBack(int hwnd, int lParam)
        /// <summary>
        /// EnumWindowCallBack
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static bool EnumWindowCallBack(int hwnd, int lParam)
        {
            windowHandle = (IntPtr)hwnd;

            StringBuilder sbuilder = new StringBuilder(256);
            GetWindowText((int)windowHandle, sbuilder, sbuilder.Capacity);
            string strTitle = sbuilder.ToString();

            if (strTitle == sTitle)
            {
                ShowWindow(windowHandle, SW_RESTORE);
                SetForegroundWindow(windowHandle);
                return false;
            }
            return true;
        }//EnumWindowCallBack
        #endregion

        #region public static bool Run(System.Windows.Forms.Form frmMain)
        /// <summary>
        /// Execute a form base application if another instance already running on
        /// the system activate previous one
        /// </summary>
        /// <param name="frmMain">main form</param>
        /// <returns>true if no previous instance is running</returns>
        public static bool Run(System.Windows.Forms.Form frmMain)
        {
            if (IsAlreadyRunning())
            {
                sTitle = frmMain.Text;
                //set focus on previously running app
                EnumWindows(new EnumWinCallBack(EnumWindowCallBack), 0);
                return false;
            }
            Application.Run(frmMain);
            return true;
        }
        #endregion

        #region public static bool Run()
        /// <summary>
        /// for console base application
        /// </summary>
        /// <returns></returns>
        public static bool Run()
        {
            if (IsAlreadyRunning())
            {
                return false;
            }
            return true;
        }
        #endregion

        #region private static bool IsAlreadyRunning()
        /// <summary>
        /// check if given exe alread running or not
        /// </summary>
        /// <returns>returns true if already running</returns>
        private static bool IsAlreadyRunning()
        {
            string strLoc = Assembly.GetExecutingAssembly().Location;

            FileSystemInfo fileInfo = new FileInfo(strLoc);
            string sExeName = fileInfo.Name;
            mutex = new Mutex(true, sExeName);

            if (mutex.WaitOne(0, false))
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
