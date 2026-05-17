using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.CashManagement
{
    public class ExpenseDto
    {
        public long Id { get; set; }

        public long ExpenseCategoryId { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; }

        public long CashAccountId { get; set; }

        public string? Note { get; set; }
    }
}
