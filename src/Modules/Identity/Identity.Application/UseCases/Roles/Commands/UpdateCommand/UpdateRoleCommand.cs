using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Roles.Commands.UpdateCommand;

public class UpdateRoleCommand : ICommand<bool>
{
    public int RoleId {  get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? State { get; set; }
    public ICollection<PermissionUpdateRequestDto> Permissions { get; set; } = null!;
    public ICollection<MenuUpdateRequestDto> Menus { get; set; } = null!;
}

public record PermissionUpdateRequestDto
{
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = null!;
    public string PermissionDescription { get; set; } = null!;
    public bool Selected { get; set; }
}

public record MenuUpdateRequestDto
{
    public int MenuId { get; set; }
}