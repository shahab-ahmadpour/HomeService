using App.Domain.Core.Enums;
using App.Domain.Core.Services.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Db.SqlServer.Ef.Configurations.ProposalConfigurations
{
    public class ProposalConfiguration : IEntityTypeConfiguration<Proposal>
    {
        public void Configure(EntityTypeBuilder<Proposal> builder)
        {
            builder.HasKey(p => p.Id);

            builder.ToTable("Proposals");

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.ExecutionDate)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.IsEnabled)
                .HasDefaultValue(true);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.HasOne(p => p.Expert)
                .WithMany(e => e.Proposals)
                .HasForeignKey(p => p.ExpertId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Request)
                .WithMany(r => r.Proposals)
                .HasForeignKey(p => p.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Skill)
                .WithMany()
                .HasForeignKey(p => p.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            // اضافه کردن رابطه با Order
            builder.Property(p => p.OrderId)
                .IsRequired();

            builder.HasOne(p => p.Order)
                .WithMany(o => o.Proposals)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // به‌روزرسانی Seed Data با OrderId
            builder.HasData(
                new Proposal
                {
                    Id = 1,
                    ExpertId = 1,
                    RequestId = 1,
                    SkillId = 1,
                    OrderId = 1,
                    Price = 450000,
                    ExecutionDate = DateTime.UtcNow.AddDays(5),
                    Description = "پیشنهاد انجام خدمات برای درخواست بنایی",
                    Status = ProposalStatus.Pending,
                    ResponseTime = DateTime.UtcNow.AddDays(1),
                    CreatedAt = DateTime.UtcNow,
                    IsEnabled = true
                },
                new Proposal
                {
                    Id = 2,
                    ExpertId = 1,
                    RequestId = 2,
                    SkillId = 2,
                    OrderId = 2,
                    Price = 600000,
                    ExecutionDate = DateTime.UtcNow.AddDays(3),
                    Description = "پیشنهاد انجام خدمات برای درخواست کاغذ دیواری",
                    Status = ProposalStatus.Pending,
                    ResponseTime = DateTime.UtcNow.AddDays(1),
                    CreatedAt = DateTime.UtcNow,
                    IsEnabled = true
                },
                new Proposal
                {
                    Id = 3,
                    ExpertId = 2,
                    RequestId = 3,
                    SkillId = 3,
                    OrderId = 3,
                    Price = 780000,
                    ExecutionDate = DateTime.UtcNow.AddDays(7),
                    Description = "پیشنهاد انجام خدمات برای درخواست سنگ کاری",
                    Status = ProposalStatus.Pending,
                    ResponseTime = DateTime.UtcNow.AddDays(2),
                    CreatedAt = DateTime.UtcNow,
                    IsEnabled = false
                }
            );
        }
    }
}
