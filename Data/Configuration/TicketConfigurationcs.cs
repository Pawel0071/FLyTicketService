using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");
            builder.HasKey(t => t.TicketId);
            builder.Property(t => t.TicketId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(t => t.TicketNumber).IsRequired().HasMaxLength(20);
            builder.Property(t => t.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(t => t.Status).IsRequired();
            builder.HasOne(t => t.FlightSeat)
                   .WithOne(fs => fs.Ticket)
                   .HasForeignKey<FlightSeat>(fs => fs.TicketId)
                   .IsRequired();

            // Configure the one-to-many relationship explicitly
            builder.HasOne(t => t.Tenant)
                   .WithMany(tenant => tenant.Tickets)
                   .IsRequired();
        }
    }

}