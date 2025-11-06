using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class FlightSeatConfiguration : IEntityTypeConfiguration<FlightSeat>
    {
        public void Configure(EntityTypeBuilder<FlightSeat> builder)
        {
            builder.ToTable("FlightSeats");

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

            builder.Property(fs => fs.Locked)
                   .IsRequired(false);

            builder.Property(fs => fs.TicketId)
                   .IsRequired(false);

            builder.HasOne(fs => fs.FlightSchedule)
                   .WithMany(f => f.Seats)
                   .HasForeignKey(fs => fs.FlightScheduleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fs => fs.Ticket)
                   .WithOne(t => t.FlightSeat)
                   .HasForeignKey<Ticket>(t => t.TicketId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Navigation(fs => fs.Ticket)
                   .AutoInclude();

            builder.Navigation(fs => fs.FlightSchedule)
                   .AutoInclude();
        }
    }
}