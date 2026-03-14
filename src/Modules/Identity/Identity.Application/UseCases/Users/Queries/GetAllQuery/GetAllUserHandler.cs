using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Dtos.Users;
using Identity.Application.Interfaces.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Helper = SharedKernel.Helpers.Helpers;

namespace Identity.Application.UseCases.Users.Queries.GetAllQuery;

public class GetAllUserHandler(IUnitOfWork unitOfWork, IOrderingQuery orderingQuery) : IQueryHandler<GetAllUserQuery, IEnumerable<UserResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOrderingQuery _orderingQuery = orderingQuery;

    public async Task<BaseResponse<IEnumerable<UserResponseDto>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<UserResponseDto>>();

        try
        {
            var users = _unitOfWork.User.GetAllQueryable();

            if (request.NumFilter is not null && !string.IsNullOrEmpty(request.TextFilter))
            {
                switch (request.NumFilter)
                {
                    case 1:
                        users = users.Where(x => x.FirstName.Contains(request.TextFilter));
                        break;
                    case 2:
                        users = users.Where(x => x.LastName.Contains(request.TextFilter));
                        break;
                }
            }

            if (request.StateFilter is not null)
            {
                var stateFilter = Helper.SplitStateFilter(request.StateFilter);
                users = users.Where(x => stateFilter.Contains(x.State));
            }

            if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
            {
                users = users.Where(x => x.AuditCreateDate >= Convert.ToDateTime(request.StartDate) &&
                    x.AuditCreateDate <= Convert.ToDateTime(request.EndDate).AddDays(1));
            }

            request.Sort ??= "Id";

            var items = await _orderingQuery.Ordering(request, users).ToListAsync(cancellationToken);

            response.IsSuccess = true;
            response.TotalRecords = await users.CountAsync(cancellationToken);
            response.Data = items.Adapt<IEnumerable<UserResponseDto>>();
            response.Message = "Consulta existosa.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
