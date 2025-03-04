using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class FlightsPlanConfiguration: IEntityTypeConfiguration<FlightsPlan>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<FlightsPlan> builder)
        {
            builder.HasKey(fp => fp.FlightsPlanId);

            builder.Property(fp => fp.FlightsPlanId)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(fp => fp.FlyNumber)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(fp => fp.Departure)
                   .IsRequired();

            builder.Property(fp => fp.Arrival)
                   .IsRequired();

            builder.Property(fp => fp.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(fp => fp.Airline)
                   .WithMany()
                   .IsRequired();

            builder.HasOne(fp => fp.Aircraft)
                   .WithMany()
                   .IsRequired();

            builder.HasOne(fp => fp.Origin)
                   .WithMany()
                   .IsRequired();

            builder.HasOne(fp => fp.Destination)
                   .WithMany()
                   .IsRequired();

            builder.HasMany(fp => fp.Seats)
                   .WithOne(s => s.FlightsPlan)
                   .HasForeignKey(s => s.FlightsPlanId)
                   .IsRequired();
        }

        #endregion
    }
}