namespace ApexWebAPI.Entities
{
    public class AboutVideoSection : AuditableEntity
    {
        public string? YouTubeUrl { get; set; }
        public ICollection<AboutVideoSectionTranslation>? Translations { get; set; }
    }
}
