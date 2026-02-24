namespace ApexWebAPI.Entities
{
    public class SummerSchoolHeaderTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int SummerSchoolHeaderId { get; set; }
        public SummerSchoolHeader? SummerSchoolHeader { get; set; }
    }
}
