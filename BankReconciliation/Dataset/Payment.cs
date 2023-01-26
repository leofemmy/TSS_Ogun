using System;

namespace BankReconciliation.Dataset
{
    public class Payment
    {
        public string PaymentRefNumber { get; set; }
        public string PayerName { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime Transdate { get; set; }
        public string AgecnyName { get; set; }
        public string BankName { get; set; }

    }
}
