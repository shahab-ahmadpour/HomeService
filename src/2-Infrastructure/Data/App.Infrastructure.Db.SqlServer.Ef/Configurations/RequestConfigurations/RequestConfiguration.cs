using App.Domain.Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Core.Services.Entities;

namespace App.Infrastructure.Db.SqlServer.Ef.Configurations.RequestConfigurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.HasKey(r => r.Id);

            builder.ToTable("Requests");

            builder.Property(r => r.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(r => r.Status)
                .IsRequired();

            builder.Property(r => r.Deadline)
                .IsRequired();

            builder.Property(r => r.ExecutionDate)
                .IsRequired();

            builder.Property(r => r.EnvironmentImage)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(r => r.IsEnabled)
                .HasDefaultValue(true);

            builder.Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.HasOne(r => r.Customer)
                .WithMany(c => c.Requests)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.SubHomeService)
                .WithMany(s => s.Requests)
                .HasForeignKey(r => r.SubHomeServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
