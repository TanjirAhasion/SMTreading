using SMT.Domain.Entities.Items;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class RentalItem
    {
        public long Id { get; set; }

        public long RentalId { get; set; }
        public Rental Rental { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }

        public long ProductSerialId { get; set; } // 🔥 specific machine
        public ProductSerial ProductSerial { get; set; }

        public RateType RateType { get; set; } // Daily / Monthly
        public decimal Rate { get; set; }

        public decimal Quantity { get; set; } = 1; // usually 1 for serial-based
    }
}
