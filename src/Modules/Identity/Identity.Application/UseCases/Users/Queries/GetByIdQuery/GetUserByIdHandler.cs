using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Dtos.Users;
using Identity.Application.Interfaces.Services;
using Mapster;

namespace Identity.Application.UseCases.Users.Queries.GetByIdQuery;

public class GetUserByIdHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetUserByIdQuery, UserByIdResponseDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<UserByIdResponseDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<UserByIdResponseDto>();

        try
        {
            var user = await _unitOfWork.User.GetByIdAsync(request.UserId);

            if (user is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron registros.";
                return response;
            }

            response.IsSuccess = true;
            response.Data = user.Adapt<UserByIdResponseDto>();
            response.Message = "Consulta exitosa";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
