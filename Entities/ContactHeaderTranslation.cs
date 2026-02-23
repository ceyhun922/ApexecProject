namespace ApexWebAPI.Entities
{
    public class ContactHeaderTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int ContactHeaderId { get; set; }
        public ContactHeader? ContactHeader { get; set; }
    }
}
