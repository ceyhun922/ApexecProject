namespace ApexWebAPI.DTOs.UniversityDTOs
{
    public class GetByIdUniversityDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; }
        public int CountryId { get; set; }
        public int EducationLevelId { get; set; }
        public int DepartmentId { get; set; }
    }
}
