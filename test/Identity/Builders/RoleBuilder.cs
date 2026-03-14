using Bogus;
using Identity.Domain.Entities;

namespace Identity.Tests.Builders;

public sealed class RoleBuilder
{
    private readonly Faker _faker = new();
    private string _name;
    private string? _description;
    private string _state = "1";

    public RoleBuilder()
    {
        _name = _faker.Name.JobTitle();
        _description = _faker.Lorem.Sentence();
    }

    public RoleBuilder WithName(string name) { _name = name; return this; }
    public RoleBuilder WithDescription(string description) { _description = description; return this; }
    public RoleBuilder WithState(string state) { _state = state; return this; }

    public Role Build() => new()
    {
        Name = _name,
        Description = _description,
        State = _state,
        AuditCreateUser = 1,
        AuditCreateDate = DateTime.UtcNow
    };
}
