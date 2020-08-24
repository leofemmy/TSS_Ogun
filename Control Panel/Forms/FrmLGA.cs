using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using Control_Panel.Class;
using System.Globalization;

namespace Control_Panel.Forms
{
    public partial class FrmLGA : Form
    {
        //DBConnection connect = new DBConnection();
        //Methods extMethods = new Methods();
        public static FrmLGA publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;
        
        protected int ID;

        public FrmLGA()
        {
            InitializeComponent();
            //connect.ConnectionString();
        
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

            txtStreetGroup.LostFocus += txtStreetGroup_LostFocus;
        }

        void txtStreetGroup_LostFocus(object sender, EventArgs e)
        {
            txtStreetGroup.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtStreetGroup.Text);
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            
            setDBComboBox();
            
            setReload();
        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
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
                if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?",""))
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
            //tsbReload.PerformClick();
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

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnCancel)
            {
                tsbReload.PerformClick();
            }
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        private bool FillField(int fieldid)
        {
            bool bResponse = false;
            //load data from the table into the forms for edit

            string query = String.Format("select * from tblLga where LgaID = '{0}'", fieldid);

            DataTable dts = (new Logic()).getSqlStatement(query).Tables[0];

            //DataTable dts = extMethods.LoadData(String.Format("select * from tblLga where LgaID ='{0}'", fieldid));
            if (dts != null)
            {
                bResponse = true;
                txtStreetGroup.Text = dts.Rows[0]["LgaName"].ToString();
            }
            else
            bResponse = false;

            return bResponse;
        }

        private void Clear()
        {
            txtStreetGroup.Clear();
            setDBComboBox();
            txtStreetGroup.Focus();
        }

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("select LgaCode, LgaName  from tblLga ", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                this.gridControl1.DataSource = dt.DefaultView;
            }
            this.gridView1.OptionsBehavior.Editable = false;
            //this.gridView1.BestFitColumns();
            gridView1.Columns["LgaCode"].Width = 15;
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
                    ID = Convert.ToInt32(dr["LgaCode"]);
                    bResponse = FillField(Convert.ToInt32(dr["LgaCode"]));
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
            //DataTable Dt;
            //if ((new BusinessLogic()).LoadDataTable(ProgramLoadTableCode.TownFilterByState, Program.StateCode, out Dt))
            //    Common.setComboList(cbTown, Dt, "TownID", "TownName");
        }

        private void UpdateRecord()
        {
            try
            {
                if (txtStreetGroup.Text == "")
                {
                    Common.setEmptyField("Street Group", Program.ApplicationName);
                    txtStreetGroup.Focus();
                }
               else
                {
                    string streetGroup = txtStreetGroup.Text.Trim();

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

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblLga]([LgaName],[StateCode]) VALUES ('{0}','{1}');", txtStreetGroup.Text.Trim().ToUpperInvariant(), Program.stateCode), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback();
                            }
                            db.Close();
                        }
                       setReload();
                   Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);
                    
                            if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                            {
                                Clear();

                                bttnCancel.PerformClick(); 
                            }
                            else 
                            {
                                Clear(); txtStreetGroup.Focus();
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
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblLga] SET [LgaName]='{{0}}',[StateCode]='{{1}}' where  LgaID='{0}'", ID), txtStreetGroup.Text.Trim().ToUpperInvariant(), Program.stateCode), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (SqlException sqlError)
                            {
                                transaction.Rollback();
                            }
                            db.Close();
                        }

                            setReload();
                            Common.setMessageBox("Changes in record has been successfully saved.", Program.ApplicationName, 1);
                            Clear();

                            tsbReload.PerformClick();

                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(ex);
            }

        }

    }
}
