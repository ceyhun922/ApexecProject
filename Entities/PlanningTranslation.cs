namespace ApexWebAPI.Entities
{
    public class PlanningTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Badge { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int PlanningId { get; set; }
        public Planning? Planning { get; set; }
    }
}
