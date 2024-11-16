namespace Gvz.Laboratory.UserService.Contracts
{
    public record UpdateUserDetailsRequest(
        Guid Id,
        string Surname,
        string UserName,
        string Patronymic);
}
