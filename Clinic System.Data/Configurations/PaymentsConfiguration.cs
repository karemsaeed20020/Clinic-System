
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic_System.Data.Configurations
{
    public class PaymentsConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // ============================================
            // Primary Key
            // ============================================
            builder.HasKey(p => p.Id);

            builder.ToTable("Payments", table =>
            {
                // Check Constraint: AmountPaid يجب أن يكون أكبر من 0
                table.HasCheckConstraint("CK_Payments_AmountPaid_Positive",
                    "[AmountPaid] > 0");
            });

            // ============================================
            // AmountPaid Property
            // ============================================
            builder.Property(p => p.AmountPaid)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasColumnName("AmountPaid");

            // ============================================
            // PaymentDate Property
            // ============================================
            builder.Property(p => p.PaymentDate)
                .IsRequired(false)
                .HasColumnName("PaymentDate");

            // Index على PaymentDate للبحث السريع
            builder.HasIndex(p => p.PaymentDate)
                .HasDatabaseName("IX_Payments_PaymentDate");

            // ============================================
            // PaymentMethod Property
            // ============================================
            builder.Property(p => p.PaymentMethod)
                .IsRequired(false)
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName("PaymentMethod");

            // Index على PaymentMethod للبحث السريع
            builder.HasIndex(p => p.PaymentMethod)
                .HasDatabaseName("IX_Payments_PaymentMethod");


            // ============================================
            // PaymentStatus Property
            // ============================================
            builder.Property(p => p.PaymentStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName("PaymentStatus");

            // Index على PaymentMethod للبحث السريع
            builder.HasIndex(p => p.PaymentStatus)
                .HasDatabaseName("IX_Payments_PaymentStatus");

            // ============================================
            // AdditionalNotes Property (Optional)
            // ============================================
            builder.Property(p => p.AdditionalNotes)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnName("AdditionalNotes");

            // ============================================
            // Appointment Relationship (One-to-One)
            // ============================================
            builder.HasOne(p => p.Appointment)
                .WithOne(a => a.Payment)
                .HasForeignKey<Payment>(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // تسمية Foreign Key Column
            builder.Property(p => p.AppointmentId)
                .HasColumnName("AppointmentId");

            // Index على AppointmentId (لكنه Unique بالفعل بسبب One-to-One)
            builder.HasIndex(p => p.AppointmentId)
                .IsUnique()
                .HasDatabaseName("IX_Payments_AppointmentId");
            // IsUnique: يضمن أن كل Appointment له Payment واحد فقط

            // Composite Index على PaymentDate و PaymentMethod
            builder.HasIndex(p => new { p.PaymentDate, p.PaymentMethod })
                .HasDatabaseName("IX_Payments_Date_Method");

            // ============================================
            // Soft Delete
            // ============================================
            builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(p => p.DeletedAt)
                .IsRequired(false)
                .HasColumnName("DeletedAt");

            // ============================================
            // Audit Fields
            // ============================================
            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(p => p.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("UpdatedAt");

            builder.HasIndex(p => p.CreatedAt)
                .HasDatabaseName("IX_Payments_CreatedAt");
        }
    }
}
