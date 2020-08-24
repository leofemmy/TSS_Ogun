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

namespace Control_Panel.Forms
{
    public partial class FrmState : Form
    {
        public static FrmState publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        public static string CallOption;

        bool isFirst = true;

        public FrmState()
        {
            InitializeComponent();

            publicStreetGroup = this;

            //if (CallOption == "1")
            //{
            //}
            //else
            //{  }

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            Load += OnFormLoad;

            bttnCancel.Click += Bttn_Click;

            //bttnReset.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            OnFormLoad(null, null);
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
                Common.setMessageBox("Please Wait Quting Application, and start again", Program.ApplicationName, 1);

                Application.Exit();

                
            }
            else if (sender == tsbNew)
            {
                //groupControl2.Text = "Add New Record";
                iTransType = TransactionTypeCode.New;
                ShowForm();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                //groupControl2.Text = "Edit Record Mode";
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

        void OnFormLoad(object sender, EventArgs e)
        {
            ////test for default state here
            //if (Program.stateCode != "" || Program.stateCode != null)
            //{
            //    Common.setMessageBox("Default State have Been Set", Program.ApplicationName, 1);

            //    if (MessageBox.Show("Do you want to set another satet as default ?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            //    {
            //        Close();

            //    }
            //    else
            //    {
            ShowForm();
            setDBComboBox();
            isFirst = false;
            //    }
            //}


            //LoadState();
            //setDBComboBoxTn();

            //setReload();
            //cboBank.KeyPress += cboBank_KeyPress;
            //cboBranch.KeyPress += cboBranch_KeyPress;
            //cboBank.SelectedIndexChanged += cboBank_SelectedIndexChanged;
            //cboBank_SelectedIndexChanged(null, null);

        }

        public void setDBComboBox()
        {
            DataTable Dt;

            //connect.connect.Close();

            using (var ds = new System.Data.DataSet())
            {
                //connect.connect.Open();
                string query = "select  StateCode,StateName from tblState";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboState, Dt, "StateCode", "StateName");

            cboState.SelectedIndex = -1;


        }

        public void RefreshForm()
        {
            NavBars.ToolStripEnableDisableControls(toolStrip, Tag as String);
        }

        void LoadState()
        {
            if (Program.stateCode != "" || Program.stateCode != null)
            {
                Common.setMessageBox("Default State have Been Set", Program.ApplicationName, 1);

                if (MessageBox.Show("Do you want to set another satet as default ?", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    Close();

                }
                else
                {
                    //continue;
                    return;
                }
            }
        }

        void Bttn_Click(object sender, EventArgs e)
        {

            if (sender == bttnUpdate)
            {
                reStartState(); UpdateRecord();

                Common.setMessageBox("Default state change", Program.ApplicationName, 1);

                Common.setMessageBox("Please Wait Quting Application, and start again", Program.ApplicationName, 1);

                Application.Exit();

                //tsbClose.PerformClick();
            }
            else if (sender == bttnCancel)
            {
                tsbReload.PerformClick();
            }
        }

        private void UpdateRecord()
        {
            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();
                try
                {
                    //MessageBox.Show(MDIMain.stateCode);
                    //fieldid
                    using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("UPDATE tblState SET Flag=1 where  StateCode ='{0}'", cboState.SelectedValue), db, transaction))
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

            

        }

        void reStartState()
        {

            using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
            {
                SqlTransaction transaction;

                db.Open();

                transaction = db.BeginTransaction();
                try
                {
                    //MessageBox.Show(MDIMain.stateCode);
                    //fieldid

                    using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("UPDATE tblState SET Flag=0 where  StateCode ='{0}'", Program.stateCode), db, transaction))
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

        }

    }
}
