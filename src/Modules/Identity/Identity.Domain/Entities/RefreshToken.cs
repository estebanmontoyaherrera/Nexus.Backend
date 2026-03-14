namespace Identity.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = null!;
    public int UserId { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    public User User { get; set; } = null!;
}
