using Bogus;
using Identity.Domain.Entities;

namespace Identity.Tests.Builders;

public sealed class PermissionBuilder
{
    private readonly Faker _faker = new();
    private string _name;
    private string? _description;
    private string _slug;
    private int _menuId = 1;

    public PermissionBuilder()
    {
        _name = "Permission_" + _faker.Random.Word();
        _description = _faker.Lorem.Sentence();
        _slug = _faker.Internet.Slug();
    }

    public PermissionBuilder WithName(string name) { _name = name; return this; }
    public PermissionBuilder WithSlug(string slug) { _slug = slug; return this; }
    public PermissionBuilder WithMenuId(int menuId) { _menuId = menuId; return this; }

    public Permission Build() => new()
    {
        Name = _name,
        Description = _description,
        Slug = _slug,
        MenuId = _menuId,
        State = "1",
        AuditCreateUser = 1,
        AuditCreateDate = DateTime.UtcNow
    };
}
