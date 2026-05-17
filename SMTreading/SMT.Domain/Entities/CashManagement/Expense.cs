using SMT.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.CashManagement
{
    public class Expense : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public long ExpenseCategoryId { get; set; }

        [ForeignKey("ExpenseCategoryId")]
        public virtual ExpenseCategory ExpenseCategory { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PaidAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DueAmount { get; set; } = 0;

        [StringLength(500)]
        public string? Description { get; set; }

        public long? CashAccountId { get; set; }

        [ForeignKey("CashAccountId")]
        public virtual CashAccount CashAccount { get; set; }

        [StringLength(500)]
        public string? Note { get; set; }
    }
}
