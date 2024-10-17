using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Dto;
using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Infrastructure
{
    public partial class UserMapper : IUserMapper
    {
        public UserDto? MapTo(UserModel p1)
        {
            return p1 == null ? null : new UserDto()
            {
                Id = p1.Id,
                Surname = p1.Surname,
                UserName = p1.UserName,
                Patronymic = p1.Patronymic
            };
        }
    }
}
