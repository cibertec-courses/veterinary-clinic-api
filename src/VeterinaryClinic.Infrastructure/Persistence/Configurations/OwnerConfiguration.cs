using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeterinaryClinic.Domain.Entities;



namespace VeterinaryClinic.Infrastructure.Persistence.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.ToTable("Owners");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Phone)
                .HasMaxLength(15);

            builder.Property(o => o.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(o => o.Email)
                .IsUnique();

            builder.HasMany(o => o.Pets)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
