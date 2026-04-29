using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SMT.Domain.Entities
{
    public class Tenant
    {
        [Key] 
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required, MaxLength(150)] 
        public string BusinessName { get; set; } = "";

        [MaxLength(200)]
        public string? LogoUrl { get; set; }

        [MaxLength(200)]
        public string? Website { get; set; }
        
        [MaxLength(100)] 
        public string? Industry { get; set; }
        
        [Required, MaxLength(100)] 
        public string Email { get; set; } = "";

        [Required, MaxLength(100)]
        public string UserName { get; set; } = "";

        [Required, MaxLength(200)]
        public string PasswordHash { get; private set; } = string.Empty;

        [MaxLength(30)] 
        public string? BillingPhone { get; set; }
        public string? BillingAddress { get; set; }
        
        public TenantStatus Status { get; set; } = TenantStatus.Active;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public IReadOnlyCollection<User> Users => _users.AsReadOnly();
        private readonly List<User> _users = new();

        private Tenant() { } // EF Core

        public static Tenant Create(string userName, string businessName,
            string email, string phone, string address, string industry,string logoUrl, string website, string passwordHash)
        {
            return new Tenant
            {
                BusinessName = businessName,
                Email = email,
                UserName= userName,
                BillingPhone = phone,
                BillingAddress= address,
                Status = TenantStatus.Active,
                Industry = industry,
                LogoUrl= logoUrl,
                Website= website,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };
        }

    }
}
