using SMT.Domain.Entities.Items;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class RentalReturnItem
    {
        public long Id { get; set; }

        public long RentalReturnId { get; set; }
        public RentalReturn RentalReturn { get; set; }

        public long ProductSerialId { get; set; }
        public ProductSerial ProductSerial { get; set; }

        public ReturnCondition Condition { get; set; } // Good / Damaged

        public decimal ExtraCharge { get; set; } // per item damage cost
    }
}
