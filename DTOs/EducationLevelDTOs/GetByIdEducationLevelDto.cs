namespace ApexWebAPI.DTOs.EducationLevelDTOs
{
    public class GetByIdEducationLevelDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CountryId { get; set; }
    }
}