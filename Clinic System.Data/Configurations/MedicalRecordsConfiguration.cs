

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic_System.Data.Configurations
{
    public class MedicalRecordsConfiguration : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            // ============================================
            // Primary Key
            // ============================================
            builder.HasKey(m => m.Id);

            builder.ToTable("MedicalRecords");

            // ============================================
            // Diagnosis Property
            // ============================================
            builder.Property(m => m.Diagnosis)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("Diagnosis");
            // Diagnosis: التشخيص الطبي

            // ============================================
            // DescriptionOfTheVisit Property
            // ============================================
            builder.Property(m => m.DescriptionOfTheVisit)
                .IsRequired()
                .HasMaxLength(2000)
                .HasColumnName("VisitDescription");

            // ============================================
            // AdditionalNotes Property (Optional)
            // ============================================
            builder.Property(m => m.AdditionalNotes)
                .IsRequired(false)
                .HasMaxLength(1000)
                .HasColumnName("AdditionalNotes");

            // ============================================
            // Appointment Relationship (Many-to-One)
            // ============================================
            builder.HasOne(m => m.Appointment)
                .WithOne(a => a.MedicalRecord)
                .HasForeignKey<MedicalRecord>(m => m.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // تسمية Foreign Key Column
            builder.Property(m => m.AppointmentId)
                .HasColumnName("AppointmentId");

            // Index على AppointmentId (لكنه Unique بالفعل بسبب One-to-One)
            builder.HasIndex(m => m.AppointmentId)
                .IsUnique()
                .HasDatabaseName("IX_MedicalRecords_AppointmentId");

            // ============================================
            // Prescriptions Relationship (One-to-Many)
            // ============================================
            builder.HasMany(m => m.Prescriptions)
                .WithOne(p => p.MedicalRecord)
                .HasForeignKey(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================================
            // Soft Delete
            // ============================================
            builder.Property(m => m.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(m => m.DeletedAt)
                .IsRequired(false)
                .HasColumnName("DeletedAt");

            // ============================================
            // Audit Fields
            // ============================================
            builder.Property(m => m.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(m => m.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("UpdatedAt");

            builder.HasIndex(m => m.CreatedAt)
                .HasDatabaseName("IX_MedicalRecords_CreatedAt");
        }
    }
}
