namespace ApexWebAPI.Entities
{
    public class CoursesHeaderTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public int CoursesHeaderId { get; set; }
        public CoursesHeader? CoursesHeader { get; set; }
    }
}
