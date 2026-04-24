
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic_System.Data.Configurations
{
    public class PatientsConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.Id);
            builder.ToTable("Patients");
            builder.Property(p => p.FullName)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnName("PatientName");
            builder.Property(d => d.Gender)
              .IsRequired()
              .HasConversion<string>()
              .HasMaxLength(10)
              .HasColumnName("Gender");
            builder.Property(p => p.DateOfBirth)
               .IsRequired()
               .HasColumnName("DateOfBirth");

            // Address: Required, MaxLength 200
            builder.Property(p => p.Address)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Address");

            // Phone: Required, MaxLength 20
            builder.Property(p => p.Phone)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("PhoneNumber");

            builder.HasOne<ApplicationUser>()
               .WithOne(u => u.Patient)
               .HasForeignKey<Patient>(p => p.ApplicationUserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.ApplicationUserId)
              .IsRequired()
              .HasColumnName("ApplicationUserId");

            // Index على ApplicationUserId للأداء والـ Unique constraint
            builder.HasIndex(p => p.ApplicationUserId)
                .IsUnique()
                .HasDatabaseName("IX_Patients_ApplicationUserId_Unique")
                .HasFilter("[IsDeleted] = 0");
            builder.HasMany(p => p.Appointments)
               .WithOne(a => a.Patient)
               .HasForeignKey(a => a.PatientId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.IsDeleted)
              .IsRequired()
              .HasDefaultValue(false)
              .HasColumnName("IsDeleted");
            // HasDefaultValue: قيمة افتراضية في Database

            builder.Property(p => p.DeletedAt)
                .IsRequired(false)
                .HasColumnName("DeletedAt");

            builder.Property(p => p.CreatedAt)
              .IsRequired()
              .HasColumnName("CreatedAt");

            builder.Property(p => p.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("UpdatedAt");

            // Index على CreatedAt للاستعلامات السريعة
            builder.HasIndex(p => p.CreatedAt)
                .HasDatabaseName("IX_Patients_CreatedAt");
        }
    }
}
