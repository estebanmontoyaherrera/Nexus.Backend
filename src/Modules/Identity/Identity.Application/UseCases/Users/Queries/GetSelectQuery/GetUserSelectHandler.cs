using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;
using SharedKernel.Dtos.Commons;
using Identity.Application.Interfaces.Services;
using Mapster;

namespace Identity.Application.UseCases.Users.Queries.GetSelectQuery;

public class GetUserSelectHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetUserSelectQuery, IEnumerable<SelectResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<IEnumerable<SelectResponseDto>>> Handle(GetUserSelectQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<SelectResponseDto>>();

        try
        {
            var users = await _unitOfWork.User.GetAllAsync();

            if (users is null)
            {
                response.IsSuccess = false;
                response.Message = GlobalMessages.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.IsSuccess = true;
            response.Data = users.Adapt<IEnumerable<SelectResponseDto>>();
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
