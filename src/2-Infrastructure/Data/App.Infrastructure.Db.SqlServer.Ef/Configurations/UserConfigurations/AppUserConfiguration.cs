using App.Domain.Core.Enums;
using App.Domain.Core.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Db.SqlServer.Ef.Configurations.UserConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.ProfilePicture)
                .HasMaxLength(255);

            builder.Property(u => u.AccountBalance)
                .HasPrecision(18, 2);

            builder.Property(u => u.Role)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            var passwordHasher = new PasswordHasher<AppUser>();

            var adminUser = new AppUser
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@example.com",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                AccountBalance = 1000,
                ProfilePicture = "images\\User\\Admin\\admin.png",
                CreatedAt = DateTime.UtcNow,
                IsEnabled = true,
                Role = UserRole.Admin,
                PasswordHash = passwordHasher.HashPassword(null, "Admin123!")
            };

            var customerUser = new AppUser
            {
                Id = 2,
                FirstName = "Ali",
                LastName = "Abasi",
                Email = "ali@example.com",
                UserName = "ali123",
                NormalizedUserName = "ALI123",
                NormalizedEmail = "ALI@EXAMPLE.COM",
                AccountBalance = 500,
                ProfilePicture = "images\\User\\Customer\\ali.jpg",
                CreatedAt = DateTime.UtcNow,
                IsEnabled = true,
                Role = UserRole.Customer,
                PasswordHash = passwordHasher.HashPassword(null, "Ali123!")
            };

            var expertUser = new AppUser
            {
                Id = 3,
                FirstName = "Shahin",
                LastName = "Hasani",
                Email = "shahin@example.com",
                UserName = "shahin",
                NormalizedUserName = "SHAHIN",
                NormalizedEmail = "SHAHIN@EXAMPLE.COM",
                AccountBalance = 750,
                ProfilePicture = "images\\User\\Expert\\shahin.png",
                CreatedAt = DateTime.UtcNow,
                IsEnabled = true,
                Role = UserRole.Expert,
                PasswordHash = passwordHasher.HashPassword(null, "Shahin123!")
            };

            builder.HasData(adminUser, customerUser, expertUser);
        }
    }
}
