using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReconciliation.Dataset
{
   public class Reportyear
    {
        public Decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string PaymentRef { get; set; }
    }
}
