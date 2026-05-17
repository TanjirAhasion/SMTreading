using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Enums
{
    public enum RentalEnum
    {
    }
    public enum RentalStatus
    {
        Active = 1,
        Returned = 2,
        Cancelled = 3
    }

    public enum RateType
    {
        Daily = 1,
        Monthly = 2,
        Yearly = 3,
        Quarterly = 4,
    }

    public enum ReturnCondition
    {
        Good = 1,
        Damaged = 2
    }

    public enum BillingCycle
    {
        Monthly = 1,
        Yearly = 2
    }

    public enum RentalContractStatus
    {
        Draft = 1,        // created but not started

        Active = 2,       // currently running

        Suspended = 3,    // temporarily paused

        Completed = 4,    // contract finished normally

        Cancelled = 5,    // cancelled before completion

        Overdue = 6       // end date passed but not returned/closed
    }
}
