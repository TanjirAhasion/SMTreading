using SMT.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMT.Domain.Entities.Items
{
    public class Brand : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        // Optional: URL to the brand's logo image
        public string? LogoUrl { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property: One brand can have many products
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
