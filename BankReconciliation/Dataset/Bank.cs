using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReconciliation.Dataset
{
    public class Bank
    {public Decimal Amount { get; set; }
        public DateTime BSDate { get; set; }
        public Int32 PostingRequestID { get; set; }
    }
}
