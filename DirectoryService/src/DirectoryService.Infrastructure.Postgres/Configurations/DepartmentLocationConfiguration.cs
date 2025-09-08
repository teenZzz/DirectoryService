using DirectoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class DepartmentLocationConfiguration : IEntityTypeConfiguration<DepartmentLocation>
{
    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        builder.ToTable("department_locations");

        builder.HasKey(dl => new { dl.DepartmentId, dl.LocationId }).HasName("pk_department_locations");

        builder.Property(dl => dl.DepartmentId)
            .IsRequired()
            .HasColumnName("department_id");

        builder.Property(dl => dl.LocationId)
            .IsRequired()
            .HasColumnName("location_id");

        builder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(dl => dl.LocationId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_department_locations_location");
    }
}