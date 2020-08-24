using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaxSmartRegistration
{
    public partial class MDIMainForm : Form
    {
        public MDIMainForm()
        {
            InitializeComponent();
            setImages();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            var sForm = new Form1();
            sForm.MdiParent = this;
            sForm.Show();
        }

        private void setImages()
        {
            tsbNew.Image = i32x32.Images[0];
            tsbModify.Image = i32x32.Images[1];
            tsbDelete.Image = i32x32.Images[3];
            tsbReload.Image = i32x32.Images[4];
            tsbClose.Image = i32x32.Images[6];
            tsbCardDetails.Image = i32x32.Images[11];
            tsbMakePayment.Image = i32x32.Images[12];
            tsbAccountStatement.Image = i32x32.Images[5];

            //cmsItemNew.Image = MDIMainForm.publicMDIParent.i16x16.Images[5];
            //cmsItemModify.Image = MDIMainForm.publicMDIParent.i16x16.Images[6];
            //cmsItemDelete.Image = MDIMainForm.publicMDIParent.i16x16.Images[7];
            //cmsItemReload.Image = MDIMainForm.publicMDIParent.i16x16.Images[9];
            //cmsItemClose.Image = MDIMainForm.publicMDIParent.i16x16.Images[11];
            //cmsMakePayment.Image = MDIMainForm.publicMDIParent.i16x16.Images[12];
            //cmsAccountStatement.Image = MDIMainForm.publicMDIParent.i16x16.Images[10];

            //gridView1.Images = MDIMainForm.publicMDIParent.i24x24;

        }

        private void MDIMainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
