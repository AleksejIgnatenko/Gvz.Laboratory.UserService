using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Abstractions
{
    public interface IUserRepository
    {
        Task<Guid> CreateUserAsync(UserModel userModel);
        Task<(List<UserModel> users, int numberUsers)> GetUsersForPageAsync(int pageNumber);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel> GetUserByEmailAsync(string email);
        Task<Guid> UpdateUserAsync(UserModel userModel);
    }
}