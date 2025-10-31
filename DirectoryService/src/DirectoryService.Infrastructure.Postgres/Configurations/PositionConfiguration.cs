using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions");

        builder.HasKey(x => x.Id).HasName("pk_positions");

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName("id");
        
        builder.Property(x => x.Name)
            .HasConversion(x => x.Value, name => Name.Create(name).Value)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasColumnName("description")
            .HasMaxLength(1000);
        
        builder
            .Property(p => p.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
        
        builder
            .Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder
            .Property(p => p.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
        
        builder
            .HasMany(p => p.DepartmentPositions)
            .WithOne()
            .HasForeignKey(dp => dp.PositionId);
    }
    
}