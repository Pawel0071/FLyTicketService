using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class AircraftSeatConfiguration : IEntityTypeConfiguration<AircraftSeat>
    {
        public void Configure(EntityTypeBuilder<AircraftSeat> builder)
        {
            builder.ToTable("AircraftSeats");
            builder.HasKey(s => s.AircraftSeatId);
            builder.Property(s => s.AircraftSeatId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(s => s.SeatNumber).IsRequired().HasMaxLength(10);
            builder.Property(s => s.Class).IsRequired();
            builder.Property(s => s.OutOfService).IsRequired();

        }
    }

}