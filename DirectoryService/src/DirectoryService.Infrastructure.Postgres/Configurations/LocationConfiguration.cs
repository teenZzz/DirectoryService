using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        ConfigureTable(builder);
        ConfigureProperties(builder);
        ConfigureValueObjects(builder);
    }

    private static void ConfigureTable(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");
        builder.HasKey(l => l.Id).HasName("pk_locations");
    }

    private static void ConfigureProperties(EntityTypeBuilder<Location> builder)
    {
        builder.Property(l => l.Id)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(l => l.IsActive)
            .IsRequired()
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(l => l.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(l => l.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");
    }

    private static void ConfigureValueObjects(EntityTypeBuilder<Location> builder)
    {
        ConfigureLocationName(builder);
        ConfigureLocationAddress(builder);
        ConfigureLocationTimezone(builder);
    }

    private static void ConfigureLocationName(EntityTypeBuilder<Location> builder)
    {
        builder.OwnsOne(l => l.Name, nb =>
        {
            nb.Property(l => l.Value)
                .HasMaxLength(Const.Text.MAX_LENGHT)
                .HasColumnName("name");
        });

        builder.Navigation(l => l.Name).IsRequired();
    }

    private static void ConfigureLocationAddress(EntityTypeBuilder<Location> builder)
    {
        builder.OwnsOne(l => l.Address, ab =>
        {
            ab.Property(l => l.Country)
                .HasColumnName("country");

            ab.Property(l => l.City)
                .HasColumnName("city");

            ab.Property(l => l.Street)
                .HasColumnName("street");

            ab.Property(l => l.HouseNumber)
                .HasColumnName("house_number");

            ab.Property(l => l.OfficeNumber)
                .HasColumnName("office_number");

            ab.Property(l => l.AdditionalInfo)
                .HasColumnName("additional_info");
        });

        builder.Navigation(l => l.Address).IsRequired();
    }

    private static void ConfigureLocationTimezone(EntityTypeBuilder<Location> builder)
    {
        builder.OwnsOne(l => l.Timezone, tb =>
        {
            tb.Property(l => l.Value)
                .HasColumnName("timezone");
        });

        builder.Navigation(l => l.Timezone).IsRequired();
    }
    
}