using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class FlightScheduleConfiguration: IEntityTypeConfiguration<FlightSchedule>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<FlightSchedule> builder)
        {
            {
                builder.HasKey(fs => fs.FlightScheduleId);

                builder.Property(fs => fs.FlightId)
                       .IsRequired();

                builder.Property(fs => fs.Type)
                       .IsRequired();

                builder.Property(fs => fs.Departure)
                       .IsRequired();

                builder.Property(fs => fs.Arrival)
                       .IsRequired();

                builder.Property(fs => fs.Price)
                       .IsRequired()
                       .HasColumnType("decimal(18,2)");

                builder.HasOne(fs => fs.Airline)
                       .WithMany()
                       .HasForeignKey("AirlineId")
                       .OnDelete(DeleteBehavior.NoAction);

                builder.HasOne(fs => fs.Aircraft)
                       .WithMany()
                       .HasForeignKey("AircraftId")
                       .OnDelete(DeleteBehavior.NoAction);

                builder.HasOne(fs => fs.Origin)
                       .WithMany()
                       .HasForeignKey("OriginId")
                       .OnDelete(DeleteBehavior.NoAction);

                builder.HasOne(fs => fs.Destination)
                       .WithMany()
                       .HasForeignKey("DestinationId")
                       .OnDelete(DeleteBehavior.NoAction);

                builder.HasMany(fs => fs.Seats)
                       .WithOne()
                       .HasForeignKey("FlightScheduleId")
                       .OnDelete(DeleteBehavior.Cascade);
            }
        }

        #endregion
    }
}