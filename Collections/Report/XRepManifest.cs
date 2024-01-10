using System.Drawing;

namespace Collection.Report
{
    public partial class XRepManifest : DevExpress.XtraReports.UI.XtraReport
    {
        public string logoPath;
        public XRepManifest()
        {
            InitializeComponent();

            this.BeforePrint += XRepManifest_BeforePrint;
        }

        private void XRepManifest_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var fullPath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, logoPath);
            if (!string.IsNullOrWhiteSpace(fullPath) && System.IO.File.Exists(fullPath))
                xrPictureBox1.Image = Image.FromFile(fullPath);
        }
    }
}
