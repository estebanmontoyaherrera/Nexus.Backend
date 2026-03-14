namespace Identity.Application.Dtos.UserRole;

public record UserRoleByIdResponseDto
{
    public int UserRoleId {  get; init; }
    public string UserId { get; init; } = null!;
    public string RoleId { get; init; } = null!;
    public string? State { get; init; }
}
