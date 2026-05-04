using SMT.Domain.Common;
using SMT.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class Purchase : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string PurchaseNumber { get; set; } = string.Empty; // 🔥 important // PUR-2026-00001
        
        [Required]
        public long VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }

        public bool IsPaid { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ICollection<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
    }
}
