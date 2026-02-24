namespace ApexWebAPI.Entities
{
    public class SocialMedia
    {
        public int Id { get; set; }
        public string? FbUrl { get; set; }
        public string? InstaUrl { get; set; }
        public string? LnUrl { get; set; }
        public string? XUrl { get; set; }
        public string? OtherUrl { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
