using SharedKernel.Abstractions.Messaging;
using Identity.Application.Dtos.Menus;

namespace Identity.Application.UseCases.Menus.Queries.GetByIdQuery;

public class GetMenuByUserIdQuery : IQuery<IEnumerable<MenuResponseDto>>
{
    public int UserId { get; set; }
}
