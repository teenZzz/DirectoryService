using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        ConfigureTable(builder);
        ConfigureProperties(builder);
        ConfigureValueObjects(builder);
    }

    private static void ConfigureTable(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions");
        builder.HasKey(p => p.Id).HasName("pk_positions");
    }

    private static void ConfigureProperties(EntityTypeBuilder<Position> builder)
    {
        builder.Property(p => p.Id)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasColumnName("description");

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasColumnName("is_active")
            .HasDefaultValue(true);
        
        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(p => p.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");
    }

    private static void ConfigureValueObjects(EntityTypeBuilder<Position> builder)
    {
        ConfigurePositionName(builder);
    }

    private static void ConfigurePositionName(EntityTypeBuilder<Position> builder)
    {
        builder.OwnsOne(p => p.Name, nb =>
        {
            nb.Property(p => p.Value)
                .HasMaxLength(Const.Text.MAX_LENGHT)
                .HasColumnName("name");
        });

        builder.Navigation(p => p.Name).IsRequired();
    }
}