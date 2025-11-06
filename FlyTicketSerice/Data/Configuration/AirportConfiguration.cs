using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class AirportConfiguration : IEntityTypeConfiguration<Airport>
    {
        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            builder.ToTable("Airports");
            builder.HasKey(a => a.AirportId);
            builder.Property(a => a.AirportId).IsRequired().ValueGeneratedOnAdd(); // Automatically generate a new GUID for each record
            builder.Property(a => a.AirportName).IsRequired().HasMaxLength(150); // Set a maximum length for the airport name
            builder.Property(a => a.City).IsRequired().HasMaxLength(100); // Maximum length for the city name
            builder.Property(a => a.Country).IsRequired().HasMaxLength(100); // Maximum length for the country name
            builder.Property(a => a.IATA).IsRequired().HasMaxLength(3); // IATA code is always 3 characters
            builder.Property(a => a.ICAO).IsRequired().HasMaxLength(4); // ICAO code is always 4 characters
            builder.Property(a => a.Latitude).IsRequired();
            builder.Property(a => a.Longitude).IsRequired();
            builder.Property(a => a.Altitude).IsRequired();
            builder.Property(a => a.Timezone).IsRequired();
            builder.Property(a => a.DST).IsRequired();
            builder.Property(a => a.Continent).IsRequired().HasMaxLength(50); // Maximum length for the continent name
            builder.HasIndex(a => a.IATA).HasDatabaseName("IX_Airport_IATA").IsUnique(); // Ensure IATA codes are unique
            builder.HasIndex(a => a.ICAO).HasDatabaseName("IX_Airport_ICAO").IsUnique(); // Ensure ICAO codes are unique
        }
    }

}