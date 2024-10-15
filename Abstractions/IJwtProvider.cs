using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Abstractions
{
    public interface IJwtProvider
    {
        string GenerateToken(UserModel user);
        Guid GetUserIdFromToken(string jwtToken);
    }
}