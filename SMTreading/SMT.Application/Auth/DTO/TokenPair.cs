using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Auth.DTO
{
    public sealed record TokenPair
    {
        public required string AccessToken { get; init; }
        public required string RefreshToken { get; init; }
        public required DateTime ExpiresAt { get; init; }
    }
}
