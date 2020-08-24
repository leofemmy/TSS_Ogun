using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraEditors;

namespace Collection.Report
{
    public partial class XRepGlobalManifest : DevExpress.XtraReports.UI.XtraReport
    {
        public string logoPath;
        public XRepGlobalManifest()
        {
            InitializeComponent();
            this.BeforePrint += XRepGlobalManifest_BeforePrint;
            paramDate.Value = DateTime.Today.AddDays(-1);
            //paramDate.Value=string.Format("{0:dd/MM/yyyy}",DateTime.Today.AddDays(-1));
        }

        private void XRepGlobalManifest_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var fullPath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, logoPath);
            if (!string.IsNullOrWhiteSpace(fullPath) && System.IO.File.Exists(fullPath))
                xrPictureBox1.Image = Image.FromFile(fullPath);
        }

        private void XRepGlobalManifest_ParametersRequestSubmit(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
            string objDate = Convert.ToDateTime(paramDate.Value).Date.ToShortDateString() + " 00:00:00";
            DateTime dtDate = DateTime.Parse(objDate);
            paramStartDate.Value = (object)dtDate;

            objDate = Convert.ToDateTime(paramDate.Value).Date.ToShortDateString() + " 23:59:59";
            dtDate = DateTime.Parse(objDate);
            paramEndDate.Value = (object)dtDate;

            if (!Convert.ToBoolean(paramMemo.Value))
            {
                parameter1.Value = (byte)0;
            }
            else
                parameter1.Value = (byte)1;

        }

        private void XRepGlobalManifest_ParametersRequestBeforeShow(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
            foreach (ParameterInfo info in e.ParametersInformation)
            {
                if (info.Parameter.Name == paramDate.Name)
                {
                   
                    var editor = info.Editor as DateEdit;
                    if (editor != null)
                    {
                        
                        editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        editor.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
                    }

                }
            }
        }

        private void XRepGlobalManifest_ParametersRequestValueChanged(object sender, ParametersRequestValueChangedEventArgs e)
        {
            foreach (ParameterInfo info in e.ParametersInformation)
            {
                if (info.Parameter.Name == paramDate.Name)
                {

                    var editor = info.Editor as DateEdit;
                    if (editor != null)
                    {

                        editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        editor.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
                    }

                }
            }
        }


    }
}
