namespace ApexWebAPI.Entities
{
    public class Layout
    {
        public int Id { get; set; }
        public string? Logo { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<LayoutTranslation>? Translations { get; set; }
    }
}
