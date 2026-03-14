using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Users.Commands.DeleteCommand;

public class DeleteUserCommand : ICommand<bool>
{
    public int UserId { get; set; }
}
