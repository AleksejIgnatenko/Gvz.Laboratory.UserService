﻿using Gvz.Laboratory.UserService.Abstractions;
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
        private readonly IUserMapper _userMapper;

        public UserService(IUserRepository userRepository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher, IUserMapper userMapper)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
            _userMapper = userMapper;
        }

        public async Task<string> CreateUserAsync(Guid id, UserRole role, string surname, string userName, string patronymic, 
            string email, string password, string repeatPassword)
        {
            var (errors, user) = UserModel.Create(id, role, surname, userName, patronymic, email, password, repeatPassword);
            if (errors.Count > 0)
            {
                throw new UserValidationException(errors);
            }

            await _userRepository.CreateUserAsync(user);

            //var userDto = _userMapper.MapTo(user);
            //Console.WriteLine(userDto.Surname);
            //Console.WriteLine(userDto.UserName);
            //Console.WriteLine(userDto.Patronymic);

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

        public async Task<List<UserModel>> GetUsersForPageAsync(int page)
        {
            return await _userRepository.GetUsersForPageAsync(page);
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task UpdateUserAsync(Guid id, string surname, string name, string patronymic)
        {
            var (errors, user) = UserModel.Create(id, surname, name, patronymic);
            errors = errors.Where(x => x.Key.Equals("Surname") || x.Key.Equals("Name") || x.Key.Equals("Patronymic"))
                           .ToDictionary(x => x.Key, x => x.Value);

            if (errors.Count > 0)
            {
                throw new UserValidationException(errors);
            }

            await _userRepository.UpdateUserAsync(user);
        }
    }
}
