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
using Inventory.Reports;
using System.IO;
using System.Collections;

namespace Inventory.Forms
{
    public partial class FrmIssues : Form
    {
        ArrayList arrDataList = new ArrayList();

        public static FrmIssues publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected int ID;

        bool isFirst = true;

        bool isSecond = true;

        int qty, stnumb;

        string plate, strquery, path, paths, textfile;

        string numRange, filecontent, strRow,strColumn;

        Methods extMethods = new Methods();

        SqlConnection conn = null;

        SqlDataReader rdr = null;

        SqlCommand cmd = null;

        ListViewItem lvi;

        public FrmIssues()
        {
            InitializeComponent();

            groupControl2.Enabled = false;

            publicStreetGroup = this;

            setImages();

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            OnFormLoad(null, null);

            bttnList.Click += Bttn_Click;

            bttnUpdate.Click += Bttn_Click;

            cboType.SelectedIndexChanged += cboType_SelectedIndexChanged;

            cboSuffix.SelectedIndexChanged += new EventHandler(cboSuffix_SelectedIndexChanged);
        }

        void cboSuffix_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSuffix.SelectedValue != null || isSecond)
            {
                CountRec();
            }
        }

        void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.SelectedValue != null || isFirst)
            {
                if (cboType.SelectedValue.ToString() == "0005")
                {
                    // populate the prefix and suffix combo box

                    setDBComboBoxPrefix();

                    setDBComboBoxSurffix();

                    setDBComboBoxPlate();

                    groupBox1.Visible = false;

                    groupBox2.Visible = true;

                    //txtPlate.Visible = false;

                }
                else
                {
                    // populate the prefix and suffix combo box

                    setDBComboBoxPrefix();

                    setDBComboBoxSurffix();

                    groupBox1.Visible = true;

                    groupBox2.Visible = false;

                    //label8.Visible = false; txtPlate.Visible = false;
                }
            }
        }

        void rdbSingle_Click(object sender, EventArgs e)
        {
            label7.Visible = false; txtQty.Visible = false;

            groupBox1.Text = "Signle Issue";
        }

        void rdbBatch_Click(object sender, EventArgs e)
        {
            label7.Visible = true; txtQty.Visible = true;

            groupBox1.Text = "Batch Issues";
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbDelete.Image = MDIMain.publicMDIParent.i16x16.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            bttnList.Image = MDIMain.publicMDIParent.i32x32.Images[32];
            //bttnGenerate.Image = MDIMain.publicMDIParent.i32x32.Images[31];
            ////bttnReset.Image = MDIMain.publicMDIParent.i32x32.Images[8];
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
                //ShowForm();
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
                clear2();
                groupControl2.Text = " ";
                groupControl2.Enabled = false;
                //ShowForm();
            }
            //bttnReset.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //ShowForm();
            isFirst = false;
            setDBComboBox();
            setDBComboBox1();
            isSecond = false;


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

        void setDBComboBox1()
        {
            DataTable Dt;
            using (var ds = new System.Data.DataSet())
            {
                string query = "select *  from tblRegMLO";

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboOffices, Dt, "MLOID", "Name");

            cboOffices.SelectedIndex = -1;
        }

        void setDBComboBoxPlate()
        {
            DataTable Dt;
            using (var ds = new System.Data.DataSet())
            {
                string query = String.Format("SELECT PlateNo,PlateStockID FROM tblMLVPlateStock WHERE Issued<> 1 and PlateTypeID = '{0}' ", cboType.SelectedValue);
                
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboPlate, Dt, "PlateStockID", "PlateNo");

            cboPlate.SelectedIndex = -1;
        }

        void setDBComboBoxPrefix()
        {
            DataTable Dt;
            using (var ds = new System.Data.DataSet())
            {
                string query = String.Format("SELECT DISTINCT SUBSTRING(plateno,1,2) AS Preffix FROM tblMLVPlateStock WHERE Issued<> 1 and PlateTypeID = '{0}' ", cboType.SelectedValue);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboPrefix, Dt, "Preffix", "Preffix");

            cboPrefix.SelectedIndex = -1;
        }

        void setDBComboBoxSurffix()
        {
            DataTable Dt;
            using (var ds = new System.Data.DataSet())
            {
                string query = String.Format("SELECT DISTINCT SUBSTRING(PlateNo,6,3) AS Suffix FROM tblMLVPlateStock WHERE Issued<> 1 and PlateTypeID = '{0}' ", cboType.SelectedValue);

                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboSuffix, Dt, "Suffix", "Suffix");

            cboSuffix.SelectedIndex = -1;
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnList)
            {
                FunctionList();
            }
            else if (sender == bttnUpdate)
            {
                UpdateRecord();
            }
        }

        void FunctionList()
        {

            if (Common.IsNullOrEmpty(cboType, "Issue Plate Type", Program.ApplicationName))
            {
                cboType.Focus(); return;
            }
            else if (Common.IsNullOrEmpty(cboOffices, "MLA Office", Program.ApplicationName))
            {
                  cboOffices.Focus(); return;
            }
            else if (cboType.SelectedValue.ToString() == "0005")
            {
                if (Common.IsNullOrEmpty(cboPlate, "Plate number", Program.ApplicationName))
                {
                     cboPlate.Focus(); return;
                }
                else
                {
                    strquery = String.Format("SELECT PlateNo FROM tblMLVPlateStock WHERE Issued=0 AND PlateTypeID ='{0}' AND PlateNo= '{1}'", cboType.SelectedValue, cboPlate.Text.Trim());
                }

            }
            else
            {
                if (Common.IsNullOrEmpty(cboPrefix, "Plate Prefix ", Program.ApplicationName))
                {
                     cboPrefix.Focus(); return;
                }
                else if (Common.IsNullOrEmpty(cboSuffix, "Plate Suffix", Program.ApplicationName))
                {
                    cboSuffix.Focus(); return;
                }
                else if (Common.IsNullOrEmpty(txtQty,"Qty", Program.ApplicationName))
                {
                      txtQty.Focus(); return;
                }
                else
                {

                    strquery = String.Format("SELECT TOP {0} PlateNo FROM tblMLVPlateStock WHERE SUBSTRING(PlateNo,1,2) = '{1}' AND SUBSTRING(PlateNo,6,3) = '{2}' AND PlateTypeID = '{3}' AND Issued=0 ", Convert.ToInt32(txtQty.Text.Trim()), cboPrefix.SelectedValue, cboSuffix.SelectedValue, cboType.SelectedValue);
                }
            }

            DataTable Dt;
            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter(strquery, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            if (Dt != null && Dt.Rows.Count > 0)
            {
                if (cboType.SelectedValue.ToString() == "0005")
                {
                    string lol = string.Format("{0}", Dt.Rows[0][0]);
                    lvi = new ListViewItem(lol);
                    lvi.SubItems.Add("True");
                    lvi.SubItems.Add(cboType.SelectedValue.ToString());
                    listView1.Items.Add(lvi);
                    ;
                }
                else
                {
                    string lol = string.Format("{0} - {1}", Dt.Rows[0][0], Dt.Rows[Dt.Rows.Count - 1][0]);
                    lvi = new ListViewItem(lol);
                    lvi.SubItems.Add("True");
                    lvi.SubItems.Add(cboType.SelectedValue.ToString());
                    listView1.Items.Add(lvi);

                }
                arrDataList.Add(Dt);
                clear();
            }
            else
            {
                Common.setMessageBox("Either the Plate Number is already Issued or Invalid Please Confirm", Program.ApplicationName, 1);

                clear();
                return;
            }
        }

        void clear()
        {
            txtQty.Clear(); cboPrefix.SelectedIndex = -1; cboSuffix.SelectedIndex = -1; cboType.SelectedIndex = -1; 
            //txtPlate.Clear();
            cboPlate.SelectedIndex = -1;
        }

        void clear2()
        {
            txtQty.Clear(); cboPrefix.SelectedIndex = -1; cboSuffix.SelectedIndex = -1;

            //txtPlate.Clear();
            cboPlate.SelectedIndex = -1;            cboType.SelectedIndex = -1; cboOffices.SelectedIndex = -1;

            listView1.Items.Clear();

        }

        //void GetLasFirst()
        //{
        //    string numRange = String.Format("{0}-{1}", listView1.Items[0].Text, listView1.Items[listView1.Items.Count - 1].Text);

        //    MessageBox.Show(numRange);

        //    //string test = CryptorEngine.Encrypt(numRange, true);


        //    //MessageBox.Show(test);

        //    //MessageBox.Show(listView1.Items[listView1.Items.Count - 1].Text);
        //    ////test listview count and colums
        //    //for (int i = 0; i < listView1.Items.Count; i++)
        //    //{
        //    //    MessageBox.Show(listView1.Items[i].Text);
        //    //}

        //}

        void RefNumber()
        {
            string refnum = Methods.generateRandomString(8);

            txtRef.Text = String.Format("{0}", refnum);

        }

        void UpdateRecord()
        {
            //chech output type before update records

            if (rdgOut.SelectedIndex == -1)
            {
                Common.setEmptyField("Output Type", Program.ApplicationName);
                rdgOut.Focus(); return;
            }
            else
            {
                DataTable Dtt = new DataTable("ViewReport");
                Dtt.BeginInit();
                Dtt.Columns.Add("PlateRange");
                Dtt.Columns.Add("RefNum");
                Dtt.Columns.Add("PlateType");
                Dtt.Columns.Add("DateIssued");
                Dtt.Columns.Add("BatchCode");
                Dtt.EndInit();

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    Dtt.Rows.Add(new object[] { listView1.Items[i].Text, txtRef.Text.Trim(), listView1.Items[i].SubItems[2].Text, dtpIssued.Value.Date.ToString("yyyy-MM-dd"), EncDec.EncryptString(listView1.Items[i].Text, "12345") });
                }

                DataSet Ds = new DataSet("Issues");

                Ds.Tables.Add(Dtt);

                //Ds.WriteXmlSchema(@"C:/Issues.xml");

                //update records here
                using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                {
                    SqlTransaction transaction;

                    db.Open();

                    transaction = db.BeginTransaction();

                    try
                    {
                        ////update plate stock record

                        foreach (Object item in arrDataList)//get item from arraylist
                        {
                            if (item == null) continue;

                            DataTable lol = item as DataTable;

                            foreach (DataRow Dr in lol.Rows)
                            {
                                //object plate = Dr[0];

                                using (SqlCommand sqlCommand1 = new SqlCommand(String.Format(String.Format("UPDATE [tblMLVPlateStock] SET [Issued]='{{0}}',[IssuedRef]='{{1}}',[IssuedDate]='{{2}}',[MLOID]='{{3}}' where  [PlateNo] ='{0}' ", Dr[0]), true, txtRef.Text.Trim(), dtpIssued.Value.Date.ToString("yyyy-MM-dd hh:mm:ss"), cboOffices.SelectedValue), db, transaction))
                                {
                                    sqlCommand1.ExecuteNonQuery();
                                }
                            }
                        }


                        ////insert into issued history records

                        for (int i = 0; i < listView1.Items.Count; i++)
                        {
                            using (SqlCommand sqlCommand = new SqlCommand(String.Format("INSERT INTO [tblIssuedHistory]([RefNumber],[Range],[MLOID],[PlateType])VALUES ('{0}','{1}','{2}','{3}');", txtRef.Text.Trim(), listView1.Items[i].Text, cboOffices.SelectedValue, listView1.Items[i].SubItems[2].Text), db, transaction))
                            {
                                sqlCommand.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (SqlException sqlError)
                    {
                        transaction.Rollback();
                    }
                    db.Close();
                }
                //print issued Report
                if (rdgOut.SelectedIndex == 1)
                {
                    RepIssued report = new RepIssued { DataSource = Ds };

                    report.ShowPreviewDialog();
                }
                else
                {
                    string str = MosesClassLibrary.Serialization.Serialization.SerializeObjectToXML(Dtt);
//                    object lop = MosesClassLibrary.Serialization.Serialization.DeserializeXMLToObject(str, new DataTable().
//GetType());
//                    DataTable dty = lop as DataTable;
                    export(EncDec.EncryptString(str, "12345"));
                }

              

            }
            Common.setMessageBox(String.Format("Plate Number Issued to {0}  Motor License Office", cboOffices.Text), Program.ApplicationName, 1);
            tsbReload.PerformClick();

        }

        private void export(string filename)
        {
            //call file dialog function

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath;
            }

            paths = String.Format("{0}_{1:yyyy-MM-dd}", cboOffices.Text.Trim(), DateTime.Now);

            path = string.Format(@"{0}\{1}{2}", path.ToString(), paths.ToString(), ".txt");


            //call save dialog function
            using (SaveFileDialog saveFileDialogCSV = new SaveFileDialog { InitialDirectory = Application.ExecutablePath.ToString(), Filter = "TEXT files (*.txt)|*.txt|All files (*.*)|*.*", FilterIndex = 1, RestoreDirectory = true, FileName = path })
            {
                //call write function
                using (StreamWriter sw = new StreamWriter(saveFileDialogCSV.FileName.ToString(), false))
                {
                    sw.WriteLine(filename);
                    sw.Close();
                }
            }
        }

        void CountRec()
        {
            DataTable Dt = (new Logic()).getSqlStatement(String.Format("SELECT COUNT(*) as Remains FROM tblMLVPlateStock WHERE SUBSTRING(PlateNo,1,2)= '{0}' AND SUBSTRING(PlateNo,6,3) = '{1}' AND PlateTypeID = '{2}' AND Issued=0", cboPrefix.Text.Trim(), cboSuffix.Text.Trim(), cboType.SelectedValue)).Tables[0];

            if (Dt != null && Dt.Rows.Count > 0)
            {
                txtAva.Text = String.Format("{0}", Dt.Rows[0]["Remains"]);
            }

        }


    }
}
