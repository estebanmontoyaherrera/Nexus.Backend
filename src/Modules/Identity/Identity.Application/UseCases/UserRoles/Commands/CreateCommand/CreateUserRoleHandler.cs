using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;

namespace Identity.Application.UseCases.UserRoles.Commands.CreateCommand;

public class CreateUserRoleHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateUserRoleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var userRole = request.Adapt<UserRole>();
            await _unitOfWork.UserRole.CreateAsync(userRole);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Registro exitoso";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
