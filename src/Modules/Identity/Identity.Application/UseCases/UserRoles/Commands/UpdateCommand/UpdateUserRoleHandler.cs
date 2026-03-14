using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;

namespace Identity.Application.UseCases.UserRoles.Commands.UpdateCommand;

public class UpdateUserRoleHandler : ICommandHandler<UpdateUserRoleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserRoleHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<bool>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var userRole = request.Adapt<UserRole>();
            userRole.Id = request.UserRoleId;
            _unitOfWork.UserRole.UpdateAsync(userRole);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Actualización exitosa";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
