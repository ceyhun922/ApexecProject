namespace ApexWebAPI.DTOs.EducationLevelDTOs
{
    public class UpdateEducationLevelDto
    {
        public int Id { get; set; }
        public string? NameAz { get; set; }
        public string? NameEn { get; set; }
        public string? NameTr { get; set; }
        public string? NameRu { get; set; }
        public int CountryId { get; set; }
    }
}