using System.ComponentModel;

namespace ApexWebAPI.DTOs.LayoutDTOs
{
    public class CreateLayoutDto
    {
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
        public string? Logo { get; set; }
        public string? FooterTextAz { get; set; }
        public string? FooterTextEn { get; set; }
        public string? FooterTextRu { get; set; }
        public string? FooterTextTr { get; set; }
    }
}
