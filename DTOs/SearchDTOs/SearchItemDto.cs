using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.DTOs.DepartmentDTOs;
using ApexWebAPI.DTOs.EducationLevelDTOs;

namespace ApexWebAPI.DTOs.SearchDTOs
{
    public class SearchItemDto
    {
        public int UniversityId { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public ResultCountryDto? Country { get; set; }
        public ResultEducationLevelDto? EducationLevel { get; set; }
        public ResultDepartmentDto? Department { get; set; }
    }
}