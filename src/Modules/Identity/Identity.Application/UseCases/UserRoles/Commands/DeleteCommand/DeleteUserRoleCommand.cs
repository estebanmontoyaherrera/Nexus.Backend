using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.UserRoles.Commands.DeleteCommand;

public class DeleteUserRoleCommand : ICommand<bool>
{
    public int UserRoleId { get; init; }
}
