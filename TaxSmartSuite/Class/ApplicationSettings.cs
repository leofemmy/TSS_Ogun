namespace TaxSmartSuite.Class
{
    public class ApplicationSettings
    {
        public static string ApplicationFolder
        {
            get
            {
                //return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), "TaxSmartSuite");
                return System.IO.Path.Combine(System.Configuration.ConfigurationManager.AppSettings["strPath"], "TaxSmartSuite");
            }
        }
    }
}
