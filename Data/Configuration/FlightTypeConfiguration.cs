using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class FlightTypeConfiguration: IEntityTypeConfiguration<FlightType>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<FlightType> builder)
        {
            builder.HasKey(ft => ft.FlightTypeId);

            builder.Property(ft => ft.Name)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(ft => ft.Description)
                   .HasMaxLength(500);
        }

        #endregion
    }
}