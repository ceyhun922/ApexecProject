namespace ApexWebAPI.Entities
{
    public class LanguageTranslation
    {
        public int Id { get; set; }
        public string? Lang { get; set; }
        public string? Name { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
    }
}
