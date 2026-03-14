using Bogus;
using Identity.Application.UseCases.Users.Commands.CreateCommand;

namespace Identity.Tests.Builders;

public sealed class CreateUserCommandBuilder
{
    private readonly Faker _faker = new();
    private string _firstName;
    private string _lastName;
    private string _email;
    private string _password;

    public CreateUserCommandBuilder()
    {
        _firstName = _faker.Name.FirstName();
        _lastName = _faker.Name.LastName();
        _email = _faker.Internet.Email();
        _password = "P@ssw0rd!";
    }

    public CreateUserCommandBuilder WithEmail(string email) { _email = email; return this; }
    public CreateUserCommandBuilder WithPassword(string password) { _password = password; return this; }

    public CreateUserCommand Build() => new()
    {
        FirstName = _firstName,
        LastName = _lastName,
        Email = _email,
        Password = _password,
        State = "1"
    };
}
