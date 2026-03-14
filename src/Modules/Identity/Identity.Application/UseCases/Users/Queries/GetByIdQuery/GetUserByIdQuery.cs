using SharedKernel.Abstractions.Messaging;
using Identity.Application.Dtos.Users;

namespace Identity.Application.UseCases.Users.Queries.GetByIdQuery;

public class GetUserByIdQuery : IQuery<UserByIdResponseDto>
{
    public int UserId { get; set; }
}
