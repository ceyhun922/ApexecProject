namespace ApexWebAPI.DTOs.LanguageDTOs
{
    public class UpdateLanguageDto
    {
        public int Id { get; set; }
        public string? NameAz { get; set; }
        public string? NameEn { get; set; }
        public string? NameRu { get; set; }
        public string? NameTr { get; set; }
        public bool Status { get; set; }
    }
}
