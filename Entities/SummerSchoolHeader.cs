namespace ApexWebAPI.Entities
{
    public class SummerSchoolHeader
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; } = true;
        public ICollection<SummerSchoolHeaderTranslation>? Translations { get; set; }
    }
}
