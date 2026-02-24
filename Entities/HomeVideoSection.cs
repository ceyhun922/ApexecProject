namespace ApexWebAPI.Entities
{
    public class HomeVideoSection : AuditableEntity
    {
        public string? YouTubeUrl { get; set; }
        public ICollection<HomeVideoSectionTranslation>? Translations { get; set; }
    }
}
