namespace ApexWebAPI.DTOs.LanguageCourseDTOs
{
    public class UpdateLanguageCourseDto
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public int LanguageId { get; set; }
        public int CountryId { get; set; }
        public string? TitleAz { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleRu { get; set; }
        public string? TitleTr { get; set; }
        public string? SubTitleAz { get; set; }
        public string? SubTitleEn { get; set; }
        public string? SubTitleRu { get; set; }
        public string? SubTitleTr { get; set; }
        public bool Status { get; set; }
    }
}
