

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinic_System.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {

            // ============================================
            // Primary Key & Table Name
            // ============================================
            builder.HasKey(rt => rt.Id);
            builder.ToTable("RefreshTokens");

            // ============================================
            // Token Property
            // ============================================
            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(rt => rt.Token)
                .IsUnique()
                .HasDatabaseName("IX_RefreshTokens_Token");

            // ============================================
            // Dates Configuration
            // ============================================
            builder.Property(rt => rt.ExpiresOn)
                .IsRequired();

            builder.Property(rt => rt.CreatedOn)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(rt => rt.RevokedOn)
                .IsRequired(false);

            // ============================================
            // Foreign Key (ApplicationUser)
            // ============================================
            // هنا نربط التوكن باليوزر (One-to-Many)
            // لاحظ أننا لم نضع Navigation Property في الـ Core، لذا نستخدم المعرف النصي
            builder.Property(rt => rt.ApplicationUserId)
                .IsRequired();

            // Index على Foreign Key لتحسين أداء الـ Join
            builder.HasIndex(rt => rt.ApplicationUserId)
                .HasDatabaseName("IX_RefreshTokens_UserId");
        }
    }
}
