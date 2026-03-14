using Identity.Application.Dtos.UserRole;
using Identity.Application.UseCases.UserRoles.Commands.CreateCommand;
using Identity.Application.UseCases.UserRoles.Commands.UpdateCommand;
using Identity.Domain.Entities;
using Mapster;

namespace Identity.Application.Mappings;

public class UserRoleMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserRole, UserRoleResponseDto>()
          .Map(dest => dest.UserRoleId, src => src.Id)
          .Map(dest => dest.User, src => src.User.FirstName + " " + src.User.LastName)
          .Map(dest => dest.Role, src => src.Role.Name)
          .Map(dest => dest.StateDescription, src => src.State == "1" ? "Enabled" : "Disabled")
          .TwoWays();

        config.NewConfig<UserRole, UserRoleByIdResponseDto>()
          .Map(dest => dest.UserRoleId, src => src.Id)
          .TwoWays();

        config.NewConfig<CreateUserRoleCommand, UserRole>();
        config.NewConfig<UpdateUserRoleCommand, UserRole>();
    }
}
