using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;
using BC = BCrypt.Net.BCrypt;

namespace Identity.Application.UseCases.Users.Commands.CreateCommand;

public class CreateUserHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var user = request.Adapt<User>();
            user.Password = BC.HashPassword(user.Password);
            await _unitOfWork.User.CreateAsync(user);
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
