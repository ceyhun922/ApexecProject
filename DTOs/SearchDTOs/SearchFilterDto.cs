namespace ApexWebAPI.DTOs.SearchDTOs
{
    public class SearchFilterDto
    {
        public int? CountryId { get; set; }
        public int? EducationLevelId { get; set; }
        public int? DepartmentId { get; set; }
        public string Language { get; set; } = "az";

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}