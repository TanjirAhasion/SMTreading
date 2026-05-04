using SMT.Domain.Entities.Contacts;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class Rental
    {
        [Key]
        public long Id { get; set; }

        public string RentalNumber { get; set; } = string.Empty; // RENT-2026-00001

        public long CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpectedReturnDate { get; set; }

        public RentalStatus Status { get; set; } = RentalStatus.Active;

        public string? Note { get; set; }

        public ICollection<RentalItem> Items { get; set; } = new List<RentalItem>();
        public ICollection<RentalReturn> Returns { get; set; } = new List<RentalReturn>();
    }
}
