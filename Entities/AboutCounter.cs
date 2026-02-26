namespace ApexWebAPI.Entities
{
    public class AboutCounter
    {
        public int Id { get; set; }
        public string? Count1 { get; set; }
        public string? Count2 { get; set; }
        public string? Count3 { get; set; }
        public string? Count4 { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<AboutCounterTranslation>? Translations { get; set; }
    }
}
