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
using EasyReg.Class;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraLayout.Customization;
using DevExpress.XtraLayout.Utils;

namespace EasyReg.Forms
{
    public partial class FrmRenew : Form
    {
        EasyRegService.EasyRegService EasyServices = new EasyReg.EasyRegService.EasyRegService();

        RepositoryItemMemoEdit repMemo = new RepositoryItemMemoEdit();

        private bool boolIsUpdate;

        protected int ID;

        public static FrmRenew publicStreetGroup;

        protected TransactionTypeCode iTransType;

        DataTable dt = new DataTable();

        SqlDataAdapter adp;

        public FrmRenew()
        {
            InitializeComponent();

            dtpIssues.CustomFormat = "dd/MM/yyyy";
            dtpIssues.Format = DateTimePickerFormat.Custom;
            dtpExpiry.CustomFormat = "dd/MM/yyyy";
            dtpExpiry.Format = DateTimePickerFormat.Custom;

            lkNext.Click += lkNext_Click;

            setImages();

            publicStreetGroup = this;

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            OnFormLoad(null, null);
        }

        void lkNext_Click(object sender, EventArgs e)
        {
            if (dtpIssues.Value >= dtpExpiry.Value)
            {
                Common.setMessageBox("Issue date must be less than expiry date", Program.ApplicationName, 1); return;
                //
            }
            else if (dtpIssues.Value.Date.Year >= dtpExpiry.Value.Date.Year)
            {
                Common.setMessageBox("Issue year must be less than expiry year", Program.ApplicationName, 1); return;
            }
            else if (txtPlate.Text == null || txtPlate.Text == "")
            {
                Common.setEmptyField("Vehicle Plate Number", Program.ApplicationName);
                txtPlate.Focus(); return;
            }
            else
            {
                string strconnect = Logic.ConnectionString;

                SqlConnection connect = new SqlConnection(strconnect);

                connect.Open();


                SqlCommand command = new SqlCommand("EasyReg_doCalculateLicenseFee", connect) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@LastIssueDate", SqlDbType.Date)).Value = dtpIssues.Value.Date.ToString("yyyy-MM-dd");
                command.Parameters.Add(new SqlParameter("@LastExpiryDate", SqlDbType.Date)).Value = dtpExpiry.Value.Date.ToString("yyyy-MM-dd");
                command.Parameters.Add(new SqlParameter("@VehiclePlateNumber", SqlDbType.VarChar)).Value = txtPlate.Text.Trim();

                using (DataSet ds = new DataSet())
                {
                    adp = new SqlDataAdapter(command);
                    adp.Fill(ds);
                    dt = ds.Tables[0];

                    connect.Close();
                    //return dt;
                }

                //DataTable dts = EasyServices.doCalculateLicenseFee(dtpIssues.Value.Date.ToString("yyyy-MM-dd"), dtpExpiry.Value.Date.ToString("yyyy-MM-dd"), txtPlate.Text.Trim());

                if (dt.Rows[0][0].ToString() == "00")
                {
                    //setReload();
                    //MDIMain.publicMDIParent.RemoveControls();
                    //MDIMain.publicMDIParent.tableLayoutPanel2.Controls.Add((new FrmMakePayment(dt).panelContainer), 1, 0);
                    gridControl1.DataSource = dt;
                    layoutView1.Columns["returnCode"].Visible = false;
                    layoutView1.Columns["vehicleRegListID"].Visible = false;
                    layoutView1.Columns["Address"].ColumnEdit = repMemo;
                    //layoutView1.Columns["Address"].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    gridControl1.Height = 283;
                    iTransType = TransactionTypeCode.Null;
                    ShowForm();
                    //MessageBox.Show(gridControl1.Size.ToString());

                }
                else if (dt.Rows[0][0].ToString() == "-1")
                {
                    Common.setMessageBox("This Vehicle has been renewed for this date before.", Program.ApplicationName, 1); return;
                }
                else if (dt.Rows[0][0].ToString() == "01")
                {
                    Common.setMessageBox("You have some pending transactions.", Program.ApplicationName, 1); return;
                }
            }
        }

        void OnFormLoad(object sender, EventArgs e)
        {

            repMemo.WordWrap = true;
            repMemo.ScrollBars = ScrollBars.Both;
            repMemo.ReadOnly = true;

            ShowForm();

            //setDBComboBox();

            //setDBComboBoxCat();

            //setReload();

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
                    splitContainer1.Panel2Collapsed = true;
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

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];
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

                //if (EditRecordMode())
                //{
                ShowForm();

                boolIsUpdate = true;
                //}
            }
            else if (sender == tsbDelete)
            {
                //groupControl2.Text = "Delete Record Mode";
                //iTransType = TransactionTypeCode.New;
                //if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
                //{
                //}
                //else
                //Update_Click();
                tsbReload.PerformClick();
                boolIsUpdate = false;

            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload;
                ShowForm();
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridControl1.Print();
        }


    }
}
