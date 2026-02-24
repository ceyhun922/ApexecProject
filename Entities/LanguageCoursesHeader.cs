namespace ApexWebAPI.Entities
{
    public class LanguageCoursesHeader
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; } = true;
        public ICollection<LanguageCoursesHeaderTranslation>? Translations { get; set; }
    }
}
