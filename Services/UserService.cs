using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Enums;
using Gvz.Laboratory.UserService.Exceptions;
using Gvz.Laboratory.UserService.Models;

namespace Gvz.Laboratory.UserService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> CreateUserAsync(Guid id, UserRole role, string email, string password,
            string surname, string name)
        {
            var (errors, user) = UserModel.Create(id, role, email, password, surname, name);
            if (errors.Count > 0)
            {
                throw new UserValidationException(errors);
            }

            await _userRepository.CreateUserAsync(user);

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }

        public async Task<string> LoginUserAsync(string email, string password)
        {

            var user = await _userRepository.GetUserByEmailAsync(email);

            var isVerify = _passwordHasher.Verify(password, user.Password);

            if (!isVerify)
            {
                throw new UnauthorizedAccessException("Неправильный пароль");
            }

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task UpdateUserAsync(Guid id, string surname, string name)
        {
            var (errors, user) = UserModel.Create(id, surname, name);
            errors = errors.Where(x => x.Key.Equals("Surname") || x.Key.Equals("Name"))
                           .ToDictionary(x => x.Key, x => x.Value);

            if (errors.Count > 0)
            {
                throw new UserValidationException(errors);
            }

            await _userRepository.UpdateUserAsync(user);
        }
    }
}
