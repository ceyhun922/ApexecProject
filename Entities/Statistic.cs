namespace ApexWebAPI.Entities
{
    public class Statistic : AuditableEntity
    {
        public string? Count1 { get; set; }
        public string? Count2 { get; set; }
        public string? Count3 { get; set; }
        public string? Count4 { get; set; }
        public ICollection<StatisticTranslation>? Translations { get; set; }
    }
}
