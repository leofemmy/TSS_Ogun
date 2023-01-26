using System;

namespace BankReconciliation.Dataset
{
    public class Bank
    {
        public Decimal Amount { get; set; }
        public DateTime BSDate { get; set; }
        public Int32 PostingRequestID { get; set; }
    }
}
