namespace ApexWebAPI.Entities
{
    public class LanguageCourse
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public ICollection<LanguageCourseTranslation>? Translations { get; set; }
    }
}
