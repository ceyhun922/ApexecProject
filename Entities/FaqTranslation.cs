namespace ApexWebAPI.Entities
{
    public class FaqTranslation
    {
        public int Id { get; set; }
        public string? Language { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int FaqId {get;set;}
        public Faq? Faq {get;set;}
    }
}