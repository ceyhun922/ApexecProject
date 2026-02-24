namespace ApexWebAPI.Entities
{
    public class CountryHeaderTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int CountryHeaderId { get; set; }
        public CountryHeader? CountryHeader { get; set; }
    }
}
