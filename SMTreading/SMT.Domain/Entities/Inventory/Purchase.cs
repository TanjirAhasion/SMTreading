using SMT.Domain.Common;
using StockoraPOS.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class Purchase : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public string PurchaseNumber { get; set; } = string.Empty; // 🔥 important // PUR-2026-00001
        
        [Required]
        public long VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public decimal Discount { get; set; }
        public decimal NetTotal { get; set; }
        public decimal SubTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ICollection<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
    }
}
