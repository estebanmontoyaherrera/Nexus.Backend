using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Users.Commands.CreateCommand;

public class CreateUserCommand : ICommand<bool>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? State { get; set; }
}
