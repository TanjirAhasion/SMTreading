using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SMT.Application.Auth.DTO;
using SMT.Application.Auth.Interfaces;
using SMT.Application.Common;
using SMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SMT.Infrastructure.Auth.Services
{
    public class TokenService(IOptions<JwtSettings> opts) : ITokenService
    {
        private readonly JwtSettings _cfg = opts.Value;

        public TokenPair GenerateForTenant(Tenant tenant)
        {
            var claims = new List<Claim>
        {
            new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,  tenant.Id.ToString()),
            new("type",               "tenant"),
            new("tenantId",           tenant.Id.ToString()),
            new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, tenant.Email),
            new("name",              tenant.BusinessName),
        };
            return BuildTokenPair(claims);
        }

        public TokenPair GenerateForUser(User user)
        {
            var claims = new List<Claim>
        {
            new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new("type",             "user"),
            new("tenantId",         user.TenantId.ToString()),
            new("userId",           user.Id.ToString()),
            new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email),
            new("name",            user.FirstName + " " + user.LastName),
        };
            return BuildTokenPair(claims);
        }

        private TokenPair BuildTokenPair(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(8);

            var token = new JwtSecurityToken(
                issuer: _cfg.Issuer,
                audience: _cfg.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new TokenPair
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = expires
            };
        }
    }

}
