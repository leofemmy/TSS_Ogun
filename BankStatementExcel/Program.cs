using Exceptionless;
using System;
using System.Linq;
using System.Windows.Forms;

namespace BankStatementExcel
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //string path = tmpFolder;

            bool isAutoLogin = args != null && args.Count() > 1;

            //check whether another instance of the program, then kill it
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1) System.Diagnostics.Process.GetCurrentProcess().Kill();

            Application.Run(new Form1());
        }

        public static string ApplicationName = "Excel Capture Manager";

        static Program()
        {
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var error = e.ExceptionObject as Exception;
            error.ToExceptionless().Submit();
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            e.Exception.ToExceptionless().Submit();
            MessageBox.Show(e.Exception.Message);
        }

    }
}
