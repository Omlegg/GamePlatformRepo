using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Core.Models;
using GamePlatformRepo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GamePlatformRepo.Core.Data
{
    public class GamePlatformDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EntryLog> Logs { get; set; }
        public DbSet<User> Users { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        
        public DbSet<UsersRoles> UsersRolesTable { get; set; }

        public GamePlatformDbContext(DbContextOptions<GamePlatformDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntryLog>().HasKey(user => user.RequestId);
            modelBuilder.Entity<UsersRoles>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            base.OnModelCreating(modelBuilder);
        }
    }
}