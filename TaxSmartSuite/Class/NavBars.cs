using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DevExpress.XtraNavBar;
using TaxSmartSuite;
using System.Collections;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace TaxSmartRegistration.Classess
{
    public class NavBars
    {
//        //SqlConnection 
//        public static ArrayList ArrUserRight = new ArrayList();

//        public static void ManageNavBarControls(NavBarControl NavBar, string AppCode)
//        {
//            var Blogic = new BusinessLogic();
//            foreach (NavBarGroup GroupList in NavBar.Groups)
//            {
//                if (Blogic.InsertNavModules(GroupList.Tag as string, GroupList.Caption, AppCode))
//                {
//                    foreach (NavBarItemLink ItemList in GroupList.ItemLinks)
//                    {
//                        NavBarItem items = ItemList.Item;
//                        if (!Blogic.InsertNavModulesAccess(items.Tag as string, items.Caption, GroupList.Tag as string))
//                            if (String.IsNullOrEmpty(Blogic.GetLastErrorMessage))
//                            {
//                                Common.setMessageBox("Error Updating Application Modules Access", Program.ApplicationName, 3);
//                                break;
//                            }
//                    }
//                }
//                else
//                {
//                    if (String.IsNullOrEmpty(Blogic.GetLastErrorMessage))
//                    {
//                        Common.setMessageBox("Error Updating Application Modules", Program.ApplicationName, 3);
//                        break;
//                    }
//                }
//            }
//        }

//        public static void NavBarEnableDisableControls(NavBarControl NavBar, bool status)
//        {
//            if (NavBar == null) return;
//            foreach (NavBarGroup GroupList in NavBar.Groups)
//            {
//                if (object.Equals(GroupList.Tag, "002-004"))
//                    GroupList.Visible = true;
//                else
//                    GroupList.Visible = status;
//                //if (GroupList.Tag == "002-004" as object) continue;
//                //GroupList.Visible = status;
//                foreach (NavBarItemLink ItemList in GroupList.ItemLinks)
//                {
//                    if ((ItemList.ItemName == "navBarItemExit") || (ItemList.ItemName == "navBarItemLogout")) continue;
//                    ItemList.Visible = status;
//                }
//            }
//        }

//        public static void NavBarEnableDisableControls(NavBarControl NavBar, bool status, string Code, bool IsFirstLevel)
//        {
//            if (NavBar == null) return;
//            foreach (NavBarGroup GroupList in NavBar.Groups)
//            {
//                if (IsFirstLevel)
//                {
//                    if (GroupList.Tag == "002-004" as object) continue;
//                    if (object.Equals(GroupList.Tag, Code))
//                        GroupList.Visible = status;
//                }
//                else
//                {
//                    foreach (NavBarItemLink ItemList in GroupList.ItemLinks)
//                    {
//                        if (ItemList.ItemName == "navBarItemExit") continue;
//                        NavBarItem items = ItemList.Item;
//                        if (object.Equals(items.Tag, Code))
//                            ItemList.Visible = status;
//                    }
//                }
//            }
//        }

//        public static bool ManageUserLoginNavBar(NavBarControl NavBar)
//        {
//            if (NavBar == null) return false;
//            string UserID = Program.UserID;
//            string AppCode = Program.ApplicationCode;
//            if (String.IsNullOrEmpty(UserID)) return false;
//            bool bResponse = false;

//            /* •———————————————————————————————•
//               | Load User Application Modules |
//               •———————————————————————————————• */
//            DataTable Dt;
//            string SQL = String.Format(@"SELECT [ModulesCode],[ModulesName] FROM [dbo].[ViewUserApplicationModules] 
//                    WHERE [UserID] = '{0}' AND [ApplicationCode] = '{1}'", UserID, AppCode);
//            Dt = (new BusinessLogic()).getSqlStatement(SQL).Tables[0];
//            if (Dt != null && Dt.Rows.Count > 0)
//            {
//                foreach (DataRow Dr in Dt.Rows)
//                {
//                    // Make visible the Menu group
//                    NavBarEnableDisableControls(NavBar, true, Dr["ModulesCode"] as string, true);
//                    /* •——————————————————————————————————————•
//                       | Load User Application Modules Access |
//                       •——————————————————————————————————————• */
//                    SQL = String.Format(@"SELECT [ModuleAccessCode],[ModuleAccessName] 
//                            FROM [dbo].[ViewUserApplicationModulesAccess] 
//                            WHERE [UserID] = '{0}' AND [ModulesCode] = '{1}' AND [ApplicationCode] = '{2}'"
//                        , UserID, Dr["ModulesCode"], AppCode);
//                    DataTable Dtt = (new BusinessLogic()).getSqlStatement(SQL).Tables[0];
//                    if (Dtt != null && Dtt.Rows.Count > 0)
//                    {
//                        foreach (DataRow Drr in Dtt.Rows)
//                        {
//                            // Make Visible the Menu Group Items
//                            NavBarEnableDisableControls(NavBar, true, Drr["ModuleAccessCode"] as string, false);
//                            LoadUserApplicationRight(UserID, Dr["ModulesCode"] as string, Drr["ModuleAccessCode"] as string, AppCode);
//                        }
//                    }
//                }
//                bResponse = true;
//            }
//            return bResponse;
//        }

//        protected static void LoadUserApplicationRight(string UserID, string ModulesCode
//            , string ModuleAccessCode, string AppCode)
//        {
//            if (String.IsNullOrEmpty(UserID) || String.IsNullOrEmpty(ModulesCode) || String.IsNullOrEmpty(ModuleAccessCode) || String.IsNullOrEmpty(AppCode))
//                return;
//            string SQL = String.Format(@"SELECT [ApplicationRghtID],[ApplicationRightName],[ModulesCode],[ModuleAccessCode] FROM [dbo].[ViewUserApplicationRight]
//                                        WHERE [UserID] = '{0}' AND [ModulesCode] = '{1}' AND [ModuleAccessCode] = '{2}' AND [ApplicationCode] = '{3}'"
//                , UserID, ModulesCode, ModuleAccessCode, AppCode);
//            DataTable Dt = (new BusinessLogic()).getSqlStatement(SQL).Tables[0];
//            if (Dt != null && Dt.Rows.Count > 0)
//            {
//                foreach (DataRow Dr in Dt.Rows)
//                {
//                    if (Dr != null)
//                        ArrUserRight.Add(Dr);
//                }
//            }
//        }

//        public static void ToolStripEnableDisableControls(ToolStrip toolStrip, string ModulesAccessCode)
//        {
//            if (toolStrip == null) return;
//            if (String.IsNullOrEmpty(ModulesAccessCode)) return;
//            ToolStripEnableDisable(toolStrip, null, false);
//            if (ArrUserRight != null && ArrUserRight.Count > 0)
//            {
//                foreach (DataRow Dr in ArrUserRight)
//                {
//                    if (Dr != null)
//                    {
//                        if (String.Equals(Dr["ModuleAccessCode"] as string, ModulesAccessCode))
//                        {
//                            int right = Convert.ToInt32(Dr["ApplicationRghtID"]);
//                            string Code = string.Empty;
//                            switch (right)
//                            {
//                                case 1:
//                                    Code = ApplicationRightCode.Add;
//                                    break;
//                                case 2:
//                                    Code = ApplicationRightCode.Edit;
//                                    break;
//                                case 3:
//                                    Code = ApplicationRightCode.Delete;
//                                    break;
//                                default:
//                                    Code = String.Empty;
//                                    break;
//                            }
//                            if (!String.IsNullOrEmpty(Code))
//                                ToolStripEnableDisable(toolStrip, Code, true);
//                        }
//                    }
//                }
//            }
//        }

        public static void ToolStripEnableDisable(ToolStrip toolStrip, string Code, bool Status)
        {
            if (toolStrip == null) return;
            foreach (ToolStripItem item in toolStrip.Items)
            {
                if (item.GetType().ToString() == typeof(ToolStripButton).ToString())
                {
                    ToolStripButton toolStripButton = item as ToolStripButton;
                    if (String.IsNullOrEmpty(Code))
                    {
                        if (String.Equals(toolStripButton.Tag, ApplicationRightCode.Close)) continue;
                        item.Enabled = Status;
                        continue;
                    }
                    else if (String.Equals(toolStripButton.Tag, Code))
                    {
                        toolStripButton.Enabled = Status;
                    }
                }
            }
        }
    }

    /* •—————————————————————————————•
       | public enum ApplicationCode |
       | {                           |
       |     ControlPanel=1,         |
       |     Registration,           |
       |     Collection,             |
       |     Normalization           |
       | }                           |
       •—————————————————————————————• */

    public struct ApplicationRightCode
    {
        public const string Add = "001";
        public const string Edit = "002";
        public const string Delete = "003";
        public const string Reload = "004";
        public const string Close = "005";
    }
}
