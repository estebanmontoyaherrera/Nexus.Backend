using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Persistence;

public interface IPermissionRepository
{
    Task<bool> RegisterRolePermissions(IEnumerable<RolePermission> rolePermissions);
    Task<IEnumerable<Permission>> GetPermissionsByMenuId(int menuId);
    Task<IEnumerable<Permission>> GetRolePermissionsByMenuId(int roleId, int menuId);
    Task<List<RolePermission>> GetPermissionRolesByRoleId(int roleId);
    Task<bool> DeleteRolePermission(List<RolePermission> permissions);
}
