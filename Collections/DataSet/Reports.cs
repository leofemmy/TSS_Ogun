using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collection.DataSet
{
    public class Reports
    {
        public string PaymentRefNumber { get; set; }
        public string DepositSlipNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PayerID { get; set; }
        public string PayerName { get; set; }
        public string AgencyName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Bank { get; set; }
        public string EReceipts { get; set; }
        public string StationCode { get; set; }
        public string StationName { get; set; }
    }
}
