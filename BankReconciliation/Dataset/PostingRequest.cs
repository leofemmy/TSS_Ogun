using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankReconciliation.Dataset
{
    public class PostingRequest
    {
        public Decimal Openingbal { get; set; }
        public Decimal closingbal { get; set; }
        public Decimal Debit { get; set; }
        public Decimal Credit { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BatchCode { get; set; }
        public string BatchName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int TransID { get; set; }
        public string Acctnumber { get; set; }
        public string ActDescription { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public Int32 PostingRequestID { get; set; }

    }
}
