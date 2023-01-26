using System;

namespace BankReconciliation.Dataset
{
    public class Allocation
    {
        public string BankName
        {
            get; set;
        }
        public string AccountName
        {
            get; set;
        }
        public string AccountNumber
        {
            get; set;
        }
        public DateTime BSDate
        {
            get; set;
        }
        public decimal Debit
        {
            get; set;
        }

        public decimal Credit
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public string AllocateBy
        {
            get; set;
        }
        public DateTime DateAllocate
        {
            get; set;
        }
        public string Period
        {
            get; set;
        }
        public decimal ClosingBalance
        {
            get; set;
        }
        public decimal OpeningBalance
        {
            get; set;
        }
        public decimal TotalCredit
        {
            get; set;
        }
        public decimal TotalDebit
        {
            get; set;
        }
    }
}
