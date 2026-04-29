using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMT.Application.DTO.Items
{
    public class BrandDto
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = default!;

        [StringLength(500)]
        public string? Description { get; set; }

        // Optional: URL to the brand's logo image
        public string? LogoUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
