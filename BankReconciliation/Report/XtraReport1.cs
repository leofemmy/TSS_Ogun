﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BankReconciliation.Report
{
    public partial class XtraReport1 : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport1()
        {
            InitializeComponent();
            viewPostingRequestDetailsTableAdapter1.Connection.ConnectionString = Class.Logic.ConnectionString;
        }

    }
}
