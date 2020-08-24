using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Selection;
using TaxSmartSuite.Class;

namespace EasyReg.Forms
{
    public partial class FrmCreateUser : Form
    {
        EasyRegService.EasyRegService EasyServices = new EasyReg.EasyRegService.EasyRegService();

        GridCheckMarksSelection selection;

        bool isFirstGrid = true;

        private DataTable _dtb = new DataTable();

        private DataSet _ds = new DataSet();

        private object retval;

        public FrmCreateUser()
        {
            InitializeComponent();
            setImages();
            setReload();

            if (isFirstGrid)
            {
                selection = new GridCheckMarksSelection(gridView1);
                selection.CheckMarkColumn.VisibleIndex = 0;
                isFirstGrid = false;
            }

          bttnEnable.Click += Bttn_Click;
          bttnUpdate.Click += Bttn_Click;
          bttnDisable.Click += Bttn_Click;
          bttnClose.Click += Bttn_Click;
        }

        internal GridCheckMarksSelection Selection
        {
            get { return selection; }
        }

        private void setImages()
        {
            bttnUpdate.Image = MDIMain.publicMDIParent.i32x32.Images[7];
            bttnClose.Image = MDIMain.publicMDIParent.i16x16.Images[11];
        }

        private void setReload()
        {
            DataTable dt;

            using (var ds = new System.Data.DataSet())
            {
                dt = EasyServices.doSelectUsers();
                gridControl1.DataSource = dt.DefaultView;
            }
            //gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["UserListID"].Visible = false;
            gridView1.BestFitColumns();
        }

        void Bttn_Click(object sender, EventArgs e)
        {
            if (sender == bttnEnable)
            {
                EnableClick();
            }
            else if (sender == bttnUpdate)
            {
                updateRec();        
            }
            else if (sender == bttnDisable)
            {
                DisableClick();
            }
            else if (sender == bttnClose)
            {
                MDIMain.publicMDIParent.RemoveControls();
            }
           

        }

        void EnableClick()
        {
            _dtb.Columns.Add("UserID", typeof(string));//give the datatable a structure, in this case, it has only two columns
            _dtb.Columns.Add("LoginStatusListID", typeof(Int64));

            for (int i = 0; i < selection.SelectedCount; i++)
            {
                string lol = ((selection.GetSelectedRow(i) as DataRowView)["UserListID"].ToString());

                object[] data = { lol, 1 };//2 means disable and 1 means enable
                //Dtt.Rows.Add(new object[] { filename, image });

                //_dtb.Rows.Add(new object[] { lol, 1 });//2 means disable and 1 means enable
                _dtb.Rows.Add(data);

            }
            retval = EasyServices.DoEnableDisables(_dtb,4);

            _ds = (DataSet)retval;

            
        }

        void updateRec()
        {
            if (txtId.Text == null || txtId.Text == "")
            {
                Common.setEmptyField("User ID", Program.ApplicationName);
                txtId.Focus(); return ;
            }
            else if (txtName.Text == null || txtName.Text == "")
            {
                Common.setEmptyField("User Name", Program.ApplicationName);
                txtName.Focus(); return;
            }
            else if (txtPassword.Text == null || txtPassword.Text == "")
            {
                Common.setEmptyField("User Password", Program.ApplicationName);
                txtPassword.Focus(); return;
            }
            else
            {
                DataTable dt = EasyServices.CreateUsers(null, null, txtId.Text.Trim(), txtPassword.Text.Trim(), 4, Program.AgentListId, txtName.Text.Trim());

                if (dt.Rows[0][0].ToString() == "00")
                {
                    setReload();
                }

                Common.setMessageBox("User Created", Program.ApplicationName, 1);
                txtId.Clear(); txtName.Clear(); txtPassword.Clear();


            }
        }

        void DisableClick()
        {
            _dtb.Columns.Add("UserID", typeof(string));//give the datatable a structure, in this case, it has only two columns
            _dtb.Columns.Add("LoginStatusListID", typeof(Int64));

            for (int i = 0; i < selection.SelectedCount; i++)
            {
                string lol = ((selection.GetSelectedRow(i) as DataRowView)["UserListID"].ToString());

                object[] data = { lol, 2 };//2 means disable and 1 means enable
                //Dtt.Rows.Add(new object[] { filename, image });

                //_dtb.Rows.Add(new object[] { lol, 1 });//2 means disable and 1 means enable
                _dtb.Rows.Add(data);

            }
            DataTable dt = EasyServices.DoEnableDisables(_dtb as object, 4);                    


            _ds = (DataSet)retval;
        }

    }
}
