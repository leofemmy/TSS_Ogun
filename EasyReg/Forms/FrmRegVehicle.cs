using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using TaxSmartSuite.Class;
using DevExpress.XtraEditors;
using ColorAsKnownColor;
using EasyReg.Class;
using DevExpress.XtraGrid.Views.Grid;

namespace EasyReg.Forms
{
    public partial class FrmRegVehicle : Form
    {
        private bool boolIsUpdate;

        protected int ID;

        EasyRegService.EasyRegService EasyServices = new EasyReg.EasyRegService.EasyRegService();

        public static FrmRegVehicle publicStreetGroup;

        protected TransactionTypeCode iTransType;

        public FrmRegVehicle()
        {
            InitializeComponent();
          
            setImages();

            publicStreetGroup = this;

            ToolStripEvent();

            iTransType = TransactionTypeCode.Null;

            OnFormLoad(null, null);

            colorEdit1.Properties.ShowCustomColors = false;
            
            colorEdit1.Properties.ShowSystemColors = false;
            
            colorEdit1.Properties.ShowWebColors = true;

            txtEmail.LostFocus += txtEmail_LostFocus;

            txtPlate.LostFocus += txtPlate_LostFocus;

            //bttnClose.Click += bttnClose_Click;

            //bttnUpdate.Click += bttnUpdate_Click;
            //call gridview doubclick function
            gridView1.DoubleClick += gridView1_DoubleClick;
        }

        void gridView1_DoubleClick(object sender, EventArgs e)
        {
            tsbEdit.PerformClick();
        }

