using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory.Rental
{
    public class RentalContractItem
    {
        [Key]
        public long Id { get; set; }

        public long RentalContractId { get; set; }
        public RentalContract RentalContract { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }

        public long ProductSerialId { get; set; }
        public ProductSerial ProductSerial { get; set; }

        public decimal Rate { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
