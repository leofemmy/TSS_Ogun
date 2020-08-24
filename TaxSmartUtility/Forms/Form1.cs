using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Selection;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using TaxSmartSuite.CommonLibrary.Controls;
using TaxSmartUtility.Classes;
using TaxSmartUtility.Forms;

namespace TaxSmartUtility
{
    public partial class Form1 : Form
    {
        public static Form1 publicStreetGroup; bool isFirstGrid = true;

        GridCheckMarksSelection selection;

        private GridRadioGroupColumnHelper _Helper;

        System.Data.DataSet ds = new System.Data.DataSet();

        int rowHandle;

        DataTable temTable = new DataTable();

        DataTable tempResult = new DataTable();

        private SqlCommand _command; private SqlDataAdapter adp; private string BatchNumber; private SqlCommand _command1; private SqlCommand _command2;
        public Form1()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            sbtnFind.Click += SbtnFind_Click;

            spbAdd.Click += SpbAdd_Click;

            sbtnSend.Click += SbtnSend_Click;

            spbRemove.Click += SpbRemove_Click;

            OnFormLoad(null, null);

            //temTable.Columns.Add("Check", typeof(Boolean));
            temTable.Columns.Add("UTIN", typeof(string));
            temTable.Columns.Add("OrganizationName", typeof(string));

            tempResult.Columns.Add("UTIN", typeof(string));
            tempResult.Columns.Add("IsPrimary", typeof(Boolean));


            gridControl2.DataSource = temTable;

            _Helper = new GridRadioGroupColumnHelper(gridView2);

            _Helper.SelectedRowChanged += new EventHandler(_Helper_SelectedRowChanged);

            SplashScreenManager.CloseForm(false);
        }

        private void SpbRemove_Click(object sender, EventArgs e)
        {
            DataRow rows = temTable.Rows[rowHandle];

            if (rows != null)
            {
                int? vasl = rows["UTIN"] == DBNull.Value ? (int?)null : Convert.ToInt32(rows["UTIN"]);

                string vals = rows["UTIN"].ToString();
                 
                var revRow = temTable.Select($"UTIN = {vasl}").Single();

                temTable.Rows.Remove(revRow);

                DeleteSelectedRows(gridView2, rowHandle);
            }
        }

        private void DeleteSelectedRows(DevExpress.XtraGrid.Views.Grid.GridView view, int rowhandle)
        {
            if (view == null) return;

            view.DeleteRow(view.GetRowHandle(rowhandle));

            view.RefreshData();

        }


