using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;
using Identity.Application.Dtos.Roles;
using Identity.Application.UseCases.Roles.Commands.CreateCommand;
using Identity.Application.UseCases.Roles.Commands.DeleteCommand;
using Identity.Application.UseCases.Roles.Commands.UpdateCommand;
using Identity.Application.UseCases.Roles.Queries.GetAllQuery;
using Identity.Application.UseCases.Roles.Queries.GetByIdQuery;
using Identity.Application.UseCases.Roles.Queries.GetSelectQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RoleController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpGet]
    public async Task<IActionResult> RoleList([FromQuery] GetAllRoleQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllRoleQuery, IEnumerable<RoleResponseDto>>
            (query, CancellationToken.None);

        return Ok(response);
    }

    [HttpGet("Select")]
    public async Task<IActionResult> RoleSelect()
    {
        var response = await _dispatcher
           .Dispatch<GetRoleSelectQuery, IEnumerable<SelectResponseDto>>
           (new GetRoleSelectQuery(), CancellationToken.None);

        return Ok(response);
    }

    [HttpGet("{roleId:int}")]
    public async Task<IActionResult> RoleById(int roleId)
    {
        var response = await _dispatcher.Dispatch<GetRoleByIdQuery, RoleByIdResponseDto>
            (new GetRoleByIdQuery() { RoleId = roleId }, CancellationToken.None);

        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> RoleCreate([FromBody] CreateRoleCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateRoleCommand, bool>
           (command, CancellationToken.None);

        return Ok(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> RoleUpdate([FromBody] UpdateRoleCommand command)
    {
        var response = await _dispatcher.Dispatch<UpdateRoleCommand, bool>
           (command, CancellationToken.None);

        return Ok(response);
    }

    [HttpPut("Delete/{roleId:int}")]
    public async Task<IActionResult> RoleDelete(int roleId)
    {
        var response = await _dispatcher.Dispatch<DeleteRoleCommand, bool>
            (new DeleteRoleCommand() { RoleId = roleId }, CancellationToken.None);

        return Ok(response);
    }
}
