using SMT.Domain.Common;
using SMT.Domain.Enums;
using SMT.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Accounts
{
    public class CustomerLedger : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }

        [Required]
        public CustomerLedgerSourceType SourceType { get; set; } // Purchase, Payment, Adjustment
        public long SourceId { get; set; }

        public decimal Debit { get; set; }   // Purchase increases payable
        public decimal Credit { get; set; }  // Payment/discount reduces payable

        public decimal Balance { get; set; } // running balance
    }
}
