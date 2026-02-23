namespace ApexWebAPI.Entities
{
    public class PresentationTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int PresentationId { get; set; }
        public Presentation? Presentation { get; set; }
    }
}
