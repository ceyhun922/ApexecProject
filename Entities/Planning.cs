namespace ApexWebAPI.Entities
{
    public class Planning
    {
        public int Id { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<PlanningTranslation>? Translations { get; set; }
    }
}
