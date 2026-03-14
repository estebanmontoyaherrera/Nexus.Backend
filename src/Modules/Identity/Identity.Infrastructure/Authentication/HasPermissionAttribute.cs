using Microsoft.AspNetCore.Authorization;

namespace Identity.Infrastructure.Authentication;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission) : base(policy: permission.ToString()!)
    {

    }
}
