using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;

namespace Identity.Application.UseCases.Roles.Commands.CreateCommand;

public class CreateRoleHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateRoleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        using var transaction = _unitOfWork.BeginTransaction();

        try
        {
            var role = request.Adapt<Role>();
            await _unitOfWork.Role.CreateAsync(role);
            await _unitOfWork.SaveChangesAsync();

            var menus = request.Menus
                    .Select(menu => new MenuRole
                    {
                        RoleId = role.Id,
                        MenuId = menu.MenuId
                    }).ToList();

            var permissions = request.Permissions
                    .Select(permission => new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permission.PermissionId
                    }).ToList();

            await _unitOfWork.Permission.RegisterRolePermissions(permissions);
            await _unitOfWork.Menu.RegisterRoleMenus(menus);

            transaction.Commit();
            response.IsSuccess = true;
            response.Message = "Registro exitoso";
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
