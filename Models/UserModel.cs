using FluentValidation.Results;
using Gvz.Laboratory.UserService.Enums;
using Gvz.Laboratory.UserService.Validations;

namespace Gvz.Laboratory.UserService.Models
{
    public class UserModel
    {
        public Guid Id { get; }
        public UserRole Role { get; }
        public string Surname { get; } = string.Empty;
        public string UserName { get; } = string.Empty;
        public string Patronymic { get; } = string.Empty;
        public string Email { get; } = string.Empty;
        public string Password { get; } = string.Empty;
        public string RepeatPassword { get; } = string.Empty;

        private UserModel(Guid id, string surname, string userName, string patronymic)
        {
            Id = id;
            Surname = surname;
            UserName = userName;
            Patronymic = patronymic;
        }

        private UserModel(Guid id, UserRole role, string surname, string userName,
            string patronymic, string email, string password) : this(id, surname, userName, patronymic)
        {
            Role = role;
            Email = email;
            Password = password;
        }

        private UserModel(Guid id, UserRole role, string surname, string userName,
             string patronymic, string email, string password, string repeatPassword) : this(id, role, surname, userName, patronymic, email, password)
        {
            RepeatPassword = repeatPassword;
        }

        public static (Dictionary<string, string> errors, UserModel user) Create(Guid id, string surname, string userName, string patronymic,
            bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            UserModel user = new UserModel(id, surname, userName, patronymic);
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

        public static (Dictionary<string, string> errors, UserModel user) Create(Guid id, UserRole role, string surname, string userName,
              string patronymic, string email, string password, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            UserModel user = new UserModel(id, role, surname, userName, patronymic, email, password);
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

        public static (Dictionary<string, string> errors, UserModel user) Create(Guid id, UserRole role, string surname, string userName, 
             string patronymic, string email, string password, string repeatPassword, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            UserModel user = new UserModel(id, role, surname, userName, patronymic, email, password, repeatPassword);
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
