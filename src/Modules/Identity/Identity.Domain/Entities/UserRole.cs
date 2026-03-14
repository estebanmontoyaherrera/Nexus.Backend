namespace Identity.Domain.Entities;

public class UserRole : BaseEntity
{
    public int UserId { get; init; }
    public int RoleId { get; init; }
    public User User { get; init; } = null!;
    public Role Role { get; init; } = null!;
}
