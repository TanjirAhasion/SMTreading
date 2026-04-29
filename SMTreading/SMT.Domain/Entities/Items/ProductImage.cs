using SMT.Domain.Common;
using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMT.Domain.Entities.Items
{
    public class ProductImage :BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public long ProductSerialId { get; set; }

        [ForeignKey("ProductSerialId")]
        public virtual required ProductSerial ProductSerial { get; set; }
    }
}
