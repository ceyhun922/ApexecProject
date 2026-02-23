namespace ApexWebAPI.DTOs.CountryDTOs
{
    public class CreateCountryDto
    {
        public bool Status { get; set; } = true;
        public string? NameAz { get; set; }
        public string? NameEn { get; set; }
        public string? NameRu { get; set; }
        public string? NameTr { get; set; }
    }
}