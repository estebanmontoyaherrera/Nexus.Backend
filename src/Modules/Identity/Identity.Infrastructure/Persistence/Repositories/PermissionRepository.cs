using Identity.Application.Interfaces.Persistence;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly ApplicationDbContext _context;

    public PermissionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteRolePermission(List<RolePermission> permissions)
    {
        _context.RolePermissions.RemoveRange(permissions);
        var recordsAffected = await _context.SaveChangesAsync();
        return recordsAffected > 0;
    }

    public async Task<List<RolePermission>> GetPermissionRolesByRoleId(int roleId)
    {
        return await _context.RolePermissions
                .AsNoTracking()
                .Where(pr => pr.RoleId == roleId)
                .ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetPermissionsByMenuId(int menuId)
    {
        var menuPermissions = await _context.Permissions
                .AsNoTracking()
                .Where(x => x.MenuId == menuId)
                .ToListAsync();

        return menuPermissions;
    }

    public async Task<IEnumerable<Permission>> GetRolePermissionsByMenuId(int roleId, int menuId)
    {
        var rolePermissions = _context.RolePermissions
                .Where(pr => pr.RoleId == roleId && pr.Permission.MenuId == menuId)
                .Select(pr => pr.Permission);

        var data = await rolePermissions.ToListAsync();

        return rolePermissions;
    }

    public async Task<bool> RegisterRolePermissions(IEnumerable<RolePermission> rolePermissions)
    {
        foreach (var rolePermission in rolePermissions)
        {
            rolePermission.AuditCreateUser = 1;
            rolePermission.AuditCreateDate = DateTime.Now;
            rolePermission.State = "1";

            _context.RolePermissions.Add(rolePermission);
        }

        var recordsAffected = await _context.SaveChangesAsync();
        return recordsAffected > 0;
    }
}
