using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Roles.Commands.DeleteCommand;

public class DeleteRoleCommand : ICommand<bool>
{
    public int RoleId { get; set; }
}
