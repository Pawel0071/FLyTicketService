using FLyTicketService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLyTicketService.Data.Configuration
{
    public class TenantConfiguration: IEntityTypeConfiguration<Tenant>
    {
        #region Methods

        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(t => t.TenantId);

            builder.Property(t => t.TenantId)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(t => t.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(t => t.Address)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(t => t.Phone)
                   .HasMaxLength(15) // Typical length for phone number
                   .HasAnnotation("Phone", true); // Custom annotation for phone validation

            builder.Property(t => t.Email)
                   .HasAnnotation("EmailAddress", true); // Custom annotation for email validation
        }

        #endregion
    }
}