namespace ApexWebAPI.DTOs.FeatureDTOs
{
    public class UpdateHeroDto
    {
        public int Id { get; set; }
          public IFormFile? Video { get; set; }
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