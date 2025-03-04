using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class AircraftConfiguration: IEntityTypeConfiguration<Aircraft>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<Aircraft> builder)
        {
            builder.HasKey(a => a.AircraftId);

            builder.Property(a => a.AircraftId)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.Model)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.RegistrationNumber)
                   .IsRequired()
                   .HasMaxLength(15);

            builder.HasMany(a => a.Seats)
                   .WithOne()
                   .HasForeignKey("AircraftId")
                   .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion
    }
}