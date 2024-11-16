using Gvz.Laboratory.UserService.Entities;
using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Abstractions
{
    public interface IUserRepository
    {
        Task<Guid> CreateUserAsync(UserModel userModel);
        Task<(List<UserModel> users, int numberUsers)> GetUsersForPageAsync(int pageNumber);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel> GetUserByEmailAsync(string email);
        Task<UserEntity?> GetUserEntityByIdAsync(Guid userId);
        Task<UserModel> GetUserByIdAsync(Guid id);
        Task<Guid> UpdateUserAsync(UserModel userModel);
        Task<Guid> UpdateUserDetailsAsync(UserModel userModel);
    }
}