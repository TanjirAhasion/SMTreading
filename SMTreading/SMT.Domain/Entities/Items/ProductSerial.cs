using SMT.Domain.Common;
using SMT.Domain.Enums;
using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMT.Domain.Entities.Items
{
    public class ProductSerial : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string SerialNumber { get; set; }

        [Required]
        public long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual required Product Product { get; set; }

        public ProductSerialStatus Status { get; set; } = ProductSerialStatus.InStock;

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal PurchaseCost { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal SellingCost { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal RentalCost { get; set; }

        public string? BatchNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }

        [StringLength(500)]
        public string? Note { get; set; }

        // Navigation property: One brand can have many products
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
