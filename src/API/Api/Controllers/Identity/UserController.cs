using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;
using Identity.Application.Dtos.Users;
using SharedKernel.Helpers;
using Identity.Application.Interfaces.Services;
using Identity.Application.UseCases.Users.Commands.CreateCommand;
using Identity.Application.UseCases.Users.Commands.DeleteCommand;
using Identity.Application.UseCases.Users.Commands.UpdateCommand;
using Identity.Application.UseCases.Users.Queries.GetAllQuery;
using Identity.Application.UseCases.Users.Queries.GetByIdQuery;
using Identity.Application.UseCases.Users.Queries.GetSelectQuery;
using Identity.Application.UseCases.Users.Queries.UserRolePermissionsQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController(IDispatcher dispatcher, IExcelService excelService, IPdfService pdfService) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;
    private readonly IExcelService _excelService = excelService;
    private readonly IPdfService _pdfService = pdfService;

    [HttpGet]
    public async Task<IActionResult> UserList([FromQuery] GetAllUserQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllUserQuery, IEnumerable<UserResponseDto>>
            (query, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("Excel")]
    public async Task<IActionResult> UserReportExcel([FromQuery] GetAllUserQuery query)
    {
        var response = await _dispatcher
            .Dispatch<GetAllUserQuery, IEnumerable<UserResponseDto>>(query, CancellationToken.None);

        var columnNames = ReportColumns.GetColumnsUsers();
        var fileBytes = _excelService.GenerateToExcel(response.Data!, columnNames);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [HttpGet("Pdf")]
    public async Task<IActionResult> UserReportPdf([FromQuery] GetAllUserQuery query)
    {
        var response = await _dispatcher
            .Dispatch<GetAllUserQuery, IEnumerable<UserResponseDto>>(query, CancellationToken.None);

        var columnNames = ReportColumns.GetColumnsUsers();
        var fileBytes = _pdfService.GenerateToPdf(response.Data!, columnNames, "Usuarios");
        return File(fileBytes, "application/pdf");
    }

    [HttpGet("Select")]
    public async Task<IActionResult> UserSelect()
    {
        var response = await _dispatcher
          .Dispatch<GetUserSelectQuery, IEnumerable<SelectResponseDto>>
          (new GetUserSelectQuery(), CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> UserById(int userId)
    {
        var response = await _dispatcher.Dispatch<GetUserByIdQuery, UserByIdResponseDto>
            (new GetUserByIdQuery() { UserId = userId }, CancellationToken.None);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("UserWithRoleAndPermissions/{userId:int}")]
    public async Task<IActionResult> UserWithRoleAndPermissions(int userId)
    {
        var response = await _dispatcher.Dispatch<GetUserWithRoleAndPermissionsQuery, UserWithRoleAndPermissionsDto>
            (new GetUserWithRoleAndPermissionsQuery() { UserId = userId }, CancellationToken.None);
        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> UserCreate([FromBody] CreateUserCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateUserCommand, bool>
           (command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UserUpdate([FromBody] UpdateUserCommand command)
    {
        var response = await _dispatcher.Dispatch<UpdateUserCommand, bool>
           (command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("Delete/{userId:int}")]
    public async Task<IActionResult> UserDelete(int userId)
    {
        var response = await _dispatcher.Dispatch<DeleteUserCommand, bool>
            (new DeleteUserCommand() { UserId = userId }, CancellationToken.None);
        return Ok(response);
    }
}
