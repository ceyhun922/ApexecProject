namespace ApexWebAPI.DTOs.FaqDTO.cs
{
    public class CreateFaqDto
    {
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