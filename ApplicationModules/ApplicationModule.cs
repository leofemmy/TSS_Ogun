using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationModules
{
    public class ApplicationModule
    {
    }

    public struct AppModules
    {
        public const string ControlPanel = "001";
        public const string Registration = "002";
        public const string Collection = "003";
        public const string Normalization = "004";
        public const string Field = "005";
        public const string Bank = "006";
        public const string DirectAssessment = "007";
        public const string PAYE = "008";
        public const string TCCMANAGER = "009";
        public const string TAXAUDIT = "010";
        public const string ROADTAX = "011";
        public const string BUSSINESS = "012";
        public const string STAMP = "013";

    }

    public struct Modules
    {
        public const string CP_File = "003-001";
        public const string CP_Rules = "003-002";
        public const string CP_Download = "003-003";
        public const string CP_Import = "003-004";
        public const string CP_Export = "003-005";
        public const string CP_InputCollection = "003-006";
    }
    public struct ModuleAccess
    {
        public const string CP_File_Exit = "003-001-001";
        public const string CP_Rules_Annual = "003-002-001";
        public const string CP_Rules_Change = "003-002-002";
        public const string CP_Rules_User = "003-002-003";
        //public const string CP_Rules_Annual = "003-002-001";
        public const string CP_Download_Collection = "003-003-001";
        public const string CP_Import = "003-003-001";

    }
}
