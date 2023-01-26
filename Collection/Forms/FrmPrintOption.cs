using Collection.Classess;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Collection.Forms
{
    public partial class FrmPrintOption : Form
    {
        public static FrmPrintOption publicStreetGroup;

        protected TransactionTypeCode iTransType;

        public static FrmPrintOption publicInstance;

        protected bool boolIsUpdate;

        public FrmPrintOption()
        {
            InitializeComponent();

            publicInstance = this;

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            Load += OnFormLoad;

            btnSelect.Click += Bttn_Click;

            OnFormLoad(null, null);

        }

        public void setDBComboBox()
        {
            DataTable Dt;



            using (var ds = new System.Data.DataSet())
            {


                string query = "select *  from tblReceipOption";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboOption, Dt, "RecOptID", "Name");


            cboOption.SelectedIndex = -1;


        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnCancel.Image = MDIMain.publicMDIParent.i32x32.Images[9];
            btnSelect.Image = MDIMain.publicMDIParent.i32x32.Images[8];
            //btnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //btnSearch.Image = MDIMain.publicMDIParent.i32x32.Images[2];

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
                //label11.Visible = false;

                //txtPaymentRef.Visible = false;

                //groupControl2.Text = "Add New Record";

                //iTransType = TransactionTypeCode.New;

                //ShowForm();

                //clear();

                //groupControl2.Enabled = true;

                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";

                iTransType = TransactionTypeCode.Edit;

                //ShowForm();

                boolIsUpdate = true;

            }
            //else if (sender == tsbDelete)
            //{
            //    groupControl2.Text = "Delete Record Mode";
            //    iTransType = TransactionTypeCode.Delete;
            //    if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
            //    {
            //    }
            //    else
            //        tsbReload.PerformClick();
            //    boolIsUpdate = false;
            //}
            //else if (sender == tsbReload)
            //{
            //    iTransType = TransactionTypeCode.Reload;
            //    ShowForm();
            //}
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            setDBComboBox();
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == btnSelect)
            {
                if (cboOption.SelectedValue.ToString() == "1")
                {
                    //MDIMain.publicMDIParent.RemoveControls();
                    MDIMain.publicMDIParent.tableLayoutPanel2.Controls.Add((new FrmReceipts().panelContainer), 1, 0);
                }

            }

        }


    }
}
