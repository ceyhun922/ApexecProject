using System.ComponentModel;
namespace ApexWebAPI.DTOs.SummerSchoolDTOs
{
    public class CreateSummerSchoolDto
    {
        public string? TitleAz { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleRu { get; set; }
        public string? TitleTr { get; set; }
        public string? SubTitleAz { get; set; }
        public string? SubTitleEn { get; set; }
        public string? SubTitleRu { get; set; }
        public string? SubTitleTr { get; set; }
        public string? DescriptionAz { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionRu { get; set; }
        public string? DescriptionTr { get; set; }
        public string? ImageUrl { get; set; }
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
        public int CountryId { get; set; }
    }
}