namespace Gvz.Laboratory.UserService.Contracts
{
    public record UpdateUserRequest(
        string Surname,
        string Name,
        string Patronymic
        );
}
