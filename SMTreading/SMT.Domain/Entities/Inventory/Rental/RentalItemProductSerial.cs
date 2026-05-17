using SMT.Domain.Entities.Items;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory.Rental
{
    public class RentalItemProductSerial
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long RentalItemId { get; set; }
        public RentalInvoiceItem RentalItem { get; set; }

        [Required]

        public long ProductSerialId { get; set; }
        public ProductSerial ProductSerial { get; set; }

        [Required]
        public ProductSerialStatus Status { get; set; }

        public DateTime RentalDate { get; set; } = DateTime.Now;
    }
}
