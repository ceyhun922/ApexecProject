namespace ApexWebAPI.Entities
{
    public class About
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<AboutTranslation>? AboutTranslations {get;set;}
    }
}