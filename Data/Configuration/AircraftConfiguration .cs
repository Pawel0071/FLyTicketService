using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class AircraftConfiguration : IEntityTypeConfiguration<Aircraft>
    {
        public void Configure(EntityTypeBuilder<Aircraft> builder)
        {
            builder.ToTable("Aircrafts");
            builder.HasKey(a => a.AircraftId);
            builder.Property(a => a.AircraftId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(a => a.Model).IsRequired().HasMaxLength(100);
            builder.Property(a => a.RegistrationNumber).IsRequired().HasMaxLength(20);
            builder.HasMany(a => a.Seats)
                   .WithOne()
                   .HasForeignKey(s => s.AircraftId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(a => a.RegistrationNumber).IsUnique();
            builder.Navigation(a => a.Seats).AutoInclude();

        }
    }

}