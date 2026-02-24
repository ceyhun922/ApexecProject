namespace ApexWebAPI.Entities
{
    public class Language
    {
        public int Id { get; set; }
        public bool Status { get; set; } = true;
        public ICollection<LanguageTranslation>? LanguageTranslations { get; set; }
        public ICollection<LanguageCourse>? LanguageCourses { get; set; }
    }
}
