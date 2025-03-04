using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class AircraftSeatConfiguration: IEntityTypeConfiguration<AircraftSeat>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<AircraftSeat> builder)
        {
            builder.HasKey(ast => ast.AircraftSeatId);

            builder.Property(ast => ast.AircraftSeatId)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(ast => ast.SeatNumber)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(ast => ast.Class)
                   .IsRequired();

            builder.Property(ast => ast.OutOfService)
                   .IsRequired();

            builder.HasOne(ast => ast.Aircraft)
                   .WithMany(a => a.Seats)
                   .HasForeignKey(ast => ast.AircraftId)
                   .IsRequired();
        }

        #endregion
    }
}