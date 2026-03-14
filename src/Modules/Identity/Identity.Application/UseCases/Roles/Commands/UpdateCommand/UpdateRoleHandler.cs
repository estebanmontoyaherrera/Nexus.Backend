using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;

namespace Identity.Application.UseCases.Roles.Commands.UpdateCommand;

public class UpdateRoleHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateRoleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        using var transaction = _unitOfWork.BeginTransaction();

        try
        {
            var role = request.Adapt<Role>();
            role.Id = request.RoleId;
            _unitOfWork.Role.UpdateAsync(role);
            await _unitOfWork.SaveChangesAsync();

            var existingPermissions = await _unitOfWork.Permission
                    .GetPermissionRolesByRoleId(request.RoleId);

            var existingMenus = await _unitOfWork.Menu
                    .GetMenuRolesByRoleId(request.RoleId);

            var newPermissions = request.Permissions
                    .Where(p => p.Selected && !existingPermissions.Any(ep => ep.PermissionId == p.PermissionId))
                    .Select(p => new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = p.PermissionId
                    });

            await _unitOfWork.Permission.RegisterRolePermissions(newPermissions);

            var newMenus = request.Menus
                    .Where(p => !existingMenus.Any(ep => ep.MenuId == p.MenuId))
                    .Select(p => new MenuRole
                    {
                        RoleId = role.Id,
                        MenuId = p.MenuId
                    });

            await _unitOfWork.Menu.RegisterRoleMenus(newMenus);

            var permissionsToDelete = existingPermissions
                    .Where(ep => !request.Permissions.Any(p => p.PermissionId == ep.PermissionId && p.Selected))
                    .ToList();

            await _unitOfWork.Permission.DeleteRolePermission(permissionsToDelete);

            var menusToDelete = existingMenus
                    .Where(ep => !request.Menus.Any(p => p.MenuId == ep.MenuId))
                    .ToList();

            await _unitOfWork.Menu.DeleteMenuRole(menusToDelete);

            transaction.Commit();
            response.IsSuccess = true;
            response.Message = "Actualización exitosa";
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
