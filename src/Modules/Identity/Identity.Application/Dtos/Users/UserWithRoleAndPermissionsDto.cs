namespace Identity.Application.Dtos.Users;

public record UserWithRoleAndPermissionsDto
{
    public int UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public RoleDto? Role { get; set; }
}

public class RoleDto
{
    public int RoleId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    //public List<MenuDto>? Menus { get; set; }
    public List<PermissionDto>? Permissions { get; set; }
}

public class PermissionDto
{
    public int PermissionId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Slug { get; set; }
    //public MenuDto? Menu { get; set; }
}

public class MenuDto
{
    public int MenuId { get; set; }
    public int Position { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public string? Url { get; set; }
    public int? FatherId { get; set; }
}