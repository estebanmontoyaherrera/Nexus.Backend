using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Roles.Commands.CreateCommand;

public class CreateRoleCommand : ICommand<bool>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? State { get; set; }
    public ICollection<PermissionRequestDto> Permissions { get; set; } = null!;
    public ICollection<MenuRequestDto> Menus { get; set; } = null!;
}

public record PermissionRequestDto
{
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = null!;
    public string PermissionDescription { get; set; } = null!;
    public bool Selected { get; set; }
}

public record MenuRequestDto
{
    public int MenuId { get; set; }
}