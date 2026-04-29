using SMT.Domain.Entities.Items;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class PurchaseItemProductSerial
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long PurchaseItemId { get; set; }
        public PurchaseItem PurchaseItem { get; set; }

        [Required]

        public long ProductSerialId { get; set; }
        public ProductSerial ProductSerial { get; set; }

        [Required]
        public ProductSerialStatus Status { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}
