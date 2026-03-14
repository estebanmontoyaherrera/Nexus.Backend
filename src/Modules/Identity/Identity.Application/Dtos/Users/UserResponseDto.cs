namespace Identity.Application.Dtos.Users;

public record UserResponseDto
{
    public int UserId { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? State { get; init; }
    public string? StateDescription { get; init; }
    public DateTime AuditCreateDate { get; init; }  
}
