using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.UserRoles.Commands.UpdateCommand;

public class UpdateUserRoleCommand : ICommand<bool>
{
    public int UserRoleId {  get; init; }
    public int UserId { get; init; }
    public int RoleId { get; init; }
    public string? State { get; init; }
}
