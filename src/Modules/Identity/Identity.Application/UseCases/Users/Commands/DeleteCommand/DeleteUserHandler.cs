using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Interfaces.Services;

namespace Identity.Application.UseCases.Users.Commands.DeleteCommand;

public class DeleteUserHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var existsUser = await _unitOfWork.User.GetByIdAsync(request.UserId);

            if (existsUser is null)
            {
                response.IsSuccess = false;
                response.Message = "El usuario no existe en la base de datos.";
                return response;
            }

            await _unitOfWork.User.DeleteAsync(request.UserId);
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
