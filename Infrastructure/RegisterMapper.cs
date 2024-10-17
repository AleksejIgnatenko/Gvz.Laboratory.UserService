using Gvz.Laboratory.UserService.Dto;
using Gvz.Laboratory.UserService.Models;
using Mapster;

namespace Gvz.Laboratory.UserService.Infrastructure
{
    public class RegisterMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserModel, UserDto>()
                .RequireDestinationMemberSource(true);
        }
    }
}
