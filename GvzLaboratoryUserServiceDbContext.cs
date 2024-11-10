using Gvz.Laboratory.UserService.Entities;
using Gvz.Laboratory.UserService.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.UserService
{
    public class GvzLaboratoryUserServiceDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public GvzLaboratoryUserServiceDbContext(DbContextOptions<GvzLaboratoryUserServiceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //конфигурация

            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity[]
                {
                    new UserEntity { Id = Guid.NewGuid(), Role = UserRole.Admin, Surname = "Admin", UserName = "Admin", Patronymic = "Admin", Email = "admin@gmail.com", Password = "$2a$11$8fWCZqmV0sBx14PtKqn90.iiV4PyxdQEwrOva9PpE2C3skxMshN92", DateCreate = DateTime.UtcNow },
                    new UserEntity { Id = Guid.NewGuid(), Role = UserRole.Manager, Surname = "Manager", UserName = "Manager", Patronymic = "Manager", Email = "manager@gmail.com", Password = "$2a$11$8fWCZqmV0sBx14PtKqn90.iiV4PyxdQEwrOva9PpE2C3skxMshN92", DateCreate = DateTime.UtcNow },
                    new UserEntity { Id = Guid.NewGuid(), Role = UserRole.Worker, Surname = "Worker", UserName = "Worker", Patronymic = "Worker", Email = "worker@gmail.com", Password = "$2a$11$8fWCZqmV0sBx14PtKqn90.iiV4PyxdQEwrOva9PpE2C3skxMshN92", DateCreate = DateTime.UtcNow },
                    new UserEntity { Id = Guid.NewGuid(), Role = UserRole.User, Surname = "User", UserName = "User", Patronymic = "User", Email = "user@gmail.com", Password = "$2a$11$8fWCZqmV0sBx14PtKqn90.iiV4PyxdQEwrOva9PpE2C3skxMshN92", DateCreate = DateTime.UtcNow },
                }
            );
        }
    }
}
