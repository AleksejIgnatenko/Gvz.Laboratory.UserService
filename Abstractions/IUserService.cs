using Gvz.Laboratory.UserService.Enums;
using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Abstractions
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(Guid id, UserRole role, string surname, string userName,  string patronymic, string email, string password, string repeatPassword);
        Task<(List<UserModel> users, int numberUsers)> GetUsersForPageAsync(int pageNumber);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<string> LoginUserAsync(string email, string password);
        Task<Guid> UpdateUserAsync(Guid id, string surname, string name, string patronymic);
    }
}