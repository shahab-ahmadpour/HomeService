using App.Domain.Core.Enums;
using App.Domain.Core.Services.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Db.SqlServer.Ef.Configurations.OrderConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.ToTable("Orders");

            builder.Property(o => o.FinalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.PaymentStatus)
                .IsRequired();

            builder.Property(o => o.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Expert)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.ExpertId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Request)
                .WithOne()
                .HasForeignKey<Order>(o => o.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Proposal)
                .WithOne()
                .HasForeignKey<Order>(o => o.ProposalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Order { Id = 1, FinalPrice = 450, PaymentStatus = PaymentStatus.Pending, CreatedAt = DateTime.UtcNow, CustomerId = 1, ExpertId = 1, RequestId = 1, ProposalId = 1 },
                new Order { Id = 2, FinalPrice = 600, PaymentStatus = PaymentStatus.Pending, CreatedAt = DateTime.UtcNow, CustomerId = 1, ExpertId = 1, RequestId = 2, ProposalId = 2 },
                new Order { Id = 3, FinalPrice = 780, PaymentStatus = PaymentStatus.Pending, CreatedAt = DateTime.UtcNow, CustomerId = 2, ExpertId = 2, RequestId = 3, ProposalId = 3 }
            );

        }
    }
}
