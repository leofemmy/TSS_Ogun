using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EasyReg.Forms;
using EasyReg.Class;

namespace EasyReg
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string path = tmpFolder;

            if (!MosesClassLibrary.Configuration2.SetConfigurationFilePath(path))
            {
                TaxSmartSuite.Class.Common.setMessageBox("Error loading application information", ApplicationName, 3);

                return;
            }


            Logic.ConfigureSettings();

            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new Form1());
            Application.Run(new FrmLogIn());

            if (APP_CONNECTED)
            {
                Application.Run(new MDIMain());
            }
            else
                MessageBox.Show("Application Not Accessible");
            
        }

        public static bool APP_CONNECTED = false;

        public static string ApplicationName = "EasyReg";

        public static string AppicationUserCode;

        public static string AgentListId;

        public static string UserType;

        public static string UserName;
        public static string UserId;

        public static string tmpFolder
        {
            get
            {
                     return TaxSmartSuite.Class.ApplicationSettings.ApplicationFolder;
            }
        }


        
    }
}
