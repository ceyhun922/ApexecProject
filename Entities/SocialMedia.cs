namespace ApexWebAPI.Entities
{
    public class SocialMedia
    {
        public int Id { get; set; }
        public string Key { get; set; } = null!;
        public string? Url { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
