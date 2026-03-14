using Identity.Application.Dtos.Users;
using Identity.Application.UseCases.Users.Commands.CreateCommand;
using Identity.Application.UseCases.Users.Commands.UpdateCommand;
using Identity.Domain.Entities;
using Mapster;
using SharedKernel.Dtos.Commons;

namespace Identity.Application.Mappings;

public class UserMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserResponseDto>()
          .Map(dest => dest.UserId, src => src.Id)
          .Map(dest => dest.StateDescription, src => src.State == "1" ? "Enabled" : "Disabled")
          .TwoWays();

        config.NewConfig<User, UserByIdResponseDto>()
          .Map(dest => dest.UserId, src => src.Id)
          .TwoWays();

        config.NewConfig<CreateUserCommand, User>();
        config.NewConfig<UpdateUserCommand, User>();

        config.NewConfig<User, SelectResponseDto>()
          .Map(dest => dest.Code, src => src.Id)
          .Map(dest => dest.Description, src => src.FirstName + " " + src.LastName)
          .TwoWays();
    }
}
