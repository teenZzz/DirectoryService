using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

        builder.HasKey(x => x.Id).HasName("pk_locations");

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasConversion(x => x.Value, name => Name.Create(name).Value)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(150);
        
        builder.HasIndex(l => l.Name).IsUnique();

        builder.OwnsOne(x => x.Address, ab =>
        {
            ab.Property(l => l.Country)
                .IsRequired()
                .HasColumnName("country");

            ab.Property(l => l.City)
                .IsRequired()
                .HasColumnName("city");

            ab.Property(l => l.Street)
                .IsRequired()
                .HasColumnName("street");

            ab.Property(l => l.HouseNumber)
                .IsRequired()
                .HasColumnName("house_number");

            ab.Property(l => l.OfficeNumber)
                .IsRequired(false)
                .HasColumnName("office_number");

            ab.Property(l => l.AdditionalInfo)
                .IsRequired(false)
                .HasColumnName("additional_info");
        });

        builder.Property(x => x.Timezone)
            .HasConversion(x => x.Value, timezone => Timezone.Create(timezone).Value)
            .IsRequired()
            .HasColumnName("timezone");
        
        builder.Property(l => l.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
        
        builder.Property(l => l.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.Property(l => l.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
        
        builder
            .HasMany(l => l.DepartmentLocations)
            .WithOne()
            .HasForeignKey(dl => dl.LocationId);

    }
    
}