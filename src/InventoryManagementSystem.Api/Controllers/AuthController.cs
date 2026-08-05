using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Auth.Commands.Register;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command, CancellationToken cancellationToken)
    {
        var userId = await mediator.Send(command, cancellationToken);
        return Ok(new { id = userId });
    }
}
