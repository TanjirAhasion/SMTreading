using SMT.Domain.Common;
using SMT.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory.Rental
{
    public class RentalInvoice : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public long CustomerId { get; set; }
        public Customer Customer { get; set; }

        public long RentalContractId { get; set; }
        public RentalContract RentalContract { get; set; }

        public DateTime BillingFrom { get; set; }
        public DateTime BillingTo { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Discount { get; set; }

        public decimal NetTotal { get; set; }

        public bool IsPaid { get; set; }

        public ICollection<RentalInvoiceItem> Items { get; set; }
            = new List<RentalInvoiceItem>();
    }
}
