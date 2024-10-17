using FluentValidation;
using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Validations
{
    internal class UserValidation : AbstractValidator<UserModel>
    {
        public UserValidation()
        {
            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Фамилия не может быть пустая")
                .MinimumLength(2).WithMessage("Длина фамилии не может быть меньше двух символов")
                .Matches(@"^[A-Za-zА-Яа-яЁё]+(-[A-Za-zА-Яа-яЁё]+)?$").WithMessage("Фамилия должна содержать только буквы");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Имя не может быть пустая")
                .MinimumLength(2).WithMessage("Длина имени не может быть меньше двух символов")
                .Matches(@"^[A-Za-zА-Яа-яЁё]+$").WithMessage("Имя должно содержать только буквы");

            RuleFor(x => x.Patronymic)
                .NotEmpty().WithMessage("Отчество не может быть пустым")
                .MinimumLength(2).WithMessage("Длина отчества не может быть меньше двух символов")
                .Matches(@"^[A-Za-zА-Яа-яЁё]+$").WithMessage("Отчество должно содержать только буквы");

            RuleFor(x => x.Email).EmailAddress().WithMessage("Email неправильный");

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Длина пароля должна составлять минимум 8 символов")
                .MaximumLength(16).WithMessage("Длина пароля недолжна  превышать 16 символов");

            RuleFor(x => x.RepeatPassword)
                .Equal(x => x.Password)
                .WithMessage("Пароли не совпали");
        }
    }
}
