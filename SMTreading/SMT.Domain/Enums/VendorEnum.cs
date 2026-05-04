using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Enums
{
    public enum VendorEnum
    {
    }

    public enum VendorAdjustmentType
    {
        Discount = 1,
        Return = 2
    }

    public enum VendorLedgerSourceType
    {
        Purchase = 1,
        Payment = 2,
        Adjustment = 3
    } 
}
