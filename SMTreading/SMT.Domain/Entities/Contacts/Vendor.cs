using SMT.Domain.Common;
using SMT.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMT.Domain.Entities.Contacts
{
    public class Vendor : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(20)]
        public string? SecondaryPhone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? CompanyName { get; set; }
        
        [MaxLength(100)]
        public string? BusinessCardPath { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public ICollection<Purchase> Purchases { get; set; }
    }
}
