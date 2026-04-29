using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class PurchaseItem
    {
        [Key]
        public long Id { get; set; }
        public long PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
        public long ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }  // 🔥 MAIN SOURCE OF COST
        public decimal DiscountAllocated { get; set; }
        public decimal TotalCost { get; set; }
    }
}
