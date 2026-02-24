namespace ApexWebAPI.DTOs.LayoutDTOs
{
    public class UpdateLayoutDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public string? Logo { get; set; }
        public string? FooterTextAz { get; set; }
        public string? FooterTextEn { get; set; }
        public string? FooterTextRu { get; set; }
        public string? FooterTextTr { get; set; }
    }
}
