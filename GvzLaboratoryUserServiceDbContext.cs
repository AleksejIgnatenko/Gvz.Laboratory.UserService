using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Dto;
using Gvz.Laboratory.UserService.Entities;
using Gvz.Laboratory.UserService.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.UserService
{
    public class GvzLaboratoryUserServiceDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        private readonly IUserKafkaProducer _userKafkaProducer;

        public GvzLaboratoryUserServiceDbContext(DbContextOptions<GvzLaboratoryUserServiceDbContext> options, IUserKafkaProducer userKafkaProducer = null) : base(options)
        {
            Database.EnsureCreated();
            _userKafkaProducer = userKafkaProducer;
        }

        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
            //конфигурация
            //UserEntity userEntityAdmin = new UserEntity { Id = Guid.NewGuid(), Role = UserRole.Admin, Surname = "Admin", UserName = "Admin", Patronymic = "Admin", Email = "admin@gmail.com", Password = "$2a$11$8fWCZqmV0sBx14PtKqn90.iiV4PyxdQEwrOva9PpE2C3skxMshN92", DateCreate = DateTime.UtcNow };
            //UserDto userDtoAdmin = new UserDto { Id = userEntityAdmin.Id, Surname = userEntityAdmin.Surname, UserName = userEntityAdmin.UserName, Patronymic = userEntityAdmin.Patronymic };
            //await _userKafkaProducer.SendUserToKafkaAsync(userDtoAdmin, "add-user-topic");
            
            //UserEntity userEntityManager = new UserEntity { Id = Guid.NewGuid(), Role = UserRole.Manager, Surname = "Manager", UserName = "Manager", Patronymic = "Manager", Email = "manager@gmail.com", Password = "$2a$11$8fWCZqmV0sBx14PtKqn90.iiV4PyxdQEwrOva9PpE2C3skxMshN92", DateCreate = DateTime.UtcNow };
            //UserDto userDtoManager = new UserDto { Id = userEntityManager.Id, Surname = userEntityManager.Surname, UserName = userEntityManager.UserName, Patronymic = userEntityManager.Patronymic };
            //await _userKafkaProducer.SendUserToKafkaAsync(userDtoManager, "add-user-topic");
            
            //UserEntity userEntityWorker = new UserEntity { Id = Guid.NewGuid(), Role = UserRole.Worker, Surname = "Worker", UserName = "Worker", Patronymic = "Worker", Email = "worker@gmail.com", Password = "$2a$11$8fWCZqmV0sBx14PtKqn90.iiV4PyxdQEwrOva9PpE2C3skxMshN92", DateCreate = DateTime.UtcNow };
            //UserDto userDtoWorker = new UserDto { Id = userEntityWorker.Id, Surname = userEntityWorker.Surname, UserName = userEntityWorker.UserName, Patronymic = userEntityWorker.Patronymic };
            //await _userKafkaProducer.SendUserToKafkaAsync(userDtoWorker, "add-user-topic");
            
            //UserEntity userEntityUser = new UserEntity { Id = Guid.NewGuid(), Role = UserRole.User, Surname = "User", UserName = "User", Patronymic = "User", Email = "user@gmail.com", Password = "$2a$11$8fWCZqmV0sBx14PtKqn90.iiV4PyxdQEwrOva9PpE2C3skxMshN92", DateCreate = DateTime.UtcNow };
            //UserDto userDtoUser = new UserDto { Id = userEntityUser.Id, Surname = userEntityUser.Surname, UserName = userEntityUser.UserName, Patronymic = userEntityUser.Patronymic };
            //await _userKafkaProducer.SendUserToKafkaAsync(userDtoUser, "add-user-topic");

            //List<UserEntity> userEntities = new List<UserEntity> { userEntityAdmin, userEntityManager, userEntityWorker, userEntityUser };

            //modelBuilder.Entity<UserEntity>().HasData(userEntities.ToArray());
        }
    }
}
