namespace ApexWebAPI.Entities
{
    public class SummerSchoolTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public int SummerSchoolId { get; set; }
        public SummerSchool SummerSchool { get; set; } = null!;
    }
}