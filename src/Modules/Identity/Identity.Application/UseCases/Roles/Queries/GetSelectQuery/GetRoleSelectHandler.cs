using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;
using SharedKernel.Dtos.Commons;
using Identity.Application.Interfaces.Services;
using Mapster;

namespace Identity.Application.UseCases.Roles.Queries.GetSelectQuery;

public class GetRoleSelectHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetRoleSelectQuery, IEnumerable<SelectResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<IEnumerable<SelectResponseDto>>> Handle(GetRoleSelectQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<SelectResponseDto>>();

        try
        {
            var roles = await _unitOfWork.Role.GetAllAsync();

            if (roles is null)
            {
                response.IsSuccess = false;
                response.Message = GlobalMessages.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.IsSuccess = true;
            response.Data = roles.Adapt<IEnumerable<SelectResponseDto>>();
            response.Message = GlobalMessages.MESSAGE_QUERY;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
