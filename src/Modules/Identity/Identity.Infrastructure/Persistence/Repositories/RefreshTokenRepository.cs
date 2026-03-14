using Identity.Application.Interfaces.Persistence;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context) : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context = context;

    public void CreateToken(RefreshToken refreshToken)
    {
        _context.Add(refreshToken);
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == refreshToken);

        return token!;
    }

    public async Task<bool> RevokeRefreshTokenAsync(int userId)
    {
        await _context.RefreshTokens
            .Where(x => x.UserId == userId)
            .ExecuteDeleteAsync();

        return true;
    }
}
