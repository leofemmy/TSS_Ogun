using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxDrive.Class
{
    public class Report
    {
        public Decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string AgencyName { get; set; }
        public string PayerName { get; set; }
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
        public string BankName { get; set; }
    }
}
