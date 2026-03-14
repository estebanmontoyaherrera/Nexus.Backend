using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Users.Commands.LoginRefreshTokenCommand;

public class LoginRefreshTokenCommand : ICommand<string>
{
    public string? RefreshToken { get; set; }
}
