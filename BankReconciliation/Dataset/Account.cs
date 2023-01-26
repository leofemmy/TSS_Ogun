using System;

namespace BankReconciliation.Dataset
{
    public class Account
    {
        public Decimal Openingbal { get; set; }
        public Decimal closingbal { get; set; }
        public Decimal Amount { get; set; }
        public string BankName { get; set; }
        public string Acctnumber { get; set; }
        public string BatchCode { get; set; }
        public string BankShortCode { get; set; }
        public Int32 Transid { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

    }
}
