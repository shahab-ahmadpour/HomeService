using App.Domain.Core.Services.Entities;
using App.Domain.Core.Skills.Entities;
using App.Domain.Core.Users.Entities;
using App.Domain.Core.Transactions.Entities;
using App.Infrastructure.Db.SqlServer.Ef.Configurations.CategoryConfigurations;
using App.Infrastructure.Db.SqlServer.Ef.Configurations.ExpertSkillConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Infrastructure.Db.SqlServer.Ef.Configurations.SkillConfigurations;
using App.Infrastructure.Db.SqlServer.Ef.Configurations.UserConfigurations;
using App.Infrastructure.Db.SqlServer.Ef.Configurations.OrderConfigurations;
using App.Infrastructure.Db.SqlServer.Ef.Configurations.RequestConfigurations;
using App.Infrastructure.Db.SqlServer.Ef.Configurations.TransactionConfigurations;
using App.Infrastructure.Db.SqlServer.Ef.Configurations.ProposalConfigurations;
using App.Infrastructure.Db.SqlServer.Ef.Configurations.ReviewConfigurations;
using App.Infrastructure.Db.SqlServer.Ef.Configurations.SubHomeServiceConfigurations;
using App.Infrastructure.Db.SqlServer.Ef.Configurations.HomeServiceConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace App.Infrastructure.Db.SqlServer.Ef
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int> 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<ExpertSkill> ExpertSkills { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HomeService> HomeServices { get; set; }
        public DbSet<SubHomeService> SubHomeServices { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        public DbSet<IdentityRole<int>> Roles { get; set; }
        public DbSet<IdentityUserRole<int>> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ExpertConfiguration());
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new HomeServiceConfiguration());
            modelBuilder.ApplyConfiguration(new SubHomeServiceConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new ProposalConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new SkillConfiguration());
            modelBuilder.ApplyConfiguration(new ExpertSkillConfiguration());

        }
    }
}
