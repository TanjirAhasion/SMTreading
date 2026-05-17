using SMT.Domain.Enums;
using SMT.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.Accounts
{
    public class CustomerLedgerDto
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public CustomerLedgerSourceType SourceType { get; set; } // Purchase, Payment, Adjustment
        public long SourceId { get; set; }
        public decimal Debit { get; set; }   // Purchase increases payable
        public decimal Credit { get; set; }  // Payment/discount reduces payable
        public decimal Balance { get; set; } // running balance

        public string? Description { get; set; } // e.g. "Purchase #123", "Payment for Invoice #123"
    }
}
