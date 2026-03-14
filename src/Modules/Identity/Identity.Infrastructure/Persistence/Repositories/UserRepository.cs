using Dapper;
using Identity.Application.Dtos.Users;
using Identity.Application.Interfaces.Persistence;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Metadata;

namespace Identity.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : GenericRepository<User>(context), IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<UserWithRoleAndPermissionsDto> GetUserWithRoleAndPermissionsAsync(int userId)
    {
        var userWithRoles = await (from u in _context.Users
                                   join ur in _context.UserRoles on u.Id equals ur.UserId
                                   join r in _context.Roles on ur.RoleId equals r.Id
                                   where u.Id == userId
                                   select new
                                   {
                                       User = u,
                                       Role = r
                                   }).FirstOrDefaultAsync();

        if (userWithRoles == null)
        {
            return null!;
        }

        var menus = await (from mr in _context.MenuRoles
                           join m in _context.Menus on mr.MenuId equals m.Id
                           where mr.RoleId == userWithRoles.Role.Id
                           select new MenuDto
                           {
                               MenuId = m.Id,
                               Name = m.Name,
                               Icon = m.Icon,
                               Url = m.Url,
                               FatherId = m.FatherId
                           }).ToListAsync();

        var permissions = await (from rp in _context.RolePermissions
                                 join p in _context.Permissions on rp.PermissionId equals p.Id
                                 join m in _context.Menus on p.MenuId equals m.Id
                                 where rp.RoleId == userWithRoles.Role.Id
                                 select new PermissionDto
                                 {
                                     PermissionId = p.Id,
                                     Name = p.Name,
                                     Description = p.Description,
                                     Slug = p.Slug,
                                     //Menu = new MenuDto
                                     //{
                                     //    MenuId = m.Id,
                                     //    Name = m.Name,
                                     //    Icon = m.Icon,
                                     //    Url = m.Url,
                                     //    FatherId = m.FatherId
                                     //}
                                 }).ToListAsync();

        return new UserWithRoleAndPermissionsDto
        {
            UserId = userWithRoles.User.Id,
            FirstName = userWithRoles.User.FirstName,
            LastName = userWithRoles.User.LastName,
            Email = userWithRoles.User.Email,
            Role = new RoleDto
            {
                RoleId = userWithRoles.Role.Id,
                Name = userWithRoles.Role.Name,
                Description = userWithRoles.Role.Description,
                //Menus = menus,
                Permissions = permissions
            }
        };
    }

    public async Task<User> UserByEmailAsync(string email)
    {
        var user = await _context.Users
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.Email.Equals(email) &&
                                      x.State == "1" &&
                                      x.AuditDeleteUser == null &&
                                      x.AuditDeleteDate == null);

        return user!;
    }

    public async Task<User> UserAsync(User user)
    {
        using var connection = _context.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("UserId", user.Id);
        parameters.Add("FirstNam", user.FirstName);
        return await connection.QuerySingleOrDefaultAsync<User>
            ("PROCEDURE", param: parameters, commandType: CommandType.StoredProcedure);
    }
}