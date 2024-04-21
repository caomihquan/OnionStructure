using Microsoft.EntityFrameworkCore;
using Onion.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Datas
{
    public class OnionDbContext : DbContext
    {
        
        public OnionDbContext(DbContextOptions<OnionDbContext> options) : base(options)
        {

        }
       
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(s => s.UserID);
            modelBuilder.Entity<UserRole>().HasKey(s => s.RoleID);
            modelBuilder.Entity<Room>().HasKey(s => s.RoomId);
            modelBuilder.Entity<RoomLanguage>().HasKey(s => s.LanguageCode);
            modelBuilder.Entity<RoomLevel>().HasKey(s => s.LevelCode);

            modelBuilder.Entity<User>()
            .HasOne(s => s.Role)
            .WithMany(g => g.Users)
            .HasForeignKey(s => s.RoleID);

            modelBuilder.Entity<Room>()
            .HasOne(s => s.User)
            .WithMany(g => g.Rooms)
            .HasForeignKey(s => s.UserID);
            modelBuilder.Entity<Room>()
            .HasOne(s => s.Language)
            .WithMany(g => g.Rooms)
            .HasForeignKey(s => s.LanguageCode);
            modelBuilder.Entity<Room>()
            .HasOne(s => s.Level)
            .WithMany(g => g.Rooms)
            .HasForeignKey(s => s.LevelCode);
        }

        
    }
}
