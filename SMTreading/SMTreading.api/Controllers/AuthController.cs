using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.Auth.DTO;
using SMT.Application.DTO.Identity;
using SMT.Infrastructure.Auth.Commands;
using System.Security.Claims;

namespace SMTreading.api.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            CancellationToken ct)
        {
            var result = await mediator.Send(new LoginCommand(request), ct);

            return result.IsSuccess
                ? Ok(result.Value)
                : Unauthorized(new { message = result.Error });
        }

        [HttpPost("users")]
        [Authorize(Policy = "TenantOnly")]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserRequest request,
            CancellationToken ct)
        {
            var tenantId = Guid.Parse(User.FindFirstValue("tenantId")!);

            var result = await mediator.Send(new CreateUserCommand(request, tenantId), ct);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [Authorize]
        [HttpGet("debug")]
        public IActionResult Debug()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }

}
