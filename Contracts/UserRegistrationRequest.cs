namespace Gvz.Laboratory.UserService.Contracts
{
    public record UserRegistrationRequest(
        string Surname,
        string UserName,
        string Patronymic,
        string Email,
        string Password,
        string RepeatPassword
        );
}
