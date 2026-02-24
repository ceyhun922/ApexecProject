namespace ApexWebAPI.Entities
{
    public class LanguageCoursesHeaderTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int LanguageCoursesHeaderId { get; set; }
        public LanguageCoursesHeader? LanguageCoursesHeader { get; set; }
    }
}
