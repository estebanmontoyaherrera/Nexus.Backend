using SharedKernel.Abstractions.Messaging;
using Identity.Application.Dtos.Permissions;

namespace Identity.Application.UseCases.Permissions.Queries.GetByIdQuery;

public class GetPermissionsByRoleIdQuery : IQuery<IEnumerable<PermissionsByRoleResponseDto>>
{
    public int? RoleId {  get; set; } 
}
