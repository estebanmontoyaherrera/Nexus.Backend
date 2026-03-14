namespace Identity.Domain.Entities;

public class Permission : BaseEntity
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string Slug { get; set; } = null!;
    public int MenuId { get; set; }
    public virtual Menu Menu { get; set; } = null!;
}
