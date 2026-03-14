using Identity.Application.Interfaces.Persistence;
using Identity.Domain.Entities;
using System.Data;

namespace Identity.Application.Interfaces.Services;

public interface IUnitOfWork : IDisposable
{
    IUserRepository User { get; }
    IMenuRepository Menu { get; }
    IGenericRepository<Role> Role { get; }
    IGenericRepository<UserRole> UserRole { get; }
    IPermissionRepository Permission { get; }
    IRefreshTokenRepository RefreshToken { get; }
    Task SaveChangesAsync();
    IDbTransaction BeginTransaction();
}
