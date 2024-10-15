namespace Gvz.Laboratory.UserService.Contracts
{
    public record UserRegistrationRequest(
        string Email,
        string Password,
        string Surname,
        string Name
        );
}
