using ApexWebAPI.DTOs.CountryDTOs;
using ApexWebAPI.DTOs.DepartmentDTOs;
using ApexWebAPI.DTOs.EducationLevelDTOs;

namespace ApexWebAPI.DTOs.SearchDTOs
{
    public class SearchItemDto
    {
        public ResultCountryDto? Country { get; set; }
        public ResultEducationLevelDto? EducationLevel { get; set; }
        public ResultDepartmentDto? Department { get; set; }
    }
}