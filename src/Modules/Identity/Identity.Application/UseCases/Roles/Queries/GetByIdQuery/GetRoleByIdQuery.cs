using SharedKernel.Abstractions.Messaging;
using Identity.Application.Dtos.Roles;

namespace Identity.Application.UseCases.Roles.Queries.GetByIdQuery;

public class GetRoleByIdQuery : IQuery<RoleByIdResponseDto>
{
    public int RoleId { get; set; }
}
