using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Identity.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Identity.Infrastructure.Services;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private readonly IUserRepository _user = null!;
    private readonly IMenuRepository _menu = null!;
    private readonly IGenericRepository<Role> _role = null!;
    private readonly IGenericRepository<UserRole> _userRole = null!;
    private readonly IPermissionRepository _permission = null!;
    private readonly IRefreshTokenRepository _refreshToken = null!;

    public IUserRepository User => _user ?? new UserRepository(_context);
    public IMenuRepository Menu => _menu ?? new MenuRepository(_context);
    public IGenericRepository<Role> Role => _role ?? new GenericRepository<Role>(_context);
    public IGenericRepository<UserRole> UserRole => _userRole ?? new GenericRepository<UserRole>(_context);
    public IPermissionRepository Permission => _permission ?? new PermissionRepository(_context);
    public IRefreshTokenRepository RefreshToken => _refreshToken ?? new RefreshTokenRepository(_context);

    public IDbTransaction BeginTransaction()
    {
        var transaction = _context.Database.BeginTransaction();
        return transaction.GetDbTransaction();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
