using Clinic_System.Core.Enums;
using Clinic_System.Core.Interfaces;

namespace Clinic_System.Core.Entities
{
    public abstract class Person : ISoftDelete, IAuditable
    {
        public virtual int Id { get; set; }

        public virtual string FullName { get; set; } = null!;
        public virtual Gender Gender { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
        public virtual string Address { get; set; } = null!;
        public virtual string Phone { get; set; } = null!;
        // Soft Delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        // Audit Fields (automatically set by SaveChanges)
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
