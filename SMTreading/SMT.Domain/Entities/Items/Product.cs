
using SMT.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMT.Domain.Entities.Items
{
    public class Product : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Name { get; set; } 

        [Required,StringLength(100)]
        public string Model { get; set; }

        [Required]
        public long BrandId { get; set; }


        [ForeignKey("BrandId")]
        public virtual required Brand Brand { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal DefaultSalePrice { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal DefaultRentPrice { get; set; }

        public int? LowStockThreshold { get; set; } = 10;
        
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property: One brand can have many products
        public virtual ICollection<ProductSerial> ProductSerials { get; set; } = new List<ProductSerial>();
    }
}
