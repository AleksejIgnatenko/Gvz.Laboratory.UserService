using Gvz.Laboratory.UserService.Enums;
using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Abstractions
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(Guid id, UserRole role, string email, string password, string surname, string name);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<string> LoginUserAsync(string email, string password);
        Task UpdateUserAsync(Guid id, string surname, string name);
    }
}