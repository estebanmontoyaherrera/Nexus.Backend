namespace Identity.Application.Dtos.Menus;

public record MenuResponseDto
{
    public int MenuId { get; init; }
    public string? Item { get; init; }
    public string? Icon { get; init; }
    public string? Path { get; init; }
    public int FatherId { get; init; }
}
