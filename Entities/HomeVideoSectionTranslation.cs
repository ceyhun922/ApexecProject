namespace ApexWebAPI.Entities
{
    public class HomeVideoSectionTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int HomeVideoSectionId { get; set; }
        public HomeVideoSection? HomeVideoSection { get; set; }
    }
}
