using System.Data;
using Bogus;
using Identity.Application.Dtos.Users;
using Identity.Application.Interfaces.Authentication;
using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;

namespace Nexus.Identity.SecurityTests.Utilities;

public sealed class SecurityTestFixture
{
    private readonly Faker _faker = new();

    public User BuildUser(string? email = null, string? passwordHash = null)
        => new()
        {
            Id = _faker.Random.Int(1, 1000),
            FirstName = _faker.Name.FirstName(),
            LastName = _faker.Name.LastName(),
            Email = email ?? _faker.Internet.Email(),
            Password = passwordHash ?? BCrypt.Net.BCrypt.HashPassword("Secure#Password1"),
            State = "1"
        };

    public FakeUnitOfWork BuildUnitOfWork(User? existingUser = null)
        => new(existingUser);
}

public sealed class FakeJwtTokenGenerator : IJwtTokenGenerator
{
    public string GenerateToken(User user) => $"token-for-{user.Id}";
    public string GenerateRefreshToken() => Guid.NewGuid().ToString("N");
}

public sealed class FakeUnitOfWork : IUnitOfWork
{
    public FakeUnitOfWork(User? existingUser)
    {
        User = new FakeUserRepository(existingUser);
        RefreshToken = new FakeRefreshTokenRepository();
        Menu = new FakeMenuRepository();
        Role = new FakeGenericRepository<Role>();
        UserRole = new FakeGenericRepository<UserRole>();
        Permission = new FakePermissionRepository();
    }

    public IUserRepository User { get; }
    public IMenuRepository Menu { get; }
    public IGenericRepository<Role> Role { get; }
    public IGenericRepository<UserRole> UserRole { get; }
    public IPermissionRepository Permission { get; }
    public IRefreshTokenRepository RefreshToken { get; }
    public int SaveChangesCalls { get; private set; }

    public Task SaveChangesAsync()
    {
        SaveChangesCalls++;
        return Task.CompletedTask;
    }

    public IDbTransaction BeginTransaction() => throw new NotSupportedException();
    public void Dispose() { }
}

public sealed class FakeUserRepository(User? existingUser) : IUserRepository
{
    public int UserByEmailCalls { get; private set; }
    public User? LastCreatedUser { get; private set; }

    public Task<User> UserByEmailAsync(string email)
    {
        UserByEmailCalls++;
        if (existingUser is null || !string.Equals(existingUser.Email, email, StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult<User>(null!);
        }

        return Task.FromResult(existingUser);
    }

    public Task<UserWithRoleAndPermissionsDto> GetUserWithRoleAndPermissionsAsync(int userId)
        => Task.FromResult<UserWithRoleAndPermissionsDto>(null!);

    public IQueryable<User> GetAllQueryable() => new List<User>().AsQueryable();
    public Task<IEnumerable<User>> GetAllAsync() => Task.FromResult<IEnumerable<User>>(Array.Empty<User>());
    public Task<User> GetByIdAsync(int id) => Task.FromResult<User>(null!);

    public Task CreateAsync(User entity)
    {
        LastCreatedUser = entity;
        return Task.CompletedTask;
    }

    public void UpdateAsync(User entity) { }
    public Task DeleteAsync(int id) => Task.CompletedTask;
}

public sealed class FakeRefreshTokenRepository : IRefreshTokenRepository
{
    public readonly List<RefreshToken> Tokens = [];

    public void CreateToken(RefreshToken refreshToken) => Tokens.Add(refreshToken);

    public Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        => Task.FromResult(Tokens.FirstOrDefault(x => x.Token == refreshToken)!);

    public Task<bool> RevokeRefreshTokenAsync(int userId)
    {
        Tokens.RemoveAll(t => t.UserId == userId);
        return Task.FromResult(true);
    }
}

public sealed class FakeMenuRepository : IMenuRepository
{
    public Task<bool> DeleteMenuRole(List<MenuRole> menuRoles) => Task.FromResult(true);
    public Task<IEnumerable<Menu>> GetMenuByUserIdAsync(int userId) => Task.FromResult<IEnumerable<Menu>>(Array.Empty<Menu>());
    public Task<IEnumerable<Menu>> GetMenuPermissionByRoleIdAsync(int? roleId) => Task.FromResult<IEnumerable<Menu>>(Array.Empty<Menu>());
    public Task<List<MenuRole>> GetMenuRolesByRoleId(int roleId) => Task.FromResult(new List<MenuRole>());
    public Task<bool> RegisterRoleMenus(IEnumerable<MenuRole> menuRoles) => Task.FromResult(true);
}

public sealed class FakePermissionRepository : IPermissionRepository
{
    public Task<bool> DeleteRolePermission(List<RolePermission> permissions) => Task.FromResult(true);
    public Task<List<RolePermission>> GetPermissionRolesByRoleId(int roleId) => Task.FromResult(new List<RolePermission>());
    public Task<IEnumerable<Permission>> GetPermissionsByMenuId(int menuId) => Task.FromResult<IEnumerable<Permission>>(Array.Empty<Permission>());
    public Task<IEnumerable<Permission>> GetRolePermissionsByMenuId(int roleId, int menuId) => Task.FromResult<IEnumerable<Permission>>(Array.Empty<Permission>());
    public Task<bool> RegisterRolePermissions(IEnumerable<RolePermission> rolePermissions) => Task.FromResult(true);
}

public sealed class FakeGenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
{
    public IQueryable<T> GetAllQueryable() => new List<T>().AsQueryable();
    public Task<IEnumerable<T>> GetAllAsync() => Task.FromResult<IEnumerable<T>>(Array.Empty<T>());
    public Task<T> GetByIdAsync(int id) => Task.FromResult(new T());
    public Task CreateAsync(T entity) => Task.CompletedTask;
    public void UpdateAsync(T entity) { }
    public Task DeleteAsync(int id) => Task.CompletedTask;
}
