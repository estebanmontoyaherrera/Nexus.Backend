using Identity.Application.Dtos.Roles;
using Identity.Application.UseCases.Roles.Commands.CreateCommand;
using Identity.Application.UseCases.Roles.Commands.UpdateCommand;
using Identity.Domain.Entities;
using Mapster;
using SharedKernel.Dtos.Commons;

namespace Identity.Application.Mappings;

public class RoleMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Role, RoleResponseDto>()
          .Map(dest => dest.RoleId, src => src.Id)
          .Map(dest => dest.StateDescription, src => src.State == "1" ? "Enabled" : "Disabled")
          .TwoWays();

        config.NewConfig<Role, RoleByIdResponseDto>()
          .Map(dest => dest.RoleId, src => src.Id)
          .TwoWays();

        config.NewConfig<CreateRoleCommand, Role>();
        config.NewConfig<UpdateRoleCommand, Role>();

        config.NewConfig<Role, SelectResponseDto>()
          .Map(dest => dest.Code, src => src.Id)
          .Map(dest => dest.Description, src => src.Name)
          .TwoWays();
    }
}
