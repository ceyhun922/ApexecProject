namespace ApexWebAPI.Entities
{
    public class ContactHeader
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; } = true;
        public ICollection<ContactHeaderTranslation>? Translations { get; set; }
    }
}
