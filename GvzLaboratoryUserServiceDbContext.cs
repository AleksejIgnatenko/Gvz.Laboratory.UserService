using Gvz.Laboratory.UserService.Entities;
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
        }
    }
}
