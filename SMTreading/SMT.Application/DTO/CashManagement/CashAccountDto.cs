using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.CashManagement
{
    public class CashAccountDto
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public CashAccountType AccountType { get; set; }

        public string AccountTypeName => AccountType.ToString();

        public MobileBankType? MobileBankType { get; set; }

        public string? MobileBankTypeName => MobileBankType?.ToString();

        public string? AccountNumber { get; set; }

        public string? AccountHolderName { get; set; }

        public string? BankName { get; set; }

        public string? BranchName { get; set; }

        public string? Note { get; set; }

        public decimal OpeningBalance { get; set; }

        public decimal CurrentBalance { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }
    }
}
