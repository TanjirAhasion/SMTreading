using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.Accounts
{
    public class CustomerPaymentDto
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long? SaleId { get; set; }
        public long? RentalId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMethod { get; set; }
        public int CustomerPaymentType { get; set; }
    }
}
