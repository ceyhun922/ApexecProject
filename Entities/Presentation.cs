namespace ApexWebAPI.Entities
{
    public class Presentation : AuditableEntity
    {
        public string? YouTubeUrl { get; set; }
        public ICollection<PresentationTranslation>? Translations { get; set; }
    }
}
