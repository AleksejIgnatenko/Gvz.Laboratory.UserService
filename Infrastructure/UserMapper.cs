using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Dto;
using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Infrastructure
{
    public partial class UserMapper : IUserMapper
    {
        public UserDto? MapTo(UserModel user)
        {
            return user == null ? null : new UserDto()
            {
                Id = user.Id,
                Surname = user.Surname,
                UserName = user.UserName,
                Patronymic = user.Patronymic
            };
        }
    }
}
