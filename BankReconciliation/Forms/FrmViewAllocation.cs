using BankReconciliation.Class;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BankReconciliation.Forms
{
    public partial class FrmViewAllocation : Form
    {
        public static FrmViewAllocation publicStreetGroup;

        DataTable dts; DataTable dts2;
        RepositoryItemSearchLookUpEdit repComboboxRevenue = new RepositoryItemSearchLookUpEdit();
        RepositoryItemGridLookUpEdit repComboLookBox = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit repComboLookBoxCredit = new RepositoryItemGridLookUpEdit();

        public FrmViewAllocation()
        {
            InitializeComponent();
        }

        public FrmViewAllocation(DataTable debit, DataTable credit)
        {
            InitializeComponent();

            dts = new DataTable(); dts.Clear();

            dts2 = new DataTable(); dts2.Clear();

            dts = debit;
            dts2 = credit;

            //if (!dts.Columns.Contains("Description")) dts.Columns.Add("Description", typeof(String));
            //if (!dts2.Columns.Contains("Description")) dts2.Columns.Add("Description", typeof(String));

            //dts.AcceptChanges();
            //dts2.AcceptChanges();

            Init();
        }

        void Init()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            //btnPrint.Click += btnPrint_Click;

            SplashScreenManager.CloseForm(false);
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
            //bttnClose.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            //bttnSave.Image = MDIMains.publicMDIParent.i32x32.Images[7];

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
                //MDIMains.publicMDIParent.RemoveControls();
                this.Close();

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
            AddCombCredit();

            AddCombDebit();


            gridControl1.DataSource = dts;
            //gridView1.Columns["STATUS"].Visible = false;
            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView1.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView1.Columns["TransID"].ColumnEdit = repComboLookBox;
            gridView1.Columns["BSID"].Visible = false;
            gridView1.Columns["TransID"].Caption = "Description";
            gridView1.Columns["PayerName"].Caption = "Narration";
            //gridView1.Columns["TransID"].Visible = false;
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.BestFitColumns();

            gridControl2.DataSource = dts2;
            gridView2.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView2.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView2.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView2.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView2.Columns["TransID"].ColumnEdit = repComboLookBoxCredit;
            gridView2.Columns["BSID"].Visible = false;
            gridView2.Columns["TransID"].Caption = "Description";
            gridView2.Columns["PayerName"].Caption = "Narration";
            //gridView2.Columns["TransID"].Visible = false;
            gridView2.OptionsView.ColumnAutoWidth = false;
            gridView2.BestFitColumns();
            gridView1.OptionsBehavior.Editable = false; gridView2.OptionsBehavior.Editable = false;
            //gridControl2.DataSource = dts.Tables[2];
            //gridView2.Columns["STATUS"].Visible = false;
            //gridView2.Columns["psAmount"].DisplayFormat.FormatType = FormatType.Numeric;
            //gridView2.Columns["psAmount"].DisplayFormat.FormatString = "n2";
            //gridView2.Columns["psDate"].DisplayFormat.FormatType = FormatType.DateTime;
            //gridView2.Columns["psDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            //gridView2.OptionsView.ColumnAutoWidth = false;
            //gridView2.BestFitColumns();
        }

        void AddCombDebit()
        {
            try
            {
                DataTable dtsed = new DataTable();

                repComboLookBox.DataSource = null;
                dtsed.Clear();
                //System.Data.DataSet ds;
                using (var ds = new System.Data.DataSet())
                {
                    ds.Clear();
                    using (SqlDataAdapter ada = new SqlDataAdapter("SELECT Description,TransID FROM Reconciliation.tblTransDefinition WHERE Type='dr' AND IsActive=1", Logic.ConnectionString))
                    {
                        ada.Fill(ds, "table");
                    }

                    repComboLookBox.NullText = "select";

                    dtsed = ds.Tables[0];

                    if (dtsed != null && dtsed.Rows.Count > 0)
                    {
                        repComboLookBox.DataSource = dtsed;
                        repComboLookBox.DisplayMember = "Description";
                        repComboLookBox.ValueMember = "TransID";
                        repComboLookBox.AllowNullInput = DefaultBoolean.True;

                        //if (!boolIsUpdate2)
                        //{
                        repComboLookBox.PopulateViewColumns();
                        var view = repComboLookBox.View;
                        view.Columns["TransID"].Visible = false;
                        //}


                        repComboLookBox.AutoComplete = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }

        void AddCombCredit()
        {
            try
            {
                DataTable dtse = new DataTable();

                repComboLookBoxCredit.DataSource = null;

                dtse.Clear();
                //System.Data.DataSet ds;
                using (var dsed = new System.Data.DataSet())
                {
                    dsed.Clear();

                    using (SqlDataAdapter adas = new SqlDataAdapter("SELECT Description,TransID  FROM Reconciliation.tblTransDefinition WHERE Type='cr' AND IsActive=1", Logic.ConnectionString))
                    {
                        adas.Fill(dsed, "table");

                    }

                    repComboLookBoxCredit.NullText = "select";

                    dtse = dsed.Tables[0];

                    if (dtse != null && dtse.Rows.Count > 0)
                    {
                        repComboLookBoxCredit.DataSource = dtse;
                        repComboLookBoxCredit.DisplayMember = "Description";
                        repComboLookBoxCredit.ValueMember = "TransID";
                        repComboLookBoxCredit.AllowNullInput = DefaultBoolean.True;
                        //Autocomplete on all values

                        repComboLookBoxCredit.PopulateViewColumns();
                        var view = repComboLookBoxCredit.View;
                        view.Columns["TransID"].Visible = false;

                        repComboLookBoxCredit.AutoComplete = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Tripous.Sys.ErrorBox(String.Format("{0}{1}", ex.Message, ex.StackTrace)); return;
            }
        }


    }
}
