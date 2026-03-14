using SharedKernel.Abstractions.Messaging;
using Identity.Application.Dtos.Users;

namespace Identity.Application.UseCases.Users.Queries.UserRolePermissionsQuery;

public class GetUserWithRoleAndPermissionsQuery : IQuery<UserWithRoleAndPermissionsDto>
{
    public int UserId { get; set; }
}
