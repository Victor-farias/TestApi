using Microsoft.EntityFrameworkCore;
using TestApi.Models;
using System;

namespace TestApi.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly string _connectionString;

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        //public DbSet<UserRoles> UserRoles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext(string connectionString) : base()
        {
            _connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                Id = 1,
                RoleName = "Admin"
            });
            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                Id = 2,
                RoleName = "User"
            });

            modelBuilder.Entity<User>().HasData(new
            {
                Id = 1,
                UserName = "Tiago_Oliveira",
                Name = "Tiago Oliveira",
                UserRolesId = 1
            });
            modelBuilder.Entity<User>().HasData(new
            {
                Id = 2,
                UserName = "Victor_Farias",
                Name = "Victor_Farias",
                UserRolesId = 2
            });
        }
    }
}