        void OnFormLoad(object sender, EventArgs e)
        {
            //isFirst = false;

            //isSecond = false;

            //isThird = false;

            //isAvailable = false;

            ShowForm();

            setDBComboBox();

            setDBComboBoxCat();

            setReload();

            //setDBComboBoxMake();

            //setDBComboBoxCat();

            //setDBComboBoxType();

            

            //setDBComboBox2();

            //setDBComboBoxSeries();

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

        void Update_Click()
        {
            if (txtSurname.Text == null || txtSurname.Text == "")
            {
                Common.setEmptyField("Surname", Program.ApplicationName);
                txtSurname.Focus(); return;
            }
            else if (txtOther.Text == null || txtOther.Text == "")
            {
                Common.setEmptyField("Other Name", Program.ApplicationName);
                txtOther.Focus(); return;
            }
            else if (txtHouse.Text == null || txtHouse.Text == "")
            {
                Common.setEmptyField("ATP or House Number", Program.ApplicationName);
                txtHouse.Focus(); return;
            }
            else if (txtStreet.Text == null || txtStreet.Text == "")
            {
                Common.setEmptyField("Street Name", Program.ApplicationName);
                txtStreet.Focus(); return;
            }
            else if (txtTown.Text == null || txtTown.Text == "")
            {
                Common.setEmptyField("Town Name", Program.ApplicationName);
                txtTown.Focus(); return;
            }
            else if (cboState.SelectedIndex == -1)
            {
                Common.setEmptyField("State Name", Program.ApplicationName);
                cboState.Focus(); return;
            }
            else if (txtPhone.Text == null || txtPhone.Text == "")
            {
                Common.setEmptyField("Phone Number", Program.ApplicationName);
                txtPhone.Focus(); return;
            }
            else if (txtPlate.Text == null || txtPlate.Text == "")
            {
                Common.setEmptyField("Vehicle Plate Number", Program.ApplicationName);
                txtPlate.Focus(); return;
            }
            else if (txtMake.Text == null || txtMake.Text == "")
            {
                Common.setEmptyField("Vehicle Make", Program.ApplicationName);
                txtMake.Focus(); return;
            }
            else if (txtChasis.Text == null || txtChasis.Text == "")
            {
                Common.setEmptyField("Vehicle chasis Number", Program.ApplicationName);
                txtChasis.Focus(); return;
            }
            else if (txtEngine.Text == null || txtEngine.Text == "")
            { 
            Common.setEmptyField("Vehicle Engine Number",Program.ApplicationName);
                txtEngine.Focus();return;
            }
            else if (cboCategory.SelectedIndex==-1)
            {
            Common.setEmptyField("Vehicle Category", Program.ApplicationName);
                cboCategory.Focus();return;
            }
            else if (cboPlace.SelectedIndex==-1)
            {
            Common.setEmptyField("Last Place of Registration",Program.ApplicationName);
                cboPlace.Focus();return;
            }
            else if (cboType.SelectedIndex==-1)
            {
            Common.setEmptyField("Vehicle Type",Program.ApplicationName);
                cboType.Focus();return;
            }
            else
            {
                //string strconnect = Logic.ConnectionString;

                //SqlConnection connect = new SqlConnection(strconnect);

                //connect.Open();


                //SqlCommand command = new SqlCommand("EasyReg_doVehicleRegistration", connect) { CommandType = CommandType.StoredProcedure };
                if (!boolIsUpdate) //check edit mode of record before committe record
                { 
                    //new record insertion
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();

                        try
                        {
                            string inserveh = string.Format("INSERT INTO EasyReg_tblVehicleRegInfo( VehiclePlateNumber , VehicleChasisNumber , VehicleEngineNumber , VehicleColor , VehicleMake , VehicleTypeID , VehicleCapacityID , VehicleLastPlaceReg , OwnerSurName , OwnerOtherNames , OwnerHouseNumber , OwnerStreetName ,OwnerTown , OwnerState , OwnerTelephone , OwnerEmail, RegistrationDate )VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}');", txtPlate.Text.Trim(),txtChasis.Text.Trim(),txtEngine.Text.Trim(),colorEdit1.Text.Trim(),txtMake.Text.Trim(),cboType.SelectedValue,cboCategory.SelectedValue,cboPlace.Text,txtSurname.Text.Trim(),txtOther.Text.Trim(),txtHouse.Text.Trim(),txtStreet.Text.Trim(),txtTown.Text.Trim(),cboState.Text.Trim(),txtPhone.Text.Trim(),txtEmail.Text.Trim(),DateTime.Now.ToLongDateString());
                            using (SqlCommand sqlCommand2 = new SqlCommand(inserveh, db, transaction))
                            {
                                sqlCommand2.ExecuteNonQuery();
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
                else
                {
                    //update record here
                    using (SqlConnection db = new SqlConnection(Logic.ConnectionString))
                    {
                        SqlTransaction transaction;

                        db.Open();

                        transaction = db.BeginTransaction();
                        try
                        {
                            using (SqlCommand sqlCommand = new SqlCommand(String.Format(String.Format("UPDATE [EasyReg_tblVehicleRegInfo] SET VehiclePlateNumber ='{{0}}', VehicleChasisNumber='{{1}}' , VehicleEngineNumber='{{2}}' , VehicleColor ='{{3}}', VehicleMake ='{{4}}', VehicleTypeID='{{5}}' , VehicleCapacityID='{{6}}' , VehicleLastPlaceReg='{{7}}' , OwnerSurName='{{8}}' , OwnerOtherNames ='{{9}}', OwnerHouseNumber='{{10}}' , OwnerStreetName='{{11}}' ,OwnerTown='{{12}}' , OwnerState='{{13}}' , OwnerTelephone='{{14}}' , OwnerEmail='{{15}}', RegistrationDate ='{{16}}' where  [vehicleRegListID] ='{0}'", ID), txtPlate.Text.Trim(), txtChasis.Text.Trim(), txtEngine.Text.Trim(), colorEdit1.Text.Trim(), txtMake.Text.Trim(), cboType.SelectedValue, cboCategory.SelectedValue, cboPlace.Text, txtSurname.Text.Trim(), txtOther.Text.Trim(), txtHouse.Text.Trim(), txtStreet.Text.Trim(), txtTown.Text.Trim(), cboState.Text.Trim(), txtPhone.Text.Trim(), txtEmail.Text.Trim(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), db, transaction))

                            {
                                sqlCommand.ExecuteNonQuery();
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
            //DataTable dts = EasyServices.doVehicleRegistration(txtPlate.Text.Trim(),txtChasis.Text.Trim(),txtEngine.Text.Trim(),colorEdit1.Text,txtMake.Text.Trim(),cboType.SelectedValue,cboCategory.SelectedValue,cboPlace.Text,txtOther.Text.Trim().ToUpper(),txtSurname.Text.Trim().ToUpper(),txtHouse.Text.Trim(),txtStreet.Text.Trim(),txtTown.Text.Trim(),cboState.Text,txtPhone.Text,txtEmail.Text.Trim());

            //Class.RegData reg = new EasyReg.Class.RegData();
            //reg.Capacity = cboCategory.Text;
            //reg.Chasis = txtChasis.Text;
            //reg.Color = colorEdit1.Text;
            //reg.Date = DateTime.Now.ToLongDateString();
            //reg.Engine = txtEngine.Text;
            //reg.House = txtHouse.Text;
            //reg.Make = txtMake.Text;
            //reg.OtherName = txtOther.Text;
            //reg.State =cboState.Text;
            //reg.Street = txtStreet.Text;
            //reg.Surname = txtSurname.Text;
            //reg.Town = txtTown.Text;
            //reg.Plate = txtPlate.Text;

            //     if (dts.Rows[0][0].ToString() == "00")
            //    {
            //        //setReload();
            //         MDIMain.publicMDIParent.RemoveControls();
            //         MDIMain.publicMDIParent.tableLayoutPanel2.Controls.Add((new FrmRegSucess(reg).panelContainer),1,0);
                     
            //    }

                setReload();
                Common.setMessageBox("Record has been Successfully Update", Program.ApplicationName, 1);
            }
        }

        void txtPlate_LostFocus(object sender, EventArgs e)
        {
            txtPlate.Text = txtPlate.Text.Trim().ToUpper();
        }

        void txtEmail_LostFocus(object sender, EventArgs e)
        {
           if (!Common.IsValidEmail(txtEmail.Text))
            {
                Common.setMessageBox("Invalid Email Address", Program.ApplicationName, 2);
                txtEmail.Clear(); txtEmail.Focus(); return;
            }
        }

        void setDBComboBox()
        {
           DataTable Dt;
            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT * FROM EasyReg_tblVehicleTypes";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboType, Dt, "VehicleTypeID", "VehicleType");

            cboType.SelectedIndex = -1;

        }

        void setDBComboBoxCat()
        {
            DataTable Dt;
            using (var ds = new System.Data.DataSet())
            {
                string query = "SELECT * FROM EasyReg_tblVehicleCapacity";
                using (SqlDataAdapter ada = new SqlDataAdapter(query, Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                Dt = ds.Tables[0];
            }

            Common.setComboList(cboCategory, Dt, "VehicleCapacityID", "VehicleCapacity");

            cboCategory.SelectedIndex = -1;

        }

        private void colorEdit1_EditValueChanged(object sender, EventArgs e)
        {
            ColorEdit editor = (ColorEdit)sender;
            object value = editor.EditValue;
            if (ColorHelper.TryConvertToKnownColor(ref value))
                editor.EditValue = value;
        }

        private void setImages()
        {
            tsbNew.Image = MDIMain.publicMDIParent.i16x16.Images[5];
            tsbEdit.Image = MDIMain.publicMDIParent.i16x16.Images[6];
            tsbUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            tsbReload.Image = MDIMain.publicMDIParent.i16x16.Images[0];
            tsbClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];

            //bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            //bttnClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        void ToolStripEvent()
        {
            tsbClose.Click += OnToolStripItemsClicked;
            tsbNew.Click += OnToolStripItemsClicked;
            tsbEdit.Click += OnToolStripItemsClicked;
            tsbUpdate.Click += OnToolStripItemsClicked;
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
            else if (sender == tsbUpdate)
            {
                //groupControl2.Text = "Delete Record Mode";
                //iTransType = TransactionTypeCode.New;
                //if (MosesClassLibrary.Utilities.Common.AskQuestion("Deleting this record will delete attached record.\nDo you want to continue?", ""))
                //{
                //}
                //else
                Update_Click();
                tsbReload.PerformClick();
                boolIsUpdate = false;

            }
            else if (sender == tsbReload)
            {
                iTransType = TransactionTypeCode.Reload;
                ShowForm();
            }
            
        }

        private void setReload()
        {
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                using (SqlDataAdapter ada = new SqlDataAdapter("SELECT vehicleRegListID,VehiclePlateNumber,Name,VehicleEngineNumber,VehicleChasisNumber,RegistrationDate FROM dbo.EasyReg_vwVehicleInfo", Logic.ConnectionString))
                {
                    ada.Fill(ds, "table");
                }

                dt = ds.Tables[0];
                this.gridControl1.DataSource = dt.DefaultView;
            }
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["vehicleRegListID"].Visible = false;
            gridView1.BestFitColumns();
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
                    ID = Convert.ToInt32(dr["vehicleRegListID"]);

                    bResponse = FillField(Convert.ToInt32(dr["vehicleRegListID"]));
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

            DataTable dts = (new Logic()).getSqlStatement((String.Format("select * from EasyReg_vwVehicleInfo where vehicleRegListID ='{0}'", fieldid))).Tables[0];

            if (dts.Rows.Count == 0)
            {
                bResponse = false;
            }
            else
            {
                bResponse = true;

                txtSurname.Text = dts.Rows[0]["OwnerSurName"].ToString();
                txtOther.Text = dts.Rows[0]["OwnerOtherNames"].ToString();
                txtHouse.Text = dts.Rows[0]["OwnerHouseNumber"].ToString();
                txtStreet.Text = dts.Rows[0]["OwnerStreetName"].ToString();
                txtTown.Text = dts.Rows[0]["OwnerTown"].ToString();
                txtPhone.Text = dts.Rows[0]["OwnerTelephone"].ToString();
                txtEmail.Text = dts.Rows[0]["OwnerEmail"].ToString();
                cboState.Text = dts.Rows[0]["OwnerState"].ToString();
                txtPlate.Text = dts.Rows[0]["VehiclePlateNumber"].ToString();

                cboType.Text = dts.Rows[0]["VehicleType"].ToString();
                txtMake.Text = dts.Rows[0]["VehicleMake"].ToString();
                txtChasis.Text = dts.Rows[0]["VehicleChasisNumber"].ToString();
                txtEngine.Text = dts.Rows[0]["VehicleEngineNumber"].ToString();
                colorEdit1.Text = dts.Rows[0]["VehicleColor"].ToString();
                cboCategory.Text = dts.Rows[0]["VehicleCapacity"].ToString();
                cboPlace.Text = dts.Rows[0]["VehicleLastPlaceReg"].ToString();

            }

            return bResponse;
        }

    }
}
