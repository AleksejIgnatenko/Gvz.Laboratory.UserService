﻿using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Entities;
using Gvz.Laboratory.UserService.Exceptions;
using Gvz.Laboratory.UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GvzLaboratoryUserServiceDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(GvzLaboratoryUserServiceDbContext context, IPasswordHasher passwordHasher, ILogger<UserRepository> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<Guid> CreateUserAsync(UserModel userModel)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.Equals(userModel.Email));

            if (existingUser != null) { throw new RepositoryException("Пользователь с такой почтой уже есть"); }
            else
            {
                UserEntity userEntity = new UserEntity
                {
                    Id = userModel.Id,
                    Role = userModel.Role,
                    Surname = userModel.Surname,
                    UserName = userModel.UserName,
                    Patronymic = userModel.Patronymic,
                    Email = userModel.Email,
                    Password = _passwordHasher.Generate(userModel.Password),
                    DateCreate = DateTime.UtcNow,
                };

                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();

                return userEntity.Id;
            }
        }

        public async Task<(List<UserModel> users, int numberUsers)> GetUsersForPageAsync(int pageNumber)
        {
            var userEntities = await _context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.DateCreate)
                .Skip(pageNumber * 10)
                .Take(10)
                .ToListAsync();

            var numberUsers = await _context.Users.CountAsync();

            var users = userEntities.Select(u => UserModel.Create(u.Id,
                                            u.Role,
                                            u.Surname,
                                            u.UserName,
                                            u.Patronymic,
                                            u.Email,
                                            u.Password,
                                            false).user).ToList();

            return (users, numberUsers);
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var userEntities = await _context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.DateCreate)
                .ToListAsync();

            var users = userEntities.Select(u => UserModel.Create(u.Id,
                                            u.Role,
                                            u.Surname,
                                            u.UserName,
                                            u.Patronymic,
                                            u.Email,
                                            u.Password,
                                            false).user).ToList();

            return users;
        }

        public async Task<UserModel> GetUserByEmailAsync(string email)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new AuthenticationFailedException("Неверный логин или пароль.");

            var user = UserModel.Create(userEntity.Id,
                userEntity.Role,
                userEntity.Surname,
                userEntity.UserName,
                userEntity.Patronymic,
                userEntity.Email,
                userEntity.Password,
                false).user;

            return user;
        }

        public async Task<UserModel> GetUserByIdAsync(Guid id)
        {
            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new InvalidOperationException("Пользователь не найден.");

            var user = UserModel.Create(userEntity.Id,
                userEntity.Role,
                userEntity.Surname,
                userEntity.UserName,
                userEntity.Patronymic,
                userEntity.Email,
                userEntity.Password,
                false).user;

            return user;
        }

        public async Task<UserEntity?> GetUserEntityByIdAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<(List<UserModel> users, int numberUsers)> SearchUsersAsync(string searchQuery, int pageNumber)
        {
            var userEntities = await _context.Users
                    .AsNoTracking()
                    .Where(u => 
                        u.Surname.ToLower().Contains(searchQuery.ToLower()) ||
                        u.UserName.ToLower().Contains(searchQuery.ToLower()) ||
                        u.Patronymic.ToLower().Contains(searchQuery.ToLower()) ||
                        u.Email.ToLower().Contains(searchQuery.ToLower())
                    )
                    .OrderByDescending(u => u.DateCreate)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();


            var numberUsers = await _context.Users
                    .AsNoTracking()
                    .CountAsync(u =>
                        u.Surname.ToLower().Contains(searchQuery.ToLower()) ||
                        u.UserName.ToLower().Contains(searchQuery.ToLower()) ||
                        u.Patronymic.ToLower().Contains(searchQuery.ToLower()) ||
                        u.Email.ToLower().Contains(searchQuery.ToLower()));

            var users = userEntities.Select(u => UserModel.Create(u.Id,
                                            u.Role,
                                            u.Surname,
                                            u.UserName,
                                            u.Patronymic,
                                            u.Email,
                                            u.Password,
                                            false).user).ToList();

            return (users, numberUsers);
        }

        public async Task<Guid> UpdateUserAsync(UserModel userModel)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userModel.Id)
                ?? throw new RepositoryException("Пользователя не найден.");


            userEntity.Role = userModel.Role;
            userEntity.Surname = userModel.Surname;
            userEntity.UserName = userModel.UserName;
            userEntity.Patronymic = userModel.Patronymic;

            await _context.SaveChangesAsync();

            return userModel.Id;
        }

        public async Task<Guid> UpdateUserDetailsAsync(UserModel userModel)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userModel.Id)
                ?? throw new RepositoryException("Пользователя не найден."); ;

            userEntity.Surname = userModel.Surname;
            userEntity.UserName = userModel.UserName;
            userEntity.Patronymic = userModel.Patronymic;

            await _context.SaveChangesAsync();

            return userModel.Id;
        }
    }
}
