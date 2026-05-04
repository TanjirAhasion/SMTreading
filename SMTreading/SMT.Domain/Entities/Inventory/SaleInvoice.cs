using SMT.Domain.Common;
using SMT.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Inventory
{
    public class SaleInvoice : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }


        [Required]
        [Display(Name = "Customer ID")]
        public long CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Sale Date")]
        public DateTime SaleDate { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Net Amount")]
        public decimal SubTotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Paid Amount")]
        public decimal Discount { get; set; }


        [Required]
        public bool IsPaid { get; set; } // e.g., Unpaid, PartPaid, Paid

        [StringLength(500)]
        public string? Note { get; set; }
    }
}
