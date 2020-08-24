using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaxSmart.Data;

namespace UserManager.Model
{
    public class EntityModel
    {
        public static TaxSmartDataContext GetContext
        {
            get
            {
                return new TaxSmartDataContext();
            }
        }
    }
}
