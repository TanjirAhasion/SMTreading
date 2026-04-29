using SMT.Application.Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Auth.Services
{
    public sealed class BcryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
            => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

        public bool Verify(string password, string hash)
            => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