        private void SbtnSend_Click(object sender, EventArgs e)
        {
            DataRow row = temTable.Rows[rowHandle];

            if (row != null)
            {
                var utin = row["UTIN"];

                for (int i = 0; i < gridView2.DataRowCount; i++)
                {
                    int rowHandles = i;

                    var obj = gridView2.GetRowCellValue(rowHandles, "UTIN");

                    if (utin.ToString() == obj.ToString())
                    {
                        tempResult.Rows.Add(new object[] { obj, 1 });
                    }
                    else
                    {
                        tempResult.Rows.Add(new object[] { obj, 0 });
                    }

                }

                using (SqlConnection connect = new SqlConnection(Logic.ConnectionString))
                {
                    connect.Open();
                    _command = new SqlCommand("TaxAgentMergeTin", connect) { CommandType = CommandType.StoredProcedure };
                    _command.Parameters.Add(new SqlParameter("@user", SqlDbType.VarChar)).Value = "Jumia"; //Program.UserID;
                    _command.Parameters.Add(new SqlParameter("@pTransaction", SqlDbType.Structured)).Value = tempResult;
                    _command.CommandTimeout = 0;
                    //@Years
                    using (System.Data.DataSet ds = new System.Data.DataSet())
                    {
                        ds.Clear();
                        adp = new SqlDataAdapter(_command);
                        adp.Fill(ds);
                        connect.Close();

                        if (ds.Tables[0].Rows[0]["returnCode"].ToString() != "00")
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                            return;

                        }
                        else
                        {
                            Common.setMessageBox(ds.Tables[0].Rows[0]["returnMessage"].ToString(), Program.ApplicationName, 2);

                        }

                    }
                }
                setReload();
            }
            else
            {
                Common.setMessageBox("No Primary Tin Selected", "", 1); return;
            }
        }

        void _Helper_SelectedRowChanged(object sender, EventArgs e)
        {
            rowHandle = _Helper.SelectedDataSourceRowIndex;

        }

        private void SpbAdd_Click(object sender, EventArgs e)
        {

            if (selection.SelectedCount == 0)
            {
                Common.setMessageBox("No Selection Made to Merge", Program.ApplicationName, 3);
                return;

            }
            else
            {
                for (int i = 0; i < selection.SelectedCount; i++)
                {
                    temTable.Rows.Add(new object[] { String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["UTIN"]), String.Format("{0}", (selection.GetSelectedRow(i) as DataRowView)["OrganizationName"]) });
                }

                if (temTable != null && temTable.Rows.Count > 0)
                {
                    gridControl2.DataSource = temTable;

                    gridView2.Columns["UTIN"].OptionsColumn.AllowEdit = false;

                    gridView2.Columns["OrganizationName"].OptionsColumn.AllowEdit = false;

                    gridView2.BestFitColumns();

                    selection.ClearSelection();

                }
                
                //_Helper = new GridRadioGroupColumnHelper(gridView2);
            }
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMains.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMains.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMains.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMains.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMains.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMains.publicMDIParent.i32x32.Images[9];
            ////bttnReset.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            //btnAllocate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            //bttncompare.Image = MDIMains.publicMDIParent.i32x32.Images[8];
            ////bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnSave.Image = MDIMains.publicMDIParent.i32x32.Images[7];
            //sbnUpdate.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            //sbnDisapprove.Image = MDIMains.publicMDIParent.i32x32.Images[6];
            //btnToken.Image = MDIMains.publicMDIParent.i32x32.Images[7];
        }

        void ToolStripEvent()
        {
            tsbClose.Click += OnToolStripItemsClicked;
            tsbNew.Click += OnToolStripItemsClicked;
            tsbEdit.Click += OnToolStripItemsClicked;
            tsbDelete.Click += OnToolStripItemsClicked;
            tsbReload.Click += OnToolStripItemsClicked;
        }

        void OnToolStripItemsClicked(object sender, EventArgs e)
        {
            if (sender == tsbClose)
            {
                MDIMains.publicMDIParent.RemoveControls();
                //this.Close();

            }
            else if (sender == tsbNew)
            {
                //groupControl2.Text = "Add New Record";
                //iTransType = TransactionTypeCode.New;
                //ShowForm();
                //boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
                //iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                //    ShowForm();
                //    boolIsUpdate = true;
                //}
            }
            else if (sender == tsbDelete)
            {
                //groupControl2.Text = "Disable Record Mode";
                //iTransType = TransactionTypeCode.Delete;
                //if (MosesClassLibrary.Utilities.Common.AskQuestion("Disable this record will delete attached record.\nDo you want to continue?", ""))
                //{
                //    if (string.IsNullOrEmpty(ID.ToString()))
                //    {
                //        Common.setMessageBox("No Record Selected for Disable", Program.ApplicationName, 3);
                //        return;
                //    }
                //    else
                //        //deleteRecord(ID);
                //}
                //else
                tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                //iTransType = TransactionTypeCode.Reload; setReload();
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            setReload();
            //DevExpress.XtraGrid.Columns.GridColumn chkColumn;
            //chkColumn = gridView2.Columns.AddField("Chk");
            //chkColumn.OptionsColumn.AllowEdit = true;
            //chkColumn.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            //chkColumn.VisibleIndex = 0;

            //setReload();
            //selection = new GridCheckMarksSelection(gridView1);
            //sbnUpdate.Click += SbnUpdate_Click;
            //sbnDisapprove.Click += SbnDisapprove_Click; btnToken.Click += BtnToken_Click;
        }

        private void SbtnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtfind.Text.ToString()))
            {
                Common.setEmptyField("Organization Name", Program.ApplicationName);
                return;
            }
            else
            {

                //using (var ds = new System.Data.DataSet())
                //{
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    var qry = string.Format("SELECT  tblTaxAgent.UTIN , OrganizationName , HouseNumber + ' ' + StreetName + ' ' + StreetGroup AS Description   FROM  Registration.tblTaxAgent WHERE OrganizationName LIKE '{0}%' AND UTIN NOT IN (SELECT UTIN FROM Registration.tblMergeTin) ORDER BY OrganizationName ASC", txtfind.Text.ToString());

                    using (SqlDataAdapter ada = new SqlDataAdapter(qry, Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        gridControl1.DataSource = ds.Tables[0];

                        gridView1.OptionsBehavior.Editable = false;

                        //gridView1.Columns["RegistrationDate"].DisplayFormat.FormatType = FormatType.DateTime;
                        //gridView1.Columns["RegistrationDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
                        gridView1.BestFitColumns();

                        if (isFirstGrid)
                        {
                            selection = new GridCheckMarksSelection(gridView1, ref lblSelect);
                            selection.CheckMarkColumn.VisibleIndex = 0;
                            isFirstGrid = false;
                        }
                    }
                    else
                    {
                        Common.setMessageBox("No Record Found for Search Name", Program.ApplicationName, 1); return;
                    }


                }
                catch (Exception ex)
                {
                    Tripous.Sys.ErrorBox(ex.Message); return;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }

                //}

            }
        }

        void setReload()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                var qry = string.Format("SELECT  tblTaxAgent.UTIN , OrganizationName , HouseNumber + ' ' + StreetName + ' ' + StreetGroup AS Description   FROM  Registration.tblTaxAgent WHERE UTIN NOT IN (SELECT UTIN FROM Registration.tblMergeTin) ORDER BY OrganizationName ASC", txtfind.Text.ToString());

                using (SqlDataAdapter ada = new SqlDataAdapter(qry, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    gridControl1.DataSource = ds.Tables[0];

                    gridView1.OptionsBehavior.Editable = false;

                    gridView1.BestFitColumns();

                    if (isFirstGrid)
                    {
                        selection = new GridCheckMarksSelection(gridView1, ref lblSelect);
                        selection.CheckMarkColumn.VisibleIndex = 0;
                        isFirstGrid = false;
                    }
                }
                else
                {
                    Common.setMessageBox("No Record Found for Search Name", Program.ApplicationName, 1); return;
                }


            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex.Message); return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

    }
}
