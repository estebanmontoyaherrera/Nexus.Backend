using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Dtos.Users;

namespace Identity.Application.UseCases.Users.Queries.GetAllQuery;

public class GetAllUserQuery : BaseFilters, IQuery<IEnumerable<UserResponseDto>> { }
