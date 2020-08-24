using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaxSmartSuite.Class;

namespace Control_Panel.Forms
{
    public partial class FrmDeduction : Form
    {
        public static FrmDeduction publicStreetGroup;

        protected TransactionTypeCode iTransType;

        protected bool boolIsUpdate;

        protected string ID;

        bool isFirst = true;

        public FrmDeduction()
        {
            InitializeComponent();
        }
    }
}
