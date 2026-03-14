using Identity.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Authentication;

public class PermissionService : IPermissionService
{
    private readonly ApplicationDbContext _context;

    public PermissionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HashSet<string>> GetPermissionAsync(int userId)
    {
        //var permissions = await _context.Users
        //.Where(u => u.Id == userId)
        //.Join(
        //    _context.Roles,
        //    u => u.Id,
        //    r => r.Id,
        //    (u, r) => new { Role = r }
        //)
        //.Join(
        //    _context.RolePermissions,
        //    ur => ur.Role.Id,
        //    rp => rp.RoleId,
        //    (ur, rp) => new { RolePermission = rp }
        //)
        //.Join(
        //    _context.Permissions,
        //    urp => urp.RolePermission.PermissionId,
        //    p => p.Id,
        //    (urp, p) => p.Name
        //)
        //.ToListAsync();

        var permissions = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => _context.RolePermissions
                .Where(rp => rp.RoleId == ur.RoleId)
                .Select(rp => rp.Permission.Name)
        )
        .ToListAsync();

        return new HashSet<string>(permissions);
    }
}
