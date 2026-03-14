namespace Identity.Application.Dtos.Users;

public record UserByIdResponseDto
{
    public int UserId { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? State { get; init; }
}
