using MediatR;
using SMT.Application.Auth.DTO;
using SMT.Application.Common;
using SMT.Domain.Entities;
using SMT.Infrastructure.context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SMT.Application.Auth.Interfaces;

namespace SMT.Infrastructure.Auth.Commands
{
    public sealed class CreateUserCommandHandler(
    AppDbContext db,
    IPasswordHasher hasher)
    : IRequestHandler<CreateUserCommand, Result<UserResponse>>
    {
        public async Task<Result<UserResponse>> Handle(
        CreateUserCommand cmd,
        CancellationToken ct)
        {
            var req = cmd.Request;

            // ── 1. Check username against Users table ──────────────────────
            bool usernameInUsers = await db.Users
                .AsNoTracking()
                .AnyAsync(u => u.Username == req.Username, ct);

            if (usernameInUsers)
                return Result<UserResponse>.Failure("Username is already taken");

            // ── 2. Check username against Tenants table ────────────────────
            bool usernameInTenants = await db.Tenants
                .AsNoTracking()
                .AnyAsync(t => t.UserName == req.Username
                            || t.Email == req.Username, ct);

            if (usernameInTenants)
                return Result<UserResponse>.Failure("Username is already taken");

            // ── 3. Check email against Users table ─────────────────────────
            bool emailInUsers = await db.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == req.Email, ct);

            if (emailInUsers)
                return Result<UserResponse>.Failure("Email is already registered");

            // ── 4. Check email against Tenants table ───────────────────────
            bool emailInTenants = await db.Tenants
                .AsNoTracking()
                .AnyAsync(t => t.Email == req.Email, ct);

            if (emailInTenants)
                return Result<UserResponse>.Failure("Email is already registered");

            // ── 5. Hash password & build entity ───────────────────────────
            var user = User.Create(
                tenantId: cmd.CallerTenantId,
                username: req.Username,
                passwordHash: hasher.Hash(req.Password),
                firstName: req.FirstName,
                lastName: req.LastName,
                phone: req.Phone,
                avatarUrl : req.AvatarUrl,
                email: req.Email
            );
            // ── 6. Save ───────────────────────────────────────────────────
            db.Users.Add(user);
            await db.SaveChangesAsync(ct);

            // ── 7. Return ─────────────────────────────────────────────────
            return Result<UserResponse>.Success(new UserResponse
            {
                Id = user.Id,
                TenantId = user.TenantId,
                Username = user.Username,
                FullName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                Status = user.Status
            });
        }
    }
}
