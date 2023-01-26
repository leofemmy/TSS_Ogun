using System;

namespace BankReconciliation.Dataset
{
    class NTCollection
    {
        public string PaymentRef { get; set; }
        public Decimal Amount { get; set; }
        public DateTime CollDate { get; set; }
    }
}
