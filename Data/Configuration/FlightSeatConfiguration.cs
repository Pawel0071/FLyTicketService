using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class FlightSeatConfiguration: IEntityTypeConfiguration<FlightSeat>
    {
        #region Methods

        #region Methods

        public void Configure(EntityTypeBuilder<FlightSeat> builder)
        {
            builder.HasKey(fs => fs.FlightSeatId);

            builder.Property(fs => fs.SeatNumber)
                   .IsRequired();

            builder.HasOne<FlightSchedule>()
                   .WithMany()
                   .HasForeignKey(fs => fs.FlightScheduleId)
                   .OnDelete(DeleteBehavior.ClientCascade);

            builder.Property(fs => fs.Class)
                   .IsRequired();

            builder.Property(fs => fs.IsAvailable)
                   .IsRequired();

            builder.Property(fs => fs.Locked)
                   .IsRequired(false);

            builder.HasOne(fs => fs.Ticket)
                   .WithOne()
                   .HasForeignKey<Ticket>(t => t.TicketId)
                   .OnDelete(DeleteBehavior.NoAction);

            #endregion
        }

        #endregion
    }
}