namespace ApexWebAPI.Entities
{
    public class LanguageCourseTranslation
    {
        public int Id { get; set; }
        public string? Lang { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public int LanguageCourseId { get; set; }
        public LanguageCourse? LanguageCourse { get; set; }
    }
}
