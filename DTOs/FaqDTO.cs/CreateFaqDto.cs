using System.ComponentModel;
namespace ApexWebAPI.DTOs.FaqDTO.cs
{
    public class CreateFaqDto
    {
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
        public string? TitleAz { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleRu { get; set; }
        public string? TitleTr { get; set; }
        public string? ContentAz { get; set; }
        public string? ContentEn { get; set; }
        public string? ContentRu { get; set; }
        public string? ContentTr { get; set; }
    }
}