namespace ApexWebAPI.Entities
{
    public class AboutVideoSectionTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int AboutVideoSectionId { get; set; }
        public AboutVideoSection? AboutVideoSection { get; set; }
    }
}
