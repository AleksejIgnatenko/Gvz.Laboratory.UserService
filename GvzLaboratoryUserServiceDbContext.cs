using Gvz.Laboratory.UserService.Entities;
using Gvz.Laboratory.UserService.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.UserService
{
    public class GvzLaboratoryUserServiceDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PartyEntity> Parties { get; set; }

        public GvzLaboratoryUserServiceDbContext(DbContextOptions<GvzLaboratoryUserServiceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var user = new UserEntity
            {
                Id = Guid.Parse("CA6456C9-6062-481B-89B1-FF53E954A027"),
                Role = UserRole.Admin,
                Surname = "Admin",
                UserName = "Admin",
                Patronymic = "Admin",
                Email = "admin@gmail.com",
                Password = "$2a$11$KQv8JuY2YmgQ9kt.8Q.xTeC9WG8JxeltoXUTNP7PzJPHssockyL7O",
                DateCreate = DateTime.UtcNow
            };

            modelBuilder.Entity<UserEntity>().HasData(user);
        }
    }
}
