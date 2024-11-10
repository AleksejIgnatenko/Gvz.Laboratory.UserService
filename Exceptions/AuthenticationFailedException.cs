namespace Gvz.Laboratory.UserService.Exceptions
{
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException(string message) : base(message) { }
    }
}
