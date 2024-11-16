using Gvz.Laboratory.UserService.Enums;

namespace Gvz.Laboratory.UserService.Contracts
{
    public record GetUsersResponse(
        Guid Id,
        string Role,
        string Surname,
        string UserName,
        string Patronymic,
        string Email
        );
}
