using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Dtos.Roles;

namespace Identity.Application.UseCases.Roles.Queries.GetAllQuery;

public class GetAllRoleQuery : BaseFilters, IQuery<IEnumerable<RoleResponseDto>> { }
