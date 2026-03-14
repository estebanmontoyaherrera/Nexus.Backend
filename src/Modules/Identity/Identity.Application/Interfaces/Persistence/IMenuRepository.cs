using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Persistence;

public interface IMenuRepository
{
    Task<IEnumerable<Menu>> GetMenuByUserIdAsync(int userId);
    Task<IEnumerable<Menu>> GetMenuPermissionByRoleIdAsync(int? roleId);
    Task<bool> RegisterRoleMenus(IEnumerable<MenuRole> menuRoles);
    Task<List<MenuRole>> GetMenuRolesByRoleId(int roleId);
    Task<bool> DeleteMenuRole(List<MenuRole> menuRoles);
}
