namespace Identity.Domain.Entities;

public class Menu : BaseEntity
{
    public int Position { get; init; }
    public string Name { get; init; } = null!;
    public string? Icon { get; init; }
    public string? Url { get; init; }
    public int? FatherId { get; init; }
}
