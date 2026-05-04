using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.Accounts
{
    public class VendorPaymentDto
    {
        public long Id { get; set; }
        public long VendorId { get; set; }
        public long? PurchaseId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMethod { get; set; }
    }
}
