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
using Inventory.Class;

namespace Inventory.Forms
{
    public partial class FrmFRSCReceive : Form
    {
        public static FrmFRSCReceive publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        int qty,stnumb,lastnumber,endNumb;

        string plate,refnum;

        Methods extMethods = new Methods();

        DataTable Dtt = new DataTable("DataList");
                
        public FrmFRSCReceive()
        {
            InitializeComponent();

            groupControl2.Enabled = false;

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            OnFormLoad(null, null);

            txtNUmPre.LostFocus += txtNUmPre_LostFocus;

            txtNumSuff.LostFocus += txtNumSuff_LostFocus;

            txtStart.LostFocus += txtStart_LostFocus;

            txtPlateNumb.LostFocus += txtPlateNumb_LostFocus;

            bttnList.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            bttnUpdate2.Click += Bttn_Click;

            bttnList2.Click += Bttn_Click;

            cboType.SelectedIndexChanged += cboType_SelectedIndexChanged;

            //create an offline data table
            Dtt.BeginInit();
            Dtt.Columns.Add("PlateType");
            Dtt.Columns.Add("PlateNo");
            Dtt.Columns.Add("Qty");
            Dtt.Columns.Add("PlateCode");
            Dtt.Columns.Add("RefNumber");
            Dtt.Columns.Add("Prefix");
            Dtt.Columns.Add("Suffix");
            Dtt.Columns.Add("Start");
            Dtt.EndInit();

        }

        void txtPlateNumb_LostFocus(object sender, EventArgs e)
        {
            txtPlateNumb.Text = txtPlateNumb.Text.Trim().ToUpper();
        }

        void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (cboType.SelectedValue != null || isFirst)
            {
                if (cboType.SelectedValue.ToString() == "0005")
                {
                    groupBox2.Visible = true;

                    groupBox1.Visible = false; listView1.Visible = false; 
                    
                    label13.Visible = false; 
                    //bttnUpdate.Visible=false;
                    
                }
                else
                {
                    groupBox2.Visible = false;

                    groupBox1.Visible = true;

                }
            }
        }

        void txtStart_LostFocus(object sender, EventArgs e)
        {
            if (txtStart.Text.Trim().Length > 3)
            {
                Common.setMessageBox("Start Number Can not be More Than three (3) digits", Program.ApplicationName, 2);
                txtStart.Clear();
                txtStart.Focus();
            }
            else
            {
                return;
            }
        }

        void txtNumSuff_LostFocus(object sender, EventArgs e)
        {
            txtNumSuff.Text = txtNumSuff.Text.Trim().ToUpper();
        }

        void txtNUmPre_LostFocus(object sender, EventArgs e)
        {
            txtNUmPre.Text = txtNUmPre.Text.Trim().ToUpper();
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            bttnList.Image = MDIMain.publicMDIParent.i32x32.Images[32];
            bttnList2.Image = MDIMain.publicMDIParent.i32x32.Images[32];
            bttnUpdate2.Image = MDIMain.publicMDIParent.i32x32.Images[7];
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
                groupControl2.Enabled = true;
                iTransType = TransactionTypeCode.New;
                RefNumber();
                boolIsUpdate = false;
            }
            else if (sender == tsbEdit)
            {
                groupControl2.Text = "Edit Record Mode";
                groupControl2.Enabled = true;
               
                iTransType = TransactionTypeCode.Edit;
                //if (EditRecordMode())
                //{
                //    //ShowForm();
                //    boolIsUpdate = true;
                //}
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
                groupControl2.Enabled = false;
                groupControl2.Text = " ";
                gridControl1.DataSource = null;
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //ShowForm();
            isFirst = false;
            //setReload();
            setDBComboBox();

        }

