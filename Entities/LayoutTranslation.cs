namespace ApexWebAPI.Entities
{
    public class LayoutTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? FooterText { get; set; }

        public int LayoutId { get; set; }
        public Layout? Layout { get; set; }
    }
}
