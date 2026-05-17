using SMT.Domain.Entities.Items;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory.Rental
{
    public class RentalInvoiceItem
    {
        public long Id { get; set; }

        public long RentalInvoiceId { get; set; }
        public RentalInvoice RentalInvoice { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }

        public RateType RateType { get; set; } // Daily / Monthly
        public decimal Rate { get; set; }
        public int Quantity { get; set; }

        public ICollection<RentalItemProductSerial> Serials { get; set; } = new List<RentalItemProductSerial>();
    }
}
