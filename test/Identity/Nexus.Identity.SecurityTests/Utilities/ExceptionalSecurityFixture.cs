using System.Data;
using Identity.Application.Dtos.Users;
using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;

namespace Nexus.Identity.SecurityTests.Utilities;

public sealed class ThrowingUnitOfWork(string errorMessage) : IUnitOfWork
{
    public IUserRepository User { get; } = new ThrowingUserRepository(errorMessage);
    public IMenuRepository Menu { get; } = new FakeMenuRepository();
    public IGenericRepository<Role> Role { get; } = new FakeGenericRepository<Role>();
    public IGenericRepository<UserRole> UserRole { get; } = new FakeGenericRepository<UserRole>();
    public IPermissionRepository Permission { get; } = new FakePermissionRepository();
    public IRefreshTokenRepository RefreshToken { get; } = new FakeRefreshTokenRepository();

    public Task SaveChangesAsync() => Task.CompletedTask;
    public IDbTransaction BeginTransaction() => throw new NotSupportedException();
    public void Dispose() { }
}

public sealed class ThrowingUserRepository(string errorMessage) : IUserRepository
{
    public Task<User> UserByEmailAsync(string email) => throw new Exception(errorMessage);
    public Task<UserWithRoleAndPermissionsDto> GetUserWithRoleAndPermissionsAsync(int userId) => throw new Exception(errorMessage);
    public IQueryable<User> GetAllQueryable() => throw new Exception(errorMessage);
    public Task<IEnumerable<User>> GetAllAsync() => throw new Exception(errorMessage);
    public Task<User> GetByIdAsync(int id) => throw new Exception(errorMessage);
    public Task CreateAsync(User entity) => throw new Exception(errorMessage);
    public void UpdateAsync(User entity) => throw new Exception(errorMessage);
    public Task DeleteAsync(int id) => throw new Exception(errorMessage);
}
