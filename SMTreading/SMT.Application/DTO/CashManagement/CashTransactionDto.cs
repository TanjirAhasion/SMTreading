using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.CashManagement
{
    public class CashTransactionDto
    {
        public long Id { get; set; }
        public long CashAccountId { get; set; }
        public string CashAccountName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }
        public int SourceType { get; set; }
        public long? ReferenceId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
    }
}
