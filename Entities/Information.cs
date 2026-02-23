namespace ApexWebAPI.Entities
{
    public class Information
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Education { get; set; }
        public string? ClassOrYear { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}