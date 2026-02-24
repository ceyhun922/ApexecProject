namespace ApexWebAPI.Entities
{
    public class FivePProgramCounter
    {
        public int Id { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<FivePProgramCounterTranslation>? Translations { get; set; }
    }
}
