using SMT.Domain.Common;
using SMT.Domain.Entities.Contacts;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory.Rental
{
    public class RentalContract : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public string ContractNumber { get; set; } = string.Empty;
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public BillingCycle BillingCycle { get; set; }

        public RentalContractStatus Status { get; set; }

        public decimal? SecurityDeposit { get; set; }

        public string? Note { get; set; }

        public DateTime NextBillingDate { get; set; } // 🔥 IMPORTANT

        public ICollection<RentalContractItem> Items { get; set; }
            = new List<RentalContractItem>();

        public ICollection<RentalInvoice> RentalInvoices { get; set; }
            = new List<RentalInvoice>();
    }
}
