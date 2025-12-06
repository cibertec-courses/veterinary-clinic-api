using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeterinaryClinic.Domain.Entities;

namespace VeterinaryClinic.Infrastructure.Persistence.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.AppointmentDate)
                .IsRequired();

            builder.Property(a => a.Reason)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Status)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(a => a.Notes)
                .HasMaxLength(1000);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.HasIndex(a => a.PetId);
            builder.HasIndex(a => a.AppointmentDate);
            builder.HasIndex(a => a.Status);

        }
    }
}