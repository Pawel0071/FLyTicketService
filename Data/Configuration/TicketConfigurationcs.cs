using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class TicketConfiguration: IEntityTypeConfiguration<Ticket>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.TicketId);

            builder.Property(t => t.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Discount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Status)
                   .IsRequired();

            builder.HasOne(t => t.FlightSeat)
                   .WithOne()
                   .HasForeignKey<FlightSeat>(t => t.FlightSeatId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.Tenant)
                   .WithOne()
                   .HasForeignKey<Tenant>(t => t.TenantId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Property(t => t.ReleaseDate)
                   .IsRequired(false);
        }

        #endregion
    }
}