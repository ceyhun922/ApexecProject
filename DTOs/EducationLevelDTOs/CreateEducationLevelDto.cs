namespace ApexWebAPI.DTOs.EducationLevelDTOs
{
    public class CreateEducationLevelDto
    {
        public bool Status { get; set; } = true;
        public string? NameAz { get; set; }
        public string? NameEn { get; set; }
        public string? NameTr { get; set; }
        public string? NameRu { get; set; }
        public int CountryId { get; set; }
    }
}