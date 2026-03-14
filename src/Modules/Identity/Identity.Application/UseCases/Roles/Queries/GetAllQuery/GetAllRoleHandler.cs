using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Dtos.Roles;
using Identity.Application.Interfaces.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Helper = SharedKernel.Helpers.Helpers;

namespace Identity.Application.UseCases.Roles.Queries.GetAllQuery;

public class GetAllRoleHandler(IUnitOfWork unitOfWork, IOrderingQuery orderingQuery) : IQueryHandler<GetAllRoleQuery, IEnumerable<RoleResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOrderingQuery _orderingQuery = orderingQuery;

    public async Task<BaseResponse<IEnumerable<RoleResponseDto>>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<RoleResponseDto>>();

        try
        {
            var roles = _unitOfWork.Role.GetAllQueryable();

            if (request.NumFilter is not null && !string.IsNullOrEmpty(request.TextFilter))
            {
                switch (request.NumFilter)
                {
                    case 1:
                        roles = roles.Where(x => x.Name.Contains(request.TextFilter));
                        break;
                    case 2:
                        roles = roles.Where(x => x.Description!.Contains(request.TextFilter));
                        break;
                }
            }

            if (request.StateFilter is not null)
            {
                var stateFilter = Helper.SplitStateFilter(request.StateFilter);
                roles = roles.Where(x => stateFilter.Contains(x.State));
            }

            if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
            {
                roles = roles.Where(x => x.AuditCreateDate >= Convert.ToDateTime(request.StartDate) &&
                    x.AuditCreateDate <= Convert.ToDateTime(request.EndDate).AddDays(1));
            }

            request.Sort ??= "Id";

            var items = await _orderingQuery.Ordering(request, roles).ToListAsync(cancellationToken);

            response.IsSuccess = true;
            response.TotalRecords = await roles.CountAsync(cancellationToken);
            response.Data = items.Adapt<IEnumerable<RoleResponseDto>>();
            response.Message = "Consulta existosa.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
