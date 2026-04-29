using StockoraPOS.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class VendorPayment
    {
        [Key]
        public long Id { get; set; }

        public long VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public long? PurchaseId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
