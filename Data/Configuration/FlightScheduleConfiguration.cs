using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class FlightScheduleConfiguration : IEntityTypeConfiguration<FlightSchedule>
    {
        public void Configure(EntityTypeBuilder<FlightSchedule> builder)
        {
            builder.ToTable("FlightSchedules");
            builder.HasKey(f => f.FlightScheduleId);
            builder.Property(f => f.FlightScheduleId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(f => f.FlightId).IsRequired().HasMaxLength(50);
            builder.Property(f => f.Departure).IsRequired();
            builder.Property(f => f.Arrival).IsRequired();
            builder.Property(f => f.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.HasOne(f => f.Airline).WithMany().IsRequired();
            builder.HasOne(f => f.Aircraft).WithMany().IsRequired();
            builder.HasOne(f => f.Origin).WithMany().IsRequired();
            builder.HasOne(f => f.Destination).WithMany().IsRequired();
        }
    }

}