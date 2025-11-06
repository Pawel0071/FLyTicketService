using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class ConditionConfiguration : IEntityTypeConfiguration<Condition>
    {
        public void Configure(EntityTypeBuilder<Condition> builder)
        {
            builder.ToTable("Conditions");
            builder.HasKey(c => c.ConditionId);
            builder.Property(c => c.ConditionId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(c => c.Category).IsRequired();
            builder.Property(c => c.Property).IsRequired().HasMaxLength(100);
            builder.Property(c => c.ConditionType).IsRequired();
            builder.Property(c => c.ConditionValue).HasMaxLength(255);
        }
    }


}
