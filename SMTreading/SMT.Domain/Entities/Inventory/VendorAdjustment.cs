using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class VendorAdjustment
    {
        [Key]
        public long Id { get; set; }

        public long VendorId { get; set; }

        public decimal Amount { get; set; } // discount received

        public string Type { get; set; } // "Discount", "Return"

        public DateTime Date { get; set; }
    }
}
