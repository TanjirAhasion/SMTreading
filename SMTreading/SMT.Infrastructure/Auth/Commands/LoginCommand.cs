using MediatR;
using SMT.Application.Auth.DTO;
using SMT.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Auth.Commands
{
    public sealed record LoginCommand(LoginRequest Request)
    : IRequest<Result<LoginResponse>>;

}
