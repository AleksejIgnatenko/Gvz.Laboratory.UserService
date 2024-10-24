using Gvz.Laboratory.UserService.Enums;

namespace Gvz.Laboratory.UserService.Contracts
{
    public record GetUsersForPageResponse(
        Guid Id,
        UserRole Role,
        string Surname,
        string UserName,
        string Patronymic,
        string Email
        );
}
