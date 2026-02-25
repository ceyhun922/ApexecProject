namespace ApexWebAPI.Entities
{
    public class SummerSchool
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public List<SummerSchoolTranslation> Translations { get; set; } = new();
    }
}