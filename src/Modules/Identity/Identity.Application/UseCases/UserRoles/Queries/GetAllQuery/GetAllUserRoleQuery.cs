using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Dtos.UserRole;

namespace Identity.Application.UseCases.UserRoles.Queries.GetAllQuery;

public class GetAllUserRoleQuery : BaseFilters, IQuery<IEnumerable<UserRoleResponseDto>> { }
