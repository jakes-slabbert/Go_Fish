using GoFish.Data.Entities;

namespace GoFish.Data.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid CreatedById { get; set; }

        public virtual AppUser CreatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public virtual AppUser? UpdatedBy { get; set; }

        public DateTimeOffset? DeletedOn { get; set; }

        public virtual AppUser? DeletedBy { get; set; }

    }
}
