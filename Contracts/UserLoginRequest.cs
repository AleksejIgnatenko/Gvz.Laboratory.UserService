namespace Gvz.Laboratory.UserService.Contracts
{
    public record UserLoginRequest(
        string Email,
        string Password
        );
}
