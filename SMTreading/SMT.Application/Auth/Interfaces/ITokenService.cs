using SMT.Application.Auth.DTO;
using SMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Auth.Interfaces
{
    public interface ITokenService
    {
        TokenPair GenerateForTenant(Tenant tenant);
        TokenPair GenerateForUser(User user);
    }
}
