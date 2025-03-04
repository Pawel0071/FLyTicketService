using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class AirlineConfiguration: IEntityTypeConfiguration<Airline>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<Airline> builder)
        {
            builder.HasKey(a => a.AirlineId);

            builder.Property(a => a.AirlineId)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.Country)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.IATA)
                   .IsRequired()
                   .HasMaxLength(3);

            builder.Property(a => a.AirlineName)
                   .IsRequired()
                   .HasMaxLength(200);
        }

        #endregion
    }
}