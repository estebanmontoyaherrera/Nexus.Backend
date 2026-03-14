namespace Identity.Application.Dtos.Roles;

public record RoleByIdResponseDto
{
    public int RoleId { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string? State { get; init; }
}
