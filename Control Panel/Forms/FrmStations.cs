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
using Control_Panel.Class;
using System.Globalization;
using DevExpress.XtraGrid.Views.Grid;

namespace Control_Panel.Forms
{
    public partial class FrmStations : Form
    {
        public static FrmStations publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        public FrmStations()
        {
            InitializeComponent();

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            txtStation.LostFocus += txtStation_LostFocus;

            bttnUpdate.Click += bttnUpdate_Click;

            OnFormLoad(null, null);

            gridView1.DoubleClick += new EventHandler(gridView1_DoubleClick);
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        void bttnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStation.Text))
            {
                Common.setEmptyField("Station Name", Program.ApplicationName);
                txtStation.Focus(); return;
            }
            else
            {
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

                            using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblStation]([StationName])VALUES ('{0}');",txtStation.Text.Trim()), db, transaction))
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
                    Common.setMessageBox("Record has been Successfully Added", Program.ApplicationName, 1);
                    if (MessageBox.Show("Do you want to add another record?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        //bttnCancel.PerformClick();
                        tsbReload.PerformClick();
                    }
                    else
                    {
                        //bttnReset.PerformClick();
                        setReload(); Clear(); txtStation.Focus();
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
                            string query = String.Format("UPDATE [tblStation] SET [StationName]='{0}' where  StationID ='{1}'", txtStation.Text.Trim(), ID);

                            using (SqlCommand sqlCommand1 = new SqlCommand(query, db, transaction))
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
                    Common.setMessageBox("Changes in Record has been Successfully Saved.", Program.ApplicationName, 1);
                    setReload();
                    tsbReload.PerformClick();

                }
            }
        }

        void txtStation_LostFocus(object sender, EventArgs e)
        {
            txtStation.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtStation.Text);
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
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

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
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

        void OnFormLoad(object sender, EventArgs e)
        {
            ShowForm();
            //setDBComboBox();
            isFirst = false;
            setReload();
            //cboNature.KeyPress += cboNature_KeyPress;

        }

        private void setReload()
        {
            //connect.connect.Close();
            DataTable dt;
            //System.Data.DataSet ds;
            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                using (SqlDataAdapter ada = new SqlDataAdapter("select * from tblStation", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }
                //connect.connect.Close();
                dt = ds.Tables[0];
                gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["StationID"].Visible = false;
            gridView1.BestFitColumns();
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

        void Clear()
        {
            txtStation.Clear();
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
                    ID = Convert.ToInt32(dr["StationID"]);
                    bResponse = FillField(Convert.ToInt32(dr["StationID"]));
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

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from [tblStation] where StationID ='{0}'", fieldid))).Tables[0];

            if (dts != null)
            {
                bResponse = true;
                txtStation.Text = dts.Rows[0]["StationName"].ToString();

            }
            else
                bResponse = false;

            return bResponse;
        }

    }
}
