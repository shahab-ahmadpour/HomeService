using App.Domain.Core.Skills.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Db.SqlServer.Ef.Configurations.SkillConfigurations
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.HasKey(s => s.Id);

            builder.ToTable("Skills");

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(s => s.SubHomeService)
                .WithMany(shs => shs.Skills)
                .HasForeignKey(s => s.SubHomeServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.ExpertSkills)
                .WithOne(es => es.Skill)
                .HasForeignKey(es => es.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
