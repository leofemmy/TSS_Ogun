using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Collection.Classess;
using Collections;
using DevExpress.XtraSplashScreen;
using TaxSmartSuite.Class;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Selection;

namespace Collection.Forms
{
    public partial class FrmProfile : Form
    {
        public static FrmProfile publicInstance;

        string values, values1, values2, values3;

        protected bool boolIsUpdate;

        protected int ID; bool isBankGrid = true; bool isRevenueGrid = true;

        GridCheckMarksSelection selection;

        int newID;
        public FrmProfile()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            InitializeComponent();

            publicInstance = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            SplashScreenManager.CloseForm(false);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            ////bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnMain.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            //bttnprints.Image = MDIMain.publicMDIParent.i32x32.Images[29];
            //btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];
            //btnClear.Image = MDIMain.publicMDIParent.i32x32.Images[3];

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
                this.Close();
            }
            else if (sender == tsbNew)
            {

            }
            else if (sender == tsbEdit)
            {
                //if (EditRecordMode())
                //{
                //    //ShowForm();
                //    boolIsUpdate = true;
                //}
            }
            else if (sender == tsbDelete)
            {

                tsbReload.PerformClick();
                //boolIsUpdate = false;
            }
            else if (sender == tsbReload)
            {
                //iTransType = TransactionTypeCode.Reload; setReload();
                //ShowForm();
            }

        }

        void OnFormLoad(object sender, EventArgs e)
        {
            setReload();

            //setDBComboBox();

            setDBComboBoxAgency();

            //setDBComboBoxBank();

            cboAgencyTest.EditValueChanged += cboAgencyTest_EditValueChanged;

            cboBankEdt.EditValueChanged += cboBankEdt_EditValueChanged;

            gridView1.DoubleClick += gridView1_DoubleClick;

            //bttnUpdate.Click += bttnUpdate_Click;
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        //void bttnUpdate_Click(object sender, EventArgs e)
        //{
        //    //if (!string.IsNullOrEmpty(cboAgencyTest.EditValue.ToString()) || cboAgencyTest.EditValue != null)
        //    //{
        //    //}
        //    if (string.IsNullOrEmpty(txtName.Text.Trim().ToString()) || txtName.Text == null)
        //    {
        //        Common.setEmptyField("Profile Name", "Profile Setup");
        //        return;
        //    }
        //    else
        //    {

        //        if (cboAgencyTest.EditValue == null || cboAgencyTest.EditValue.ToString() != "")
        //        {
        //            values = string.Empty;

        //            object[] lol = cboAgencyTest.EditValue.ToString().Split(',');

        //            int i = 0;

        //            foreach (object obj in lol)
        //            {
        //                values += string.Format("{0}", obj.ToString().Trim());

        //                if (i + 1 < lol.Count())
        //                    values += ",";
        //                ++i;
        //            }
        //        }

        //        if (cboRevenueEdt.EditValue == null || cboRevenueEdt.EditValue.ToString() != "")
        //        {
        //            values1 = string.Empty;

        //            object[] lol = cboRevenueEdt.EditValue.ToString().Split(',');

        //            int i = 0;

        //            foreach (object obj in lol)
        //            {
        //                values1 += string.Format("{0}", obj.ToString().Trim());

        //                if (i + 1 < lol.Count())
        //                    values1 += ",";
        //                ++i;
        //            }


        //        }

        //        if (cboBankEdt.EditValue == null || cboBankEdt.EditValue.ToString() != "")
        //        {
        //            values2 = string.Empty;

        //            object[] lol = cboBankEdt.EditValue.ToString().Split(',');

        //            int i = 0;

        //            foreach (object obj in lol)
        //            {
        //                values2 += string.Format("{0}", obj.ToString().Trim());

        //                if (i + 1 < lol.Count())
        //                    values2 += ",";
        //                ++i;
        //            }

        //        }

        //        if (cboBranchesEdt.EditValue == null || cboBranchesEdt.EditValue.ToString() != "")
        //        {

        //            values3 = string.Empty;

        //            object[] lol = cboBranchesEdt.EditValue.ToString().Split(',');

        //            int i = 0;

        //            foreach (object obj in lol)
        //            {
        //                values3 += string.Format("{0}", obj.ToString().Trim());

        //                if (i + 1 < lol.Count())
        //                    values3 += ",";
        //                ++i;
        //            }

        //        }

        //        if (!boolIsUpdate)
        //        {
        //            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
        //            {
        //                SqlTransaction transaction;

        //                db.Open();

        //                transaction = db.BeginTransaction();
        //                try
        //                {


        //                    using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO Receipt.tblProfile(Name) VALUES ('{0}');SELECT CAST(scope_identity() AS int)", txtName.Text.ToString().Trim()), db, transaction))
        //                    {
        //                        //sqlCommand1.ExecuteNonQuery();

        //                        newID = (int)sqlCommand1.ExecuteScalar();
        //                    }

        //                    string qry = string.Format("INSERT INTO Receipt.tblProfileCriteria( ProfileID ,Agency ,Revenue ,Bank ,BankBranch)VALUES ({0},'{1}','{2}','{3}','{4}');", newID, values, values1, values2, values3);

        //                    using (SqlCommand sql2 = new SqlCommand(qry, db, transaction))
        //                    {
        //                        sql2.ExecuteNonQuery();
        //                    }

        //                    transaction.Commit();
        //                }
        //                catch (SqlException sqlError)
        //                {
        //                    transaction.Rollback();
        //                }
        //                db.Close();
        //            }

        //            Common.setMessageBox("Record has been Successfully Added", Program.ApplicationName, 1);

        //            if (
        //                MessageBox.Show("Do you want to add another record?", Program.ApplicationName,
        //                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        //            {
        //                setClear();

        //            }
        //            else
        //            {
        //                setClear();

        //            }
        //        }
        //        else
        //        {
        //            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
        //            {
        //                SqlTransaction transaction;

        //                db.Open();

        //                transaction = db.BeginTransaction();
        //                try
        //                {
        //                    //MessageBox.Show(MDIMain.stateCode);
        //                    //fieldid
        //                    string query = String.Format("UPDATE Receipt.tblProfileCriteria SET [Agency]='{0}',Revenue='{1}',Bank='{2}',BankBranch='{3}' where  ProfileID ='{4}'", values, values1, values2, values3, ID);

        //                    using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
        //                    {
        //                        sqlCommand1.ExecuteNonQuery();
        //                    }

        //                    transaction.Commit();
        //                }
        //                catch (SqlException sqlError)
        //                {
        //                    transaction.Rollback();
        //                }
        //                db.Close();
        //            }

        //            setReload();
        //            Common.setMessageBox("Changes in Record has been Successfully Saved.", Program.ApplicationName, 1);
        //            setReload(); setClear();
        //            //tsbReload.PerformClick();
        //        }


        //    }
        //}

        public void setDBComboBox()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT DISTINCT ZoneName  FROM Collection.tblCollectionReport WHERE ZoneName IS NOT NULL AND ZoneName <>'' ORDER BY ZoneName", Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                //Common.setComboList(cboZone, Dt, "ZoneName", "ZoneName");

                //cboZone.SelectedIndex = -1;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        public void setDBComboBoxAgency()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                using (var ds = new System.Data.DataSet())
                {
                    using (SqlDataAdapter ada = new SqlDataAdapter((string)"SELECT AgencyCode,AgencyName FROM Registration.tblAgency ORDER BY AgencyName", Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];
                }

                //Common.setComboList(cboAgency, Dt, "AgencyCode", "AgencyName");

                Common.setCheckEdit(cboAgencyTest, Dt, "AgencyCode", "AgencyName");

                //cboAgency.SelectedIndex = -1;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        void setDBComboBoxReveneu()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable DtRev;

                string values = string.Empty;

                object[] lol = cboAgencyTest.EditValue.ToString().Split(',');

                int i = 0;

                foreach (object obj in lol)
                {
                    values += string.Format("'{0}'", obj.ToString().Trim());

                    if (i + 1 < lol.Count())
                        values += ",";
                    ++i;
                }

                using (var ds = new System.Data.DataSet())
                {
                    string query = string.Format("SELECT RevenueCode,Description FROM Collection.tblRevenueType WHERE AgencyCode IN ({0})  ORDER BY Description", values);

                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    DtRev = ds.Tables[0];
                }

                //Common.setComboList(cboRevenue, Dt, "RevenueCode", "Description");

                //Common.setCheckEdit(cboRevenueEdt, Dt, "RevenueCode", "Description");

                //cboRevenue.SelectedIndex = -1;
                if (DtRev != null && DtRev.Rows.Count > 0)
                {
                    gridControl3.DataSource = DtRev.DefaultView;
                    gridView3.OptionsBehavior.Editable = false;
                    //gridView2.Columns["ProfileID"].Visible = false;
                    gridView3.BestFitColumns();
                }
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        //public void setDBComboBoxBank()
        //{
        //    try
        //    {
        //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

        //        DataTable Dt;

        //        using (var ds = new System.Data.DataSet())
        //        {
        //            using (SqlDataAdapter ada = new SqlDataAdapter("SELECT BankShortCode,BankName FROM Collection.tblBank ORDER BY BankName", Logic.ConnectionString))
        //            {
        //                ada.SelectCommand.CommandTimeout = 0;

        //                ada.Fill(ds, "table");
        //            }

        //            Dt = ds.Tables[0];
        //        }

        //        Common.setComboList(cboBank, Dt, "BankShortCode", "BankName");

        //        Common.setCheckEdit(cboBankEdt, Dt, "BankShortCode", "BankName");

        //        cboBank.SelectedIndex = -1;
        //    }
        //    finally
        //    {
        //        SplashScreenManager.CloseForm(false);
        //    }

        //}

        public void setDBComboBoxBranch()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                DataTable Dt;

                string values = string.Empty;

                object[] lol = cboBankEdt.EditValue.ToString().Split(',');

                int i = 0;

                foreach (object obj in lol)
                {
                    values += string.Format("'{0}'", obj.ToString().Trim());

                    if (i + 1 < lol.Count())
                        values += ",";
                    ++i;
                }

                using (var ds = new System.Data.DataSet())
                {
                    string query = string.Format("SELECT BranchCode, BankName +','+BranchName AS BranchName FROM Collection.tblBankBranch INNER JOIN Collection.tblBank ON Collection.tblBank.BankShortCode = Collection.tblBankBranch.BankShortCode WHERE Collection.tblBank.BankShortCode in ({0}) ORDER BY BranchName", values);

                    using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                    {
                        ada.SelectCommand.CommandTimeout = 0;

                        ada.Fill(ds, "table");
                    }

                    Dt = ds.Tables[0];


                }
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    gridControl2.DataSource = Dt.DefaultView;
                    gridView2.OptionsBehavior.Editable = false;
                    //gridView2.Columns["ProfileID"].Visible = false;

                    gridView2.BestFitColumns();

                }
                //Common.setComboList(cboBranch, Dt, "BranchCode", "BranchName");

                //Common.setCheckEdit(cboBranchesEdt, Dt, "BranchCode", "BranchName");

                //cboBranch.SelectedIndex = -1;
                //selection.ClearSelection();

                if (isBankGrid)
                {
                    selection = new GridCheckMarksSelection(gridView2, ref label7);
                    selection.CheckMarkColumn.VisibleIndex = 0;
                    isBankGrid = false;
                }
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void cboAgencyTest_EditValueChanged(object sender, EventArgs e)
        {
            if (cboAgencyTest.EditValue != null && !string.IsNullOrEmpty(cboAgencyTest.EditValue.ToString()))
            {
                setDBComboBoxReveneu();
            }
        }

        void cboBankEdt_EditValueChanged(object sender, EventArgs e)
        {
            if (cboBankEdt.EditValue != null && !string.IsNullOrEmpty(cboBankEdt.EditValue.ToString()))
            {
                setDBComboBoxBranch();
            }
        }

        void setClear()
        {
            txtName.Text = String.Empty;
            cboBankEdt.EditValue = null; //cboBankEdt.RefreshEditValue();
            cboAgencyTest.EditValue = null; //cboAgencyTest.RefreshEditValue();
            
        }

        private void setReload()
        {

            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {

                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT Name,ProfileID FROM Receipt.tblProfile ", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    gridControl1.DataSource = dt.DefaultView;
                    gridView1.OptionsBehavior.Editable = false;
                    gridView1.Columns["ProfileID"].Visible = false;
                    gridView1.BestFitColumns();
                }



            }

        }

        protected bool EditRecordMode()
        {
            bool bResponse = false;

            GridView view = (GridView)gridControl1.FocusedView;

            if (view != null)
            {
                DataRow dr = view.GetDataRow(view.FocusedRowHandle);

                if (dr != null)
                {
                    ID = Convert.ToInt32(dr["ProfileID"]);
                    bResponse = FillField(Convert.ToInt32(dr["ProfileID"]));
                }
                else
                {
                    Common.setMessageBox("No Record is seleected", "", 1);
                    bResponse = false;
                }
            }
            return bResponse;
        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit

            DataTable dts = (new Logic()).getSqlStatement((String.Format("SELECT Name,Agency,Revenue,Bank,BankBranch FROM Receipt.tblProfileCriteria INNER JOIN Receipt.tblProfile ON Receipt.tblProfile.ProfileID = Receipt.tblProfileCriteria.ProfileID WHERE Receipt.tblProfileCriteria.ProfileID  ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;

                txtName.Text = dts.Rows[0]["Name"].ToString();

                txtName.Enabled = false;

                //if (!string.IsNullOrEmpty(dts.Rows[0]["Agency"].ToString()))
                //{
                //    //cboAgencyTest.SetEditValue(dts.Rows[0]["Agency"].ToString());
                //    removeCombinedFlags(cboAgencyTest.Properties);
                //}

                //if (!string.IsNullOrEmpty(dts.Rows[0]["Revenue"].ToString()))
                //{
                //    cboRevenueEdt.SetEditValue(dts.Rows[0]["Revenue"].ToString());
                //}

                //if (!string.IsNullOrEmpty(dts.Rows[0]["Bank"].ToString()))
                //{
                //    cboBankEdt.SetEditValue(dts.Rows[0]["Bank"].ToString());
                //}

                //if (!string.IsNullOrEmpty(dts.Rows[0]["BankBranch"].ToString()))
                //{
                //    cboBranchesEdt.SetEditValue(dts.Rows[0]["BankBranch"].ToString());
                //}
            }
            else
                bResponse = false;

            return bResponse;
        }

        //Traverse through items and remove those that correspond to bitwise combinations of simple flags.
        private void removeCombinedFlags(RepositoryItemCheckedComboBoxEdit ri)
        {
            for (int i = ri.Items.Count - 1; i > 0; i--)
            {
                Enum val1 = ri.Items[i].Value as Enum;
                for (int j = i - 1; j >= 0; j--)
                {
                    Enum val2 = ri.Items[j].Value as Enum;
                    if (val1.HasFlag(val2))
                    {
                        ri.Items.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
