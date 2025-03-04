using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class AirportConfiguration: IEntityTypeConfiguration<Airport>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            builder.HasKey(a => a.AirportId);

            builder.Property(a => a.AirportId)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.AirportName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.City)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Country)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.IATA)
                   .IsRequired()
                   .HasMaxLength(3);

            builder.Property(a => a.ICAO)
                   .IsRequired()
                   .HasMaxLength(4);

            builder.Property(a => a.Latitude)
                   .IsRequired();

            builder.Property(a => a.Longitude)
                   .IsRequired();

            builder.Property(a => a.Altitude)
                   .IsRequired();

            builder.Property(a => a.Timezone)
                   .IsRequired();

            builder.Property(a => a.DST)
                   .IsRequired()
                   .HasMaxLength(1);
        }

        #endregion
    }
}