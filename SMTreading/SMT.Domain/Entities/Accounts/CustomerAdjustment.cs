using SMT.Domain.Common;
using SMT.Domain.Entities.Contacts;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Accounts
{
    public class CustomerAdjustment : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = new Customer();

        public decimal Amount { get; set; } // discount received

        public CustomerAdjustmentType AdjustmentType { get; set; } // "Discount", "Return"
    }
}
