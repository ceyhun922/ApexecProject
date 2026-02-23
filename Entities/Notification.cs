namespace ApexWebAPI.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Type { get; set; }
        public bool IsRead { get; set; } = false;
        public int? ReferenceId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