        public void setDBComboBox()
        {
            DataTable Dt;
            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT * FROM dbo.tblPlateType";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboType, Dt, "PlateCode", "Description");

            cboType.SelectedIndex = -1;

        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnList)
            {
                PopulateList(); 

            }
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
            else if (sender == bttnUpdate2)
            {
                updateFancyplate();
            }
            else if (sender == bttnList2)
            {
                ListData(); 
            }
        }

        void updateFancyplate()
        {
            if (!boolIsUpdate)
            {
                if (cboType.SelectedValue.ToString() == "0005")
                {
                    if (Common.IsNullOrEmpty(txtFRSC, "Ref. Number", Program.ApplicationName))
                    {
                        txtFRSC.Focus(); return;
                    }
                    else if (Common.IsNullOrEmpty(txtPlateNumb,"Fancy Plate Number",Program.ApplicationName))
                    {
                     txtPlateNumb.Focus();return;
                    }
                    else
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            try
                            {
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblMLVPlateStock]([ReceiveRef],[PlateTypeID],[PlateNo],[ReceiveDate],[BatchNo],[DeliverDate])VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", refnum, cboType.SelectedValue, txtPlateNumb.Text.Trim().ToUpper(), dtpReceivedDate.Value.Date.ToString("yyyy-MM-dd hh:mm:ss"), txtFRSC.Text.Trim(), dtpDeliver.Value.Date.ToString("yyyy-MM-dd hh:mm:ss")), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
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
                    }

                    Common.setMessageBox("Record has been successfully added", Program.ApplicationName, 1);
                    clear();
                    tsbReload.PerformClick();
                }
            }
        }

        void PopulateList()
        {
            bool blStructure = false;


            checkEndNumber();

            if (Convert.ToInt32(txtEndNumb.Text) != lastnumber)
            {
                Common.setMessageBox("Invalid Date Entry, Please check Start and End Number", Program.ApplicationName, 2);
                txtStart.Focus(); return;
            }
            else
            {
              

                if (checkBox1.Checked)
                {
                    blStructure = true;
                }
                else
                {
                    blStructure = false;
                }

                listView1.Items.Clear();

                ListViewItem lvi = new ListViewItem(cboType.Text.Trim());
                lvi.SubItems.Add(txtNUmPre.Text.Trim());
                lvi.SubItems.Add(txtNumSuff.Text.Trim());
                lvi.SubItems.Add(txtQty.Text.Trim());
                lvi.SubItems.Add(txtStart.Text.Trim());
                lvi.SubItems.Add(blStructure.ToString());

                listView1.Items.Add(lvi);

                listView1.Visible = true; 
                //label13.Visible = true; 

                //call list data to gridview
                ListData();
            
            }
          
           
            //bttnUpdate.Visible = true;
        }

        void RefNumber()
        { 
           refnum =  Methods.generateRandomString(6);

        }

        void clear()
        {
            cboType.SelectedIndex = -1; txtNUmPre.Clear(); txtNumSuff.Clear(); txtQty.Clear(); 

            txtStart.Clear(); checkBox1.Checked = false; txtPlateNumb.Clear(); txtFRSC.Clear();

            txtEndNumb.Clear(); txtFRSC.Focus();

            listView1.Items.Clear(); listView1.Visible = false; 
            //bttnUpdate.Visible = false; label13.Visible = false;
            groupBox1.Visible = false; groupBox2.Visible = false;
        }

        void UpdateRecord()
        {
            //checkEndNumber();

            if (Dtt.Rows.Count > 0)
            {
                foreach (DataRow dr in Dtt.Rows)
                {
                    if (dr[3].ToString() == "0005")
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            try
                            {
                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblMLVPlateStock]([ReceiveRef],[PlateTypeID],[PlateNo],[ReceiveDate],[BatchNo],[DeliverDate])VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", refnum, dr[3].ToString(),dr[1].ToString(), dtpReceivedDate.Value.Date.ToString("yyyy-MM-dd hh:mm:ss"),dr[4].ToString(), dtpDeliver.Value.Date.ToString("yyyy-MM-dd hh:mm:ss")), db, transaction))

                                {
                                    sqlCommand1.ExecuteNonQuery();
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
                    }
                    else
                    {
                        using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                        {
                            SqlTransaction transaction;

                            db.Open();

                            transaction = db.BeginTransaction();

                            try
                            {
                                for (int i = Convert.ToInt32(dr[7].ToString()); i <= Convert.ToInt32(dr[2].ToString()) + Convert.ToInt32(dr[7].ToString()); i++)
                                {
                                    plate = dr[5].ToString() + string.Format("{0:000}", i) + dr[6].ToString();

                                    using (SqlCommand sqlCommand1 = new SqlCommand(String.Format("INSERT INTO [tblMLVPlateStock]([ReceiveRef],[PlateTypeID],[PlateNo],[ReceiveDate],[BatchNo],[DeliverDate])VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", refnum, dr[3].ToString(), plate, dtpReceivedDate.Value.Date.ToString("yyyy-MM-dd hh:mm:ss"), dr[4].ToString(), dtpDeliver.Value.Date.ToString("yyyy-MM-dd hh:mm:ss")), db, transaction))

                                    {
                                        sqlCommand1.ExecuteNonQuery();
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
                    }

                }
            }
            else
            {
                Common.setMessageBox("Sorry No Data to Update Table With, Try again", Program.ApplicationName, 2);
                return;
            }

                Common.setMessageBox("Record has been Successfully Update", Program.ApplicationName, 1);
                clear();                tsbReload.PerformClick();
    
        }

        void checkEndNumber()
        {
            

            for (int i = Convert.ToInt32(txtStart.Text.Trim()); i <= Convert.ToInt32(txtQty.Text.Trim()) + Convert.ToInt32(txtStart.Text.Trim()); i++)
            {
                if (i == (Convert.ToInt32(txtQty.Text.Trim()) + Convert.ToInt32(txtStart.Text.Trim())))
                {
                    lastnumber = i;
                }
            }
       
        }

        void ListData()
        {
            //checkEndNumber();
            if (cboType.SelectedValue.ToString() == "0005")
            {
                Dtt.Rows.Add(cboType.Text, txtPlateNumb.Text.Trim(), txtFQty.Text.Trim(), cboType.SelectedValue,txtFRSC.Text.Trim());
            }
            else
            {

                string firstplate = txtNUmPre.Text.Trim() + txtStart.Text.Trim() + txtNumSuff.Text.Trim();
                string lastplate= txtNUmPre.Text.Trim()+ lastnumber +txtNumSuff.Text.Trim();

                string plates = String.Format("{0} - {1}", firstplate, lastplate);

                Dtt.Rows.Add(cboType.Text, plates,txtQty.Text.Trim(),cboType.SelectedValue,txtFRSC.Text.Trim(),txtNUmPre.Text.Trim(),txtNumSuff.Text.Trim(),txtStart.Text.Trim());

            }

            gridControl1.DataSource = Dtt.DefaultView;

            gridView1.OptionsBehavior.Editable = false;

            gridView1.Columns["PlateCode"].Visible = false;

            gridView1.BestFitColumns();

            strQuestion();


        }

        void strQuestion()
        {
            DialogResult result = MessageBox.Show("Do you Wish to Enter More Entries", Program.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                clear(); return;
            }
        }



    }
}
