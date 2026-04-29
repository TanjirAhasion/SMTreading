using MediatR;
using SMT.Application.Auth.DTO;
using SMT.Application.Common;
using SMT.Application.DTO.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Auth.Commands
{
    public sealed record CreateUserCommand(CreateUserRequest Request, Guid CallerTenantId)
    : IRequest<Result<UserResponse>>;
}
