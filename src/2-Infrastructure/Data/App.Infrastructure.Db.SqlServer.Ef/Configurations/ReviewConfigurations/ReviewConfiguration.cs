using App.Domain.Core.Services.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace App.Infrastructure.Db.SqlServer.Ef.Configurations.ReviewConfigurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);

            builder.ToTable("Reviews");

            builder.Property(r => r.Rating)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(r => r.Comment)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Expert)
                .WithMany(e => e.Reviews)
                .HasForeignKey(r => r.ExpertId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Order)
                .WithOne()
                .HasForeignKey<Review>(r => r.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Review
                {
                    Id = 1,
                    CustomerId = 1,
                    ExpertId = 1,
                    OrderId = 1,
                    Rating = 5,
                    Comment = "خیلی عالی بود سرموقع انجام شد",
                    IsApproved = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Review
                {
                    Id = 2,
                    CustomerId = 1,
                    ExpertId = 1,
                    OrderId = 2,
                    Rating = 4,
                    Comment = "سرویس خوبی انجام دادن فقط یکم تو تحویل سرویس تاخیر داشتن",
                    IsApproved = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Review
                {
                    Id = 3,
                    CustomerId = 2,
                    ExpertId = 2,
                    OrderId = 3,
                    Rating = 5,
                    Comment = "فوق العاده بود همه چی",
                    IsApproved= false,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
