using Bogus;
using Identity.Domain.Entities;

namespace Identity.Tests.Builders;

public sealed class UserBuilder
{
    private readonly Faker _faker = new();
    private string _firstName;
    private string _lastName;
    private string _email;
    private string _password;
    private string _state;

    public UserBuilder()
    {
        _firstName = _faker.Name.FirstName();
        _lastName = _faker.Name.LastName();
        _email = _faker.Internet.Email();
        _password = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd!");
        _state = "1";
    }

    public UserBuilder WithEmail(string email) { _email = email; return this; }
    public UserBuilder WithFirstName(string firstName) { _firstName = firstName; return this; }
    public UserBuilder WithLastName(string lastName) { _lastName = lastName; return this; }
    public UserBuilder WithPasswordHash(string passwordHash) { _password = passwordHash; return this; }
    public UserBuilder WithState(string state) { _state = state; return this; }

    public User Build() => new()
    {
        FirstName = _firstName,
        LastName = _lastName,
        Email = _email,
        Password = _password,
        State = _state,
        AuditCreateUser = 1,
        AuditCreateDate = DateTime.UtcNow
    };
}
