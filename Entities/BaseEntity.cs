namespace ApexWebAPI.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }

    public abstract class AuditableEntity : BaseEntity
    {
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
