using SMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMT.Domain.Common
{
    public abstract class BaseEntity
    {
        [Required]
        public Guid TenantId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public Guid? CreatedBy { get; set; }
        
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public Guid? UpdatedBy { get; set; }
        
        public string? IpAddress { get; set; } = string.Empty;

    }
}
