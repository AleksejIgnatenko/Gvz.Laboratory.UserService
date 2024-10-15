using Gvz.Laboratory.UserService.Enums;

namespace Gvz.Laboratory.UserService.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime DateCreate { get; set; }
    }
}
