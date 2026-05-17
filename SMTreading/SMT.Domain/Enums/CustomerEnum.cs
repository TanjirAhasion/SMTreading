using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Enums
{
    public class CustomerEnum
    {
    }

    public enum CustomerAdjustmentType
    {
        Discount = 1,
        Return = 2
    }

    public enum CustomerLedgerSourceType
    {
        Sale = 1,
        Payment = 2,
        Rental = 3,
        RentalContractSecurityDeposit = 4,
        Adjustment = 5

    }

    public enum CustomerPaymentType
    {
        Sale = 1,
        Rental = 2,
        Advance = 3
    }
}