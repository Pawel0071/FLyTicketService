using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class FlightSeatConfiguration: IEntityTypeConfiguration<FlightSeat>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<FlightSeat> builder)
        {
            builder.HasKey(fs => fs.FlightSeatId);

            builder.Property(fs => fs.FlightSeatId)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(fs => fs.SeatNumber)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(fs => fs.Class)
                   .IsRequired();

            builder.Property(fs => fs.IsAvailable)
                   .IsRequired();

            builder.HasOne(fs => fs.FlightsPlan)
                   .WithMany(fp => fp.Seats)
                   .HasForeignKey(fs => fs.FlightsPlanId)
                   .IsRequired();

            builder.HasOne(fs => fs.Ticket)
                   .WithMany()
                   .HasForeignKey("TicketId");

            builder.HasOne(fs => fs.Reservation)
                   .WithMany()
                   .HasForeignKey("ReservationId");
        }

        #endregion
    }
}