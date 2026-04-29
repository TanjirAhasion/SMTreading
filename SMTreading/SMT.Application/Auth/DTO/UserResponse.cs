using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Auth.DTO
{
    public sealed class UserResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string FullName { get; set; } = "";
        public Guid TenantId { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
    }
}
