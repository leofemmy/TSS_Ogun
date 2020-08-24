using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankReconciliation.Dataset
{
    class NTCollection
    {
        public string PaymentRef { get; set; }
        public Decimal Amount { get; set; }
        public DateTime CollDate { get; set; }
    }
}
