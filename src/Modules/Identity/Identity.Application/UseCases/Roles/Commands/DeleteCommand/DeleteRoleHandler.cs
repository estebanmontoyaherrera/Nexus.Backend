using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Interfaces.Services;

namespace Identity.Application.UseCases.Roles.Commands.DeleteCommand;

public class DeleteRoleHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteRoleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var existsRole = await _unitOfWork.Role.GetByIdAsync(request.RoleId);

            if (existsRole is null)
            {
                response.IsSuccess = false;
                response.Message = "El rol no existe en la base de datos.";
                return response;
            }

            await _unitOfWork.Role.DeleteAsync(request.RoleId);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Eliminación exitosa.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
