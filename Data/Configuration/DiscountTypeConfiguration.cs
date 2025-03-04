using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class DiscountTypeConfiguration : IEntityTypeConfiguration<DiscountType>
    {
        public void Configure(EntityTypeBuilder<DiscountType> builder)
        {
            builder.HasKey(dt => dt.DiscountTypeId);

            builder.Property(dt => dt.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(dt => dt.Discount)
                   .IsRequired();

            builder.Property(dt => dt.Description)
                   .HasMaxLength(500);

            builder.Property(dt => dt.Condition)
                   .HasMaxLength(200);

            builder.Property(dt => dt.ConditionValue)
                   .HasMaxLength(200);

            builder.Property(dt => dt.Type)
                   .IsRequired();
        }
    }
}
