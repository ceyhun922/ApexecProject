namespace ApexWebAPI.Entities
{
    public class CountryHeader
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; } = true;
        public ICollection<CountryHeaderTranslation>? Translations { get; set; }
    }
}
