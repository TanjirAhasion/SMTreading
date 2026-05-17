using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.CashManagement
{
    public class CashTransferDto
    {
        public long Id { get; set; }
        public long FromCashAccountId { get; set; }
        public long ToCashAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }
        public string? Note { get; set; }
    }
}
