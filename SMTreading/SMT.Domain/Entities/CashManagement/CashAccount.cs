using SMT.Domain.Common;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.CashManagement
{
    public class CashAccount : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public CashAccountType AccountType { get; set; }

        [StringLength(100)]
        public string? AccountHolderName { get; set; }
        
        [StringLength(50)]
        public string? AccountNumber { get; set; }
        
        [StringLength(100)]
        public string? BankName { get; set; }
        
        [StringLength(50)]
        public string? BranchName { get; set; }

        [StringLength(500)]
        public string? Note { get; set; }

        public MobileBankType? MobileBankType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OpeningBalance { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CurrentBalance { get; set; } = 0;

        [Required]
        public bool IsDefault { get; set; } = false;

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
