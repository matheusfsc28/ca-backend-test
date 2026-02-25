namespace BillingSystem.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; protected set; }
        public bool IsDeleted => DeletedAt != null;
        public void Delete() => DeletedAt = DateTime.UtcNow;
        public void Restore() => DeletedAt = null;
        public void Touch() => UpdatedAt = DateTime.UtcNow;
    }
}
