using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraSplashScreen;
using System;
using System.Data;
using System.Windows.Forms;

namespace BankReconciliation.Forms
{
    public partial class FrmMatched : Form
    {
        public static FrmMatched publicStreetGroup;

        DataTable dts; DataTable dts2;

        DataTable dts3 = new DataTable();

        RepositoryItemCheckEdit RepositoryItemCheck = new RepositoryItemCheckEdit();

        RepositoryItemCheckEdit RepositoryItemCheckColl = new RepositoryItemCheckEdit();
        public FrmMatched()
        {
            InitializeComponent();
        }

        public FrmMatched(DataTable debit, DataTable credit)
        {
            InitializeComponent();

            dts = new DataTable(); dts.Clear();
            dts = debit;
            dts2 = new DataTable(); dts2.Clear(); dts2 = credit;
            dts3 = null;

            dts.Columns.Add("Check", typeof(bool));

            dts2.Columns.Add("Status", typeof(bool));


            Init();
        }

        GridHitInfo downHitInfo = null;
        void Init()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

            publicStreetGroup = this;


            //MainGrid.DataSource = MasterDs.Tables[0];
            //gridview.PopulateColumns();

            //gridview.Columns["ID"].VisibleIndex = -1;
            //gridview.Columns["FLAG"].VisibleIndex = -1;

            //DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit selectnew = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            //gridview.Columns["ColName"].ColumnEdit = selectnew;
            //selectnew.NullText = "";
            //selectnew.ValueChecked = "Y";
            //selectnew.ValueUnchecked = "N";
            //selectnew.ValueGrayed = "-";

            RepositoryItemCheck.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            RepositoryItemCheck.ValueChecked = "true";
            RepositoryItemCheck.ValueUnchecked = "false";
            RepositoryItemCheck.RadioGroupIndex = 1;
            RepositoryItemCheck.BeginInit();

            RepositoryItemCheckColl.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            RepositoryItemCheckColl.ValueChecked = "true";
            RepositoryItemCheckColl.ValueUnchecked = "false";
            RepositoryItemCheckColl.RadioGroupIndex = 2; RepositoryItemCheckColl.BeginInit();

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            OnFormLoad(null, null);

            RepositoryItemCheck.EndInit();

            RepositoryItemCheckColl.EndInit();

            gridView1.CellValueChanged += GridView1_CellValueChanged;

            SplashScreenManager.CloseForm(false);
        }

        private void GridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.Name != "Active")
                return;

            //var person = gridView1.GetFocusedRow() as Person;
            //person.Active = Convert.ToBoolean(e.Value);
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
            Fillgrid();

        }

        public void Fillgrid()
        {
            //gridControl3.DataSource = dts3;


            //DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit selectnew = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            //gridview.Columns["ColName"].ColumnEdit = selectnew;
            //selectnew.NullText = "";
            //selectnew.ValueChecked = "Y";
            //selectnew.ValueUnchecked = "N";
            //selectnew.ValueGrayed = "-";

            //RepositoryItemCheck.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            //RepositoryItemCheck.ValueChecked = "true";
            //RepositoryItemCheck.ValueUnchecked = "false";
            //RepositoryItemCheck.RadioGroupIndex = 1;
            //RepositoryItemCheck.BeginInit();

            //RepositoryItemCheck

            gridControl1.DataSource = dts;
            gridView1.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView1.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView1.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView1.Columns["Bsid"].Visible = false;
            gridView1.Columns["Amount"].SummaryItem.FieldName = "Amount";
            gridView1.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:###,###,###,##0.00##;(###,###,###,##0.00##)}";

            gridView1.Columns["Check"].ColumnEdit = RepositoryItemCheck;
            RepositoryItemCheck.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            RepositoryItemCheck.ValueChecked = "true";
            RepositoryItemCheck.ValueUnchecked = "false";
            RepositoryItemCheck.ValueGrayed = "-";

            gridView1.Columns["Check"].Visible = true;
            gridView1.Columns["Check"].VisibleIndex = 0;
            gridView1.Columns["Check"].OptionsColumn.AllowEdit = true;
            gridView1.Columns["Check"].UnboundType = DevExpress.Data.UnboundColumnType.Boolean;

            gridView1.Columns["Date"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["Amount"].OptionsColumn.AllowEdit = false;


            gridView1.OptionsView.ShowFooter = true;
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.BestFitColumns();

            gridControl2.DataSource = dts2;
            gridView2.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gridView2.Columns["Amount"].DisplayFormat.FormatString = "n2";
            gridView2.Columns["Date"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView2.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";
            gridView2.Columns["Amount"].SummaryItem.FieldName = "Amount";
            gridView2.Columns["Amount"].SummaryItem.DisplayFormat = "Total = {0:###,###,###,##0.00##;(###,###,###,##0.00##)}";

            gridView2.Columns["Status"].ColumnEdit = RepositoryItemCheckColl;

            RepositoryItemCheckColl.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            RepositoryItemCheckColl.ValueChecked = "true";
            RepositoryItemCheckColl.ValueUnchecked = "false";
            RepositoryItemCheckColl.ValueGrayed = "-";


            gridView2.Columns["Status"].Visible = true;
            gridView2.Columns["Status"].VisibleIndex = 0;

            gridView2.Columns["Status"].OptionsColumn.AllowEdit = true;
            gridView2.Columns["Status"].UnboundType = DevExpress.Data.UnboundColumnType.Boolean;

            gridView2.Columns["Date"].OptionsColumn.AllowEdit = false;
            gridView2.Columns["Amount"].OptionsColumn.AllowEdit = false;


            gridView2.OptionsView.ShowFooter = true;
            gridView2.OptionsView.ColumnAutoWidth = false;
            gridView2.BestFitColumns();
            //gridView1.OptionsBehavior.Editable = false;
            //gridView2.OptionsBehavior.Editable = false;

        }


    }
}
