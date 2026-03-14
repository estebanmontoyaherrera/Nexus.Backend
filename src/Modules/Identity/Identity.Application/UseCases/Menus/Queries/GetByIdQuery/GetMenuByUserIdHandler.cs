using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Dtos.Menus;
using Identity.Application.Interfaces.Services;
using Mapster;

namespace Identity.Application.UseCases.Menus.Queries.GetByIdQuery;

public class GetMenuByUserIdHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetMenuByUserIdQuery, IEnumerable<MenuResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<IEnumerable<MenuResponseDto>>> Handle(GetMenuByUserIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<MenuResponseDto>>();

        try
        {
            var menus = await _unitOfWork.Menu.GetMenuByUserIdAsync(request.UserId);

            if (menus is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron registros.";
                return response;
            }

            response.IsSuccess = true;
            response.Data = menus.Adapt<IEnumerable<MenuResponseDto>>();
            response.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
