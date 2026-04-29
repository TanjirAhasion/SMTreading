using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Auth.DTO
{
    public sealed record LoginRequest(string Username, string Password, bool? RememberMe);

    public sealed record LoginResponse
    {
        public required string AccessToken { get; init; }
        public required string RefreshToken { get; init; }
        public required string Type { get; init; }  // "tenant" | "user"
        public required Guid TenantId { get; init; }
        public Guid? UserId { get; init; }
        public required string Name { get; init; }
    }

}
