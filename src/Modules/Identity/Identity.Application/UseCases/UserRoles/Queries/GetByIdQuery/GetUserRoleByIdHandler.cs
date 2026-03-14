using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Dtos.UserRole;
using Identity.Application.Interfaces.Services;
using Mapster;

namespace Identity.Application.UseCases.UserRoles.Queries.GetByIdQuery;

public class GetUserRoleByIdHandler : IQueryHandler<GetUserRoleByIdQuery, UserRoleByIdResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserRoleByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<UserRoleByIdResponseDto>> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<UserRoleByIdResponseDto>();

        try
        {
            var userRole = await _unitOfWork.UserRole.GetByIdAsync(request.UserRoleId);

            if (userRole is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron registros.";
                return response;
            }

            response.IsSuccess = true;
            response.Data = userRole.Adapt<UserRoleByIdResponseDto>();
            response.Message = "Consulta exitosa";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
