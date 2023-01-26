using System;

namespace BankReconciliation.Dataset
{
    public class BankDetails
    {
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string PayerName { get; set; }
        public string RevenueCode { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string AccountNumber { get; set; }
    }
}
