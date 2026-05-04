using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class RentalReturn
    {
        public long Id { get; set; }

        public long RentalId { get; set; }
        public Rental Rental { get; set; }

        public DateTime ReturnDate { get; set; } = DateTime.UtcNow;

        public decimal TotalCharge { get; set; }     // calculated rental cost
        public decimal LateFee { get; set; }
        public decimal DamageCharge { get; set; }

        public string? Note { get; set; }

        public ICollection<RentalReturnItem> Items { get; set; } = new List<RentalReturnItem>();
    }
}
