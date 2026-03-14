using SharedKernel.Abstractions.Messaging;
using Identity.Application.Dtos.UserRole;

namespace Identity.Application.UseCases.UserRoles.Queries.GetByIdQuery;

public class GetUserRoleByIdQuery : IQuery<UserRoleByIdResponseDto>
{
    public int UserRoleId { get; init; }
}
