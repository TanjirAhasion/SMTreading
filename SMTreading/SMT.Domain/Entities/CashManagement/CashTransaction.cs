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
    public class CashTransaction : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public long CashAccountId { get; set; }

        [ForeignKey("CashAccountId")]
        public virtual CashAccount CashAccount { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        public TransactionSource SourceType { get; set; }

        public long? ReferenceId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [StringLength(500)]
        public string? Note { get; set; }
    }
}
