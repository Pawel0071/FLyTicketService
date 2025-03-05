using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants");
            builder.HasKey(t => t.TenantId);
            builder.Property(t => t.TenantId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(t => t.Name).IsRequired().HasMaxLength(150);
            builder.Property(t => t.Address).IsRequired().HasMaxLength(250);
            builder.Property(t => t.Group).IsRequired();
            builder.Property(t => t.BirthDate).IsRequired();
            builder.Property(t => t.Phone).HasMaxLength(15);
            builder.Property(t => t.Email).HasMaxLength(100);
        }
    }

}