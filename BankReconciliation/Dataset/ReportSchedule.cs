using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReconciliation.Dataset
{
    public class ReportSchedule
    {
        public DateTime Date { get; set; }
        public Decimal Debit { get; set; }
        public Decimal Credit { get; set; }
        public string Comments { get; set; }
        public string BankName { get; set; }
        public string Acctnumber { get; set; }
        public string Period { get; set; }
        public string Branchname { get; set; }
    }
}
