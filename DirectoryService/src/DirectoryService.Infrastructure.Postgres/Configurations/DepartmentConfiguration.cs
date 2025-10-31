using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Path = System.IO.Path;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");
        
        builder.HasKey(d => d.Id).HasName("pk_department");

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasConversion(x => x.Value, name => Name.Create(name).Value)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnName("name");

        builder.Property(x => x.Identifier)
            .HasConversion(x => x.Value, identifier => Identifier.Create(identifier).Value)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnName("identifier");

        builder.Property(x => x.ParentId)
            .HasColumnName("parent_id")
            .IsRequired(false);

        builder.Property(x => x.Path)
            .HasConversion(x => x.Value, path => Domain.ValueObjects.Path.Create(path).Value)
            .IsRequired()
            .HasColumnType("ltree")
            .HasMaxLength(150)
            .HasColumnName("path");
        
        builder.HasIndex(d => d.Path).HasMethod("gist").HasDatabaseName("idx_departments_path");

        builder.Property(x => x.Depth)
            .HasConversion(x => x.Value, depth => DepartmentDepth.Create(depth).Value)
            .IsRequired()
            .HasColumnName("depth");

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasColumnName("is_active");

        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
        
        builder
            .HasMany(d => d.DepartmentLocations)
            .WithOne()
            .HasForeignKey(dl => dl.DepartmentId);
        
        builder
            .HasMany(d => d.DepartmentPositions)
            .WithOne()
            .HasForeignKey(dp => dp.DepartmentId);
    }
}