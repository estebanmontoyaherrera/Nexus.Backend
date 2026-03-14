using SharedKernel.Abstractions.Messaging;
using Identity.Application.Dtos.Menus;
using Identity.Application.UseCases.Menus.Queries.GetByIdQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MenuController(IDispatcher dispatcher, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;

    [HttpGet("MenuByUser")]
    public async Task<IActionResult> GetMenuByUserId()
    {
        var userId = _httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var response = await _dispatcher.Dispatch<GetMenuByUserIdQuery, IEnumerable<MenuResponseDto>>
            (new GetMenuByUserIdQuery() { UserId = int.Parse(userId!) }, CancellationToken.None);

        return Ok(response);
    }
}
