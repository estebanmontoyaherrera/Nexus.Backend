using Identity.Application.Dtos.Menus;
using Identity.Domain.Entities;
using Mapster;

namespace Identity.Application.Mappings;

public class MenuMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Menu, MenuResponseDto>()
          .Map(dest => dest.MenuId, src => src.Id)
          .Map(dest => dest.Item, src => src.Name)
          .Map(dest => dest.Path, src => src.Url)
          .TwoWays();
    }
}
