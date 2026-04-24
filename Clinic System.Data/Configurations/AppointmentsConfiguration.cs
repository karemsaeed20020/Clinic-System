
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic_System.Data.Configurations
{
    public class AppointmentsConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            // ============================================
            // Primary Key
            // ============================================
            builder.HasKey(a => a.Id);

            builder.ToTable("Appointments");

            // ============================================
            // AppointmentDate Property
            // ============================================
            builder.Property(a => a.AppointmentDate)
                .IsRequired()
                .HasColumnName("AppointmentDateTime");
            // Index على AppointmentDate للبحث السريع بالمواعيد
            builder.HasIndex(a => a.AppointmentDate)
                .HasDatabaseName("IX_Appointments_AppointmentDate");
            // Composite Index على AppointmentDate و Status
            builder.HasIndex(a => new { a.AppointmentDate, a.Status })
                .HasDatabaseName("IX_Appointments_Date_Status");

            // ============================================
            // Status Property
            // ============================================
            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName("AppointmentStatus");

            // Index على Status للبحث السريع
            builder.HasIndex(a => a.Status)
                .HasDatabaseName("IX_Appointments_Status");

            // ============================================
            // Patient Relationship (Many-to-One)
            // ============================================
            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(a => a.PatientId)
               .HasColumnName("PatientId");

            // Index على PatientId للأداء
            builder.HasIndex(a => a.PatientId)
    .HasDatabaseName("IX_Appointments_PatientId");

            // القيد الجديد لمنع المريض إنه يحجز ميعادين في نفس اللحظة
            builder.HasIndex(a => new { a.PatientId, a.AppointmentDate })
                .IsUnique()
                .HasDatabaseName("IX_Appointments_Patient_Date_Unique")
                .HasFilter("[AppointmentStatus] != 'Cancelled' AND [IsDeleted] = 0");

            // ============================================
            // Doctor Relationship (Many-to-One)
            // ============================================
            builder.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            // منع حذف Doctor إذا كان له Appointments

            // تسمية Foreign Key Column
            builder.Property(a => a.DoctorId)
                .HasColumnName("DoctorId");

            // Index على DoctorId للأداء
            builder.HasIndex(a => a.DoctorId)
                .HasDatabaseName("IX_Appointments_DoctorId");

            // Composite Index على DoctorId و AppointmentDate
            builder.HasIndex(a => new { a.DoctorId, a.AppointmentDate })
                .IsUnique()
                .HasDatabaseName("IX_Appointments_Doctor_Date_Unique")
                .HasFilter("[AppointmentStatus] != 'Cancelled' AND [IsDeleted] = 0");

            builder.HasOne(a => a.MedicalRecord)
               .WithOne(m => m.Appointment);

            // ============================================
            // Soft Delete
            // ============================================
            builder.Property(a => a.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(a => a.DeletedAt)
                .IsRequired(false)
                .HasColumnName("DeletedAt");



            // ============================================
            // Audit Fields
            // ============================================
            builder.Property(a => a.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(a => a.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("UpdatedAt");

            builder.HasIndex(a => a.CreatedAt)
                .HasDatabaseName("IX_Appointments_CreatedAt");

        }
    }
}
