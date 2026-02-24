namespace ApexWebAPI.DTOs.LanguageCourseDTOs
{
    public class ResultLanguageCourseDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; }
        public int LanguageId { get; set; }
        public int CountryId { get; set; }
    }
}
