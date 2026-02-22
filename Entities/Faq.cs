namespace ApexWebAPI.Entities
{
    public class Faq
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool Status { get; set; }
        public ICollection<FaqTranslation>? FaqTranslations {get;set;}
    }
}