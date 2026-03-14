using System.Data;

namespace Identity.Domain.Entities;

public class MenuRole : BaseEntity
{
    public int MenuId { get; init; }
    public int RoleId { get; init; }
    public Menu Menu { get; init; } = null!;
    public Role Role { get; init; } = null!;
}
