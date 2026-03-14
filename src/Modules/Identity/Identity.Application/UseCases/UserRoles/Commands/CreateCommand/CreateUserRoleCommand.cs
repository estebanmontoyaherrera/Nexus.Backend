using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.UserRoles.Commands.CreateCommand;

public class CreateUserRoleCommand : ICommand<bool>
{
    public int UserId { get; init; }
    public int RoleId { get; init; }
    public string? State { get; init; }
}
