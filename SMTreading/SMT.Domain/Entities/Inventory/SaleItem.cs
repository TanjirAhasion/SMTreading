using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class SaleItem
    {
        public long Id { get; set; }

        public long SaleId { get; set; }
        public SaleInvoice SaleInvoice { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Optional (only if you want per-item discount)
        public decimal Discount { get; set; }

        public ICollection<SaleItemProductSerial> Serials { get; set; } = new List<SaleItemProductSerial>();
    }
}
