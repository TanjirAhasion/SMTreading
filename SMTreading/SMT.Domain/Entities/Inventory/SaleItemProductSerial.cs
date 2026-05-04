using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class SaleItemProductSerial
    {
        [Key]
        public long Id { get; set; }

        public long SaleItemId { get; set; }
        public SaleItem SaleItem { get; set; }

        public long ProductSerialId { get; set; }
        public ProductSerial ProductSerial { get; set; }
    }
}
