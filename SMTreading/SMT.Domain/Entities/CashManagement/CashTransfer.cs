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
    public class CashTransfer : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public DateTime TransferDate { get; set; }

        [Required]
        public long FromCashAccountId { get; set; }

        [ForeignKey("FromCashAccountId")]
        public virtual CashAccount FromAccount { get; set; }

        [Required]
        public long ToCashAccountId { get; set; }

        [ForeignKey("ToCashAccountId")]
        public virtual CashAccount ToAccount { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Transfer amount must be greater than zero.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [StringLength(300)]
        public string? Note { get; set; }
    }
}
