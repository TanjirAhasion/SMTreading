using SMT.Domain.Common;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SMT.Domain.Entities
{
    public class User : BaseEntity
    {
        [Key] 
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required, MaxLength(255)] 
        public string Email { get; set; } = "";

        [Required, MaxLength(255)]
        public string Username { get; set; } = "";

        [MaxLength(255)] 
        public string? PasswordHash { get; set; }
        
        [Required, MaxLength(100)] 
        public string FirstName { get; set; } = "";
        
        [MaxLength(100)] 
        public string LastName { get; set; } = "";
        [MaxLength(20)] 
        public string? Phone { get; set; }

        [Required, MaxLength(50)]
        public string UserRole { get; set; } = "";

        public string? AvatarUrl { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;

        public DateTime? LastLoginAt { get; set; }

        // Navigation
        public Tenant? Tenant { get; private set; }

        private User() { } // EF Core

        public static User Create(Guid tenantId, string username,
            string passwordHash, string firstName, string lastName, string phone, string avatarUrl, string email)
        {
            return new User
            {
                TenantId = tenantId,
                Username = username,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Phone= phone,
                AvatarUrl= avatarUrl,
                Email = email,
                UserRole= "User",
                CreatedBy = tenantId,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
        }

    }
}
