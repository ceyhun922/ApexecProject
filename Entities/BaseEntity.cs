namespace ApexWebAPI.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }

    public abstract class AuditableEntity : BaseEntity
    {
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
