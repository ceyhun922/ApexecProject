namespace ApexWebAPI.Entities
{
    public class StatisticTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Text1 { get; set; }
        public string? Text2 { get; set; }
        public string? Text3 { get; set; }
        public string? Text4 { get; set; }
        public int StatisticId { get; set; }
        public Statistic? Statistic { get; set; }
    }
}
