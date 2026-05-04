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
        Monthly = 2
    }

    public enum ReturnCondition
    {
        Good = 1,
        Damaged = 2
    }
}
