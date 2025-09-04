using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        ConfigureTable(builder);
        ConfigureProperties(builder);
        ConfigureValueObjects(builder);
        ConfigureRelations(builder);
    }

    private static void ConfigureTable(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");
        builder.HasKey(d => d.Id).HasName("pk_departments");
    }

    private static void ConfigureProperties(EntityTypeBuilder<Department> builder)
    {
        builder.Property(d => d.Id)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(d => d.ParentId)
            .IsRequired(false)
            .HasColumnName("parent_id");

        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasColumnName("is_active");

        builder.Property(d => d.CreateAt)
            .IsRequired()
            .HasColumnName("create_at");

        builder.Property(d => d.UpdateAt)
            .IsRequired()
            .HasColumnName("update_at");
    }

    private static void ConfigureValueObjects(EntityTypeBuilder<Department> builder)
    {
        ConfigureDepartmentName(builder);
        ConfigureDepartmentIdentifier(builder);
        ConfigureDepartmentPath(builder);
        ConfigureDepartmentDepth(builder);
    }

    private static void ConfigureRelations(EntityTypeBuilder<Department> builder)
    {
        ConfigurePositionsRelation(builder);
        ConfigureLocationsRelation(builder);
        ConfigureParentChildRelationship(builder);
    }

    private static void ConfigureDepartmentName(EntityTypeBuilder<Department> builder)
    {
        builder.OwnsOne(d => d.Name, nb =>
        {
            nb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Const.Text.MAX_LENGHT)
                .HasColumnName("name");
        });

        builder.Navigation(d => d.Name).IsRequired();
    }

    private static void ConfigureDepartmentIdentifier(EntityTypeBuilder<Department> builder)
    {
        builder.OwnsOne(d => d.Identifier, ib =>
        {
            ib.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Const.Text.MAX_LENGHT)
                .HasColumnName("identifier");
        });

        builder.Navigation(d => d.Identifier).IsRequired();
    }
    
    private static void ConfigureDepartmentPath(EntityTypeBuilder<Department> builder)
    {
        builder.OwnsOne(d => d.Path, pb =>
        {
            pb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Const.Text.MAX_LENGHT)
                .HasColumnName("path");
        });

        builder.Navigation(d => d.Path).IsRequired();
    }
    
    private static void ConfigureDepartmentDepth(EntityTypeBuilder<Department> builder)
    {
        builder.OwnsOne(d => d.Depth, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasColumnName("depth");
        });

        builder.Navigation(d => d.Depth).IsRequired();
    }

    private static void ConfigurePositionsRelation(EntityTypeBuilder<Department> builder)
    {
        builder.HasMany(d => d.DepartmentPositions)
            .WithOne()
            .HasForeignKey(dp => dp.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(d => d.DepartmentPositions)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_departmentPositions");
    }
    
    private static void ConfigureLocationsRelation(EntityTypeBuilder<Department> builder)
    {
        builder.HasMany(d => d.DepartmentLocations)
            .WithOne()
            .HasForeignKey(dp => dp.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(d => d.DepartmentLocations)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_departmentLocations");
    }

    private static void ConfigureParentChildRelationship(EntityTypeBuilder<Department> builder)
    {
        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(v => v.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}