using System;

namespace BankReconciliation.Dataset
{
    public class Reems
    {
        public Decimal Openingbal { get; set; }
        public Decimal closingbal { get; set; }
        public Decimal ReemsCollection { get; set; }
        public Decimal Bankexcpec { get; set; }
        public Decimal Bankcredit { get; set; }
        public Decimal BankDebit { get; set; }
        public Decimal bankcharge { get; set; }
        public Decimal Transferto { get; set; }
        public Decimal PrevcreditRev { get; set; }
        public Decimal ReturnCheque { get; set; }
        public Decimal CurrentReversalDr { get; set; }
        public Decimal CurrentReversalCr { get; set; }
        public Decimal PayDirectBank { get; set; }
        public Decimal CreditInterest { get; set; }
        public Decimal TransferFromGovtAcct { get; set; }
        public Decimal PrevDebitReversed { get; set; }
        public string BankName { get; set; }
        public string Acctnumber { get; set; }
        public string Period { get; set; }
        public string Branchname { get; set; }
    }
}
