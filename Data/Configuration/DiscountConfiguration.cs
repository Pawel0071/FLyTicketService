using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable("Discounts");
            builder.HasKey(d => d.DiscountId);
            builder.Property(d => d.DiscountId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(d => d.Name).IsRequired().HasMaxLength(150);
            builder.Property(d => d.Value).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(d => d.Description).HasMaxLength(500);
        }
    }

}
