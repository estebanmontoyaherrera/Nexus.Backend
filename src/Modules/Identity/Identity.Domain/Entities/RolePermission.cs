namespace Identity.Domain.Entities;

public class RolePermission : BaseEntity
{
    public int RoleId { get; init; }
    public int PermissionId { get; init; }
    public Role Role { get; init; } = null!;
    public Permission Permission { get; init; } = null!;
}
