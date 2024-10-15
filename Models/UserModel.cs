using FluentValidation.Results;
using Gvz.Laboratory.UserService.Enums;
using Gvz.Laboratory.UserService.Validations;

namespace Gvz.Laboratory.UserService.Models
{
    public class UserModel
    {
        public Guid Id { get; }
        public UserRole Role { get; set; }
        public string Email { get; } = string.Empty;
        public string Password { get; } = string.Empty;
        public string Surname { get; } = string.Empty;
        public string Name { get; } = string.Empty;

        public UserModel(Guid id, string surname, string name)
        {
            Id = id;
            Surname = surname;
            Name = name;
        }

        public UserModel(Guid id, UserRole role, string email, string password,
            string surname, string name)
        {
            Id = id;
            Role = role;
            Email = email;
            Password = password;
            Surname = surname;
            Name = name;
        }

        public static (Dictionary<string, string> errors, UserModel user) Create(Guid id, string surname, string name,
            bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            UserModel user = new UserModel(id, surname, name);
            if (!useValidation) { return (errors, user); }

            UserValidation userValidation = new UserValidation();
            ValidationResult result = userValidation.Validate(user);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, user);
        }

        public static (Dictionary<string, string> errors, UserModel user) Create(Guid id, UserRole role, string email, string password,
            string surname, string name, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            UserModel user = new UserModel(id, role, email, password, surname, name);
            if (!useValidation) { return (errors, user); }

            UserValidation userValidation = new UserValidation();
            ValidationResult result = userValidation.Validate(user);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, user);
        }
    }
}
