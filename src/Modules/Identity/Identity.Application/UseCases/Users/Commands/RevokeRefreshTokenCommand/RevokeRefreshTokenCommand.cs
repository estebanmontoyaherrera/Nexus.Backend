using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Users.Commands.RevokeRefreshTokenCommand;

public class RevokeRefreshTokenCommand : ICommand<bool>
{
    public int UserId { get; set; }
}
