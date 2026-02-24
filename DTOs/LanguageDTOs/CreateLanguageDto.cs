using System.ComponentModel;

namespace ApexWebAPI.DTOs.LanguageDTOs
{
    public class CreateLanguageDto
    {
        public string? NameAz { get; set; }
        public string? NameEn { get; set; }
        public string? NameRu { get; set; }
        public string? NameTr { get; set; }
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
    }
}
