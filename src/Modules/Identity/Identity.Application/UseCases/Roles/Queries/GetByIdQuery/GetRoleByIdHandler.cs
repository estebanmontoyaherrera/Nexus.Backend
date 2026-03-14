using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Dtos.Roles;
using Identity.Application.Interfaces.Services;
using Mapster;

namespace Identity.Application.UseCases.Roles.Queries.GetByIdQuery;

public class GetRoleByIdHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetRoleByIdQuery, RoleByIdResponseDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<RoleByIdResponseDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<RoleByIdResponseDto>();

        try
        {
            var role = await _unitOfWork.Role.GetByIdAsync(request.RoleId);

            if (role is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron registros.";
                return response;
            }

            response.IsSuccess = true;
            response.Data = role.Adapt<RoleByIdResponseDto>();
            response.Message = "Consulta exitosa";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
