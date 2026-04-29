using MediatR;
using SMT.Application.Auth.DTO;
using SMT.Application.Auth.Interfaces;
using SMT.Application.Common;
using SMT.Infrastructure.context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;
using SMT.Domain.Enums;

namespace SMT.Infrastructure.Auth.Commands
{
    public sealed class LoginCommandHandler    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
        private readonly AppDbContext _db;
        private readonly ITokenService _tokens;

        public LoginCommandHandler(AppDbContext db, ITokenService tokens)
            => (_db, _tokens) = (db, tokens);

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand cmd, CancellationToken ct)
        {
            var req = cmd.Request;

            // ── Step 1: Check Tenants ──────────────────────────────
            var tenant = await _db.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    t => t.Email == req.Username || t.UserName == req.Username,
                    ct);

            if (tenant is not null && tenant.Status == TenantStatus.Active)
            {
                if (!BCrypt.Net.BCrypt.Verify(req.Password, tenant.PasswordHash))
                    return Result.Failure<LoginResponse>("Invalid credentials");

                var tokens = _tokens.GenerateForTenant(tenant);
                return Result.Success(new LoginResponse
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken,
                    Type = "tenant",
                    TenantId = tenant.Id,
                    UserId = null,
                    Name = tenant.BusinessName
                });
            }

            // ── Step 2: Check Users ────────────────────────────────
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => u.Email == req.Username || u.Username == req.Username,
                    ct);

            if (user is not null && user.Status == UserStatus.Active)
            {
                if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                    return Result.Failure<LoginResponse>("Invalid credentials");

                var tokens = _tokens.GenerateForUser(user);
                return Result.Success(new LoginResponse
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken,
                    Type = "user",
                    TenantId = user.TenantId,
                    UserId = user.Id,
                    Name = user.FirstName + " " + user.LastName
                });
            }

            // ── Step 3: Not found in either table ──────────────────
            return Result.Failure<LoginResponse>("Invalid credentials");
        }
    }


}
