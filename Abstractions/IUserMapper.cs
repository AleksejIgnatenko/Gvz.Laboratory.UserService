using Gvz.Laboratory.UserService.Dto;
using Gvz.Laboratory.UserService.Models;
using Mapster;

namespace Gvz.Laboratory.UserService.Abstractions
{
    [Mapper]
    public interface IUserMapper
    {
        UserDto? MapTo(UserModel user);
    }
}
