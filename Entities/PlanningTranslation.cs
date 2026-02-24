namespace ApexWebAPI.Entities
{
    public class PlanningTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Option1Title { get; set; }
        public string? Option2Title { get; set; }
        public string? Option3Title { get; set; }
        public string? Option4Title { get; set; }

        public int PlanningId { get; set; }
        public Planning? Planning { get; set; }
    }
}
