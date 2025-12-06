using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeterinaryClinic.Domain.Entities;

namespace VeterinaryClinic.Infrastructure.Persistence.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("Pets");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Species)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Breed)
                .IsRequired()
                .HasMaxLength(50);

            
            builder.Property(p => p.BirthDate)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.HasIndex(p => p.OwnerId);

            builder.HasIndex(p => p.Species);

            builder.HasMany(p => p.Appointments)
                .WithOne(a => a.Pet)
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}