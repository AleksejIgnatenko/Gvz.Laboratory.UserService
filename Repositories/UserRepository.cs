using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Entities;
using Gvz.Laboratory.UserService.Exceptions;
using Gvz.Laboratory.UserService.Models;
using Microsoft.AspNetCore.Identity;
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
            _logger.LogInformation("");

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.Equals(userModel.Email));

            if (existingUser != null) { throw new UsersRepositoryException("Пользователь с такой почтой уже есть"); }
            else
            {
                UserEntity userEntity = new UserEntity
                {
                    Id = userModel.Id,
                    Role = userModel.Role,
                    Email = userModel.Email,
                    Password = _passwordHasher.Generate(userModel.Password),
                    Surname = userModel.Surname,
                    Name = userModel.Name,
                    DateCreate = DateTime.UtcNow,
                };

                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();

                return userEntity.Id;
            }
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var userEntities = await _context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.DateCreate)
                .ToListAsync();

            var users = userEntities.Select(u => UserModel.Create(u.Id,
                                            u.Role,
                                            u.Email,
                                            u.Password,
                                            u.Surname,
                                            u.Name,
                                            false).user).ToList();

            return users;
        }

        public async Task<UserModel> GetUserByEmailAsync(string email)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new UsersRepositoryException("Пользователя с такой почтой не существует");

            var user = UserModel.Create(userEntity.Id,
                userEntity.Role,
                userEntity.Email,
                userEntity.Password,
                userEntity.Surname,
                userEntity.Name,
                false).user;

            return user;
        }

        public async Task<Guid> UpdateUserAsync(UserModel userModel)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userModel.Id);

            if (userEntity == null) { throw new UsersRepositoryException("Пользователя не найден"); }

            userEntity.Surname = userModel.Surname;
            userEntity.Name = userModel.Name;

            await _context.SaveChangesAsync();

            return userModel.Id;
        }
    }
}
