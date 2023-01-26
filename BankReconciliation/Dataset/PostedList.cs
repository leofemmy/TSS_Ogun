using System;

namespace BankReconciliation.Dataset
{
    public class PostedList
    {
        public string PaymentRefNumber { get; set; }
        public string PayerID { get; set; }
        //public string Description { get; set; }
        public Decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        //public DateTime Transdate { get; set; }
        //public string AgecnyName { get; set; }
        public string BankName { get; set; }
    }
}
