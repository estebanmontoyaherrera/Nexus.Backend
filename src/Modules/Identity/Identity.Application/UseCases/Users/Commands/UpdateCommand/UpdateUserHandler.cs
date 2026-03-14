using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;
using BC = BCrypt.Net.BCrypt;

namespace Identity.Application.UseCases.Users.Commands.UpdateCommand;

public class UpdateUserHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var user = request.Adapt<User>();
            user.Id = request.UserId;
            user.Password = BC.HashPassword(user.Password);
            _unitOfWork.User.UpdateAsync(user);
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
