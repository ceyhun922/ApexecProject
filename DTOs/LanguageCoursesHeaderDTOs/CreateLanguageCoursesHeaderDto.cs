using System.ComponentModel;

namespace ApexWebAPI.DTOs.LanguageCoursesHeaderDTOs
{
    public class CreateLanguageCoursesHeaderDto
    {
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
        public string? ImageUrl { get; set; }
        public string? TitleAz { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleRu { get; set; }
        public string? TitleTr { get; set; }
        public string? SubTitleAz { get; set; }
        public string? SubTitleEn { get; set; }
        public string? SubTitleRu { get; set; }
        public string? SubTitleTr { get; set; }
    }
}
