using SharedKernel.Abstractions.Messaging;
using Identity.Application.Dtos.UserRole;
using Identity.Application.UseCases.UserRoles.Commands.CreateCommand;
using Identity.Application.UseCases.UserRoles.Commands.DeleteCommand;
using Identity.Application.UseCases.UserRoles.Commands.UpdateCommand;
using Identity.Application.UseCases.UserRoles.Queries.GetAllQuery;
using Identity.Application.UseCases.UserRoles.Queries.GetByIdQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserRoleController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpGet]
    public async Task<IActionResult> UserRoleList([FromQuery] GetAllUserRoleQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllUserRoleQuery, IEnumerable<UserRoleResponseDto>>
            (query, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("{userRoleId:int}")]
    public async Task<IActionResult> UserRoleById(int userRoleId)
    {
        var response = await _dispatcher.Dispatch<GetUserRoleByIdQuery, UserRoleByIdResponseDto>
            (new GetUserRoleByIdQuery() { UserRoleId = userRoleId }, CancellationToken.None);
        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> UserRoleCreate([FromBody] CreateUserRoleCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateUserRoleCommand, bool>
          (command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UserRoleUpdate([FromBody] UpdateUserRoleCommand command)
    {
        var response = await _dispatcher.Dispatch<UpdateUserRoleCommand, bool>
          (command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("Delete/{userRoleId:int}")]
    public async Task<IActionResult> UserRoleDelete(int userRoleId)
    {
        var response = await _dispatcher.Dispatch<DeleteUserRoleCommand, bool>
           (new DeleteUserRoleCommand() { UserRoleId = userRoleId }, CancellationToken.None);
        return Ok(response);
    }
}
