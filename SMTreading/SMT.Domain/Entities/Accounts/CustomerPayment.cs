using SMT.Domain.Common;
using SMT.Domain.Entities.Contacts;
using SMT.Domain.Entities.Inventory;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Accounts
{
    public class CustomerPayment : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public long CustomerId { get; set; }
        public Customer Customer { get; set; }

        public long? SaleId { get; set; } // NULL = advance payment
        public SaleInvoice? Sale { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.Cash;

        public string? ReferenceNo { get; set; } // trx id / note
    }
}
