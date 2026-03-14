using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Users.Commands.UpdateCommand;

public class UpdateUserCommand : ICommand<bool>
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? State { get; set; }
}
