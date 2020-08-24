using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using DevExpress.XtraGrid.Views.Grid;
using System.Data.SqlClient;
using Control_Panel.Class;

namespace Control_Panel.Forms
{
    public partial class FrmTown : Form
    {
        //DBConnection connect = new DBConnection();

        //Methods extMethods = new Methods();

        public static FrmTown publicStreetGroup;

        string modulesAccess, modulesAccess1;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        string IDs;

        string[] split2;

        string[] split3;

        public FrmTown()
        {
            InitializeComponent();

            publicStreetGroup = this;
            setImages();
            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;


            Load += OnFormLoad;
            gridView1.DoubleClick += gridView1_DoubleClick;
            bttnCancel.Click += Bttn_Click;
            //bttnReset.Click += Bttn_Click;
            bttnUpdate.Click += Bttn_Click;

            OnFormLoad(null, null);
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            setDBComboBox();
            setDBComboBox1();
            setReload();
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //tsbEdit.PerformClick();

        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnCancel)
            {
                tsbReload.PerformClick();
            }
            //else if (sender == bttnReset)
            //{
            //    if (!boolIsUpdate)
            //        Clear();
            //    else
            //        FillField(IDs);
            //        //setReload();
            //}
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        private void Clear()
        {
            txtTown.Clear();
            cboLGA.DeselectAll();
            chkEdRevOffice.DeselectAll();
            //cboLGA.= -1;
            //cboLGA.Text = null;
            //chkEdRevOffice.Text = null;

            ////setDBComboBox();
            ////setDBComboBox1();
            ////cboLGA.Focus();
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnReset.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];

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
                MDIMain.publicMDIParent.RemoveControls();
            }
            else if (sender == tsbNew)
            {
                groupControl2.Text = "Add New Record";
                iTransType = TransactionTypeCode.New;
                ShowForm();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";
                iTransType = TransactionTypeCode.Edit;
                if (EditRecordMode())
                {
                    ShowForm();
                    boolIsUpdate = true;
                }
            }
            else if (sender == tsbDelete)
            {
                groupControl2.Text = "Delete Record Mode";
                iTransType = TransactionTypeCode.Delete;
                if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
                {
                }
                else
                    tsbReload.PerformClick();
                boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload;
                ShowForm();
            }
            //bttnReset.PerformClick();
        }

        protected void ShowForm()
        {
            switch (iTransType)
            {
                case TransactionTypeCode.Null:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.New:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.Edit:
                    splitContainer1.Panel1Collapsed = false;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.Delete:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                case TransactionTypeCode.Reload:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
                default:
                    splitContainer1.Panel1Collapsed = true;
                    splitContainer1.Panel2Collapsed = false;
                    break;
            }
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtTown.Text == "")
                {
                    Common.setEmptyField("Town Field ", Program.ApplicationName);
                    txtTown.Focus();
                }
                else if (cboLGA.EditValue.ToString() == null || cboLGA.EditValue.ToString() == "")
                {
                    Common.setEmptyField("Local Government Field ", Program.ApplicationName);
                    cboLGA.Focus();
                }
                else if (chkEdRevOffice.EditValue.ToString() == null || chkEdRevOffice.EditValue.ToString() == "")
                {
                    Common.setEmptyField("Revenue Tax Office Field ", Program.ApplicationName);
                    chkEdRevOffice.Focus();
                }
                else
                {
                    string streetGroup = txtTown.Text.Trim();

                    //int TownID = (string.IsNullOrEmpty(cbTown.SelectedValue.ToString())) ? 0 : Convert.ToInt32(cbTown.SelectedValue.ToString()) + 0;

                    //check form status either new or edit
                    if (!boolIsUpdate)
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {
                                //insert records into town table
                                int recs = Convert.ToInt32(new SqlCommand(String.Format("INSERT INTO [tblTown]([TownName],[StateCode]) VALUES ('{0}','{1}');SELECT @@IDENTITY", txtTown.Text.Trim().ToUpperInvariant(), Program.stateCode), db, transaction).ExecuteScalar());

                                //splite the LGA code
                                split2 = modulesAccess.Split(',');

                                //count d number of split
                                for (int j = 0; j < split2.Count(); j++)
                                {
                                    //insert revenue code into table with revenue office id
                                    using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO [tblTownLGARelation]([LgaID],[TownID]) VALUES ('{0}', '{1}');", Convert.ToString(split2[j]), recs), db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }
                                }


                                //splite the Revenue tax office

                                split3 = modulesAccess1.Split(',');

                                //count d number of split
                                for (int k = 0; k < split3.Count(); k++)
                                {
                                    //insert Revenue tax office code into table with revenue office id
                                    string query = String.Format("INSERT INTO [tblTownRevenueOfficeRelation]([RevenueOfficeID],[TownID]) VALUES ('{0}', '{1}');", Convert.ToString(split3[k]), recs);

                                    using (SqlCommand sqlCommand2 = new SqlCommand(query, db, transaction))
                                    {
                                        sqlCommand2.ExecuteNonQuery();
                                    }
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback();
                                Tripous.Sys.ErrorBox(sqlError);
                                return;
                            }
                            db.Close();
                        }
                        setReload();
                        Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);

                        if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            bttnCancel.PerformClick();
                        }
                        else
                        {
                            //bttnReset.PerformClick();
                            Clear(); txtTown.Focus();
                        }
                        //}
                    }
                    else
                    {
                        //update the records

                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();
                            try
                            {
                                //MessageBox.Show(MDIMain.stateCode);
                                //fieldid
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblTown] SET [TownName]='{{0}}',[StateCode]='{{1}}' where  TownCode ='{0}'", ID), txtTown.Text.Trim().ToUpperInvariant(), MDIMain.stateCode), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                //splite the LGA code
                                split2 = modulesAccess.Split(',');

                                //update table TownLGARelations after Modify records
                                //count d number of split
                                for (int j = 0; j < split2.Count(); j++)
                                {
                                    using (SqlCommand sqlCommand = new SqlCommand(String.Format(String.Format("UPDATE [tblTownLGARelation] SET [LgaID]='{{0}}',[TownID]='{{1}}' where  TownID ='{0}'", ID), Convert.ToString(split2[j]), ID), db, transaction))
                                    {
                                        sqlCommand.ExecuteNonQuery();
                                    }
                                }


                                //update table TownRevenueOfficesRelations after Modify records
                                //splite the Revenue tax office

                                split3 = modulesAccess1.Split(',');

                                //count d number of split
                                for (int k = 0; k < split3.Count(); k++)
                                {
                                    using (SqlCommand sqlCommand2 = new SqlCommand(String.Format(String.Format("UPDATE [tblTownRevenueOfficeRelation] SET [RevenueOfficeID]='{{0}}',[TownID]='{{1}}' where  TownID ='{0}'", ID), Convert.ToString(split3[k]), ID), db, transaction))
                                    {
                                        sqlCommand2.ExecuteNonQuery();
                                    }
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback();
                                Tripous.Sys.ErrorBox(sqlError);
                                return;
                            }
                            db.Close();
                        }

                        //if ((new BusinessLogic()).UpdateStreetGroup(Common.GetComboBoxValue(cbTown), streetGroup, TownID))
                        //{
                        setReload();
                        Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);
                        //bttnReset.PerformClick();
                        tsbReload.PerformClick();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);

            }

        }

        private void setReload()
        {

            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter("select * from ViewTownLGARevenueOffice ", Logic.ConnectionString))
                //using (SqlDataAdapter ada = new SqlDataAdapter("select * from tblTown ", connect.ConnectionString()))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                this.gridControl1.DataSource = dt;
            }
            this.gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["TownID"].Visible = false;
            gridView1.Columns["TownCode"].Visible = false;
            gridView1.Columns["TownName"].GroupIndex = 0;
            gridView1.Columns["LgaName"].GroupIndex = 1;
            //gridView1.Columns["OfficeName"].GroupIndex = 2;
            gridView1.Columns["TownLGA"].Visible = false;
            gridView1.Columns["TownOffice"].Visible = false;
            this.gridView1.BestFitColumns();
        }

        protected bool EditRecordMode()
        {
            bool bResponse = false;
            GridView view = (GridView)gridControl1.FocusedView;
            String[] Drr = GetRowDetails(view);
            cboLGA.EditValue = Drr[0];
            cboLGA.RefreshEditValue();
            chkEdRevOffice.EditValue = Drr[1];
            chkEdRevOffice.RefreshEditValue();
            //if (Drr != null && Drr.Count() > 0)
            //{
            //    foreach (DataRow item in Drr)
            //    {
            //        object obj = item["TownLGA"];
            //    }
            //}
            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                if (dr != null)
                {

                    ID = Convert.ToInt32(dr["TownCode"]);
                    IDs = dr["TownCode"].ToString();
                    //bResponse = FillField(Convert.ToInt32(dr["TownCode"]));
                    bResponse = FillField(IDs);
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        public void setDBComboBox()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {

                string query = string.Format("select *  from tblLga where StateCode='{0}'", Program.stateCode);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setCheckEdit(cboLGA, Dt, "LgaID", "LgaName");
        }

        public void setDBComboBox1()
        {
            DataTable Dt;

            using (var ds = new System.Data.DataSet())
            {

                string query = "select OfficeName ,RevenueOfficeID from tblRevenueOffice";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //fill the data set
                Dt = ds.Tables[0];
            }
            //populate the checkedit with data
            Common.setCheckEdit(chkEdRevOffice, Dt, "RevenueOfficeID", "OfficeName");



        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        private bool FillField(String fieldid)
        {
            bool bResponse = false;

            //load data from the table into the forms for edit
            string query = String.Format("select * from tblTown where TownCode ='{0}'", fieldid);
            //DataTable dts = extMethods.LoadData(String.Format("select * from tblTown where TownCode ='{0}'", fieldid));

            DataTable dts = (new Logic()).getSqlStatement(query).Tables[0];

            if (dts != null)
            {
                bResponse = true;
                txtTown.Text = dts.Rows[0]["TownName"].ToString();

            }
            else
                bResponse = false;

            return bResponse;
        }

        private void cboLGA_EditValueChanged(object sender, EventArgs e)
        {
            string values = string.Empty;

            object val = cboLGA.EditValue;

            object[] lol = val.ToString().Split(',');

            int i = 0;
            foreach (object obj in lol)
            {
                values += String.Format("{0}", obj.ToString().Trim());
                if (i + 1 < lol.Count())
                    values += ",";
                ++i;
            }
            modulesAccess = (string)values;
        }

        private void chkEdRevOffice_EditValueChanged(object sender, EventArgs e)
        {
            string values = string.Empty;

            object val = chkEdRevOffice.EditValue;

            object[] lol = val.ToString().Split(',');

            int i = 0;
            foreach (object obj in lol)
            {
                values += String.Format("{0}", obj.ToString().Trim());
                if (i + 1 < lol.Count())
                    values += ",";
                ++i;
            }
            modulesAccess1 = (string)values;
        }

        private String[] GetRowDetails(GridView view)
        {
            String[] strResponse = new String[2];
            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);
                if (dr != null)
                {
                    DataTable Dt = gridControl1.DataSource as DataTable;
                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        foreach (DataRow town in Dt.Rows)
                        {
                            DataRow[] Drr = Dt.Select(String.Format("[TownID] = {0}", dr["TownID"]));
                            if (Drr != null && Drr.Count() > 0)
                            {
                                strResponse[0] += String.Format("{0}, ", town["TownLGA"]);
                                String temp = null;
                                foreach (DataRow lga in Drr)
                                {
                                    temp += String.Format("{0}, ", lga["TownOffice"]);
                                }
                                strResponse[1] = temp.Remove(temp.LastIndexOf(','));
                            }
                        }
                        strResponse[0] = strResponse[0].Remove(strResponse[0].LastIndexOf(','));
                    }
                }
            }
            return strResponse;
        }

    }
}
