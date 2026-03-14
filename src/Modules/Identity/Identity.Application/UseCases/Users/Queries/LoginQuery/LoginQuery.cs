using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Users.Queries.LoginQuery;

public class LoginQuery : IQuery<string>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
